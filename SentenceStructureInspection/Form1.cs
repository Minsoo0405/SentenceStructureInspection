using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SentenceStructureInspection
{
    public partial class Form1 : Form
    {

        private List<string> jsonFilePaths = new List<string>();  // JSON 파일 경로를 저장할 리스트

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectJson_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // 초기 디렉토리 설정 (옵션)
                folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;

                // 다이얼로그 타이틀 설정 (옵션)
                folderDialog.Description = "폴더를 선택하세요";

                // 사용자가 폴더를 선택하고 'OK'를 누르면
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // 텍스트 박스에 선택된 폴더 경로를 표시
                    tbFolder.Text = folderDialog.SelectedPath;

                    // JSON 파일 목록을 로드하고 표시
                    LoadJsonFiles(folderDialog.SelectedPath);
                }
            }
        }

        private void LoadJsonFiles(string folderPath)
        {
            // 폴더 내의 모든 JSON 및 CSV 파일 찾기 (하위 디렉토리 포함)
            string[] files = Directory.GetFiles(folderPath, "*.*", System.IO.SearchOption.AllDirectories)
                                       .Where(file => file.ToLower().EndsWith(".json") || file.ToLower().EndsWith(".csv"))
                                       .ToArray();

            // 파일 경로에서 파일 이름만 추출하여 표시
            var fileNames = files.Select(Path.GetFileName).ToArray();

            // 파일 이름을 오름차순으로 정렬
            //Array.Sort(fileNames);

            // 파일 경로를 전역 변수에 저장 (검수 메소드에서 사용하기 위해)
            jsonFilePaths = files.ToList();

            // 정렬된 파일 이름을 텍스트 박스에 표시
            tbFileList.Text = string.Join(Environment.NewLine, fileNames);
        }

        // DragEnter 이벤트: 드래그된 데이터가 JSON 파일인 경우 효과를 설정
        private void tbFileList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // DragDrop 이벤트: 드롭된 파일들이 JSON 파일이 아니면 경고 팝업 표시
        private void tbFileList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // 현재 tbFileList에 추가된 파일 목록을 가져옴
                var existingFiles = tbFileList.Lines.Select(line => line.Trim()).ToHashSet();

                // JSON 및 CSV 파일들만 tbFileList에 추가하고, JSON 및 CSV 파일이 아닌 경우 에러 팝업 표시
                bool hasInvalidFiles = false;

                // JSON 및 CSV 파일들만 tbFileList에 추가
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).ToLower() == ".json" || Path.GetExtension(file).ToLower() == ".csv")
                    {
                        // 중복된 파일인지 확인 후 추가
                        if (!existingFiles.Contains(Path.GetFileName(file)))
                        {
                            tbFileList.AppendText(Path.GetFileName(file) + Environment.NewLine);
                            existingFiles.Add(Path.GetFileName(file));  // 이미 추가된 목록에 추가
                        }
                    }
                    else
                    {
                        hasInvalidFiles = true;
                    }
                }

                // JSON 및 CSV 파일 이외의 파일이 드롭된 경우 에러 팝업 표시
                if (hasInvalidFiles)
                {
                    MessageBox.Show("JSON 또는 CSV 파일만 드래그 앤 드롭이 가능합니다.", "잘못된 파일 형식", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            // 엔터키를 눌렀는지 확인
            if (e.KeyCode == Keys.Enter)
            {
                string folderPath = tbFolder.Text;

                // 유효한 디렉토리 경로인지 확인
                if (Directory.Exists(folderPath))
                {
                    LoadJsonFiles(folderPath);
                }
                else
                {
                    MessageBox.Show("유효하지 않은 폴더 경로입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 다른 키 이벤트로 전달되지 않도록 처리
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private async void btnValidate_Click(object sender, EventArgs e)
        {
            string folderPath = tbFolder.Text;

            // 디렉토리가 유효한지 확인
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("유효하지 않은 폴더 경로입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // JSON 파일 목록 확인 (jsonFilePaths에서 가져옴)
            if (jsonFilePaths == null || jsonFilePaths.Count == 0)
            {
                MessageBox.Show("검사할 JSON 파일이 폴더에 없습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 검사 중 팝업 창 표시
            Form popup = new Form
            {
                Text = "검사 중...",
                Size = new System.Drawing.Size(600, 100),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false
            };
            Label label = new Label
            {
                Text = "파일 검사가 진행 중입니다. 잠시만 기다려 주세요...",
                AutoSize = true,
                Location = new System.Drawing.Point(50, 20),
                Parent = popup
            };

            // 팝업창을 비동기적으로 열기
            popup.Show();

            // 비동기 작업을 통해 검사를 실행
            await Task.Run(() =>
            {
                List<string> csvContent = new List<string> { "파일명, 키 없음(개수), 키 없음(내용), null 값 에러(개수),  null 값 에러(내용), 타입 에러(개수), 타입 에러(내용), 유효값 에러(개수), 유효값 에러(내용), 패턴 에러(개수), 패턴 에러(내용), 에러 합계, 총 검사 개수, 파일 정확도(%) " };

                foreach (string file in jsonFilePaths)
                {
                    string content = File.ReadAllText(file);
                    JObject json = JObject.Parse(content);

                    // 각 오류 유형에 대한 키를 기록할 리스트
                    List<string> missingKeysLog = new List<string>();
                    List<string> nullErrorsLog = new List<string>();
                    List<string> typeErrorsLog = new List<string>();
                    List<string> valueErrorsLog = new List<string>();
                    List<string> patternErrorsLog = new List<string>();

                    try
                    {
                        int missingKeysCount = 0;
                        int nullErrors = 0;
                        int typeErrors = 0;
                        int valueErrors = 0;
                        int patternErrors = 0;
                        int totalKeysChecked = 0; // 총 검사한 키의 갯수

                        ValidateJson(file, json, ref missingKeysCount, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);

                        // 중복된 키 검사
                        int duplicatedErrors = CheckForDuplicateKeys(content);

                        // 에러 확률 계산
                        int totalErrors = missingKeysCount + nullErrors + typeErrors + valueErrors + patternErrors; //+ duplicatedErrors;
                        double accuracy = totalKeysChecked > 0 ? ((double)(totalKeysChecked - totalErrors) / totalKeysChecked) * 100 : 0;

                        // duplicateErrors 추후에 사용가능 있음
                        csvContent.Add($"{Path.GetFileName(file)}, {missingKeysCount}, {string.Join(" ", missingKeysLog)}, {nullErrors}, {string.Join(" ", nullErrorsLog)}, {typeErrors}, {string.Join(" ", typeErrorsLog)}, {valueErrors}, {string.Join(" ", valueErrorsLog)}, {patternErrors}, {string.Join(" ", patternErrorsLog)}, {totalErrors}, {totalKeysChecked}, {accuracy:F2}");

                    }
                    catch (JsonReaderException ex)
                    {
                        csvContent.Add($"{Path.GetFileName(file)}, ParsingError, 1, 0, 0");
                    }
                }

                // CSV 파일 저장
                bool isSaved = SaveResultsToCsv(folderPath, csvContent);
                if (isSaved)
                {
                    MessageBox.Show("모든 파일의 검사 결과가 저장되었습니다.", "검사 완료");
                }
            });

            // 검사 완료 후 팝업 창 닫기
            popup.Close();
        }


        // 데이터 검증에 사용할 설정
        public class ValidationConfig
        {
            public Dictionary<string, List<string>> ValidValues { get; set; } = new Dictionary<string, List<string>>();
            public Dictionary<string, string> Patterns { get; set; } = new Dictionary<string, string>();
            public string[] RequiredKeys { get; set; } = new string[] { };
        }

        // 법령 검증 설정
        private ValidationConfig lawConfig = new ValidationConfig
        {
            ValidValues = new Dictionary<string, List<string>>
            {
                {"lawClass", new List<string> { "01", "02" }},    // 법분야
                {"DocuType", new List<string> { "01", "02", "03", "04" }},   // 문서종류
                {"fullText", new List<string> { "Y", "N" }},  // 법원종류코드
                {"sentenceType", new List<string> { "서술형", "나열형" }}     // 문장형태
            },
            Patterns = new Dictionary<string, string>
            {
                {"lawId", @"^\d+$"},    // 법령ID
                // 초기데이터 검사
                // {"promulgDate", @"^\d+$"},  // 공포일자
                // 중간데이터 검사
                {"promulgDate", @"^\d{4}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])$"},  // 공포일자
                // 초기데이터 검사
                // {"effectDate", @"^\d+$"},   // 시행일자
                // 중간데이터 검사
                {"effectDate", @"^\d{4}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])$"},   // 시행일자
                {"promulgNum", @"^\d+$"}    // 공포번호
            },
            RequiredKeys = new string[] { "title", "ministry", "smClass" }


        };

        // 판결문 검증 설정
        private ValidationConfig judgeConfig = new ValidationConfig
        {
            ValidValues = new Dictionary<string, List<string>>
            {
                {"lawClass", new List<string> { "01", "02" }},    // 법분야
                {"DocuType", new List<string> { "01", "02", "03", "04" }},   // 문서종류
                {"courtCode", new List<string> { "400201", "400202"}},  // 법원종류코드
                {"fullText", new List<string> { "Y", "N" }},    // 전문여부
                {"sentenceType", new List<string> { "서술형", "나열형" }}     // 문장형태
            },
            Patterns = new Dictionary<string, string>
            {
                {"precedId", @"^\d+$"},     // 판결문ID
                // 초기데이터 검사
                // {"caseNum", @"^\d{2}[가-힣]\d{4}$"},      // 사건번호
                // 중간데이터 검사
                {"caseNum", @"^\d{2,4}[가-힣]{1,5}\d{1,6}$"},      // 사건번호
                // 초기데이터 검사
                // {"sentenceDate", @"^\d{4}\.\d{2}\.\d{2}$"}    // 선고일자
                // 중간데이터 검사
                {"sentenceDate", @"^\d{4}\.(0[1-9]|1[0-2])\.(0[1-9]|[12]\d|3[01])$"}    // 선고일자
            },
            RequiredKeys = new string[] { "caseName", "courtName", "caseTypeName", "smClass" }
        };

        // 결정례 검증 설정
        private ValidationConfig decisionConfig = new ValidationConfig
        {
            ValidValues = new Dictionary<string, List<string>>
            {
                {"lawClass", new List<string> { "01", "02" }},    // 법분야
                {"DocuType", new List<string> { "01", "02", "03", "04" }},   // 문서종류
                {"courtCode", new List<string> { "430201", "430202"}},  // 재판부구분코드
                {"fullText", new List<string> { "Y", "N" }},    // 전문여부
                {"sentenceType", new List<string> { "서술형", "나열형" }}     // 문장형태
            },
            Patterns = new Dictionary<string, string>
            {
                {"determintId", @"^\d+$"},     // 결정례ID
                // 초기데이터 검사
                // {"finalDate", @"^\d{4}\.\d{2}\.\d{2}$"},      // 종국일자
                // 중간데이터 검사
                {"finalDate", @"^\d{4}\.(0[1-9]|1[0-2])\.(0[1-9]|[12]\d|3[01])$"},      // 종국일자
                // 초기데이터 검사
                // {"caseNum", @"^\d{4}[가-힣]{2}\d{4}$"}    // 사건번호
                // 중간데이터 검사
                {"caseNum", @"^\d{2,4}[가-힣]{2}\d{1,4}$"}    // 사건번호
            },
            RequiredKeys = new string[] { "caseName", "caseCode", "smClass" }
        };

        // 해석례 검증 설정
        private ValidationConfig interpretationConfig = new ValidationConfig
        {
            ValidValues = new Dictionary<string, List<string>>
            {
                {"lawClass", new List<string> { "01", "02" }},    // 법분야
                {"DocuType", new List<string> { "01", "02", "03", "04" }},   // 문서종류
                {"fullText", new List<string> { "Y", "N" }},    // 전문여부
                {"sentenceType", new List<string> { "서술형", "나열형" }}     // 문장형태
            },
            Patterns = new Dictionary<string, string>
            {
                {"interpreId", @"^\d+$"},     // 해석례ID
                {"agendaNum", @"^\d{2}-\d{4}$"},      // 안건번호
                // 초기데이터 검사
                // {"interpreDate", @"^\d{4}\.\d{2}\.\d{2}$"},    // 해석일자
                // 중간데이터 검사
                {"interpreDate", @"^\d{4}\.(0[1-9]|1[0-2])\.(0[1-9]|[12]\d|3[01])$"},    // 해석일자
                {"interpreMinCode", @"^\d+$"},     // 해석기관코드
                {"questionMinCode", @"^\d+$"}     // 질의기관코드
            },
            RequiredKeys = new string[] { "agenda", "interpreMinName", "questionMinName", "smClass" }
        };

        // QA label 검증 설정
        private ValidationConfig qaLabelConfig = new ValidationConfig
        {
            Patterns = new Dictionary<string, string>
            {
                // 초기데이터 검사로 인한 수정 - originCnt 주석 삭제
                //{"originCnt", @"^\d+$" },    // 원문 문장수
                {"originwordCnt", @"^\d+$"},    // 원천 데이터 어절수
                {"labelwordCnt", @"^\d+$"}     // 라벨링 데이터 어절수
            },
            RequiredKeys = new string[] { "instruction", "input", "output" }
        };

        // 요약 label 검증 설정
        private ValidationConfig summaryLabelConfig = new ValidationConfig
        {
            Patterns = new Dictionary<string, string>
            {
                // 초기데이터 검사로 인한 수정 - originCnt 주석 삭제
                //{"originCnt", @"^\d+$" },    // 원문 문장수
                {"originwordCnt", @"^\d+$"},    // 원천 데이터 어절수
                {"labelwordCnt", @"^\d+$"}     // 라벨링 데이터 어절수
            },
            RequiredKeys = new string[] { "instruction", "output" }
        };

        // 범용 데이터 검증 메소드
        private void ValidateData(JObject data, ValidationConfig config, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int valueErrors, ref int patternErrors, ref int totalKeysChecked,
                                List<string> missingKeysLog, List<string> nullErrorsLog, List<string> typeErrorsLog, List<string> valueErrorsLog, List<string> patternErrorsLog)
        {
            foreach (var key in config.RequiredKeys)
            {
                totalKeysChecked++;
               
                if (data[key] == null)
                {
                    if (key != "smClass" && key != "ministry" && key != "questionMinName" && key != "caseCode" && key != "caseTypeName")
                    {
                        missingKeys++;
                        missingKeysLog.Add(key);  // 오류 로그에 키 추가
                        continue;
                    }
                }
                else
                {
                    totalKeysChecked++;
                    // 키의 값 확인
                    if (string.IsNullOrWhiteSpace(data[key]?.ToString()))
                    {
                        if (key != "smClass" && key != "ministry" && key != "questionMinName" && key != "caseCode" && key != "caseTypeName")
                        {
                            nullErrors++;
                            nullErrorsLog.Add(key);  // 오류 로그에 키 추가

                            totalKeysChecked++;
                            // 키의 타입 확인
                            if (data[key].Type != JTokenType.String)
                            {
                                typeErrors++;
                                typeErrorsLog.Add(key);  // 오류 로그에 키 추가
                                continue;
                            }
                        }
                    }
                    else
                    {
                        totalKeysChecked++;
                        // 키의 타입 확인
                        if (data[key].Type != JTokenType.String)
                        {
                            typeErrors++;
                            typeErrorsLog.Add(key);  // 오류 로그에 키 추가

                        }
                    }
                }
            }

            foreach (var key in config.ValidValues.Keys)
            {
                totalKeysChecked++;
                // 키의 존재 유무 확인
                if (data[key] == null)
                {
                    if (key != "courtCode")
                    {
                        missingKeys++;
                        missingKeysLog.Add(key);  // 오류 로그에 키 추가
                        continue;
                    }
                }
                else
                {
                    totalKeysChecked++;
                    // 키의 값 확인
                    if (string.IsNullOrWhiteSpace(data[key]?.ToString()))
                    {
                        if (key != "courtCode")
                        {
                            nullErrors++;
                            nullErrorsLog.Add(key);  // 오류 로그에 키 추가
                        }
                    }
                    else
                    {
                        totalKeysChecked++;
                        // 키의 타입 확인
                        if (data[key].Type != JTokenType.String)
                        {
                            typeErrors++;
                            typeErrorsLog.Add(key);  // 오류 로그에 키 추가
                            continue;

                        }
                        else
                        {
                            totalKeysChecked++; // 유효성 확인
                            CheckValidValues(data, key, config.ValidValues[key], ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, valueErrorsLog);
                        }
                    }
                }
            }

            foreach (var key in config.Patterns.Keys)
            {
                totalKeysChecked++;
                // 키의 존재 유무 확인
                if (data[key] == null)
                {
                    if (key != "interpreDate" && key != "questionMinCode")
                    {
                        missingKeys++;
                        missingKeysLog.Add(key);  // 오류 로그에 키 추가
                        continue;
                    }
                }
                else
                {
                    totalKeysChecked++;

                    // 키의 값 확인
                    if (string.IsNullOrWhiteSpace(data[key]?.ToString()))
                    {
                        if (key != "interpreDate" && key != "questionMinCode")
                        {
                            nullErrors++;
                            nullErrorsLog.Add(key);  // 오류 로그에 키 추가
                        }
                    }
                    else
                    {
                        totalKeysChecked++;
                        // 키의 타입 확인
                        if (data[key].Type != JTokenType.String)
                        {
                            typeErrors++;
                            typeErrorsLog.Add(key);  // 오류 로그에 키 추가
                            continue;
                        }
                        else
                        {
                            totalKeysChecked++;
                            // 패턴 확인
                            CheckPatternKey(data, key, config.Patterns[key], ref missingKeys, ref nullErrors, ref typeErrors, ref patternErrors, patternErrorsLog);
                        }
                    }
                }
            }
        }

        private void ValidateJson(string file, JObject json, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int valueErrors, ref int patternErrors, ref int totalKeysChecked,
                                    List<string> missingKeysLog, List<string> nullErrorsLog, List<string> typeErrorsLog, List<string> valueErrorsLog, List<string> patternErrorsLog)
        {
            // 전체 경로에서 파일명만 추출
            string fileName = Path.GetFileName(file);

            char dataType = fileName[3];
            string taskType = fileName.Contains("QA") ? "QA" : "SUM";

            // 데이터 유형에 따른 검증 설정
            switch (dataType)
            {
                case 'B':
                    ValidateLaw(json, taskType, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                    break;
                case 'P':
                    ValidateJudgment(json, taskType, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                    break;
                case 'K':
                    ValidateDecision(json, taskType, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                    break;
                case 'H':
                    ValidateInterpretation(json, taskType, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                    break;
                default:
                    break;
            }

        }

        // 데이터 유형이 법령인 경우의 검증
        private void ValidateLaw(JObject json, string taskType, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int valueErrors, ref int patternErrors, ref int totalKeysChecked,
                                List<string> missingKeysLog, List<string> nullErrorsLog, List<string> typeErrorsLog, List<string> valueErrorsLog, List<string> patternErrorsLog)
        {
            // "info" 키 검증
            if (json["info"] is JObject info)
            {
                ValidateData(info, lawConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
            }
            else
            {
                missingKeys++;
            }

            // "label" 키 검증
            if (json["label"] is JObject label)
            {
                if (taskType == "QA")
                {
                    ValidateData(label, qaLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
                else if (taskType == "SUM")
                {
                    ValidateData(label, summaryLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
            }
        }

        // 데이터 유형이 판결문인 경우의 검증
        private void ValidateJudgment(JObject json, string taskType, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int valueErrors, ref int patternErrors, ref int totalKeysChecked
                                   , List<string> missingKeysLog, List<string> nullErrorsLog, List<string> typeErrorsLog, List<string> valueErrorsLog, List<string> patternErrorsLog)
        {
            // "info" 키 검증
            if (json["info"] is JObject info)
            {
                ValidateData(info, judgeConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
            }
            else
            {
                missingKeys++;
            }

            // "label" 키 검증
            if (json["label"] is JObject label)
            {
                if (taskType == "QA")
                {
                    ValidateData(label, qaLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
                else if (taskType == "SUM")
                {
                    ValidateData(label, summaryLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
            }
        }

        // 데이터 유형이 결정례인 경우의 검증
        private void ValidateDecision(JObject json, string taskType, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int valueErrors, ref int patternErrors, ref int totalKeysChecked,
                                    List<string> missingKeysLog, List<string> nullErrorsLog, List<string> typeErrorsLog, List<string> valueErrorsLog, List<string> patternErrorsLog)
        {
            // "info" 키 검증
            if (json["info"] is JObject info)
            {
                ValidateData(info, decisionConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
            }
            else
            {
                missingKeys++;
            }

            // "label" 키 검증
            if (json["label"] is JObject label)
            {
                if (taskType == "QA")
                {
                    ValidateData(label, qaLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
                else if (taskType == "SUM")
                {
                    ValidateData(label, summaryLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
            }
        }

        // 데이터 유형이 해석례인 경우의 검증
        private void ValidateInterpretation(JObject json, string taskType, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int valueErrors, ref int patternErrors, ref int totalKeysChecked
                                        , List<string> missingKeysLog, List<string> nullErrorsLog, List<string> typeErrorsLog, List<string> valueErrorsLog, List<string> patternErrorsLog)
        {
            // "info" 키 검증
            if (json["info"] is JObject info)
            {
                ValidateData(info, interpretationConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
            }
            else
            {
                missingKeys++;
            }

            // "label" 키 검증
            if (json["label"] is JObject label)
            {
                if (taskType == "QA")
                {
                    ValidateData(label, qaLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
                else if (taskType == "SUM")
                {
                    ValidateData(label, summaryLabelConfig, ref missingKeys, ref nullErrors, ref typeErrors, ref valueErrors, ref patternErrors, ref totalKeysChecked, missingKeysLog, nullErrorsLog, typeErrorsLog, valueErrorsLog, patternErrorsLog);
                }
            }
        }

        // 패턴 검사 메소드
        private void CheckPatternKey(JObject json, string key, string pattern, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int patternErrors, List<string> patternErrorsLog)
        {
            if (!Regex.IsMatch(json[key].ToString(), pattern))
            {
                patternErrors++;
                patternErrorsLog.Add(key);
            }
        }

        // 유효값 검사 메소드
        private void CheckValidValues(JObject json, string key, List<string> validValues, ref int missingKeys, ref int nullErrors, ref int typeErrors, ref int valueErrors, List<string> valueErrorsLog)
        {
            if (!validValues.Contains(json[key].ToString()))
            {
                valueErrors++;
                valueErrorsLog.Add(key);
            }
        }

        // 중복된 키를 검사하는 메소드
        private int CheckForDuplicateKeys(string jsonContent)
        {
            var keyOccurrences = new Dictionary<string, int>();
            var jsonLines = jsonContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); ;
            int duplicateKeyCount = 0;

            foreach (var line in jsonLines)
            {
                var match = Regex.Match(line.Trim(), "\"(.*?)\":");  // 키에 해당하는 패턴을 찾음
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    if (keyOccurrences.ContainsKey(key))
                    {
                        keyOccurrences[key]++;
                        duplicateKeyCount++;
                    }
                    else
                    {
                        keyOccurrences[key] = 1;
                    }
                }
            }
            return duplicateKeyCount;   // 중복된 키의 개수를 반환
        }       

        private bool SaveResultsToCsv(string folderPath, List<string> csvContent)
        {
            // CSV 파일 저장할 디렉토리 설정
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string csvFilePath = Path.Combine(appDirectory, "ValidationResults.csv");

            // 파일이 이미 존재하는 경우 사용자에게 새 파일 이름을 요청
            while (File.Exists(csvFilePath))
            {
                string newFileName = Microsoft.VisualBasic.Interaction.InputBox("파일 이름이 이미 존재합니다. 새 파일 이름을 입력하세요 (확장자 없이):", "파일 이름 충돌", "ValidationResults");
                if (string.IsNullOrWhiteSpace(newFileName))
                {
                    MessageBox.Show("파일 이름을 제공하지 않았습니다. 파일 저장을 취소합니다.");
                    return false; // 적절한 파일 이름이 제공되지 않으면 함수를 종료
                }
                csvFilePath = Path.Combine(appDirectory, $"{newFileName}.csv");
            }

            try
            {

                File.WriteAllLines(csvFilePath, csvContent, Encoding.UTF8);
                MessageBox.Show($"파일이 성공적으로 저장되었습니다: {csvFilePath}", "파일 저장 완료");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일 저장 중 오류가 발생했습니다: {ex.Message}", "저장 오류");
                return false;
            }
        }
    }
}
