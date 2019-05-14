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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModMainForm));
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lInstallLocation = new System.Windows.Forms.Label();
            this.tDescription = new System.Windows.Forms.TextBox();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(12, 41);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(115, 23);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "Install Mod";
            this.btnInstall.UseVisualStyleBackColor = true;
            // 
            // btnUninstall
            // 
            this.btnUninstall.Location = new System.Drawing.Point(12, 70);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(115, 23);
            this.btnUninstall.TabIndex = 1;
            this.btnUninstall.Text = "Uninstall Mod";
            this.btnUninstall.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Install Location: ";
            // 
            // lInstallLocation
            // 
            this.lInstallLocation.AutoSize = true;
            this.lInstallLocation.Location = new System.Drawing.Point(102, 9);
            this.lInstallLocation.Name = "lInstallLocation";
            this.lInstallLocation.Size = new System.Drawing.Size(25, 13);
            this.lInstallLocation.TabIndex = 3;
            this.lInstallLocation.Text = "???";
            // 
            // tDescription
            // 
            this.tDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tDescription.Location = new System.Drawing.Point(133, 43);
            this.tDescription.Multiline = true;
            this.tDescription.Name = "tDescription";
            this.tDescription.Size = new System.Drawing.Size(363, 118);
            this.tDescription.TabIndex = 4;
            // 
            // pb
            // 
            this.pb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb.Location = new System.Drawing.Point(12, 167);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(484, 23);
            this.pb.TabIndex = 5;
            // 
            // ModMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 198);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.tDescription);
            this.Controls.Add(this.lInstallLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.btnInstall);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModMainForm";
            this.Text = "AAMod";
            this.Load += new System.EventHandler(this.ModMainForm_Load);
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
    }
}

