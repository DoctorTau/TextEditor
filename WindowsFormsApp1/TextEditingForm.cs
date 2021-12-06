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

namespace TextEditor
{
    public partial class EditingForm : Form
    {
        List<FileInfo> files = new List<FileInfo>();
        string fileFilter = "txt files (*.txt)|*.txt|rtf files (*.rtf)|*.rtf|All files (*.*)|*.*";
        TextBoxTab curTab = null;

        public EditingForm()
        {
            InitializeComponent();
        }
        
        private void TabControl_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            TextBoxTab newTab = new TextBoxTab("Simple", contextMenuStrip);
            TabControl.TabPages.Add(newTab);
            TabControl.SelectedTab = newTab;
            curTab = newTab;
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
                    FileInfo opennedFile = new FileInfo(dialog.FileName);
                    if (CheckIsIn(opennedFile))
                        throw new Exception("File is already openned");
                    CreateButton_Click(sender, e);
                    curTab.fileInfo = opennedFile;
                    curTab.UpdateText();
                    curTab.Text = opennedFile.Name;
                }
                curTab.Text = curTab.Text.Trim('*');  
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
                if (curTab.fileInfo != null)
                {
                    using (StreamWriter sw = new StreamWriter(curTab.fileInfo.FullName))
                    {
                        sw.Write(curTab.Controls[0].Text);
                    }
                }
                else
                {
                    SaveAsButton_Click(sender, e);
                }
                curTab.Text = curTab.Text.Trim('*');  
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

                    FileInfo fileToSave = new FileInfo(saveFileDialog.FileName);
                    using (StreamWriter sw = new StreamWriter(fileToSave.FullName))
                    {
                        sw.Write(curTab.Controls[0].Text);
                    }
                    curTab.Text = fileToSave.Name;
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

        private void SavingFile(FileInfo file, string text)
        {
            if (file.Exists)
            {
                using (StreamWriter sw = new StreamWriter(file.FullName))
                    sw.Write(text);
            }
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if(textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size,selectionFont.Style ^ FontStyle.Italic);

            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if(textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size,selectionFont.Style ^ FontStyle.Bold);

            }
        }

        private void unedrlinedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if(textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size,selectionFont.Style ^ FontStyle.Underline);

            }
        }

        private void strikeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
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
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            textBox.SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            Clipboard.SetText(textBox.SelectedText);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            Clipboard.SetText(textBox.SelectedText);
            textBox.SelectedText = "";
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
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
            if (!curTab.isSaved) { 
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
            TabControl.TabPages.Remove(curTab);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if(TabControl.SelectedIndex != -1)
                curTab = (TextBoxTab)TabControl.SelectedTab;
            else
                curTab = null;
        }

        private void ChangeTextTimer_Tick(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count > 0) {
                RichTextBox textInBox = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (!IsSaved(TabControl.SelectedTab) && !TabControl.SelectedTab.Text.Contains('*'))
                    TabControl.SelectedTab.Text += '*';
                else if(IsSaved(TabControl.SelectedTab))
                    TabControl.SelectedTab.Text = TabControl.SelectedTab.Text.Trim('*');
                CountOfWords.Text = textInBox.Text != ""? textInBox.Text.Split().Length.ToString() + " words":  "0 words";        
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var tab in TabControl.TabPages) {
                var textTab = (TextBoxTab)tab;
                if (!textTab.isSaved)
                {
                    DialogResult dialogResult = AskOfSaving();
                    switch (dialogResult)
                    {
                    case DialogResult.Yes:
                        SaveButton_Click(sender, e);
                        closeToolStripMenuItem_Click(sender, e); 
                        break;
                    case DialogResult.No:
                        TabControl.TabPages.Remove(curTab);
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    }
                }
            }

            if(Application.OpenForms.Count > 1)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var tab in TabControl.TabPages) {
                var textTab = (TextBoxTab)tab;
                if (!textTab.isSaved)
                {
                    SavingFile(textTab.fileInfo, textTab.Controls[0].Text);
                    textTab.isSaved = true;
                    textTab.Text = textTab.Text.Trim('*');
                }
            }
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = new FontDialog();
                dialogResult.ShowDialog();
                RichTextBox textInBox = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                textInBox.SelectionFont = dialogResult.Font;
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void openInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditingForm newWindow = new EditingForm();
            newWindow.OpenButton_Click(sender, e);
            newWindow.Show();
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            foreach (var tab in TabControl.TabPages)
            {
                var textTab = (TextBoxTab)tab;
                if (!textTab.isSaved)
                {
                    try
                    {
                        SavingFile(curTab.fileInfo, curTab.Controls[0].Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                        return;
                    }
                    curTab.isSaved = true;
                    curTab.Text = textTab.Text.Trim('*');
                }
            }
        }
    }
}
