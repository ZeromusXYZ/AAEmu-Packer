
namespace AAPakEditor
{
    partial class ReMD5Dlg
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lInfo = new System.Windows.Forms.Label();
            this.pbRehash = new System.Windows.Forms.ProgressBar();
            this.bgwRehash = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(374, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.UseWaitCursor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lInfo
            // 
            this.lInfo.AutoSize = true;
            this.lInfo.Location = new System.Drawing.Point(12, 51);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(59, 13);
            this.lInfo.TabIndex = 4;
            this.lInfo.Text = "progress ...";
            this.lInfo.UseWaitCursor = true;
            // 
            // pbRehash
            // 
            this.pbRehash.Location = new System.Drawing.Point(12, 12);
            this.pbRehash.Name = "pbRehash";
            this.pbRehash.Size = new System.Drawing.Size(437, 23);
            this.pbRehash.TabIndex = 3;
            this.pbRehash.UseWaitCursor = true;
            // 
            // bgwRehash
            // 
            this.bgwRehash.WorkerReportsProgress = true;
            this.bgwRehash.WorkerSupportsCancellation = true;
            this.bgwRehash.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRehash_DoWork);
            this.bgwRehash.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwRehash_ProgressChanged);
            this.bgwRehash.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRehash_RunWorkerCompleted);
            // 
            // ReMD5Dlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 106);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.pbRehash);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReMD5Dlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Recalculate MD5 Hashes";
            this.Load += new System.EventHandler(this.ReMD5Dlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lInfo;
        private System.Windows.Forms.ProgressBar pbRehash;
        private System.ComponentModel.BackgroundWorker bgwRehash;
    }
}