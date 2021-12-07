using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TextEditor
{
    internal class TextBoxTab : TabPage
    {
        RichTextBox textBox;
        public FileInfo fileInfo;
        public bool isSaved { get; set; }     

        public TextBoxTab(string name, ContextMenuStrip contextMenuStrip):base()
        {
            textBox = new RichTextBox
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Times New Roman", 12.0f),
            ContextMenuStrip = contextMenuStrip
            };
            Dock = DockStyle.Fill;
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Text = name;
            textBox.TextChanged += new EventHandler(changeSaveStatus);
            Controls.Add(textBox);
            isSaved = false;
        }

        public TextBoxTab(FileInfo fileToOpen, ContextMenuStrip contextMenuStrip):base()
        {
            textBox = new RichTextBox
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Times New Roman", 12.0f),
            ContextMenuStrip = contextMenuStrip
            };
            Dock = DockStyle.Fill;
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Text = fileToOpen.Name;
            fileInfo = fileToOpen;  
            textBox.Text = File.ReadAllText(fileToOpen.FullName);
            textBox.TextChanged += new EventHandler(changeSaveStatus);
            Controls.Add(textBox);
            isSaved = true;
        }

        private void changeSaveStatus(object sneder, EventArgs e)
        {
            isSaved = false;
            if(!Text.Contains('*'))
                Text += "*";
        }

        public void UpdateText()
        {
            if (fileInfo.Exists)
            {
                using(StreamReader sr = new StreamReader(fileInfo.FullName))
                    Controls[0].Text = sr.ReadToEnd();
                return;
            }
            throw new ArgumentException("Unknown error");
        }

        public Tuple<string, string> GetNameAndText() => new Tuple<string, string>(Text, Controls[0].Text);
    }
}
