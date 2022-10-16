using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAPakEditor.Forms
{
    public partial class EditWarningDialog : Form
    {
        public EditWarningDialog()
        {
            // https://www.flaticon.com/free-icon/warning_3756712?term=warning&page=1&position=31&page=1&position=31&related_id=3756712&origin=tag
            // Warning icon by Andrean Prabowo - Flaticon

            InitializeComponent();
        }

        private void cbSkipWarning_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SkipEditWarning = cbSkipWarning.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbOpenReadOnlyAsDefault_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenDefaultReadOnly = !cbOpenReadOnlyAsDefault.Checked;
            Properties.Settings.Default.Save();
        }

        private void EditWarningDialog_Load(object sender, EventArgs e)
        {
            cbSkipWarning.Checked = Properties.Settings.Default.SkipEditWarning;
            cbOpenReadOnlyAsDefault.Checked = !Properties.Settings.Default.OpenDefaultReadOnly;
        }
    }
}
