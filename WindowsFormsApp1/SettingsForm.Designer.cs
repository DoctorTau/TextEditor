namespace TextEditor
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Auto_saving = new System.Windows.Forms.Label();
            this.AutoSavingTextBox = new System.Windows.Forms.TextBox();
            this.msLabel = new System.Windows.Forms.Label();
            this.ApplySettingsButton = new System.Windows.Forms.Button();
            this.ToDefaultSetiingsButton = new System.Windows.Forms.Button();
            this.ThermeLabel = new System.Windows.Forms.Label();
            this.ThermeBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Auto_saving
            // 
            this.Auto_saving.AutoSize = true;
            this.Auto_saving.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Auto_saving.Location = new System.Drawing.Point(12, 18);
            this.Auto_saving.Name = "Auto_saving";
            this.Auto_saving.Size = new System.Drawing.Size(127, 20);
            this.Auto_saving.TabIndex = 0;
            this.Auto_saving.Text = "Auto-saving time";
            // 
            // AutoSavingTextBox
            // 
            this.AutoSavingTextBox.Location = new System.Drawing.Point(154, 18);
            this.AutoSavingTextBox.Name = "AutoSavingTextBox";
            this.AutoSavingTextBox.Size = new System.Drawing.Size(100, 20);
            this.AutoSavingTextBox.TabIndex = 1;
            this.AutoSavingTextBox.Text = "60000";
            this.AutoSavingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // msLabel
            // 
            this.msLabel.AutoSize = true;
            this.msLabel.Location = new System.Drawing.Point(260, 23);
            this.msLabel.Name = "msLabel";
            this.msLabel.Size = new System.Drawing.Size(20, 13);
            this.msLabel.TabIndex = 2;
            this.msLabel.Text = "ms";
            // 
            // ApplySettingsButton
            // 
            this.ApplySettingsButton.Location = new System.Drawing.Point(73, 138);
            this.ApplySettingsButton.Name = "ApplySettingsButton";
            this.ApplySettingsButton.Size = new System.Drawing.Size(75, 23);
            this.ApplySettingsButton.TabIndex = 3;
            this.ApplySettingsButton.Text = "Apply";
            this.ApplySettingsButton.UseVisualStyleBackColor = true;
            this.ApplySettingsButton.Click += new System.EventHandler(this.ApplySettingsButton_Click);
            // 
            // ToDefaultSetiingsButton
            // 
            this.ToDefaultSetiingsButton.Location = new System.Drawing.Point(154, 138);
            this.ToDefaultSetiingsButton.Name = "ToDefaultSetiingsButton";
            this.ToDefaultSetiingsButton.Size = new System.Drawing.Size(75, 23);
            this.ToDefaultSetiingsButton.TabIndex = 4;
            this.ToDefaultSetiingsButton.Text = "To default";
            this.ToDefaultSetiingsButton.UseVisualStyleBackColor = true;
            this.ToDefaultSetiingsButton.Click += new System.EventHandler(this.ToDefaultSetiingsButton_Click);
            // 
            // ThermeLabel
            // 
            this.ThermeLabel.AutoSize = true;
            this.ThermeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ThermeLabel.Location = new System.Drawing.Point(12, 49);
            this.ThermeLabel.Name = "ThermeLabel";
            this.ThermeLabel.Size = new System.Drawing.Size(63, 20);
            this.ThermeLabel.TabIndex = 5;
            this.ThermeLabel.Text = "Therme";
            // 
            // ThermeBox
            // 
            this.ThermeBox.BackColor = System.Drawing.SystemColors.Window;
            this.ThermeBox.FormattingEnabled = true;
            this.ThermeBox.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.ThermeBox.Location = new System.Drawing.Point(154, 51);
            this.ThermeBox.Name = "ThermeBox";
            this.ThermeBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ThermeBox.Size = new System.Drawing.Size(100, 21);
            this.ThermeBox.TabIndex = 6;
            this.ThermeBox.Text = "Light";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 173);
            this.Controls.Add(this.ThermeBox);
            this.Controls.Add(this.ThermeLabel);
            this.Controls.Add(this.ToDefaultSetiingsButton);
            this.Controls.Add(this.ApplySettingsButton);
            this.Controls.Add(this.msLabel);
            this.Controls.Add(this.AutoSavingTextBox);
            this.Controls.Add(this.Auto_saving);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Auto_saving;
        private System.Windows.Forms.TextBox AutoSavingTextBox;
        private System.Windows.Forms.Label msLabel;
        private System.Windows.Forms.Button ApplySettingsButton;
        private System.Windows.Forms.Button ToDefaultSetiingsButton;
        private System.Windows.Forms.Label ThermeLabel;
        private System.Windows.Forms.ComboBox ThermeBox;
    }
}