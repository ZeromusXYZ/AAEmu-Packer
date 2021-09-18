namespace AAMod
{
    partial class ModMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lInstallLocation = new System.Windows.Forms.Label();
            this.tDescription = new System.Windows.Forms.TextBox();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.gameFolderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.openGamePakDlg = new System.Windows.Forms.OpenFileDialog();
            this.modIcon = new System.Windows.Forms.PictureBox();
            this.lInstallInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.modIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.Enabled = false;
            this.btnInstall.Location = new System.Drawing.Point(12, 54);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(128, 23);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.UseWaitCursor = true;
            this.btnInstall.Click += new System.EventHandler(this.BtnInstall_Click);
            // 
            // btnUninstall
            // 
            this.btnUninstall.Enabled = false;
            this.btnUninstall.Location = new System.Drawing.Point(12, 215);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(128, 23);
            this.btnUninstall.TabIndex = 1;
            this.btnUninstall.Text = "Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.UseWaitCursor = true;
            this.btnUninstall.Click += new System.EventHandler(this.BtnUninstall_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Install Location: ";
            this.label1.UseWaitCursor = true;
            // 
            // lInstallLocation
            // 
            this.lInstallLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lInstallLocation.AutoEllipsis = true;
            this.lInstallLocation.Location = new System.Drawing.Point(102, 9);
            this.lInstallLocation.Name = "lInstallLocation";
            this.lInstallLocation.Size = new System.Drawing.Size(389, 13);
            this.lInstallLocation.TabIndex = 3;
            this.lInstallLocation.Text = "???";
            this.lInstallLocation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lInstallLocation.UseWaitCursor = true;
            // 
            // tDescription
            // 
            this.tDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDescription.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tDescription.Location = new System.Drawing.Point(146, 83);
            this.tDescription.Multiline = true;
            this.tDescription.Name = "tDescription";
            this.tDescription.ReadOnly = true;
            this.tDescription.Size = new System.Drawing.Size(345, 155);
            this.tDescription.TabIndex = 4;
            this.tDescription.UseWaitCursor = true;
            // 
            // pb
            // 
            this.pb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb.Location = new System.Drawing.Point(12, 25);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(479, 23);
            this.pb.TabIndex = 5;
            this.pb.UseWaitCursor = true;
            // 
            // gameFolderDlg
            // 
            this.gameFolderDlg.Description = "Please locate your ArcheAge installation folder";
            this.gameFolderDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.gameFolderDlg.SelectedPath = "C:\\ArcheAge\\Working";
            this.gameFolderDlg.ShowNewFolderButton = false;
            // 
            // openGamePakDlg
            // 
            this.openGamePakDlg.FileName = "game_pak";
            this.openGamePakDlg.Filter = "ArcheAge game pak|game_pak|All Files|*.*";
            this.openGamePakDlg.InitialDirectory = "C:\\ArcheAge\\Working";
            this.openGamePakDlg.RestoreDirectory = true;
            this.openGamePakDlg.Title = "Locate game_pak";
            // 
            // modIcon
            // 
            this.modIcon.Location = new System.Drawing.Point(12, 83);
            this.modIcon.Name = "modIcon";
            this.modIcon.Size = new System.Drawing.Size(128, 128);
            this.modIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.modIcon.TabIndex = 6;
            this.modIcon.TabStop = false;
            this.modIcon.UseWaitCursor = true;
            // 
            // lInstallInfo
            // 
            this.lInstallInfo.AutoSize = true;
            this.lInstallInfo.Location = new System.Drawing.Point(146, 59);
            this.lInstallInfo.Name = "lInstallInfo";
            this.lInstallInfo.Size = new System.Drawing.Size(25, 13);
            this.lInstallInfo.TabIndex = 7;
            this.lInstallInfo.Text = "???";
            this.lInstallInfo.UseWaitCursor = true;
            // 
            // ModMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 244);
            this.Controls.Add(this.lInstallInfo);
            this.Controls.Add(this.modIcon);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.tDescription);
            this.Controls.Add(this.lInstallLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.btnInstall);
            this.KeyPreview = true;
            this.Name = "ModMainForm";
            this.Text = "AAMod";
            this.UseWaitCursor = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ModMainForm_FormClosed);
            this.Load += new System.EventHandler(this.ModMainForm_Load);
            this.Shown += new System.EventHandler(this.ModMainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ModMainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.modIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lInstallLocation;
        private System.Windows.Forms.TextBox tDescription;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.FolderBrowserDialog gameFolderDlg;
        private System.Windows.Forms.OpenFileDialog openGamePakDlg;
        private System.Windows.Forms.PictureBox modIcon;
        private System.Windows.Forms.Label lInstallInfo;
    }
}

