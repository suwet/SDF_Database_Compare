using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareSDF_3_5.Models
{
    public class ContraintDiff
    {
        public string TableName { get; set; }
        public string ContraintType { get; set; }

        public string ContraintTableName { get; set; }

        public string UniqContraintTableName { get; set; }
    }
}
