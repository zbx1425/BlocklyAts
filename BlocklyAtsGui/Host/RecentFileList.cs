using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocklyAts.Host {

    class RecentFileList : List<string> {

        public const int MaxRecordNum = 16;

        public string[] GetRecentFiles(int maxAmount) {
            return this.Where(f => File.Exists(f)).Reverse().Take(maxAmount).ToArray();
        }

        public void AddRecentFile(string path) {
            if (this.Contains(path)) this.Remove(path);
            this.Add(path);
            while (this.Count > MaxRecordNum) this.RemoveAt(0);
        }
    }
}
