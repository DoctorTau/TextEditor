﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TextEditor
{
    /// <summary>
    /// Tab page with RichTextBox and file.
    /// </summary>
    internal class TextBoxTab : TabPage
    {
        RichTextBox textBox;
        public FileInfo fileInfo;
        public bool isSaved { get; set; }     

        /// <summary>
        /// Constructor of a new tab page by name.
        /// </summary>
        /// <param name="name">Name of the new tab page.</param>
        /// <param name="contextMenuStrip">Context menu in the RichTextBox.</param>
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

        /// <summary>
        /// Costructor of a new tab page by file.
        /// </summary>
        /// <param name="fileToOpen">File in tab page.</param>
        /// <param name="contextMenuStrip">Context menu in the RichTextBox.</param>
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

        /// <summary>
        /// Changes the isSaved paramether.
        /// </summary>
        /// <param name="sneder">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void changeSaveStatus(object sneder, EventArgs e)
        {
            isSaved = false;
            if(!Text.Contains('*'))
                Text += "*";
        }

        /// <summary>
        /// Updates text in the RichTextBox by reading a file.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
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
