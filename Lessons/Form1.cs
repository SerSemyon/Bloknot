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
        int indexSelectLine;
        bool counterVowelEnabled;
        bool statusStripEnabled;
        bool haveChangesFile = false;
        int haveChangesText = 0;
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
                    DialogResult message = MessageBox.Show("Данный формат файла не поддерживается программой", "Открыть файл ?", MessageBoxButtons.YesNoCancel);
                    if (message == DialogResult.No)
                    {
                        filePath = "";
                    }
                    else if (message == DialogResult.Yes)
                    {
                        ReadFile();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    ReadFile();
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
        }
        public void ChangeText(string newText)
        {
            pastText = FileText.Text;
            FileText.Text = newText;
            haveChangesText = 1;
        }

        void SaveFile()
        {
            try
            {
                System.IO.File.WriteAllText(filePath, FileText.Text);
                toolStripStatusLabel2.Text = "Кодировка по умолчанию";
                haveChangesFile = false;
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
        }


        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            filePath = openFileDialog1.FileName;
            fileName = openFileDialog1.SafeFileName.Split('.')[0];
            ReadFile();
        }
        string CountVowelInString(string line)
        {
            line=line.ToLower();
            int value = 0;
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
                        value++;
                        break;
                }
            }
            return Convert.ToString(value);
        }
        private void FileText_TextChanged(object sender, EventArgs e)
        {
            haveChangesFile = true;
            haveChangesText = 0;
            indexSelectLine = Convert.ToInt32(FileText.GetLineFromCharIndex(FileText.SelectionStart));

            
            if (FileText.Lines.Length == CounterVowel.Lines.Length)
            {
                string[] txt = CounterVowel.Text.Split('\n');
                CounterVowel.Text = "";
                for (int i = 0; i < txt.Length; i++)
                {
                    if (i == indexSelectLine) CounterVowel.Text += CountVowelInString(FileText.Lines[i])+ "\n";
                    else CounterVowel.Text += txt[i] + "\n";
                }
            }
            else
            {
                CounterVowel.Text = "";
                for (int i = 0; i < FileText.Lines.Length; i++)
                {
                    CounterVowel.Text += CountVowelInString(FileText.Lines[i]) + "\n";
                }
            }

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
                ReadOnlyToolStripMenuItem.Text = "Запретить редактирование";
            }
            else
            {
                FileText.ReadOnly = true;
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
                CounterVowel.Font = FileText.Font;
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
            Settings.Default.counterVowelEnabled = counterVowelEnabled;
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
            CounterVowel.Font = Settings.Default.Font;
            counterVowelEnabled = Settings.Default.counterVowelEnabled;
            statusStripEnabled = Settings.Default.statusStripEnabled;
            if (!counterVowelEnabled)
            {
                CounterVowel.Hide();
                tableLayoutPanel1.ColumnCount -= 1;
            }
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

        private void counterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (counterVowelEnabled)
            {
                CounterVowel.Hide();
                tableLayoutPanel1.ColumnCount -= 1;
                counterVowelEnabled = false;
            }
            else
            {
                CounterVowel.Show();
                tableLayoutPanel1.ColumnCount += 1;
                counterVowelEnabled = true;
            }
        }

        private void FileText_VScroll(object sender, EventArgs e)
        {
        }
    }
}
