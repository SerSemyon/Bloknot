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
        public RichTextBox richText;
        public static int FindTextBox(ref RichTextBox textBox, string findText, ref int findCutLength)
        {
            if (textBox.Text.ToLower().Contains(findText.ToLower()))
            {
                string text = textBox.Text.ToLower();
                string nextText = text.Remove(0, findCutLength);
                int resultPosition = nextText.IndexOf(findText.ToLower());

                if (resultPosition != -1)
                {
                    textBox.Select(resultPosition + findCutLength, findText.Length);
                    textBox.ScrollToCaret();
                    textBox.Focus();
                    findCutLength += findText.Length + resultPosition;
                }
                else if (resultPosition == -1 && findCutLength != 0)
                {
                    findCutLength = 0;
                    return FindTextBox(ref textBox, findText, ref findCutLength);
                }
            }
            else
            {
                findCutLength = 0;
                MessageBox.Show("Совпадений не найдено", "Не найдено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            return 0;
        }
        string nextText = "";
        int oldIndex = 0;
        public FormFind()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            nextText = richText.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            FindTextBox(ref richText, textBox1.Text, ref oldIndex);
        }
    }
}
