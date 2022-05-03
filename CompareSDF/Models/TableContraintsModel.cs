using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class TableContraintsModel
    {
        public int Id { get; set; }
        public string ConstraintName { get; set; }
        public string ConstraintType { get; set; }
        public string TableName { get; set; }

        public string ConstraintTableName { get; set; }

        public string UniqConstraintTableName { get; set; }

        public string UniqConstraintName { get; set; }
    }
}
