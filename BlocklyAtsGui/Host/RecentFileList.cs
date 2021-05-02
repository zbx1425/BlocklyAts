using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocklyAts.Host {

    public class RecentFileList : List<string> {

        public const int MaxRecordNum = 16;

        public string[] GetRecentFiles(int maxAmount) {
            this.RemoveAll(f => !File.Exists(f));
            return Enumerable.Reverse(this).Take(maxAmount).ToArray();
        }

        public void AddRecentFile(string path) {
            if (this.Contains(path)) this.Remove(path);
            this.Add(path);
            while (this.Count > MaxRecordNum) this.RemoveAt(0);
        }
    }
}
