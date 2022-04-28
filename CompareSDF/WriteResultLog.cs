using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareSDF
{
    public class WriteResultLog
    {
        public static void WriteLog(string content)
        {
            using (StreamWriter writer = File.AppendText("results.txt"))
            {
                var tw = StreamWriter.Synchronized(writer);
                tw.WriteLine(content);
                tw.Close();
            }
        }
    }
}
