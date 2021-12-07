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
    /// <summary>
    /// Form which chenges app's properties.
    /// </summary>
    public partial class SettingsForm : Form
    {
        Timer timer;

        /// <summary>
        /// Constructor of a settings form by timer and therme.
        /// </summary>
        /// <param name="timer">Timer from main form.</param>
        /// <param name="therme">Therme from main form.</param>
        public SettingsForm(Timer timer, string therme)
        {
            this.timer = timer;
            InitializeComponent();
            AutoSavingTextBox.Text = timer.Interval.ToString();
            ThermeBox.Text = therme;
        }

        /// <summary>
        /// Applys settings.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
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

        /// <summary>
        /// Changes settings to default.
        /// </summary>
        /// <param name="sender">Object which sent an event.</param>
        /// <param name="e">Event data.</param>
        private void ToDefaultSetiingsButton_Click(object sender, EventArgs e)
        {
            AutoSavingTextBox.Text = "60000";
            ThermeBox.Text = "Light";
            ApplySettingsButton_Click(sender, e);
        }
    }
}
