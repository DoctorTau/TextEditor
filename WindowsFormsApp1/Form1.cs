﻿using System;
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
                Dock = DockStyle.Fill
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
                    saveFileDialog.ShowDialog();

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

    }
}
