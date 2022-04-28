using CompareSDF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareSDF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Schema compare sqlce 4.0");
            var source = SchemaInfo.GetSourceTableSchemas();
            var desc = SchemaInfo.GetDescTableSchemas();
            Console.WriteLine("database source path = " + SchemaInfo.GetDatabaseSourcePath());
            Console.WriteLine("database desc path = " + SchemaInfo.GetDatabaseDescPath());
            SchemaCompare.InitSchemaCompare(source, desc);

            int s_table_count = SchemaCompare.GetTableSourceCount();
            int d_table_count = SchemaCompare.GetTableDescCount();

            WriteResultLog.WriteLog(string.Format("Number of tables in  database ( source {0}  <<<=>>> desc {1} )", s_table_count, d_table_count));
            List<string> diff_tables = SchemaCompare.GetTableNameDiff();
            WriteResultLog.WriteLog("table not contain in source table are below");
            foreach (var item in diff_tables)
            {
                WriteResultLog.WriteLog("\t" + item);
            }

            WriteResultLog.WriteLog("----------------------------------------------------------------------------------");



            WriteResultLog.WriteLog("Start Compare column count");

            foreach (var t_s in source.GroupBy(x => x.Table_Name))
            {
                int s_column_count = SchemaCompare.GetTableColumnSourceCount(t_s.Key);
                int d_column_count = SchemaCompare.GetTableColumnDescCount(t_s.Key);
                if (s_column_count != d_column_count)
                {
                    WriteResultLog.WriteLog(string.Format("Number of column in table {0} not equal ( source {1}  <<<  >>> desc {2} )", t_s.Key, s_column_count, d_column_count));
                }
            }

            WriteResultLog.WriteLog("----------------------------------------------------------------------------------");

            CompareColumnName(source);

            WriteResultLog.WriteLog("----------------------------------------------------------------------------------");
            CompareColumnDataType(source);

            WriteResultLog.WriteLog("----------------------------------------------------------------------------------");


            //Console.WriteLine("you can swap source and desc in app.config by change key");
            Console.WriteLine("success");
            Console.ReadLine();
        }

        private static void CompareColumnDataType(List<TableStructureModel> source)
        {
            // compare column datatype
            WriteResultLog.WriteLog("Start Compare Column DataType");
            foreach (var t_s in source.GroupBy(x => x.Table_Name))
            {
                var result = SchemaCompare.CompareColumnDataType(t_s.Key);
                if (result.IsDiff)
                {
                    WriteResultLog.WriteLog(string.Format("Column DataType {0} in table {1} are difference", result.CoumnDataTypeDiffs, t_s.Key));
                    var datatype_diff = SchemaCompare.GetColumnDataTypeDiff(t_s.Key);
                    WriteResultLog.WriteLog("Column DataType not contain in source table are below");
                    foreach (var item in datatype_diff)
                    {
                        WriteResultLog.WriteLog("\t" + " Column Name " + item.ColName + "Data Type " + item.DataType);
                    }
                }
            }

            WriteResultLog.WriteLog("----------------------------------------------------------------------------------");

        }

        private static void CompareColumnName(List<TableStructureModel> source)
        {
            WriteResultLog.WriteLog("Start Compare Column Name");
            // compare column name
            foreach (var t_s in source.GroupBy(x => x.Table_Name))
            {
                var result = SchemaCompare.CompareColumnName(t_s.Key);
                if (result.IsDiff)
                {
                    WriteResultLog.WriteLog(string.Format("Column {0} in table {1} are difference", result.CoumnNameDiffs, t_s.Key));
                    var list_of_diff_column = SchemaCompare.GetColumnNameDiff(t_s.Key);
                    WriteResultLog.WriteLog("Column not contain in source table are below");
                    foreach (var item in list_of_diff_column)
                    {
                        WriteResultLog.WriteLog("\t" + item);
                    }
                }

            }
            WriteResultLog.WriteLog("----------------------------------------------------------------------------------");
        }
    }
}
