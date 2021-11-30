using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class EditingForm : Form
    {
        List<FileInfo> files = new List<FileInfo>();
        string fileFilter = "txt files (*.txt)|*.txt|rtf files (*.rtf)|*.rtf|All files (*.*)|*.*";

        public EditingForm()
        {
            InitializeComponent();
        }
        
        private void TabControl_DoubleClick(object sender, EventArgs e)
        {
            
        }
        
        private void CreateButton_Click(object sender, EventArgs e)
        {
            TabPage newTabPage = new TabPage("Simple");
            RichTextBox textBox = new RichTextBox
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                Dock = DockStyle.Fill,
                ContextMenuStrip = contextMenuStrip
            };
            newTabPage.Controls.Add(textBox);
            files.Add(null);
            TabControl.TabPages.Add(newTabPage);
            TabControl.SelectedTab = newTabPage;
        }

        private bool CheckIsIn(FileInfo file)
        {
           foreach(FileInfo fileInfo in files)
            {
                if(fileInfo != null && fileInfo.FullName == file.FullName)
                    return true;
            } 
           return false;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog() { InitialDirectory = Directory.GetCurrentDirectory(), Filter = fileFilter })
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    if (CheckIsIn(new FileInfo(dialog.FileName)))
                        throw new Exception("File is already openned");
                    CreateButton_Click(sender, e);
                    TabControl.SelectedTab.Controls[0].Text = File.ReadAllText(dialog.FileName);
                    TabControl.SelectedTab.Text = dialog.FileName.Split('\\').Last();
                    files[TabControl.SelectedIndex] = new FileInfo(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (files[TabControl.SelectedIndex] != null)
                {
                    using (StreamWriter sw = new StreamWriter(files[TabControl.SelectedIndex].FullName))
                    {
                        sw.Write(TabControl.SelectedTab.Controls[0].Text);
                    }
                }
                else
                {
                    SaveAsButton_Click(sender, e);
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog() { InitialDirectory = Directory.GetCurrentDirectory(), Filter = fileFilter })
                {
                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    FileInfo file = new FileInfo(saveFileDialog.FileName);
                    using (StreamWriter sw = new StreamWriter(file.FullName))
                    {
                        sw.Write(TabControl.SelectedTab.Controls[0].Text);
                    }
                    files[TabControl.SelectedIndex] = file;
                    TabControl.SelectedTab.Text = file.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(AutoSaveTimer);
            settingsForm.Show();
        }

        private void SavingFile(FileInfo file)
        {
            if (file.Exists)
            {
                using (StreamWriter sw = new StreamWriter(file.FullName))
                {
                    sw.Write(TabControl.TabPages[files.IndexOf(file)].Controls[0].Text);
                }
            }
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            foreach(FileInfo file in files)
            {
                if(file != null) SavingFile(file);
            }
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if(textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size,selectionFont.Style ^ FontStyle.Italic);

            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if(textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size,selectionFont.Style ^ FontStyle.Bold);

            }
        }

        private void unedrlinedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if(textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size,selectionFont.Style ^ FontStyle.Underline);

            }
        }

        private void strikeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if(textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size,selectionFont.Style ^ FontStyle.Strikeout);

            }
        }

        private void boldContextMenu_Click(object sender, EventArgs e)
        {
            boldToolStripMenuItem_Click(sender, e);
        }

        private void italicContextMenu_Click(object sender, EventArgs e)
        {
            italicToolStripMenuItem_Click(sender, e);
        }

        private void underlineContexMenu_Click(object sender, EventArgs e)
        { 
            unedrlinedToolStripMenuItem_Click(sender, e);
        }

        private void strickeouteContextMenu_Click(object sender, EventArgs e)
        {
            strikeoutToolStripMenuItem_Click(sender, e);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            textBox.SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            Clipboard.SetText(textBox.SelectedText);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            Clipboard.SetText(textBox.SelectedText);
            textBox.SelectedText = "";
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, Clipboard.GetText());
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(TabControl.TabPages.Count == 0) return;
            selectAllToolStripMenuItem_Click(sender, e);
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(TabControl.TabPages.Count == 0) return;
            copyToolStripMenuItem_Click(sender, e);
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(TabControl.TabPages.Count == 0) return;
            cutToolStripMenuItem_Click(sender, e);
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(TabControl.TabPages.Count == 0) return;
            pasteToolStripMenuItem_Click(sender, e);
        }

        private bool IsSaved(TabPage tabPage)
        {
            string curText = tabPage.Controls[0].Text;
            FileInfo file = files[TabControl.TabPages.IndexOf(tabPage)];
            if (file != null && file.Exists)
            {
                using(StreamReader sr = new StreamReader(file.FullName))
                {
                    string textInFile = sr.ReadToEnd();
                    if(curText == textInFile)
                        return true;
                }
            }
            return false;
        }

        private DialogResult AskOfSaving()
        {
            string messege = "Do you want to save file before closing";
            string nameOfMessege = "File is not saved";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            return MessageBox.Show(messege, nameOfMessege, buttons);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsSaved(TabControl.SelectedTab)){
                var dialogResult = AskOfSaving();
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        SaveButton_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            files.RemoveAt(TabControl.SelectedIndex);
            TabControl.TabPages.Remove(TabControl.SelectedTab);
        }
    }
}
