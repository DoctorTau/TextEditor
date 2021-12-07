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

        /// <summary>
        /// Creates an empty file and adds it to the TabControl.  
        /// </summary>
        /// <param name="sender">Object which sends event.</param>
        /// <param name="e">Event info.</param>
        private void CreateButton_Click(object sender, EventArgs e)
        {
            TextBoxTab newTab = new TextBoxTab("Simple", contextMenuStrip);
            if (settingsParams.therme == "Dark")
                newTab.Controls[0].BackColor = SystemColors.ControlDarkDark;
            TabControl.TabPages.Add(newTab);
            TabControl.SelectedTab = newTab;
            curTab = newTab;
        }

        /// <summary>
        /// Checks is file already  pen.
        /// </summary>
        /// <param name="file">Filename</param>
        /// <returns>True if file is already open, false otherwise</returns>
        private bool CheckIsIn(FileInfo file)
        {
            foreach (FileInfo fileInfo in files)
            {
                if (fileInfo != null && fileInfo.FullName == file.FullName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Chenges forms therme.
        /// </summary>
        /// <param name="therme">Name of the new therme.</param>
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

        /// <summary>
        /// Opens file from PC in Editing window.
        /// </summary>
        /// <param name="sender">Object which sent  an event.</param>
        /// <param name="e">Event info.</param>
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

        /// <summary>
        /// Write text from the textbox to the file, which is oppend. If file is new than "SaveAs" method will be called.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Ask user to select or create new file to write text from the current textbox to it.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Opens an "Settings" Window.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(AutoSaveTimer, settingsParams.therme);
            settingsForm.Show(this);
        }

        /// <summary>
        /// Write all test to the file.
        /// </summary>
        /// <param name="file">File to write in.</param>
        /// <param name="text">Text to write.</param>
        private void SavingFile(FileInfo file, string text)
        {
            if (file.Exists)
            {
                using (StreamWriter sw = new StreamWriter(file.FullName))
                    sw.Write(text);
            }
        }

        /// <summary>
        /// Makes selected text italic.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Makes selected text bold.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Makes selected text underlined.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Makes selected text strikedout.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Makes selected text bold.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void boldContextMenu_Click(object sender, EventArgs e)
        {
            boldToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Makes selected text italic.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
       private void italicContextMenu_Click(object sender, EventArgs e)
        {
            italicToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Makes selected text underlined.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void underlineContexMenu_Click(object sender, EventArgs e)
        {
            unedrlinedToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Makes selected text strickedout.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void strickeouteContextMenu_Click(object sender, EventArgs e)
        {
            strikeoutToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Selects all text in the textbox.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            textBox.SelectAll();
        }

        /// <summary>
        /// Adds selected text to the buffer. 
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            Clipboard.SetText(textBox.SelectedText);
        }

        /// <summary>
        /// Adds selected text to buffer and delete it.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            Clipboard.SetText(textBox.SelectedText);
            textBox.SelectedText = "";
        }

        /// <summary>
        /// Paste text form buffer.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = curTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, Clipboard.GetText());
        }

        /// <summary>
        /// Selects all text in the textbox.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            selectAllToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Adds selected text to the buffer. 
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            copyToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Adds selected text to buffer and delete it.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            cutToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Paste text form buffer.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TabControl.TabPages.Count == 0) return;
            pasteToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Makes a dialog with a user to ask, do they wants to save the file.
        /// </summary>
        /// <returns>User's answer.</returns>
        private DialogResult AskOfSaving()
        {
            string messege = "Do you want to save file before closing";
            string nameOfMessege = "File is not saved";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            return MessageBox.Show(messege, nameOfMessege, buttons);
        }

        /// <summary>
        /// Closes selected tab, and ask about saving if it's not.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Change curTab variable when user changes tab.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (TabControl.SelectedIndex != -1)
                curTab = (TextBoxTab)TabControl.SelectedTab;
            else
                curTab = null;
        }


        /// <summary>
        /// Closes the form.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Ask about saving unsaved files and write settings parameters to the JSON. 
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Gets all filepathes from the openned tabs.
        /// </summary>
        /// <returns>List with filepathes.</returns>
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

        /// <summary>
        /// Saves all files in current window.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Ask user about new font and chenge selected text to it or set this font to the future text.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Opens new window with selected file.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void openInNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditingForm newWindow = new EditingForm();
            newWindow.TabControl.TabPages.Clear();
            newWindow.OpenButton_Click(sender, e);
            newWindow.ChangeTherme(settingsParams.therme);
            newWindow.ChangeAutoSaveInterval(settingsParams.autoSaveTime);
            newWindow.Show();
        }

        /// <summary>
        /// Saves all unsaved files.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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
