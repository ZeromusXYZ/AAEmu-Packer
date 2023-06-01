namespace AAPakEditor.Forms
{
    partial class PreviewForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewForm));
            this.tPreview = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tcViewer = new System.Windows.Forms.TabControl();
            this.tpBasicText = new System.Windows.Forms.TabPage();
            this.documentMap1 = new FastColoredTextBoxNS.DocumentMap();
            this.tpImage = new System.Windows.Forms.TabPage();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tPreview)).BeginInit();
            this.tcViewer.SuspendLayout();
            this.tpBasicText.SuspendLayout();
            this.tpImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // tPreview
            // 
            this.tPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tPreview.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.tPreview.AutoIndentCharsPatterns = "";
            this.tPreview.AutoScrollMinSize = new System.Drawing.Size(59, 14);
            this.tPreview.BackBrush = null;
            this.tPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tPreview.CharHeight = 14;
            this.tPreview.CharWidth = 8;
            this.tPreview.CommentPrefix = null;
            this.tPreview.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tPreview.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tPreview.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.tPreview.IsReplaceMode = false;
            this.tPreview.Language = FastColoredTextBoxNS.Language.XML;
            this.tPreview.LeftBracket = '<';
            this.tPreview.LeftBracket2 = '(';
            this.tPreview.Location = new System.Drawing.Point(0, 0);
            this.tPreview.Name = "tPreview";
            this.tPreview.Paddings = new System.Windows.Forms.Padding(0);
            this.tPreview.ReadOnly = true;
            this.tPreview.RightBracket = '>';
            this.tPreview.RightBracket2 = ')';
            this.tPreview.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tPreview.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tPreview.ServiceColors")));
            this.tPreview.ShowFoldingLines = true;
            this.tPreview.Size = new System.Drawing.Size(486, 428);
            this.tPreview.TabIndex = 0;
            this.tPreview.Text = "Info";
            this.tPreview.Zoom = 100;
            // 
            // tcViewer
            // 
            this.tcViewer.Controls.Add(this.tpBasicText);
            this.tcViewer.Controls.Add(this.tpImage);
            this.tcViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcViewer.Location = new System.Drawing.Point(0, 0);
            this.tcViewer.Name = "tcViewer";
            this.tcViewer.SelectedIndex = 0;
            this.tcViewer.Size = new System.Drawing.Size(535, 460);
            this.tcViewer.TabIndex = 1;
            // 
            // tpBasicText
            // 
            this.tpBasicText.Controls.Add(this.documentMap1);
            this.tpBasicText.Controls.Add(this.tPreview);
            this.tpBasicText.Location = new System.Drawing.Point(4, 22);
            this.tpBasicText.Name = "tpBasicText";
            this.tpBasicText.Padding = new System.Windows.Forms.Padding(3);
            this.tpBasicText.Size = new System.Drawing.Size(527, 434);
            this.tpBasicText.TabIndex = 0;
            this.tpBasicText.Text = "Text";
            this.tpBasicText.UseVisualStyleBackColor = true;
            // 
            // documentMap1
            // 
            this.documentMap1.Dock = System.Windows.Forms.DockStyle.Right;
            this.documentMap1.ForeColor = System.Drawing.Color.Maroon;
            this.documentMap1.Location = new System.Drawing.Point(492, 3);
            this.documentMap1.Name = "documentMap1";
            this.documentMap1.Size = new System.Drawing.Size(32, 428);
            this.documentMap1.TabIndex = 1;
            this.documentMap1.Target = this.tPreview;
            this.documentMap1.Text = "documentMap1";
            // 
            // tpImage
            // 
            this.tpImage.AutoScroll = true;
            this.tpImage.Controls.Add(this.pbPreview);
            this.tpImage.Location = new System.Drawing.Point(4, 22);
            this.tpImage.Name = "tpImage";
            this.tpImage.Padding = new System.Windows.Forms.Padding(3);
            this.tpImage.Size = new System.Drawing.Size(527, 434);
            this.tpImage.TabIndex = 1;
            this.tpImage.Text = "Image";
            this.tpImage.UseVisualStyleBackColor = true;
            // 
            // pbPreview
            // 
            this.pbPreview.Location = new System.Drawing.Point(0, 0);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(197, 167);
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 460);
            this.Controls.Add(this.tcViewer);
            this.Name = "PreviewForm";
            this.Text = "Preview";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PreviewForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.tPreview)).EndInit();
            this.tcViewer.ResumeLayout(false);
            this.tpBasicText.ResumeLayout(false);
            this.tpImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FastColoredTextBoxNS.DocumentMap documentMap1;
        public FastColoredTextBoxNS.FastColoredTextBox tPreview;
        public System.Windows.Forms.TabControl tcViewer;
        public System.Windows.Forms.TabPage tpBasicText;
        public System.Windows.Forms.TabPage tpImage;
        public System.Windows.Forms.PictureBox pbPreview;
    }
}