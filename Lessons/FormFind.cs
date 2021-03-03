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
        public static class TextWork
        {
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
                // Если текст изначально не содержит результатов поиска - обнуляем findCutLength, выводим сообщение
                else
                {
                    findCutLength = 0;
                    MessageBox.Show("По вашему запросу ничего не нашлось.", "Совпадений не найдено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                return 0;
            }

            // Метод "Заменить"
            public static int ReplaceTextBox(ref RichTextBox textBox, string findText, string replaceText, ref int findCutLength)
            {


                if (textBox.Text.ToLower().Contains(findText.ToLower()))
                {
                    if (textBox.SelectedText == "" || textBox.SelectedText.ToLower() != findText.ToLower())
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
                            return ReplaceTextBox(ref textBox, findText, replaceText, ref findCutLength);
                        }
                    }
                    else if (textBox.SelectedText.ToLower() == findText.ToLower())
                    {
                        textBox.SelectedText = replaceText;
                    }
                }
                else
                {
                    findCutLength = 0;
                    MessageBox.Show("По вашему запросу ничего не нашлось.", "Совпадений не найдено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return 0;
            }

            // Метод "Заменить всё"
            public static int ReplaceAllTextBox(ref RichTextBox textBox, string findText, string replaceText)
            {


                string text = textBox.Text.ToLower();
                string words = findText.ToLower();
                if (text.Contains(words))
                {
                    int startPosition = text.IndexOf(words);
                    textBox.Select(startPosition, findText.Length);
                    textBox.SelectedText = replaceText;
                    return ReplaceAllTextBox(ref textBox, findText, replaceText);
                }
                else
                {
                    MessageBox.Show("Замены произведены успешно.", "Заменить всё", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return 0;
            }


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
            
            TextWork.FindTextBox(ref richText, textBox1.Text, ref oldIndex);

            /*oldIndex += nextText.IndexOf(nextText);
            nextText = nextText.Remove(textBox1.Text.Length, nextText.IndexOf(nextText));
            if (richText.Text.Contains(textBox1.Text))
            {
                int index = richText.Text.IndexOf(textBox1.Text+oldIndex);
                richText.Select(index, textBox1.Text.Length);
            }
            else
            {
                MessageBox.Show("Не найдено");
            }*/
        }
    }
}
