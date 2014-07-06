using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Net;

namespace ModBot
{
    public partial class SettingsDialog : Form
    {
        public String nick;
        public String password;
        public String channel;
        public String currency;
        public int interval;



        public SettingsDialog()
        {
            InitializeComponent();
            checkForUpdates();
            intervalBox.SelectedIndex = global::ModBot.Properties.Settings.Default.interval;
            payoutBox.SelectedIndex = global::ModBot.Properties.Settings.Default.payout;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            String nick = botNameBox.Text.ToLower().Trim();
            String password = passwordBox.Text.Trim();
            String channel = channelBox.Text.ToLower().Trim();
            String currency = currencyBox.Text.Trim();
            int interval = int.Parse(intervalBox.SelectedItem.ToString());
            int payout = int.Parse(payoutBox.SelectedItem.ToString());

            //save session settings
            Properties.Settings.Default.name = botNameBox.Text.Trim();
            Properties.Settings.Default.password = passwordBox.Text.Trim();
            Properties.Settings.Default.channel = channelBox.Text.Trim();
            Properties.Settings.Default.currency = currencyBox.Text.Trim();
            Properties.Settings.Default.interval = intervalBox.SelectedIndex;
            Properties.Settings.Default.payout = payoutBox.SelectedIndex;
            if ((subBox.Text.StartsWith("https://spreadsheets.google.com") || subBox.Text.StartsWith("http://spreadsheets.google.com")) && subBox.Text.EndsWith("?alt=json"))
            {
                Properties.Settings.Default.subUrl = subBox.Text;
            }
            else
            {
                if (subBox.Text.Equals(""))
                {
                    Console.WriteLine("No subscriber link supplied.  Skipping.");
                    Properties.Settings.Default.subUrl = subBox.Text;
                }
                else
                {
                    Console.WriteLine("Invalida subscriber link.  Correct format starts with https://spreadsheets.google.com and ends with alt=json");
                }
            }
            Properties.Settings.Default.Save();
            ////

            //Console.WriteLine(nick + ' ' + password + ' ' + channel + ' ' + currency + ' ' + interval);
            Irc IRC = new Irc(nick, password, channel, currency, interval, payout);

        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
            about.Dispose();
        }

        private void checkForUpdates()
        {
            string backup = "http://www.areqgaming.com/unlisted/laggy/version.html";
            string url = "http://twitchmodbot.sourceforge.net/version.html";
            using (var w = new WebClient())
            {
                w.Proxy = null;
                //Console.WriteLine(w.DownloadString(url));
                string[] newestVersion;
                try
                {
                    newestVersion = w.DownloadString(url).Split('.');
                    string major = newestVersion[0];
                    string minor = newestVersion[1];
                    string build = newestVersion[2];
                    bool update = false;
                    DialogResult result = DialogResult.None;
                    if (int.Parse(major) > int.Parse(Assembly.GetExecutingAssembly().GetName().Version.Major.ToString()))
                    {
                        //update required
                        update = true;
                        result = MessageBox.Show("There is an update available.  Click Yes to close ModBot and open the download page", "Update Available", MessageBoxButtons.YesNo);
                    }
                    else if (int.Parse(minor) > int.Parse(Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString()))
                    {
                        //update required
                        update = true;
                        result = MessageBox.Show("There is an update available.  Click Yes to close ModBot and open the download page", "Update Available", MessageBoxButtons.YesNo);
                    }
                    else if (int.Parse(build) > int.Parse(Assembly.GetExecutingAssembly().GetName().Version.Build.ToString()))
                    {
                        //update required
                        update = true;
                        result = MessageBox.Show("There is an update available.  Click Yes to close ModBot and open the download page", "Update Available", MessageBoxButtons.YesNo);
                    }
                    else
                    {
                        Console.WriteLine("You have the newest version, or a beta version.  No update required.");
                    }
                    if (update && result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("http://sourceforge.net/projects/twitchmodbot/");
                        this.Enabled = false;
                    }
                }
                catch
                {
                    try
                    {
                        newestVersion = w.DownloadString(backup).Split('.');
                        string major = newestVersion[0];
                        string minor = newestVersion[1];
                        string build = newestVersion[2];
                        bool update = false;
                        DialogResult result = DialogResult.None;
                        if (int.Parse(major) > int.Parse(Assembly.GetExecutingAssembly().GetName().Version.Major.ToString()))
                        {
                            //update required
                            update = true;
                            result = MessageBox.Show("There is an update available.  Click Yes to close ModBot and open the download page", "Update Available", MessageBoxButtons.YesNo);
                        }
                        else if (int.Parse(minor) > int.Parse(Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString()))
                        {
                            //update required
                            update = true;
                            result = MessageBox.Show("There is an update available.  Click Yes to close ModBot and open the download page", "Update Available", MessageBoxButtons.YesNo);
                        }
                        else if (int.Parse(build) > int.Parse(Assembly.GetExecutingAssembly().GetName().Version.Build.ToString()))
                        {
                            //update required
                            update = true;
                            result = MessageBox.Show("There is an update available.  Click Yes to close ModBot and open the download page", "Update Available", MessageBoxButtons.YesNo);
                        }
                        else
                        {
                            Console.WriteLine("You have the newest version, or a beta version.  No update required.");
                        }
                        if (update && result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("http://sourceforge.net/projects/twitchmodbot/");
                            this.Enabled = false;
                        }
                    }
                    catch
                    {

                    }
                }
            }

        }

    }
}
