using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lessons
{
    public partial class GoToForm : Form
    {
        public RichTextBox textFind;
        public GoToForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            textFind.SelectionStart = textFind.GetFirstCharIndexFromLine(Convert.ToInt32(NumbLine.Text) - 1);
            textFind.ScrollToCaret();
            this.Close();
        }
    }
}
