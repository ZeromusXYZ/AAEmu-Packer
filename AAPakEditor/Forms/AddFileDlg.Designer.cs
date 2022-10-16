namespace AAPakEditor.Forms
{
    partial class AddFileDialog
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
            this.lPakFileName = new System.Windows.Forms.Label();
            this.ePakFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.eDiskFileName = new System.Windows.Forms.TextBox();
            this.btnSearchFile = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.gbCreateTime = new System.Windows.Forms.GroupBox();
            this.tCreateAsNumber = new System.Windows.Forms.TextBox();
            this.rbCreateTimeSpecifiedValue = new System.Windows.Forms.RadioButton();
            this.rbCreateTimeSpecifiedTime = new System.Windows.Forms.RadioButton();
            this.dtCreateTime = new System.Windows.Forms.DateTimePicker();
            this.rbCreateTimeUtcNow = new System.Windows.Forms.RadioButton();
            this.rbCreateTimePakCreateTime = new System.Windows.Forms.RadioButton();
            this.cbCreateTimeKeepExisting = new System.Windows.Forms.CheckBox();
            this.rbCreateTimeSourceModifiedTime = new System.Windows.Forms.RadioButton();
            this.rbCreateTimeSourceCreateTime = new System.Windows.Forms.RadioButton();
            this.gbModifyTime = new System.Windows.Forms.GroupBox();
            this.tModifyAsNumber = new System.Windows.Forms.TextBox();
            this.rbModifyTimeSpecifiedValue = new System.Windows.Forms.RadioButton();
            this.rbModifyTimeSpecifiedTime = new System.Windows.Forms.RadioButton();
            this.dtModifyTime = new System.Windows.Forms.DateTimePicker();
            this.rbModifyTimeUtcNow = new System.Windows.Forms.RadioButton();
            this.rbModifyTimePakCreateTime = new System.Windows.Forms.RadioButton();
            this.cbModifyTimeKeepExisting = new System.Windows.Forms.CheckBox();
            this.rbModifyTimeSourceModifiedTime = new System.Windows.Forms.RadioButton();
            this.rbModifyTimeSourceCreateTime = new System.Windows.Forms.RadioButton();
            this.gbMD5 = new System.Windows.Forms.GroupBox();
            this.tHash = new System.Windows.Forms.TextBox();
            this.rbMD5Specified = new System.Windows.Forms.RadioButton();
            this.rbMD5Recalculate = new System.Windows.Forms.RadioButton();
            this.cbMD5KeepExisting = new System.Windows.Forms.CheckBox();
            this.gbDummy1 = new System.Windows.Forms.GroupBox();
            this.tDummy1 = new System.Windows.Forms.TextBox();
            this.rbDummy1Specified = new System.Windows.Forms.RadioButton();
            this.rbDummy1Default = new System.Windows.Forms.RadioButton();
            this.cbDummy1KeepExisting = new System.Windows.Forms.CheckBox();
            this.gbDummy2 = new System.Windows.Forms.GroupBox();
            this.tDummy2 = new System.Windows.Forms.TextBox();
            this.rbDummy2Specified = new System.Windows.Forms.RadioButton();
            this.rbDummy2Default = new System.Windows.Forms.RadioButton();
            this.cbDummy2KeepExisting = new System.Windows.Forms.CheckBox();
            this.cbShowAdvanced = new System.Windows.Forms.CheckBox();
            this.cbReserveSpareSpace = new System.Windows.Forms.CheckBox();
            this.btnSetDefaults = new System.Windows.Forms.Button();
            this.btnRevertSaved = new System.Windows.Forms.Button();
            this.btnSaveDefaults = new System.Windows.Forms.Button();
            this.gbCreateTime.SuspendLayout();
            this.gbModifyTime.SuspendLayout();
            this.gbMD5.SuspendLayout();
            this.gbDummy1.SuspendLayout();
            this.gbDummy2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lPakFileName
            // 
            this.lPakFileName.AutoSize = true;
            this.lPakFileName.Location = new System.Drawing.Point(12, 9);
            this.lPakFileName.Name = "lPakFileName";
            this.lPakFileName.Size = new System.Drawing.Size(120, 13);
            this.lPakFileName.TabIndex = 0;
            this.lPakFileName.Text = "FileName inside the pak";
            // 
            // ePakFileName
            // 
            this.ePakFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ePakFileName.Location = new System.Drawing.Point(15, 25);
            this.ePakFileName.Name = "ePakFileName";
            this.ePakFileName.Size = new System.Drawing.Size(619, 20);
            this.ePakFileName.TabIndex = 1;
            this.ePakFileName.TextChanged += new System.EventHandler(this.ePakFileName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File on disk to be added";
            // 
            // eDiskFileName
            // 
            this.eDiskFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eDiskFileName.Location = new System.Drawing.Point(15, 64);
            this.eDiskFileName.Name = "eDiskFileName";
            this.eDiskFileName.Size = new System.Drawing.Size(577, 20);
            this.eDiskFileName.TabIndex = 3;
            // 
            // btnSearchFile
            // 
            this.btnSearchFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchFile.Location = new System.Drawing.Point(598, 64);
            this.btnSearchFile.Name = "btnSearchFile";
            this.btnSearchFile.Size = new System.Drawing.Size(36, 23);
            this.btnSearchFile.TabIndex = 4;
            this.btnSearchFile.Text = "...";
            this.btnSearchFile.UseVisualStyleBackColor = true;
            this.btnSearchFile.Click += new System.EventHandler(this.btnSearchFile_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(559, 102);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Add";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(478, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDlg
            // 
            this.openFileDlg.Filter = "All Files|*.*";
            this.openFileDlg.Title = "Open file for adding";
            // 
            // gbCreateTime
            // 
            this.gbCreateTime.Controls.Add(this.tCreateAsNumber);
            this.gbCreateTime.Controls.Add(this.rbCreateTimeSpecifiedValue);
            this.gbCreateTime.Controls.Add(this.rbCreateTimeSpecifiedTime);
            this.gbCreateTime.Controls.Add(this.dtCreateTime);
            this.gbCreateTime.Controls.Add(this.rbCreateTimeUtcNow);
            this.gbCreateTime.Controls.Add(this.rbCreateTimePakCreateTime);
            this.gbCreateTime.Controls.Add(this.cbCreateTimeKeepExisting);
            this.gbCreateTime.Controls.Add(this.rbCreateTimeSourceModifiedTime);
            this.gbCreateTime.Controls.Add(this.rbCreateTimeSourceCreateTime);
            this.gbCreateTime.Location = new System.Drawing.Point(15, 163);
            this.gbCreateTime.Name = "gbCreateTime";
            this.gbCreateTime.Size = new System.Drawing.Size(300, 145);
            this.gbCreateTime.TabIndex = 7;
            this.gbCreateTime.TabStop = false;
            this.gbCreateTime.Text = "File Create Time";
            // 
            // tCreateAsNumber
            // 
            this.tCreateAsNumber.Location = new System.Drawing.Point(110, 111);
            this.tCreateAsNumber.Name = "tCreateAsNumber";
            this.tCreateAsNumber.Size = new System.Drawing.Size(180, 20);
            this.tCreateAsNumber.TabIndex = 26;
            this.tCreateAsNumber.Text = "0x0";
            this.tCreateAsNumber.TextChanged += new System.EventHandler(this.tCreateAsNumber_TextChanged);
            // 
            // rbCreateTimeSpecifiedValue
            // 
            this.rbCreateTimeSpecifiedValue.AutoSize = true;
            this.rbCreateTimeSpecifiedValue.Location = new System.Drawing.Point(6, 111);
            this.rbCreateTimeSpecifiedValue.Name = "rbCreateTimeSpecifiedValue";
            this.rbCreateTimeSpecifiedValue.Size = new System.Drawing.Size(98, 17);
            this.rbCreateTimeSpecifiedValue.TabIndex = 14;
            this.rbCreateTimeSpecifiedValue.Text = "Specified value";
            this.rbCreateTimeSpecifiedValue.UseVisualStyleBackColor = true;
            this.rbCreateTimeSpecifiedValue.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbCreateTimeSpecifiedTime
            // 
            this.rbCreateTimeSpecifiedTime.AutoSize = true;
            this.rbCreateTimeSpecifiedTime.Location = new System.Drawing.Point(6, 88);
            this.rbCreateTimeSpecifiedTime.Name = "rbCreateTimeSpecifiedTime";
            this.rbCreateTimeSpecifiedTime.Size = new System.Drawing.Size(91, 17);
            this.rbCreateTimeSpecifiedTime.TabIndex = 13;
            this.rbCreateTimeSpecifiedTime.Text = "Specified time";
            this.rbCreateTimeSpecifiedTime.UseVisualStyleBackColor = true;
            this.rbCreateTimeSpecifiedTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // dtCreateTime
            // 
            this.dtCreateTime.CustomFormat = "yyyy/MM/dd - HH:mm:ss";
            this.dtCreateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtCreateTime.Location = new System.Drawing.Point(109, 88);
            this.dtCreateTime.Name = "dtCreateTime";
            this.dtCreateTime.Size = new System.Drawing.Size(180, 20);
            this.dtCreateTime.TabIndex = 12;
            this.dtCreateTime.ValueChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbCreateTimeUtcNow
            // 
            this.rbCreateTimeUtcNow.AutoSize = true;
            this.rbCreateTimeUtcNow.Location = new System.Drawing.Point(188, 65);
            this.rbCreateTimeUtcNow.Name = "rbCreateTimeUtcNow";
            this.rbCreateTimeUtcNow.Size = new System.Drawing.Size(72, 17);
            this.rbCreateTimeUtcNow.TabIndex = 4;
            this.rbCreateTimeUtcNow.Text = "UTC Now";
            this.rbCreateTimeUtcNow.UseVisualStyleBackColor = true;
            this.rbCreateTimeUtcNow.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbCreateTimePakCreateTime
            // 
            this.rbCreateTimePakCreateTime.AutoSize = true;
            this.rbCreateTimePakCreateTime.Location = new System.Drawing.Point(188, 42);
            this.rbCreateTimePakCreateTime.Name = "rbCreateTimePakCreateTime";
            this.rbCreateTimePakCreateTime.Size = new System.Drawing.Size(101, 17);
            this.rbCreateTimePakCreateTime.TabIndex = 3;
            this.rbCreateTimePakCreateTime.Text = "PAK create time";
            this.rbCreateTimePakCreateTime.UseVisualStyleBackColor = true;
            this.rbCreateTimePakCreateTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // cbCreateTimeKeepExisting
            // 
            this.cbCreateTimeKeepExisting.AutoSize = true;
            this.cbCreateTimeKeepExisting.Checked = true;
            this.cbCreateTimeKeepExisting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCreateTimeKeepExisting.Location = new System.Drawing.Point(6, 19);
            this.cbCreateTimeKeepExisting.Name = "cbCreateTimeKeepExisting";
            this.cbCreateTimeKeepExisting.Size = new System.Drawing.Size(164, 17);
            this.cbCreateTimeKeepExisting.TabIndex = 2;
            this.cbCreateTimeKeepExisting.Text = "Keep existing when replacing";
            this.cbCreateTimeKeepExisting.UseVisualStyleBackColor = true;
            this.cbCreateTimeKeepExisting.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbCreateTimeSourceModifiedTime
            // 
            this.rbCreateTimeSourceModifiedTime.AutoSize = true;
            this.rbCreateTimeSourceModifiedTime.Location = new System.Drawing.Point(6, 65);
            this.rbCreateTimeSourceModifiedTime.Name = "rbCreateTimeSourceModifiedTime";
            this.rbCreateTimeSourceModifiedTime.Size = new System.Drawing.Size(139, 17);
            this.rbCreateTimeSourceModifiedTime.TabIndex = 1;
            this.rbCreateTimeSourceModifiedTime.Text = "Source file modified time";
            this.rbCreateTimeSourceModifiedTime.UseVisualStyleBackColor = true;
            this.rbCreateTimeSourceModifiedTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbCreateTimeSourceCreateTime
            // 
            this.rbCreateTimeSourceCreateTime.AutoSize = true;
            this.rbCreateTimeSourceCreateTime.Checked = true;
            this.rbCreateTimeSourceCreateTime.Location = new System.Drawing.Point(6, 42);
            this.rbCreateTimeSourceCreateTime.Name = "rbCreateTimeSourceCreateTime";
            this.rbCreateTimeSourceCreateTime.Size = new System.Drawing.Size(130, 17);
            this.rbCreateTimeSourceCreateTime.TabIndex = 0;
            this.rbCreateTimeSourceCreateTime.TabStop = true;
            this.rbCreateTimeSourceCreateTime.Text = "Source file create time";
            this.rbCreateTimeSourceCreateTime.UseVisualStyleBackColor = true;
            this.rbCreateTimeSourceCreateTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // gbModifyTime
            // 
            this.gbModifyTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbModifyTime.Controls.Add(this.tModifyAsNumber);
            this.gbModifyTime.Controls.Add(this.rbModifyTimeSpecifiedValue);
            this.gbModifyTime.Controls.Add(this.rbModifyTimeSpecifiedTime);
            this.gbModifyTime.Controls.Add(this.dtModifyTime);
            this.gbModifyTime.Controls.Add(this.rbModifyTimeUtcNow);
            this.gbModifyTime.Controls.Add(this.rbModifyTimePakCreateTime);
            this.gbModifyTime.Controls.Add(this.cbModifyTimeKeepExisting);
            this.gbModifyTime.Controls.Add(this.rbModifyTimeSourceModifiedTime);
            this.gbModifyTime.Controls.Add(this.rbModifyTimeSourceCreateTime);
            this.gbModifyTime.Location = new System.Drawing.Point(332, 163);
            this.gbModifyTime.Name = "gbModifyTime";
            this.gbModifyTime.Size = new System.Drawing.Size(300, 145);
            this.gbModifyTime.TabIndex = 27;
            this.gbModifyTime.TabStop = false;
            this.gbModifyTime.Text = "File Modified Time";
            // 
            // tModifyAsNumber
            // 
            this.tModifyAsNumber.Location = new System.Drawing.Point(110, 111);
            this.tModifyAsNumber.Name = "tModifyAsNumber";
            this.tModifyAsNumber.Size = new System.Drawing.Size(180, 20);
            this.tModifyAsNumber.TabIndex = 26;
            this.tModifyAsNumber.Text = "0x0";
            this.tModifyAsNumber.TextChanged += new System.EventHandler(this.tModifyAsNumber_TextChanged);
            // 
            // rbModifyTimeSpecifiedValue
            // 
            this.rbModifyTimeSpecifiedValue.AutoSize = true;
            this.rbModifyTimeSpecifiedValue.Location = new System.Drawing.Point(6, 111);
            this.rbModifyTimeSpecifiedValue.Name = "rbModifyTimeSpecifiedValue";
            this.rbModifyTimeSpecifiedValue.Size = new System.Drawing.Size(98, 17);
            this.rbModifyTimeSpecifiedValue.TabIndex = 14;
            this.rbModifyTimeSpecifiedValue.Text = "Specified value";
            this.rbModifyTimeSpecifiedValue.UseVisualStyleBackColor = true;
            this.rbModifyTimeSpecifiedValue.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbModifyTimeSpecifiedTime
            // 
            this.rbModifyTimeSpecifiedTime.AutoSize = true;
            this.rbModifyTimeSpecifiedTime.Location = new System.Drawing.Point(6, 88);
            this.rbModifyTimeSpecifiedTime.Name = "rbModifyTimeSpecifiedTime";
            this.rbModifyTimeSpecifiedTime.Size = new System.Drawing.Size(91, 17);
            this.rbModifyTimeSpecifiedTime.TabIndex = 13;
            this.rbModifyTimeSpecifiedTime.Text = "Specified time";
            this.rbModifyTimeSpecifiedTime.UseVisualStyleBackColor = true;
            this.rbModifyTimeSpecifiedTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // dtModifyTime
            // 
            this.dtModifyTime.CustomFormat = "yyyy/MM/dd - HH:mm:ss";
            this.dtModifyTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtModifyTime.Location = new System.Drawing.Point(109, 88);
            this.dtModifyTime.Name = "dtModifyTime";
            this.dtModifyTime.Size = new System.Drawing.Size(180, 20);
            this.dtModifyTime.TabIndex = 12;
            this.dtModifyTime.ValueChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbModifyTimeUtcNow
            // 
            this.rbModifyTimeUtcNow.AutoSize = true;
            this.rbModifyTimeUtcNow.Location = new System.Drawing.Point(188, 65);
            this.rbModifyTimeUtcNow.Name = "rbModifyTimeUtcNow";
            this.rbModifyTimeUtcNow.Size = new System.Drawing.Size(72, 17);
            this.rbModifyTimeUtcNow.TabIndex = 4;
            this.rbModifyTimeUtcNow.Text = "UTC Now";
            this.rbModifyTimeUtcNow.UseVisualStyleBackColor = true;
            this.rbModifyTimeUtcNow.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbModifyTimePakCreateTime
            // 
            this.rbModifyTimePakCreateTime.AutoSize = true;
            this.rbModifyTimePakCreateTime.Location = new System.Drawing.Point(188, 42);
            this.rbModifyTimePakCreateTime.Name = "rbModifyTimePakCreateTime";
            this.rbModifyTimePakCreateTime.Size = new System.Drawing.Size(101, 17);
            this.rbModifyTimePakCreateTime.TabIndex = 3;
            this.rbModifyTimePakCreateTime.Text = "PAK create time";
            this.rbModifyTimePakCreateTime.UseVisualStyleBackColor = true;
            this.rbModifyTimePakCreateTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // cbModifyTimeKeepExisting
            // 
            this.cbModifyTimeKeepExisting.AutoSize = true;
            this.cbModifyTimeKeepExisting.Location = new System.Drawing.Point(6, 19);
            this.cbModifyTimeKeepExisting.Name = "cbModifyTimeKeepExisting";
            this.cbModifyTimeKeepExisting.Size = new System.Drawing.Size(164, 17);
            this.cbModifyTimeKeepExisting.TabIndex = 2;
            this.cbModifyTimeKeepExisting.Text = "Keep existing when replacing";
            this.cbModifyTimeKeepExisting.UseVisualStyleBackColor = true;
            this.cbModifyTimeKeepExisting.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbModifyTimeSourceModifiedTime
            // 
            this.rbModifyTimeSourceModifiedTime.AutoSize = true;
            this.rbModifyTimeSourceModifiedTime.Checked = true;
            this.rbModifyTimeSourceModifiedTime.Location = new System.Drawing.Point(6, 65);
            this.rbModifyTimeSourceModifiedTime.Name = "rbModifyTimeSourceModifiedTime";
            this.rbModifyTimeSourceModifiedTime.Size = new System.Drawing.Size(139, 17);
            this.rbModifyTimeSourceModifiedTime.TabIndex = 1;
            this.rbModifyTimeSourceModifiedTime.TabStop = true;
            this.rbModifyTimeSourceModifiedTime.Text = "Source file modified time";
            this.rbModifyTimeSourceModifiedTime.UseVisualStyleBackColor = true;
            this.rbModifyTimeSourceModifiedTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbModifyTimeSourceCreateTime
            // 
            this.rbModifyTimeSourceCreateTime.AutoSize = true;
            this.rbModifyTimeSourceCreateTime.Location = new System.Drawing.Point(6, 42);
            this.rbModifyTimeSourceCreateTime.Name = "rbModifyTimeSourceCreateTime";
            this.rbModifyTimeSourceCreateTime.Size = new System.Drawing.Size(130, 17);
            this.rbModifyTimeSourceCreateTime.TabIndex = 0;
            this.rbModifyTimeSourceCreateTime.Text = "Source file create time";
            this.rbModifyTimeSourceCreateTime.UseVisualStyleBackColor = true;
            this.rbModifyTimeSourceCreateTime.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // gbMD5
            // 
            this.gbMD5.Controls.Add(this.tHash);
            this.gbMD5.Controls.Add(this.rbMD5Specified);
            this.gbMD5.Controls.Add(this.rbMD5Recalculate);
            this.gbMD5.Controls.Add(this.cbMD5KeepExisting);
            this.gbMD5.Location = new System.Drawing.Point(15, 314);
            this.gbMD5.Name = "gbMD5";
            this.gbMD5.Size = new System.Drawing.Size(617, 74);
            this.gbMD5.TabIndex = 28;
            this.gbMD5.TabStop = false;
            this.gbMD5.Text = "MD5";
            // 
            // tHash
            // 
            this.tHash.Location = new System.Drawing.Point(213, 39);
            this.tHash.Name = "tHash";
            this.tHash.Size = new System.Drawing.Size(393, 20);
            this.tHash.TabIndex = 11;
            this.tHash.TextChanged += new System.EventHandler(this.tHash_TextChanged);
            // 
            // rbMD5Specified
            // 
            this.rbMD5Specified.AutoSize = true;
            this.rbMD5Specified.Location = new System.Drawing.Point(109, 42);
            this.rbMD5Specified.Name = "rbMD5Specified";
            this.rbMD5Specified.Size = new System.Drawing.Size(98, 17);
            this.rbMD5Specified.TabIndex = 5;
            this.rbMD5Specified.Text = "Specified value";
            this.rbMD5Specified.UseVisualStyleBackColor = true;
            this.rbMD5Specified.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbMD5Recalculate
            // 
            this.rbMD5Recalculate.AutoSize = true;
            this.rbMD5Recalculate.Checked = true;
            this.rbMD5Recalculate.Location = new System.Drawing.Point(6, 42);
            this.rbMD5Recalculate.Name = "rbMD5Recalculate";
            this.rbMD5Recalculate.Size = new System.Drawing.Size(82, 17);
            this.rbMD5Recalculate.TabIndex = 4;
            this.rbMD5Recalculate.TabStop = true;
            this.rbMD5Recalculate.Text = "Recalculate";
            this.rbMD5Recalculate.UseVisualStyleBackColor = true;
            this.rbMD5Recalculate.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // cbMD5KeepExisting
            // 
            this.cbMD5KeepExisting.AutoSize = true;
            this.cbMD5KeepExisting.Location = new System.Drawing.Point(6, 19);
            this.cbMD5KeepExisting.Name = "cbMD5KeepExisting";
            this.cbMD5KeepExisting.Size = new System.Drawing.Size(164, 17);
            this.cbMD5KeepExisting.TabIndex = 3;
            this.cbMD5KeepExisting.Text = "Keep existing when replacing";
            this.cbMD5KeepExisting.UseVisualStyleBackColor = true;
            this.cbMD5KeepExisting.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // gbDummy1
            // 
            this.gbDummy1.Controls.Add(this.tDummy1);
            this.gbDummy1.Controls.Add(this.rbDummy1Specified);
            this.gbDummy1.Controls.Add(this.rbDummy1Default);
            this.gbDummy1.Controls.Add(this.cbDummy1KeepExisting);
            this.gbDummy1.Location = new System.Drawing.Point(15, 394);
            this.gbDummy1.Name = "gbDummy1";
            this.gbDummy1.Size = new System.Drawing.Size(300, 96);
            this.gbDummy1.TabIndex = 29;
            this.gbDummy1.TabStop = false;
            this.gbDummy1.Text = "Dummy1";
            // 
            // tDummy1
            // 
            this.tDummy1.Location = new System.Drawing.Point(109, 65);
            this.tDummy1.Name = "tDummy1";
            this.tDummy1.Size = new System.Drawing.Size(180, 20);
            this.tDummy1.TabIndex = 11;
            this.tDummy1.TextChanged += new System.EventHandler(this.tDummy1_TextChanged);
            // 
            // rbDummy1Specified
            // 
            this.rbDummy1Specified.AutoSize = true;
            this.rbDummy1Specified.Location = new System.Drawing.Point(6, 65);
            this.rbDummy1Specified.Name = "rbDummy1Specified";
            this.rbDummy1Specified.Size = new System.Drawing.Size(98, 17);
            this.rbDummy1Specified.TabIndex = 5;
            this.rbDummy1Specified.Text = "Specified value";
            this.rbDummy1Specified.UseVisualStyleBackColor = true;
            this.rbDummy1Specified.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbDummy1Default
            // 
            this.rbDummy1Default.AutoSize = true;
            this.rbDummy1Default.Checked = true;
            this.rbDummy1Default.Location = new System.Drawing.Point(6, 42);
            this.rbDummy1Default.Name = "rbDummy1Default";
            this.rbDummy1Default.Size = new System.Drawing.Size(59, 17);
            this.rbDummy1Default.TabIndex = 4;
            this.rbDummy1Default.TabStop = true;
            this.rbDummy1Default.Text = "Default";
            this.rbDummy1Default.UseVisualStyleBackColor = true;
            this.rbDummy1Default.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // cbDummy1KeepExisting
            // 
            this.cbDummy1KeepExisting.AutoSize = true;
            this.cbDummy1KeepExisting.Checked = true;
            this.cbDummy1KeepExisting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDummy1KeepExisting.Location = new System.Drawing.Point(6, 19);
            this.cbDummy1KeepExisting.Name = "cbDummy1KeepExisting";
            this.cbDummy1KeepExisting.Size = new System.Drawing.Size(164, 17);
            this.cbDummy1KeepExisting.TabIndex = 3;
            this.cbDummy1KeepExisting.Text = "Keep existing when replacing";
            this.cbDummy1KeepExisting.UseVisualStyleBackColor = true;
            this.cbDummy1KeepExisting.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // gbDummy2
            // 
            this.gbDummy2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDummy2.Controls.Add(this.tDummy2);
            this.gbDummy2.Controls.Add(this.rbDummy2Specified);
            this.gbDummy2.Controls.Add(this.rbDummy2Default);
            this.gbDummy2.Controls.Add(this.cbDummy2KeepExisting);
            this.gbDummy2.Location = new System.Drawing.Point(332, 394);
            this.gbDummy2.Name = "gbDummy2";
            this.gbDummy2.Size = new System.Drawing.Size(300, 96);
            this.gbDummy2.TabIndex = 30;
            this.gbDummy2.TabStop = false;
            this.gbDummy2.Text = "Dummy2";
            // 
            // tDummy2
            // 
            this.tDummy2.Location = new System.Drawing.Point(109, 65);
            this.tDummy2.Name = "tDummy2";
            this.tDummy2.Size = new System.Drawing.Size(180, 20);
            this.tDummy2.TabIndex = 11;
            this.tDummy2.TextChanged += new System.EventHandler(this.tDummy2_TextChanged);
            // 
            // rbDummy2Specified
            // 
            this.rbDummy2Specified.AutoSize = true;
            this.rbDummy2Specified.Location = new System.Drawing.Point(6, 65);
            this.rbDummy2Specified.Name = "rbDummy2Specified";
            this.rbDummy2Specified.Size = new System.Drawing.Size(98, 17);
            this.rbDummy2Specified.TabIndex = 5;
            this.rbDummy2Specified.Text = "Specified value";
            this.rbDummy2Specified.UseVisualStyleBackColor = true;
            this.rbDummy2Specified.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // rbDummy2Default
            // 
            this.rbDummy2Default.AutoSize = true;
            this.rbDummy2Default.Checked = true;
            this.rbDummy2Default.Location = new System.Drawing.Point(6, 42);
            this.rbDummy2Default.Name = "rbDummy2Default";
            this.rbDummy2Default.Size = new System.Drawing.Size(59, 17);
            this.rbDummy2Default.TabIndex = 4;
            this.rbDummy2Default.TabStop = true;
            this.rbDummy2Default.Text = "Default";
            this.rbDummy2Default.UseVisualStyleBackColor = true;
            this.rbDummy2Default.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // cbDummy2KeepExisting
            // 
            this.cbDummy2KeepExisting.AutoSize = true;
            this.cbDummy2KeepExisting.Checked = true;
            this.cbDummy2KeepExisting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDummy2KeepExisting.Location = new System.Drawing.Point(6, 19);
            this.cbDummy2KeepExisting.Name = "cbDummy2KeepExisting";
            this.cbDummy2KeepExisting.Size = new System.Drawing.Size(164, 17);
            this.cbDummy2KeepExisting.TabIndex = 3;
            this.cbDummy2KeepExisting.Text = "Keep existing when replacing";
            this.cbDummy2KeepExisting.UseVisualStyleBackColor = true;
            this.cbDummy2KeepExisting.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // cbShowAdvanced
            // 
            this.cbShowAdvanced.AutoSize = true;
            this.cbShowAdvanced.Location = new System.Drawing.Point(15, 108);
            this.cbShowAdvanced.Name = "cbShowAdvanced";
            this.cbShowAdvanced.Size = new System.Drawing.Size(144, 17);
            this.cbShowAdvanced.TabIndex = 31;
            this.cbShowAdvanced.Text = "Show Advanced Options";
            this.cbShowAdvanced.UseVisualStyleBackColor = true;
            this.cbShowAdvanced.CheckedChanged += new System.EventHandler(this.cbShowAdvanced_CheckedChanged);
            // 
            // cbReserveSpareSpace
            // 
            this.cbReserveSpareSpace.AutoSize = true;
            this.cbReserveSpareSpace.Location = new System.Drawing.Point(15, 143);
            this.cbReserveSpareSpace.Name = "cbReserveSpareSpace";
            this.cbReserveSpareSpace.Size = new System.Drawing.Size(191, 17);
            this.cbReserveSpareSpace.TabIndex = 33;
            this.cbReserveSpareSpace.Text = "Reserve spare space when adding";
            this.cbReserveSpareSpace.UseVisualStyleBackColor = true;
            this.cbReserveSpareSpace.CheckedChanged += new System.EventHandler(this.SettingsCheckedChanged);
            // 
            // btnSetDefaults
            // 
            this.btnSetDefaults.Location = new System.Drawing.Point(15, 496);
            this.btnSetDefaults.Name = "btnSetDefaults";
            this.btnSetDefaults.Size = new System.Drawing.Size(170, 23);
            this.btnSetDefaults.TabIndex = 34;
            this.btnSetDefaults.Text = "Reset to program defaults";
            this.btnSetDefaults.UseVisualStyleBackColor = true;
            this.btnSetDefaults.Click += new System.EventHandler(this.btnSetDefaults_Click);
            // 
            // btnRevertSaved
            // 
            this.btnRevertSaved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRevertSaved.Location = new System.Drawing.Point(288, 496);
            this.btnRevertSaved.Name = "btnRevertSaved";
            this.btnRevertSaved.Size = new System.Drawing.Size(170, 23);
            this.btnRevertSaved.TabIndex = 35;
            this.btnRevertSaved.Text = "Revert to saved default settings";
            this.btnRevertSaved.UseVisualStyleBackColor = true;
            this.btnRevertSaved.Click += new System.EventHandler(this.btnRevertSaved_Click);
            // 
            // btnSaveDefaults
            // 
            this.btnSaveDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveDefaults.Location = new System.Drawing.Point(464, 496);
            this.btnSaveDefaults.Name = "btnSaveDefaults";
            this.btnSaveDefaults.Size = new System.Drawing.Size(170, 23);
            this.btnSaveDefaults.TabIndex = 36;
            this.btnSaveDefaults.Text = "Save settings as defaults";
            this.btnSaveDefaults.UseVisualStyleBackColor = true;
            this.btnSaveDefaults.Click += new System.EventHandler(this.btnSaveDefaults_Click);
            // 
            // AddFileDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(644, 528);
            this.Controls.Add(this.btnSaveDefaults);
            this.Controls.Add(this.btnRevertSaved);
            this.Controls.Add(this.btnSetDefaults);
            this.Controls.Add(this.cbReserveSpareSpace);
            this.Controls.Add(this.cbShowAdvanced);
            this.Controls.Add(this.gbDummy2);
            this.Controls.Add(this.gbDummy1);
            this.Controls.Add(this.gbMD5);
            this.Controls.Add(this.gbModifyTime);
            this.Controls.Add(this.gbCreateTime);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSearchFile);
            this.Controls.Add(this.eDiskFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ePakFileName);
            this.Controls.Add(this.lPakFileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AddFileDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add File";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddFileDialog_Load);
            this.gbCreateTime.ResumeLayout(false);
            this.gbCreateTime.PerformLayout();
            this.gbModifyTime.ResumeLayout(false);
            this.gbModifyTime.PerformLayout();
            this.gbMD5.ResumeLayout(false);
            this.gbMD5.PerformLayout();
            this.gbDummy1.ResumeLayout(false);
            this.gbDummy1.PerformLayout();
            this.gbDummy2.ResumeLayout(false);
            this.gbDummy2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lPakFileName;
        public System.Windows.Forms.TextBox ePakFileName;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox eDiskFileName;
        private System.Windows.Forms.Button btnSearchFile;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.GroupBox gbCreateTime;
        private System.Windows.Forms.GroupBox gbModifyTime;
        private System.Windows.Forms.GroupBox gbMD5;
        private System.Windows.Forms.GroupBox gbDummy1;
        private System.Windows.Forms.GroupBox gbDummy2;
        private System.Windows.Forms.CheckBox cbShowAdvanced;
        public System.Windows.Forms.RadioButton rbCreateTimePakCreateTime;
        public System.Windows.Forms.CheckBox cbCreateTimeKeepExisting;
        public System.Windows.Forms.RadioButton rbCreateTimeSourceModifiedTime;
        public System.Windows.Forms.RadioButton rbCreateTimeSourceCreateTime;
        public System.Windows.Forms.RadioButton rbCreateTimeSpecifiedValue;
        public System.Windows.Forms.RadioButton rbCreateTimeSpecifiedTime;
        public System.Windows.Forms.DateTimePicker dtCreateTime;
        public System.Windows.Forms.TextBox tCreateAsNumber;
        public System.Windows.Forms.RadioButton rbCreateTimeUtcNow;
        public System.Windows.Forms.TextBox tModifyAsNumber;
        public System.Windows.Forms.RadioButton rbModifyTimeSpecifiedValue;
        public System.Windows.Forms.RadioButton rbModifyTimeSpecifiedTime;
        public System.Windows.Forms.DateTimePicker dtModifyTime;
        public System.Windows.Forms.RadioButton rbModifyTimeUtcNow;
        public System.Windows.Forms.RadioButton rbModifyTimePakCreateTime;
        public System.Windows.Forms.CheckBox cbModifyTimeKeepExisting;
        public System.Windows.Forms.RadioButton rbModifyTimeSourceModifiedTime;
        public System.Windows.Forms.RadioButton rbModifyTimeSourceCreateTime;
        public System.Windows.Forms.RadioButton rbMD5Specified;
        public System.Windows.Forms.RadioButton rbMD5Recalculate;
        public System.Windows.Forms.CheckBox cbMD5KeepExisting;
        public System.Windows.Forms.TextBox tHash;
        public System.Windows.Forms.TextBox tDummy1;
        public System.Windows.Forms.RadioButton rbDummy1Specified;
        public System.Windows.Forms.RadioButton rbDummy1Default;
        public System.Windows.Forms.CheckBox cbDummy1KeepExisting;
        public System.Windows.Forms.TextBox tDummy2;
        public System.Windows.Forms.RadioButton rbDummy2Specified;
        public System.Windows.Forms.RadioButton rbDummy2Default;
        public System.Windows.Forms.CheckBox cbDummy2KeepExisting;
        public System.Windows.Forms.CheckBox cbReserveSpareSpace;
        private System.Windows.Forms.Button btnSetDefaults;
        private System.Windows.Forms.Button btnRevertSaved;
        private System.Windows.Forms.Button btnSaveDefaults;
    }
}