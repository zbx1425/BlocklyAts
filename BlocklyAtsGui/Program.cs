using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyATS {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args) {
            if (args.Name.StartsWith("CefSharp")) {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string architectureSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    assemblyName);

                return File.Exists(architectureSpecificPath)
                    ? Assembly.LoadFile(architectureSpecificPath)
                    : null;
            }

            return null;
        }
    }
}
