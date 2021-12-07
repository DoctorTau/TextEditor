using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class SettingsForm : Form
    {
        Timer timer;

        public SettingsForm(Timer timer, string therme)
        {
            this.timer = timer;
            InitializeComponent();
            AutoSavingTextBox.Text = timer.Interval.ToString();
            ThermeBox.Text = therme;
        }

        private void ThermeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ApplySettingsButton_Click(object sender, EventArgs e)
        {
            int timerInterval = 60000;
            EditingForm parrentForm  = (Owner as EditingForm);
            if(int.TryParse(AutoSavingTextBox.Text, out timerInterval))
            {
                parrentForm.ChangeAutoSaveInterval(timerInterval);
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            parrentForm.ChangeTherme(ThermeBox.Text);
        }

        private void ToDefaultSetiingsButton_Click(object sender, EventArgs e)
        {
            AutoSavingTextBox.Text = "60000";
            ThermeBox.Text = "Light";
            ApplySettingsButton_Click(sender, e);
        }
    }
}
