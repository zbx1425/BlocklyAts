using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlocklyAts {
    static class AssemblyResolveHelper {

        public static Assembly LoadFromExecutingDllData() {
            Assembly targetAssembly = null;
            using (var fs = new FileStream(Assembly.GetExecutingAssembly().Location, FileMode.Open, FileAccess.Read)) {
                fs.Seek(0x6C, SeekOrigin.Begin);
                byte[] sizeBuf = new byte[4];
                fs.Read(sizeBuf, 0, 4);
                var exeSize = BitConverter.ToInt32(sizeBuf, 0);
                fs.Read(sizeBuf, 0, 4);
                var dllSize = BitConverter.ToInt32(sizeBuf, 0);
                fs.Seek(exeSize, SeekOrigin.Begin);

                byte[] dllBuf = new byte[dllSize];
                fs.Read(dllBuf, 0, dllSize);
                if (fs.Length > exeSize + dllSize) {
                    byte[] pdbBuf = new byte[fs.Length - exeSize - dllSize];
                    fs.Read(pdbBuf, 0, (int)fs.Length - exeSize - dllSize);
                    targetAssembly = Assembly.Load(dllBuf, pdbBuf);
                } else {
                    targetAssembly = Assembly.Load(dllBuf);
                }
            }
            return targetAssembly;
        }

        public static void SetupResolveHandler() {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public static void RemoveResolveHandler() {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }
        
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            if (args.Name.Contains("AtsCallConverter")) return typeof(AssemblyResolveHelper).Assembly;
            return null;
        }

        public static void ClearEventInvocations(object obj, string eventName) {
            var fi = GetEventField(obj.GetType(), eventName);
            if (fi == null) return;
            fi.SetValue(obj, null);
        }

        private static FieldInfo GetEventField(Type type, string eventName) {
            FieldInfo field = null;
            while (type != null) {
                /* Find events defined as field */
                field = type.GetField(eventName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null && (field.FieldType == typeof(MulticastDelegate) || field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                    break;

                /* Find events defined as property { add; remove; } */
                field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                    break;
                type = type.BaseType;
            }
            return field;
        }
    }
}
