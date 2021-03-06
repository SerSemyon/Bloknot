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
        Form1 baseForm;
        public SearchForm(Form1 form)
        {
            baseForm = form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int index = richText.Text.IndexOf(textBox1.Text);
                int lengthBegin = richText.Text.Length - textBox1.Text.Length - index;
                string strBegin = richText.Text.Substring(0, index);
                string strEnd = richText.Text.Substring(index + textBox1.Text.Length, lengthBegin);
                baseForm.ChangeText(strBegin + textBox2.Text + strEnd);
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

        private void button3_Click(object sender, EventArgs e)
        {
            richText.Text = richText.Text.Replace(textBox1.Text, textBox2.Text);
        }
    }

}
