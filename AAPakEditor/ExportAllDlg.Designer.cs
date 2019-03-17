namespace AAPakEditor
{
    partial class ExportAllDlg
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
            this.pbExport = new System.Windows.Forms.ProgressBar();
            this.lInfo = new System.Windows.Forms.Label();
            this.bgwExport = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // pbExport
            // 
            this.pbExport.Location = new System.Drawing.Point(12, 12);
            this.pbExport.Name = "pbExport";
            this.pbExport.Size = new System.Drawing.Size(437, 23);
            this.pbExport.TabIndex = 0;
            // 
            // lInfo
            // 
            this.lInfo.AutoSize = true;
            this.lInfo.Location = new System.Drawing.Point(12, 51);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(59, 13);
            this.lInfo.TabIndex = 1;
            this.lInfo.Text = "progress ...";
            // 
            // bgwExport
            // 
            this.bgwExport.WorkerReportsProgress = true;
            this.bgwExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwExport_DoWork);
            this.bgwExport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwExport_ProgressChanged);
            this.bgwExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwExport_RunWorkerCompleted);
            // 
            // ExportAllDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 92);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.pbExport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ExportAllDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export All Files";
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.ExportAllDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbExport;
        private System.Windows.Forms.Label lInfo;
        private System.ComponentModel.BackgroundWorker bgwExport;
    }
}