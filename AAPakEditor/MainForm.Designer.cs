namespace AAPakEditor
{
    partial class MainForm
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
            this.MM = new System.Windows.Forms.MenuStrip();
            this.MMFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MMFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.openGamePakDialog = new System.Windows.Forms.OpenFileDialog();
            this.MM.SuspendLayout();
            this.SuspendLayout();
            // 
            // MM
            // 
            this.MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFile});
            this.MM.Location = new System.Drawing.Point(0, 0);
            this.MM.Name = "MM";
            this.MM.Size = new System.Drawing.Size(800, 24);
            this.MM.TabIndex = 0;
            this.MM.Text = "menuStrip1";
            // 
            // MMFile
            // 
            this.MMFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MMFileOpen,
            this.MMFileExit});
            this.MMFile.Name = "MMFile";
            this.MMFile.Size = new System.Drawing.Size(37, 20);
            this.MMFile.Text = "&File";
            // 
            // MMFileExit
            // 
            this.MMFileExit.Name = "MMFileExit";
            this.MMFileExit.Size = new System.Drawing.Size(180, 22);
            this.MMFileExit.Text = "E&xit";
            this.MMFileExit.Click += new System.EventHandler(this.MMFileExit_Click);
            // 
            // MMFileOpen
            // 
            this.MMFileOpen.Name = "MMFileOpen";
            this.MMFileOpen.Size = new System.Drawing.Size(180, 22);
            this.MMFileOpen.Text = "&Open";
            this.MMFileOpen.Click += new System.EventHandler(this.MMFileOpen_Click);
            // 
            // openGamePakDialog
            // 
            this.openGamePakDialog.FileName = "game_pak";
            this.openGamePakDialog.Filter = "ArcheAge Game Pak|game_pak|All Files|*.*";
            this.openGamePakDialog.RestoreDirectory = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MM);
            this.MainMenuStrip = this.MM;
            this.Name = "MainForm";
            this.Text = "AAPakEditor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MM.ResumeLayout(false);
            this.MM.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MM;
        private System.Windows.Forms.ToolStripMenuItem MMFile;
        private System.Windows.Forms.ToolStripMenuItem MMFileExit;
        private System.Windows.Forms.ToolStripMenuItem MMFileOpen;
        private System.Windows.Forms.OpenFileDialog openGamePakDialog;
    }
}

