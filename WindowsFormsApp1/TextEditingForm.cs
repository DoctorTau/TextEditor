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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TextEditor
{
    /// <summary>
    /// Struct with main settings. 
    /// </summary>
    struct SettingsParams
    {
        public int autoSaveTime { get; set; }
        public string therme { get; set; }
        public List<string> opennedFiles { get; set; }

        public SettingsParams(int autoSaveTime, string therme, List<string> ls = null)
        {
            this.autoSaveTime = autoSaveTime;
            this.therme = therme;
            this.opennedFiles = ls;
        }
    }

    public partial class EditingForm : Form
    {
        List<FileInfo> files = new List<FileInfo>();
        string fileFilter = "txt files (*.txt)|*.txt|rtf files (*.rtf)|*.rtf|All files (*.*)|*.*";
        TextBoxTab curTab = null;
        SettingsParams settingsParams = new SettingsParams(60000, "Light");


        /// <summary>
        /// Constructor for main form. 
        /// </summary>
        public EditingForm()
        {
            InitializeComponent();
            try
            {
                var settings = File.ReadAllText("settings.ini");
                settingsParams = JsonSerializer.Deserialize<SettingsParams>(settings);
                AutoSaveTimer.Interval = settingsParams.autoSaveTime;
                if (settingsParams.opennedFiles != null && settingsParams.opennedFiles.Count > 0)
                {
                    TabControl.TabPages.Clear();
                    foreach (var filePath in settingsParams.opennedFiles)
                        TabControl.TabPages.Add(new TextBoxTab(new FileInfo(filePath), ContextMenuStrip));
                    TabControl.SelectedTab = TabControl.TabPages[0];
                    curTab = (TextBoxTab)TabControl.TabPages[0];
                    ChangeTherme(settingsParams.therme);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Changes interval of autosave settings. 
        /// </summary>
        /// <param name="newInterval">New interval in milliseconds.</param>
        public void ChangeAutoSaveInterval(int newInterval)
        {
            settingsParams.autoSaveTime = newInterval;
            AutoSaveTimer.Interval = newInterval;
        }

        private void TabControl_DoubleClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Creates an empty file and adds it to the TabControl.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, EventArgs e)
        {
            TextBoxTab newTab = new TextBoxTab("Simple", contextMenuStrip);
            if (settingsParams.therme == "Dark")
                newTab.Controls[0].BackColor = SystemColors.ControlDarkDark;
            TabControl.TabPages.Add(newTab);
            TabControl.SelectedTab = newTab;
            curTab = newTab;
        }

        private bool CheckIsIn(FileInfo file)
        {
            foreach (FileInfo fileInfo in files)
            {
                if (fileInfo != null && fileInfo.FullName == file.FullName)
                    return true;
            }
            return false;
        }

        public void ChangeTherme(string therme)
        {
            if (therme == "Light")
            {
                settingsParams.therme = "Light";
                BackColor = SystemColors.Window;
                MenuStrip.BackColor = SystemColors.Window;
                foreach (var tab in TabControl.TabPages)
                {
                    var textTab = (TextBoxTab)tab;
                    RichTextBox textBoxToChange = textTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                    textBoxToChange.BackColor = SystemColors.Window;
                }
            }

            else if (therme == "Dark")
            {
                settingsParams.therme = "Dark";
                MenuStrip.BackColor = SystemColors.ControlDarkDark;
                BackColor = SystemColors.ControlDarkDark;
                foreach (var tab in TabControl.TabPages)
                {
                    var textTab = (TextBoxTab)tab;
                    RichTextBox textBoxToChange = textTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                    textBoxToChange.BackColor = SystemColors.ControlDarkDark;
                }
            }
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
                    curTab.isSaved = true;
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
            }
            catch (Exception ex)
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
            SettingsForm settingsForm = new SettingsForm(AutoSaveTimer, settingsParams.therme);
            settingsForm.Show(this);
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
            if (TabControl.TabPages.Count == 0) return;
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size, selectionFont.Style ^ FontStyle.Italic);

            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size, selectionFont.Style ^ FontStyle.Bold);

            }
        }

        private void unedrlinedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size, selectionFont.Style ^ FontStyle.Underline);

            }
        }

        private void strikeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            RichTextBox textToCahge = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (textToCahge.SelectedText != "")
            {
                var selectionFont = textToCahge.SelectionFont;

                textToCahge.SelectionFont = new Font(selectionFont.FontFamily, selectionFont.Size, selectionFont.Style ^ FontStyle.Strikeout);

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
            if (TabControl.TabPages.Count == 0) return;
            selectAllToolStripMenuItem_Click(sender, e);
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            copyToolStripMenuItem_Click(sender, e);
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            cutToolStripMenuItem_Click(sender, e);
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            pasteToolStripMenuItem_Click(sender, e);
        }

        private bool IsSaved(TabPage tabPage)
        {
            string curText = tabPage.Controls[0].Text;
            FileInfo file = files[TabControl.TabPages.IndexOf(tabPage)];
            if (file != null && file.Exists)
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    string textInFile = sr.ReadToEnd();
                    if (curText == textInFile)
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
            if (!curTab.isSaved)
            {
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

            if (TabControl.SelectedIndex != -1)
                curTab = (TextBoxTab)TabControl.SelectedTab;
            else
                curTab = null;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var tab in TabControl.TabPages)
            {
                var textTab = (TextBoxTab)tab;
                TabControl.SelectedTab = textTab;
                if (!textTab.isSaved)
                {
                    DialogResult dialogResult = AskOfSaving();
                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            SaveButton_Click(sender, e);
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;
                    }
                }
            }
            settingsParams.opennedFiles = GetTabsInList();
            var jsonString = JsonSerializer.Serialize(settingsParams, options: new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText("settings.ini", jsonString);

        }

        private List<string> GetTabsInList()
        {
            List<string> result = new List<string>();
            foreach (var tab in TabControl.TabPages)
            {
                TextBoxTab textBoxTab = (TextBoxTab)tab;
                if(textBoxTab.fileInfo != null)
                    result.Add(textBoxTab.fileInfo.FullName);
            }
            return result;
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var tab in TabControl.TabPages)
            {
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
            if (TabControl.TabPages.Count == 0)
                return;
            try
            {
                var dialogResult = new FontDialog();
                dialogResult.ShowDialog();
                RichTextBox textInBox = TabControl.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
                textInBox.SelectionFont = dialogResult.Font;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void openInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditingForm newWindow = new EditingForm();
            newWindow.TabControl.TabPages.Clear();
            newWindow.OpenButton_Click(sender, e);
            newWindow.ChangeTherme(settingsParams.therme);
            newWindow.ChangeAutoSaveInterval(settingsParams.autoSaveTime);
            newWindow.Show();
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            foreach (var tab in TabControl.TabPages)
            {
                var textTab = (TextBoxTab)tab;
                if (!textTab.isSaved && textTab.fileInfo != null)
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
