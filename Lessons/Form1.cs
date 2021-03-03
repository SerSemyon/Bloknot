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
    public partial class Form1 : Form
    {
        string fileName;
        int line;
        bool haveChanges;
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }


        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            fileName = openFileDialog1.FileName;
            FileText.Text = System.IO.File.ReadAllText(fileName);
            haveChanges = false;
        }

        private void FileText_TextChanged(object sender, EventArgs e)
        {
            line = 1+Convert.ToInt32(FileText.GetLineFromCharIndex(FileText.SelectionStart));
            toolStripStatusLabel1.Text = "Строка " + line + " Столбец ";
            haveChanges = true;
        }


        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.File.WriteAllText(fileName, FileText.Text);
            }
            catch
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                fileName = saveFileDialog1.FileName;
                System.IO.File.WriteAllText(fileName, FileText.Text);
            }
            haveChanges = false;
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            fileName = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(fileName, FileText.Text);
            haveChanges = false;
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
            FileText.Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            FormFind searchForm = new FormFind();
            searchForm.Owner = this;
            searchForm.richText = FileText;
            searchForm.ShowDialog();
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForm search = new SearchForm();
            search.richText = FileText;
            search.ShowDialog();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel) return;
            FileText.Font = fontDialog1.Font;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (haveChanges == true)
            {
                DialogResult message = MessageBox.Show("Сохранить текущий документ перед выходом?", "Выход из программы", MessageBoxButtons.YesNoCancel);
                if (message == DialogResult.Yes)
                {
                    try
                    {
                        System.IO.File.WriteAllText(fileName, FileText.Text);
                    }
                    catch
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                        fileName = saveFileDialog1.FileName;
                        System.IO.File.WriteAllText(fileName, FileText.Text);
                    }
                    haveChanges = false;
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
            if (haveChanges == true)
            {
                DialogResult message = MessageBox.Show("Сохранить текущий документ перед выходом?", "Файл не сохранён", MessageBoxButtons.YesNoCancel);
                if (message == DialogResult.Yes)
                {
                    try
                    {
                        System.IO.File.WriteAllText(fileName, FileText.Text);
                    }
                    catch
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                        fileName = saveFileDialog1.FileName;
                        System.IO.File.WriteAllText(fileName, FileText.Text);
                    }
                    haveChanges = false;
                    FileText.Text = "";
                }
                else if (message == DialogResult.No)
                {
                    FileText.Text = "";
                }
                FileText.Text = "";
            }
        }
    }
}
