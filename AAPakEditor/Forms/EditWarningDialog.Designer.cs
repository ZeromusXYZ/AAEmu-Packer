namespace AAPakEditor.Forms
{
    partial class EditWarningDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditWarningDialog));
            this.tWarning = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.cbSkipWarning = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbOpenReadOnlyAsDefault = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tWarning
            // 
            this.tWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tWarning.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tWarning.Location = new System.Drawing.Point(74, 12);
            this.tWarning.Multiline = true;
            this.tWarning.Name = "tWarning";
            this.tWarning.ReadOnly = true;
            this.tWarning.Size = new System.Drawing.Size(348, 308);
            this.tWarning.TabIndex = 2;
            this.tWarning.TabStop = false;
            this.tWarning.Text = resources.GetString("tWarning.Text");
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(12, 346);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // cbSkipWarning
            // 
            this.cbSkipWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSkipWarning.AutoSize = true;
            this.cbSkipWarning.Location = new System.Drawing.Point(248, 350);
            this.cbSkipWarning.Name = "cbSkipWarning";
            this.cbSkipWarning.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbSkipWarning.Size = new System.Drawing.Size(174, 17);
            this.cbSkipWarning.TabIndex = 1;
            this.cbSkipWarning.Text = "Do not show this warning again";
            this.cbSkipWarning.UseVisualStyleBackColor = true;
            this.cbSkipWarning.CheckedChanged += new System.EventHandler(this.cbSkipWarning_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(56, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // cbOpenReadOnlyAsDefault
            // 
            this.cbOpenReadOnlyAsDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOpenReadOnlyAsDefault.AutoSize = true;
            this.cbOpenReadOnlyAsDefault.Location = new System.Drawing.Point(248, 326);
            this.cbOpenReadOnlyAsDefault.Name = "cbOpenReadOnlyAsDefault";
            this.cbOpenReadOnlyAsDefault.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbOpenReadOnlyAsDefault.Size = new System.Drawing.Size(174, 17);
            this.cbOpenReadOnlyAsDefault.TabIndex = 4;
            this.cbOpenReadOnlyAsDefault.Text = "Open as Read/Write by default";
            this.cbOpenReadOnlyAsDefault.UseVisualStyleBackColor = true;
            this.cbOpenReadOnlyAsDefault.CheckedChanged += new System.EventHandler(this.cbOpenReadOnlyAsDefault_CheckedChanged);
            // 
            // EditWarningDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 381);
            this.Controls.Add(this.cbOpenReadOnlyAsDefault);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cbSkipWarning);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tWarning);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditWarningDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Warning";
            this.Load += new System.EventHandler(this.EditWarningDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tWarning;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox cbSkipWarning;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox cbOpenReadOnlyAsDefault;
    }
}