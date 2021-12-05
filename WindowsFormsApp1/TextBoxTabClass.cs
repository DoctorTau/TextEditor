using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
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
                ContextMenuStrip = contextMenuStrip
            };
            Dock = DockStyle.Fill;
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Text = name;
            textBox.TextChanged += new EventHandler(changeSaveStatus);
            Controls.Add(textBox);
            isSaved = false;
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
    }
}
