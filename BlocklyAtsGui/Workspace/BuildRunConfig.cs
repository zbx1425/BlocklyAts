using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlocklyAts {
    
    public class BuildRunConfig {

        public enum BveImpl {
            Custom = -1,
            Unspecified = 0,
            BveTs5 = 5,
            BveTs6 = 6,
            OpenBve = 999
        }

        public bool ShouldCompileAnyCpu { get; set; }

        public bool ShouldCompilex86 { get; set; }

        public bool ShouldCompilex64 { get; set; }

        public string CompilePathAnyCpu { get; set; }

        public string CompilePathx86 { get; set; }

        public string CompilePathx64 { get; set; }

        public BveImpl GameType { get; set; }

        public string GamePath { get; set; }

        public string GameArgs { get; set; }

        public BuildRunConfig Clone() {
            return (BuildRunConfig)this.MemberwiseClone();
        }
        
        public Process GetGameProcess() {
            string path = null;
            string args = GameArgs;

            switch (GameType) {
                case BveImpl.Custom:
                    path = GamePath;
                    break;
                case BveImpl.BveTs5:
                    path = GameDetection.BveTs5Path;
                    break;
                case BveImpl.BveTs6:
                    path = GameDetection.BveTs6Path;
                    break;
                case BveImpl.OpenBve:
                    if (PlatformFunction.IsMono) {
                        path = GameDetection.MonoPath;
                        args = string.Format("\"{0}\" {1}", GameDetection.OpenBvePath, args);
                    } else {
                        path = GameDetection.OpenBvePath;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(path) || !File.Exists(path)) {
                return null;
            }
            return new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = path,
                    Arguments = args,
                    UseShellExecute = false,
                }
            };
        }
    }
}
