using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class ContraintDiff
    {
        public string TableName { get; set; }
        public string ContraintType { get; set; }

        public string ContraintTableName { get; set; }

        public string UniqContraintTableName { get; set; }
    }
}
