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
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pbExport
            // 
            this.pbExport.Location = new System.Drawing.Point(12, 12);
            this.pbExport.Name = "pbExport";
            this.pbExport.Size = new System.Drawing.Size(437, 23);
            this.pbExport.TabIndex = 0;
            this.pbExport.UseWaitCursor = true;
            // 
            // lInfo
            // 
            this.lInfo.AutoSize = true;
            this.lInfo.Location = new System.Drawing.Point(12, 51);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(59, 13);
            this.lInfo.TabIndex = 1;
            this.lInfo.Text = "progress ...";
            this.lInfo.UseWaitCursor = true;
            // 
            // bgwExport
            // 
            this.bgwExport.WorkerReportsProgress = true;
            this.bgwExport.WorkerSupportsCancellation = true;
            this.bgwExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwExport_DoWork);
            this.bgwExport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwExport_ProgressChanged);
            this.bgwExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwExport_RunWorkerCompleted);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(374, 51);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.UseWaitCursor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ExportAllDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 85);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.pbExport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ExportAllDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export All Files";
            this.UseWaitCursor = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportAllDlg_FormClosing);
            this.Load += new System.EventHandler(this.ExportAllDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbExport;
        private System.Windows.Forms.Label lInfo;
        private System.ComponentModel.BackgroundWorker bgwExport;
        private System.Windows.Forms.Button btnCancel;
    }
}