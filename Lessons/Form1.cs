using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Блокнотик.Properties;

namespace Lessons
{
    public partial class Form1 : Form
    {
        string filePath;
        string fileName;
        string pastText;
        string nextText;
        int encodingFile = 0;
        int line;
        int indexInLine;
        bool statusStripEnabled;
        bool haveChangesFile = false;
        int haveChangesText = 0;
        int[] numbVowel;
        bool wasNumbVowel;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string[] args) : this()
        {
            try
            {
                filePath = args[0];
                string[] wholeFilePath = filePath.Split('\\');
                string[] lastPath = wholeFilePath[wholeFilePath.Length - 1].Split('.');
                fileName = lastPath[0];
                if (lastPath[lastPath.Length - 1] != "txt")
                {
                    DialogResult message = MessageBox.Show("Открыть файл ?", "Данный формат файла не поддерживается программой", MessageBoxButtons.YesNoCancel);
                    if (message == DialogResult.No)
                    {
                        filePath = "";
                    }
                    else if (message == DialogResult.Yes)
                    {
                        FileText.Layout += FileText_Layout; //читает файл после загрузки программы
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    FileText.Layout += FileText_Layout; //читает файл после загрузки программы
                }
            }
            catch 
            {
                fileName = "Новый текстовый документ";
                this.Text = fileName + " - Блокнотик";
                toolStripStatusLabel2.Text = "Кодировка по умолчанию";
            }
        }
        void ReadFile()
        {
            this.Text = fileName + " - Блокнотик";
            switch (encodingFile)
            {
                case 0:
                    FileText.Text = System.IO.File.ReadAllText(filePath);
                    toolStripStatusLabel2.Text = "Кодировка по умолчанию";
                    break;
                case 1:
                    FileText.Text = System.IO.File.ReadAllText(filePath, Encoding.Default);
                    toolStripStatusLabel2.Text = "Кодировка ANSI";
                    break;
                case 2:
                    FileText.Text = System.IO.File.ReadAllText(filePath, Encoding.ASCII);
                    toolStripStatusLabel2.Text = "Кодировка ASCII";
                    break;
                case 3:
                    FileText.Text = System.IO.File.ReadAllText(filePath, Encoding.Unicode);
                    toolStripStatusLabel2.Text = "Кодировка Unicode";
                    break;
                case 4:
                    FileText.Text = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
                    toolStripStatusLabel2.Text = "Кодировка UTF-8";
                    break;
            }
            haveChangesFile = false;
            wasNumbVowel = false;
        }
        public void ChangeText(string newText)
        {
            pastText = FileText.Text;
            FileText.Text = newText;
            haveChangesText = 1;
            haveChangesFile = true;
        }

        void SaveFile()
        {
            try
            {
                System.IO.File.WriteAllText(filePath, FileText.Text);
                toolStripStatusLabel2.Text = "Кодировка по умолчанию";
                haveChangesFile = false;
                this.Text = fileName + " - Блокнотик";
            }
            catch
            {
                SaveAs();
            }
        }
        void SaveAs()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            filePath = saveFileDialog1.FileName;
            string[] wholeFilePath = saveFileDialog1.FileName.Split('\\');
            fileName = wholeFilePath[wholeFilePath.Length - 1].Split('.')[0];
            System.IO.File.WriteAllText(filePath, FileText.Text);
            toolStripStatusLabel2.Text = "Кодировка по умолчанию";
            haveChangesFile = false;
            this.Text = fileName + " - Блокнотик";
        }


        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haveChangesFile)
            {
                DialogResult message = MessageBox.Show("Сохранить текущий документ?", "Файл не сохранён", MessageBoxButtons.YesNoCancel);
                if (message == DialogResult.Yes)
                {
                    SaveFile();
                }
            }
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            filePath = openFileDialog1.FileName;
            fileName = openFileDialog1.SafeFileName.Split('.')[0];
            ReadFile();
        }
        void CountVowelInString(ref string line, out int numb)
        {
            numb = 0;
            char[] charLine = line.ToCharArray();
            foreach (char n in charLine)
            {
                switch (n)
                {
                    case 'о':
                    case 'е':
                    case 'а':
                    case 'и':
                    case 'у':
                    case 'я':
                    case 'ы':
                    case 'ю':
                    case 'э':
                    case 'ё':
                    case 'a':
                    case 'u':
                    case 'e':
                    case 'o':
                    case 'i':
                    case 'y':
                    case 'О':
                    case 'Е':
                    case 'А':
                    case 'И':
                    case 'У':
                    case 'Я':
                    case 'Ы':
                    case 'Ю':
                    case 'Э':
                    case 'Ё':
                    case 'A':
                    case 'U':
                    case 'E':
                    case 'O':
                    case 'I':
                    case 'Y':
                        numb++;
                        break;
                }
            }
            if (numb != 0)
                line += " " + numb;
        }
        private void FileText_TextChanged(object sender, EventArgs e)
        {
            haveChangesFile = true;
            haveChangesText = 0;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileText.SelectionStart = 0;
            FileText.SelectionLength = FileText.Text.Length;
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileText.Copy();
        }

        private void PastleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileText.Paste();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileText.Cut();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileText.SelectedText = "";
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haveChangesText == 1)
            {
                nextText = FileText.Text;
                FileText.Text = pastText;
                haveChangesText = -1;
            }
            else
                FileText.Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haveChangesText == -1)
            {
                pastText = FileText.Text;
                FileText.Text = nextText;
                haveChangesText = 1;
            }
            else
                FileText.Redo();
        }

        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument1.Print();
                }
                catch { }
            }
        }

        private void TimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string bufferText = Clipboard.GetText();
                Clipboard.SetText(Convert.ToString(System.DateTime.Now));
                FileText.Paste();
                Clipboard.SetText(bufferText);
            }
            catch
            {
                Clipboard.SetText(Convert.ToString(System.DateTime.Now));
                FileText.Paste();
            }
            
        }

        private void ReadOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileText.ReadOnly)
            {
                FileText.ReadOnly = false;
                this.Text = fileName + " - Блокнотик";
                ReadOnlyToolStripMenuItem.Text = "Запретить редактирование";
            }
            else
            {
                FileText.ReadOnly = true;
                this.Text = fileName + "(только для чтения) - Блокнотик";
                ReadOnlyToolStripMenuItem.Text = "Разрешить редактирование";
            }
        }

        private void FindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFind searchForm = new FormFind(FileText);
            searchForm.Owner = this;
            searchForm.Show();
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForm search = new SearchForm(this);
            search.richText = FileText;
            search.Owner = this;
            search.Show();
        }


        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                FileText.Font = fontDialog1.Font;
                Settings.Default.Font = FileText.Font;
                Settings.Default.Save();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (haveChangesFile == true)
            {
                DialogResult message = MessageBox.Show("Сохранить текущий документ перед выходом?", "Выход из программы", MessageBoxButtons.YesNoCancel);
                if (message == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (message == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            Settings.Default.statusStripEnabled = statusStripEnabled;
            Settings.Default.Save();
        }

        private void GoToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoToForm gotoform = new GoToForm();
            gotoform.Owner = this;
            gotoform.NumbLine.Minimum = 0;
            gotoform.NumbLine.Maximum = FileText.Lines.Count();
            gotoform.textFind = FileText;
            gotoform.ShowDialog();
        }


        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haveChangesFile == true)
            {
                DialogResult message = MessageBox.Show("Сохранить текущий документ перед выходом?", "Файл не сохранён", MessageBoxButtons.YesNoCancel);
                if (message == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (message == DialogResult.No)
                {
                    FileText.Text = "";
                    fileName = "Новый текстовый документ";
                    this.Text = fileName + " - Блокнотик";
                    toolStripStatusLabel2.Text = "Кодировка по умолчанию";
                    haveChangesFile = false;
                }
            }
            else
            {
                fileName = "Новый текстовый документ";
                filePath = "";
                FileText.Text = "";
                this.Text = fileName + " - Блокнотик";
                haveChangesFile = false;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(FileText.Text, FileText.Font, Brushes.Black, 0, 0);
        }

        private void enableStatusStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!statusStripEnabled) {
                tableLayoutPanel1.RowCount += 1;
                statusStrip1.Show();
                statusStripEnabled = true;
            }
            else
            {
                tableLayoutPanel1.RowCount -= 1;
                statusStrip1.Hide();
                statusStripEnabled = false;
            }
        }

        private void FileText_SelectionChanged(object sender, EventArgs e)
        {
            line = 1 + Convert.ToInt32(FileText.GetLineFromCharIndex(FileText.SelectionStart));
            indexInLine = 1 + Convert.ToInt32(FileText.SelectionStart) - Convert.ToInt32(FileText.GetFirstCharIndexFromLine(line - 1));
            toolStripStatusLabel1.Text = "Строка " + line + " Столбец " + indexInLine;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileText.Font = Settings.Default.Font;
            statusStripEnabled = Settings.Default.statusStripEnabled;
            if (!statusStripEnabled)
            {
                tableLayoutPanel1.RowCount -= 1;
                statusStrip1.Hide();
            }
        }

        void ReadOtherEncoding(int enc)
        {
            try
            {
                if (haveChangesFile)
                {
                    DialogResult mes = MessageBox.Show("При повторном чтении внесённые изменения будут потеряны. Продолжить?", "Файл был изменён", MessageBoxButtons.OKCancel);
                    if (mes == DialogResult.OK)
                    {
                        encodingFile = enc;
                        ReadFile();
                    }
                }
                else
                {
                    encodingFile = enc;
                    ReadFile();
                }

            }
            catch
            {

            }
        }

        private void readOtherEncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (haveChangesFile)
                {
                    DialogResult mes = MessageBox.Show("При повторном чтении внесённые изменения будут потеряны. Продолжить?", "Файл был изменён", MessageBoxButtons.OKCancel);
                    if (mes == DialogResult.OK)
                    {
                        encodingFile = ++encodingFile%5;
                        ReadFile();
                    }
                }
                else
                {
                    encodingFile = ++encodingFile % 5;
                    ReadFile();
                }

            }
            catch
            {

            }
        }

        private void aNSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadOtherEncoding(1);
        }

        private void aSCIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadOtherEncoding(2);
        }

        private void unicodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadOtherEncoding(3);
        }

        private void uTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadOtherEncoding(4);
        }

        private void CounterVowelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wasNumbVowel)
                EraseNumber();
            string[] lineText = FileText.Text.Split('\n');
            numbVowel = new int[lineText.Length];
            string newText = "";
            CountVowelInString(ref lineText[0], out numbVowel[0]);
            newText += lineText[0];
            for (int i = 1; i<lineText.Length; i++)
            {
                CountVowelInString(ref lineText[i], out numbVowel[i]);
                newText += '\n'+lineText[i];
            }
            ChangeText(newText);
            wasNumbVowel = true;
        }
        void EraseNumber ()
        {
            string[] lineText = FileText.Text.Split('\n');
            string newText = "";
            newText += lineText[0].Replace(Convert.ToString(" "+numbVowel[0]), "");
            for (int i = 1; i < lineText.Length; i++)
            {
                CountVowelInString(ref lineText[i], out numbVowel[i]);
                newText += '\n' + lineText[i].Replace(Convert.ToString(" " + numbVowel[i]), "");
            }
            ChangeText(newText);
            wasNumbVowel = false;
        }
        private void EraseNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wasNumbVowel)
            {
                EraseNumber();
            }
        }

        private void FileText_Layout(object sender, LayoutEventArgs e)
        {
            ReadFile();
            FileText.Layout -= FileText_Layout;
        }
    }
}
