using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareSDF.Models
{
    public class TableDiff
    {
        public string TableName { get; set; }
        //public string ColumnSourceName { get; set; }
        //public string ColumnDescName { get; set; }
        public string CoumnNameDiffs { get; set; }
        public string CoumnDataTypeDiffs { get; set; }
        public string Remark { get; set; }
        public bool IsDiff { get; set; }
    }
}
