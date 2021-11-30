using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SettingsForm : Form
    {
        Timer timer;

        public SettingsForm(Timer timer)
        {
            this.timer = timer;
            InitializeComponent();
            AutoSavingTextBox.Text = timer.Interval.ToString();
        }

        private void ThermeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ApplySettingsButton_Click(object sender, EventArgs e)
        {
            int timerInterval = 60000;
            if(int.TryParse(AutoSavingTextBox.Text, out timerInterval))
            {
                timer.Interval = timerInterval;
            }
        }

        private void ToDefaultSetiingsButton_Click(object sender, EventArgs e)
        {
            AutoSavingTextBox.Text = "60000";
            ThermeBox.Text = "Light";
            LanguageComboBox.Text = "English";
            ApplySettingsButton_Click(sender, e);
        }
    }
}
