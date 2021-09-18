namespace AAPakEditor
{
    partial class FilePropForm
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
            this.lfiIndex = new System.Windows.Forms.Label();
            this.lDummy1 = new System.Windows.Forms.Label();
            this.lModifyTime = new System.Windows.Forms.Label();
            this.lStartOffset = new System.Windows.Forms.Label();
            this.lCreateTime = new System.Windows.Forms.Label();
            this.lHash = new System.Windows.Forms.Label();
            this.lSize = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.tName = new System.Windows.Forms.TextBox();
            this.tSize = new System.Windows.Forms.TextBox();
            this.tHash = new System.Windows.Forms.TextBox();
            this.dtCreate = new System.Windows.Forms.DateTimePicker();
            this.dtModified = new System.Windows.Forms.DateTimePicker();
            this.tOffset = new System.Windows.Forms.TextBox();
            this.tDummy1 = new System.Windows.Forms.TextBox();
            this.tDummy2 = new System.Windows.Forms.TextBox();
            this.lDummy2 = new System.Windows.Forms.Label();
            this.tSizeDuplicate = new System.Windows.Forms.TextBox();
            this.lSize2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tPaddingSize = new System.Windows.Forms.TextBox();
            this.lPadding = new System.Windows.Forms.Label();
            this.lWarning = new System.Windows.Forms.Label();
            this.tWarnings = new System.Windows.Forms.TextBox();
            this.tCreateAsNumber = new System.Windows.Forms.TextBox();
            this.tModifyAsNumber = new System.Windows.Forms.TextBox();
            this.lCTtoR = new System.Windows.Forms.Label();
            this.lDTToR = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lfiIndex
            // 
            this.lfiIndex.AutoSize = true;
            this.lfiIndex.Location = new System.Drawing.Point(12, 9);
            this.lfiIndex.Name = "lfiIndex";
            this.lfiIndex.Size = new System.Drawing.Size(33, 13);
            this.lfiIndex.TabIndex = 7;
            this.lfiIndex.Text = "Index";
            // 
            // lDummy1
            // 
            this.lDummy1.AutoSize = true;
            this.lDummy1.Location = new System.Drawing.Point(12, 232);
            this.lDummy1.Name = "lDummy1";
            this.lDummy1.Size = new System.Drawing.Size(48, 13);
            this.lDummy1.TabIndex = 6;
            this.lDummy1.Text = "Dummy1";
            // 
            // lModifyTime
            // 
            this.lModifyTime.AutoSize = true;
            this.lModifyTime.Location = new System.Drawing.Point(12, 170);
            this.lModifyTime.Name = "lModifyTime";
            this.lModifyTime.Size = new System.Drawing.Size(73, 13);
            this.lModifyTime.TabIndex = 5;
            this.lModifyTime.Text = "Modified Time";
            // 
            // lStartOffset
            // 
            this.lStartOffset.AutoSize = true;
            this.lStartOffset.Location = new System.Drawing.Point(12, 193);
            this.lStartOffset.Name = "lStartOffset";
            this.lStartOffset.Size = new System.Drawing.Size(57, 13);
            this.lStartOffset.TabIndex = 4;
            this.lStartOffset.Text = "Pak Offset";
            // 
            // lCreateTime
            // 
            this.lCreateTime.AutoSize = true;
            this.lCreateTime.Location = new System.Drawing.Point(12, 144);
            this.lCreateTime.Name = "lCreateTime";
            this.lCreateTime.Size = new System.Drawing.Size(64, 13);
            this.lCreateTime.TabIndex = 3;
            this.lCreateTime.Text = "Create Time";
            // 
            // lHash
            // 
            this.lHash.AutoSize = true;
            this.lHash.Location = new System.Drawing.Point(12, 115);
            this.lHash.Name = "lHash";
            this.lHash.Size = new System.Drawing.Size(58, 13);
            this.lHash.TabIndex = 2;
            this.lHash.Text = "MD5 Hash";
            // 
            // lSize
            // 
            this.lSize.AutoSize = true;
            this.lSize.Location = new System.Drawing.Point(12, 63);
            this.lSize.Name = "lSize";
            this.lSize.Size = new System.Drawing.Size(27, 13);
            this.lSize.TabIndex = 1;
            this.lSize.Text = "Size";
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Cursor = System.Windows.Forms.Cursors.Default;
            this.lName.Location = new System.Drawing.Point(12, 37);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(49, 13);
            this.lName.TabIndex = 0;
            this.lName.Text = "Filename";
            // 
            // tName
            // 
            this.tName.Location = new System.Drawing.Point(91, 34);
            this.tName.Name = "tName";
            this.tName.Size = new System.Drawing.Size(358, 20);
            this.tName.TabIndex = 8;
            this.tName.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // tSize
            // 
            this.tSize.Location = new System.Drawing.Point(91, 60);
            this.tSize.Name = "tSize";
            this.tSize.Size = new System.Drawing.Size(109, 20);
            this.tSize.TabIndex = 9;
            this.tSize.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // tHash
            // 
            this.tHash.Location = new System.Drawing.Point(91, 112);
            this.tHash.Name = "tHash";
            this.tHash.Size = new System.Drawing.Size(358, 20);
            this.tHash.TabIndex = 10;
            this.tHash.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // dtCreate
            // 
            this.dtCreate.CustomFormat = "yyyy/MM/dd - HH:mm:ss";
            this.dtCreate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtCreate.Location = new System.Drawing.Point(91, 138);
            this.dtCreate.Name = "dtCreate";
            this.dtCreate.Size = new System.Drawing.Size(190, 20);
            this.dtCreate.TabIndex = 11;
            this.dtCreate.ValueChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // dtModified
            // 
            this.dtModified.CustomFormat = "yyyy/MM/dd - HH:mm:ss";
            this.dtModified.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtModified.Location = new System.Drawing.Point(91, 164);
            this.dtModified.Name = "dtModified";
            this.dtModified.Size = new System.Drawing.Size(190, 20);
            this.dtModified.TabIndex = 12;
            this.dtModified.ValueChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // tOffset
            // 
            this.tOffset.Location = new System.Drawing.Point(91, 190);
            this.tOffset.Name = "tOffset";
            this.tOffset.Size = new System.Drawing.Size(109, 20);
            this.tOffset.TabIndex = 13;
            this.tOffset.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // tDummy1
            // 
            this.tDummy1.Location = new System.Drawing.Point(91, 229);
            this.tDummy1.Name = "tDummy1";
            this.tDummy1.Size = new System.Drawing.Size(109, 20);
            this.tDummy1.TabIndex = 14;
            this.tDummy1.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // tDummy2
            // 
            this.tDummy2.Location = new System.Drawing.Point(287, 229);
            this.tDummy2.Name = "tDummy2";
            this.tDummy2.Size = new System.Drawing.Size(109, 20);
            this.tDummy2.TabIndex = 15;
            this.tDummy2.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // lDummy2
            // 
            this.lDummy2.AutoSize = true;
            this.lDummy2.Location = new System.Drawing.Point(233, 232);
            this.lDummy2.Name = "lDummy2";
            this.lDummy2.Size = new System.Drawing.Size(48, 13);
            this.lDummy2.TabIndex = 16;
            this.lDummy2.Text = "Dummy2";
            // 
            // tSizeDuplicate
            // 
            this.tSizeDuplicate.Location = new System.Drawing.Point(287, 60);
            this.tSizeDuplicate.Name = "tSizeDuplicate";
            this.tSizeDuplicate.Size = new System.Drawing.Size(109, 20);
            this.tSizeDuplicate.TabIndex = 18;
            this.tSizeDuplicate.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // lSize2
            // 
            this.lSize2.AutoSize = true;
            this.lSize2.Location = new System.Drawing.Point(206, 63);
            this.lSize2.Name = "lSize2";
            this.lSize2.Size = new System.Drawing.Size(75, 13);
            this.lSize2.TabIndex = 17;
            this.lSize2.Text = "Size Duplicate";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(15, 353);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Update";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(374, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tPaddingSize
            // 
            this.tPaddingSize.Location = new System.Drawing.Point(91, 86);
            this.tPaddingSize.Name = "tPaddingSize";
            this.tPaddingSize.Size = new System.Drawing.Size(109, 20);
            this.tPaddingSize.TabIndex = 22;
            this.tPaddingSize.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // lPadding
            // 
            this.lPadding.AutoSize = true;
            this.lPadding.Location = new System.Drawing.Point(12, 89);
            this.lPadding.Name = "lPadding";
            this.lPadding.Size = new System.Drawing.Size(69, 13);
            this.lPadding.TabIndex = 21;
            this.lPadding.Text = "Padding Size";
            // 
            // lWarning
            // 
            this.lWarning.AutoSize = true;
            this.lWarning.Location = new System.Drawing.Point(12, 270);
            this.lWarning.Name = "lWarning";
            this.lWarning.Size = new System.Drawing.Size(55, 13);
            this.lWarning.TabIndex = 23;
            this.lWarning.Text = "Warnings:";
            // 
            // tWarnings
            // 
            this.tWarnings.BackColor = System.Drawing.SystemColors.Control;
            this.tWarnings.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tWarnings.Location = new System.Drawing.Point(15, 290);
            this.tWarnings.Multiline = true;
            this.tWarnings.Name = "tWarnings";
            this.tWarnings.ReadOnly = true;
            this.tWarnings.Size = new System.Drawing.Size(433, 57);
            this.tWarnings.TabIndex = 24;
            // 
            // tCreateAsNumber
            // 
            this.tCreateAsNumber.Location = new System.Drawing.Point(322, 138);
            this.tCreateAsNumber.Name = "tCreateAsNumber";
            this.tCreateAsNumber.Size = new System.Drawing.Size(126, 20);
            this.tCreateAsNumber.TabIndex = 25;
            this.tCreateAsNumber.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // tModifyAsNumber
            // 
            this.tModifyAsNumber.Location = new System.Drawing.Point(322, 164);
            this.tModifyAsNumber.Name = "tModifyAsNumber";
            this.tModifyAsNumber.Size = new System.Drawing.Size(126, 20);
            this.tModifyAsNumber.TabIndex = 26;
            this.tModifyAsNumber.TextChanged += new System.EventHandler(this.tFieldsChanged);
            // 
            // lCTtoR
            // 
            this.lCTtoR.AutoSize = true;
            this.lCTtoR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lCTtoR.Location = new System.Drawing.Point(297, 138);
            this.lCTtoR.Name = "lCTtoR";
            this.lCTtoR.Size = new System.Drawing.Size(19, 13);
            this.lCTtoR.TabIndex = 27;
            this.lCTtoR.Text = "=>";
            this.lCTtoR.Click += new System.EventHandler(this.lCTtoR_Click);
            // 
            // lDTToR
            // 
            this.lDTToR.AutoSize = true;
            this.lDTToR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lDTToR.Location = new System.Drawing.Point(297, 167);
            this.lDTToR.Name = "lDTToR";
            this.lDTToR.Size = new System.Drawing.Size(19, 13);
            this.lDTToR.TabIndex = 28;
            this.lDTToR.Text = "=>";
            this.lDTToR.Click += new System.EventHandler(this.lDTToR_Click);
            // 
            // FilePropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 388);
            this.Controls.Add(this.lDTToR);
            this.Controls.Add(this.lCTtoR);
            this.Controls.Add(this.tModifyAsNumber);
            this.Controls.Add(this.tCreateAsNumber);
            this.Controls.Add(this.tWarnings);
            this.Controls.Add(this.lWarning);
            this.Controls.Add(this.tPaddingSize);
            this.Controls.Add(this.lPadding);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tSizeDuplicate);
            this.Controls.Add(this.lSize2);
            this.Controls.Add(this.lDummy2);
            this.Controls.Add(this.tDummy2);
            this.Controls.Add(this.tDummy1);
            this.Controls.Add(this.tOffset);
            this.Controls.Add(this.dtModified);
            this.Controls.Add(this.dtCreate);
            this.Controls.Add(this.tHash);
            this.Controls.Add(this.tSize);
            this.Controls.Add(this.tName);
            this.Controls.Add(this.lfiIndex);
            this.Controls.Add(this.lDummy1);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.lModifyTime);
            this.Controls.Add(this.lSize);
            this.Controls.Add(this.lStartOffset);
            this.Controls.Add(this.lHash);
            this.Controls.Add(this.lCreateTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FilePropForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Properties";
            this.Load += new System.EventHandler(this.FilePropForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lfiIndex;
        private System.Windows.Forms.Label lDummy1;
        private System.Windows.Forms.Label lModifyTime;
        private System.Windows.Forms.Label lStartOffset;
        private System.Windows.Forms.Label lCreateTime;
        private System.Windows.Forms.Label lHash;
        private System.Windows.Forms.Label lSize;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.TextBox tName;
        private System.Windows.Forms.TextBox tSize;
        private System.Windows.Forms.TextBox tHash;
        private System.Windows.Forms.DateTimePicker dtCreate;
        private System.Windows.Forms.DateTimePicker dtModified;
        private System.Windows.Forms.TextBox tOffset;
        private System.Windows.Forms.TextBox tDummy1;
        private System.Windows.Forms.TextBox tDummy2;
        private System.Windows.Forms.Label lDummy2;
        private System.Windows.Forms.TextBox tSizeDuplicate;
        private System.Windows.Forms.Label lSize2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tPaddingSize;
        private System.Windows.Forms.Label lPadding;
        private System.Windows.Forms.Label lWarning;
        private System.Windows.Forms.TextBox tWarnings;
        private System.Windows.Forms.TextBox tCreateAsNumber;
        private System.Windows.Forms.TextBox tModifyAsNumber;
        private System.Windows.Forms.Label lCTtoR;
        private System.Windows.Forms.Label lDTToR;
    }
}