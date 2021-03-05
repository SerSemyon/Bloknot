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
    public partial class FormFind : Form
    {
        RichTextBox richText;
        string lastText;
        int oldIndex =0;
        int index = 0;
        public FormFind(RichTextBox richBox)
        {
            richText = richBox;
            lastText = richText.Text;
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            lastText = richText.Text;
            oldIndex = 0;
        }
        void findText()
        {
            try
            {
                if (oldIndex != 0)
                lastText = richText.Text.Remove(0, index + textBox1.Text.Length);
                index = lastText.IndexOf(textBox1.Text);
                if (index != -1)
                {
                    index += oldIndex;
                    richText.Select(index, textBox1.Text.Length);
                    oldIndex = index + textBox1.Text.Length;
                    this.Owner.Focus();
                }
                else throw new Exception();
            }
            catch
            {
                if (oldIndex == 0)
                    MessageBox.Show("Не найдено");
                else
                {
                    lastText = richText.Text;
                    oldIndex = 0;
                    findText();
                }
                    
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            findText();
        }
    }
}
