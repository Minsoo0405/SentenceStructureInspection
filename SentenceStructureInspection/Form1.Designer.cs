namespace SentenceStructureInspection
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.gbFolder = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.btnFolder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.subTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.tbFileList = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.gbDataType = new System.Windows.Forms.GroupBox();
            this.tablePanel.SuspendLayout();
            this.gbFolder.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.subTablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tablePanel
            // 
            this.tablePanel.ColumnCount = 4;
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29F));
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tablePanel.Controls.Add(this.gbFolder, 1, 1);
            this.tablePanel.Controls.Add(this.groupBox1, 1, 2);
            this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel.Location = new System.Drawing.Point(0, 0);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.RowCount = 5;
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tablePanel.Size = new System.Drawing.Size(782, 593);
            this.tablePanel.TabIndex = 0;
            // 
            // gbFolder
            // 
            this.tablePanel.SetColumnSpan(this.gbFolder, 2);
            this.gbFolder.Controls.Add(this.tableLayoutPanel1);
            this.gbFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFolder.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbFolder.Location = new System.Drawing.Point(26, 32);
            this.gbFolder.Name = "gbFolder";
            this.gbFolder.Size = new System.Drawing.Size(728, 171);
            this.gbFolder.TabIndex = 4;
            this.gbFolder.TabStop = false;
            this.gbFolder.Text = "폴더 디렉토리";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.tbFolder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFolder, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(722, 142);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tbFolder
            // 
            this.tbFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFolder.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbFolder.Location = new System.Drawing.Point(3, 58);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.Size = new System.Drawing.Size(499, 25);
            this.tbFolder.TabIndex = 0;
            this.tbFolder.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDirectory_KeyDown);
            // 
            // btnFolder
            // 
            this.btnFolder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFolder.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFolder.Location = new System.Drawing.Point(563, 51);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(100, 40);
            this.btnFolder.TabIndex = 1;
            this.btnFolder.Text = "폴더 선택";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnSelectJson_Click);
            // 
            // groupBox1
            // 
            this.tablePanel.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.subTablePanel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(26, 209);
            this.groupBox1.Name = "groupBox1";
            this.tablePanel.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(728, 348);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // subTablePanel
            // 
            this.subTablePanel.ColumnCount = 2;
            this.subTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.subTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.subTablePanel.Controls.Add(this.tbFileList, 0, 0);
            this.subTablePanel.Controls.Add(this.button1, 1, 1);
            this.subTablePanel.Controls.Add(this.gbDataType, 1, 0);
            this.subTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subTablePanel.Location = new System.Drawing.Point(3, 21);
            this.subTablePanel.Name = "subTablePanel";
            this.subTablePanel.RowCount = 2;
            this.subTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.subTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.subTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.subTablePanel.Size = new System.Drawing.Size(722, 324);
            this.subTablePanel.TabIndex = 0;
            // 
            // tbFileList
            // 
            this.tbFileList.AllowDrop = true;
            this.tbFileList.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFileList.Location = new System.Drawing.Point(3, 3);
            this.tbFileList.Multiline = true;
            this.tbFileList.Name = "tbFileList";
            this.tbFileList.ReadOnly = true;
            this.subTablePanel.SetRowSpan(this.tbFileList, 2);
            this.tbFileList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbFileList.Size = new System.Drawing.Size(499, 318);
            this.tbFileList.TabIndex = 0;
            this.tbFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragDrop);
            this.tbFileList.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragEnter);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(619, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "검사 시작";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // gbDataType
            // 
            this.gbDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDataType.Location = new System.Drawing.Point(508, 3);
            this.gbDataType.Name = "gbDataType";
            this.gbDataType.Size = new System.Drawing.Size(211, 156);
            this.gbDataType.TabIndex = 2;
            this.gbDataType.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 593);
            this.Controls.Add(this.tablePanel);
            this.Name = "Form1";
            this.Text = "구문 규칙";
            this.tablePanel.ResumeLayout(false);
            this.gbFolder.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.subTablePanel.ResumeLayout(false);
            this.subTablePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private System.Windows.Forms.GroupBox gbFolder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel subTablePanel;
        private System.Windows.Forms.TextBox tbFileList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox gbDataType;
    }
}

