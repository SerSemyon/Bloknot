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
        public partial class SearchForm : Form
    {
        public RichTextBox richText;
        public SearchForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = textBox1.Text.IndexOf(richText.Text);
            try
            {
                string strBegin = richText.Text.Substring(0, index);
                string strEnd = richText.Text.Substring(index + textBox1.Text.Length, richText.Text.Length - (index + textBox1.Text.Length));
                richText.Text = strBegin + strEnd;

            }
            catch
            {
                DialogResult message = MessageBox.Show("Не найдено");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
