using BlocklyAts.Host;
using BlocklyAts.UserInterface;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyAts {
    static class Program {

        [STAThread]
        static void Main() {
            PreferenceManager.LoadPreference();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormMain mainForm;
            if (Environment.GetCommandLineArgs().Length > 1) {
                mainForm = new FormMain(Environment.GetCommandLineArgs()[1]);
            } else {
                mainForm = new FormMain();
            }

            Application.Run(mainForm);

            PreferenceManager.SavePreference();
        }
    }
}
