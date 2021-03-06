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
        int line;
        int indexInLine;
        bool enableStatusSrip = true;
        bool haveChangesFile;
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
                        this.Text = fileName + " - Блокнотик";
                        FileText.Text = System.IO.File.ReadAllText(filePath);
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Text = fileName + " - Блокнотик";
                    FileText.Text = System.IO.File.ReadAllText(filePath);
                }
            }
            catch 
            {
                fileName = "Новый текстовый документ";
                this.Text = fileName + " - Блокнотик";
            }
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
            this.Text = fileName + " - Блокнотик";
            System.IO.File.WriteAllText(filePath, FileText.Text);
            haveChangesFile = false;
        }


        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            filePath = openFileDialog1.FileName;
            FileText.Text = System.IO.File.ReadAllText(filePath);
            fileName = openFileDialog1.SafeFileName.Split('.')[0];
            this.Text = fileName + " - Блокнотик";
            haveChangesFile = false;
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

        private void ReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reference reference = new Reference();
            reference.ShowDialog();
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haveChangesFile == true)
            {
                DialogResult message = MessageBox.Show("Сохранить текущий документ перед выходом?", "Файл не сохранён", MessageBoxButtons.YesNoCancel);
                if (message == DialogResult.Yes)
                {
                    try
                    {
                        System.IO.File.WriteAllText(filePath, FileText.Text);
                    }
                    catch
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                        filePath = saveFileDialog1.FileName;
                        System.IO.File.WriteAllText(filePath, FileText.Text);
                    }
                    haveChangesFile = false;
                    FileText.Text = "";
                }
                else if (message == DialogResult.No)
                {
                    FileText.Text = "";
                    fileName = "Новый текстовый документ";
                    this.Text = fileName + " - Блокнотик";
                }
            }
            else
            {
                fileName = "Новый текстовый документ";
                filePath = "";
                FileText.Text = "";
                this.Text = fileName + " - Блокнотик";
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(FileText.Text, FileText.Font, Brushes.Black, 0, 0);
        }

        private void enableStatusStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Settings.Default.statusStripDisabled) {
                statusStrip1.Hide();
                Settings.Default.statusStripDisabled = true;
                Settings.Default.Save();
            }
            else
            {
                statusStrip1.Show();
                Settings.Default.statusStripDisabled = false;
                Settings.Default.Save();
            }
        }

        private void FileText_SelectionChanged(object sender, EventArgs e)
        {
            line = 1 + Convert.ToInt32(FileText.GetLineFromCharIndex(FileText.SelectionStart));
            indexInLine = 1 + Convert.ToInt32(FileText.SelectionStart) - Convert.ToInt32(FileText.GetFirstCharIndexFromLine(line - 1));
            toolStripStatusLabel1.Text = "Строка " + line + " Столбец " + indexInLine;
        }

        private void EncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileText.Text = System.IO.File.ReadAllText(filePath, Encoding.Default);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileText.Font = Settings.Default.Font;
            if (Settings.Default.statusStripDisabled) statusStrip1.Hide();
        }
    }
}
