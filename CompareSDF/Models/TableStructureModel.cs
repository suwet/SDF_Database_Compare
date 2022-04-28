using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompareSDF.Models
{
    public class TableStructureModel
    {
        public int Id { get; set; }
        public string Table_Name { get; set; }
        public string Column_Name { get; set; }
        public string Data_Type_Name { get; set; }
        public Type Data_Type { get; set; }

        public bool Is_Nullable { get; set; }
    }
}
