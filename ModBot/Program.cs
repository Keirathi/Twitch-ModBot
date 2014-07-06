using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ModBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SettingsDialog settings = new SettingsDialog();
            if (settings.Enabled)
            {
                Application.Run(settings);
            }
            else
            {
                return;
            }
            
            while (true)
            {
            }
        }
    }
}
