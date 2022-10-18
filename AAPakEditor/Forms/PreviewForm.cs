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
    public partial class PreviewForm : Form
    {
        private static PreviewForm _instance;

        public static PreviewForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PreviewForm();
                return _instance;
            }
        }

        public PreviewForm()
        {
            InitializeComponent();
        }

        private void PreviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }

        public static bool IsActive => (_instance != null);
    }
}
