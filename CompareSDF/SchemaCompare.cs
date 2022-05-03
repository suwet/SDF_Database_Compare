using CompareSDF.Models;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareSDF
{
    public class SchemaCompare
    {
        static List<TableStructureModel> table_source;
        static List<TableStructureModel> table_desc;

        static List<TableContraintsModel> table_contraint_source = new List<TableContraintsModel>();
        static List<TableContraintsModel> table_contraint_dest = new List<TableContraintsModel>();
        public static void InitSchemaCompare(List<TableStructureModel> _source, List<TableStructureModel> _desc)
        {
            table_source = _source;
            table_desc = _desc;

            table_contraint_source.AddRange(SchemaInfo.GetSourceReferenceTableConstraints());
            table_contraint_dest.AddRange(SchemaInfo.GetDestReferenceTableConstraints());
            table_contraint_source.AddRange(SchemaInfo.GetSourceTableConstraints());
            table_contraint_dest.AddRange(SchemaInfo.GetDestTableConstraints());
        }
        public static int GetTableSourceCount()
        {
            return table_source.GroupBy(x => x.Table_Name).Select(x => x.Key).Count();
        }

        public static int GetTableDescCount()
        {
            return table_desc.GroupBy(x => x.Table_Name).Select(x => x.Key).Count();
        }
        public static int GetTableColumnDescCount(string tableName)
        {
            return table_desc.Where(w => w.Table_Name == tableName).Count();
        }

        public static int GetTableColumnSourceCount(string tableName)
        {
            return table_source.Where(w => w.Table_Name == tableName).Count();
        }

        public static List<string> GetTableNameDiff()
        {
            var s_tablename = table_source.GroupBy(x => x.Table_Name).Select(s => s.Key);
            var d_tablename = table_desc.GroupBy(x => x.Table_Name).Select(s => s.Key);

            return d_tablename.Except(s_tablename).Union(s_tablename.Except(d_tablename)).ToList();

        }

        public static List<string> GetColumnNameDiff(string tableName)
        {
            var source_col_name = table_source.Where(w => w.Table_Name == tableName)
                  .Select(s => s.Column_Name);

            var desc_col_name = table_desc.Where(w => w.Table_Name == tableName)
                  .Select(s => s.Column_Name);

            return desc_col_name.Except(source_col_name).Union(source_col_name.Except(desc_col_name)).ToList();

        }
        /// <summary>
        ///  if table name are the same and column count are the same
        ///  check name of column
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static TableDiff CompareColumnName(string tableName)
        {
            var source_col_name = table_source.Where(w => w.Table_Name == tableName)
                  .Select(s => s.Column_Name);

            var desc_col_name = table_desc.Where(w => w.Table_Name == tableName)
                  .Select(s => s.Column_Name);

            if (source_col_name.SequenceEqual(desc_col_name))
            {
                return new TableDiff()
                {
                    TableName = tableName,
                    IsDiff = false
                };
            }
            else if (source_col_name.Count() == desc_col_name.Count())
            {
                // check what column diff
                string colname_diff = string.Empty;
                foreach (var item in source_col_name)
                {
                    if (desc_col_name.Contains(item) == false)
                    {
                        colname_diff += item + ",";
                    }
                }
                return new TableDiff()
                {
                    TableName = tableName,
                    IsDiff = true,
                    CoumnNameDiffs = colname_diff,
                    Remark = "Column count are equal but name of column are difference"
                };
            }
            else
            {
                return new TableDiff()
                {
                    TableName = tableName,
                    IsDiff = true,
                    CoumnNameDiffs = "Column count are not equal",
                    Remark = "Column count are not equal"
                };
            }
        }

        /// <summary>
        ///  if table name are the same and column count are the same
        ///  check name of column
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static TableDiff CompareColumnDataType(string tableName)
        {
            var source_col_datatype = table_source.Where(w => w.Table_Name == tableName)
                  .Select(s => new { DataType = s.Data_Type, ColName = s.Column_Name });

            var desc_col_datatype = table_desc.Where(w => w.Table_Name == tableName)
                  .Select(s => new { DataType = s.Data_Type, ColName = s.Column_Name });

            if (source_col_datatype.Select(s => s.DataType).SequenceEqual(desc_col_datatype.Select(s => s.DataType)))
            {
                return new TableDiff()
                {
                    TableName = tableName,
                    IsDiff = false
                };
            }
            else if (source_col_datatype.Count() == desc_col_datatype.Count())
            {
                // check what column diff
                string col_datatype_diff = string.Empty;
                foreach (var item in source_col_datatype)
                {
                    if (source_col_datatype.Select(s => s.DataType).Contains(item.DataType) == false)
                    {
                        col_datatype_diff += item.ColName + " " + item.DataType + ",";
                    }
                }
                return new TableDiff()
                {
                    TableName = tableName,
                    IsDiff = true,
                    CoumnDataTypeDiffs = col_datatype_diff,
                    Remark = "Column count are equal but DataType of column  are difference"
                };
            }
            else
            {
                return new TableDiff()
                {
                    TableName = tableName,
                    IsDiff = true,
                    CoumnDataTypeDiffs = "Column count are not equal",
                    Remark = "Column count are not equal"
                };
            }
        }

        public static List<DataTypeDiff> GetColumnDataTypeDiff(string tableName)
        {
            var source_col_type = table_source.Where(w => w.Table_Name == tableName)
                  .Select(s => new DataTypeDiff { DataType = s.Data_Type_Name, ColName = s.Column_Name, TableName = tableName });

            var desc_col_type = table_desc.Where(w => w.Table_Name == tableName)
                  .Select(s => new DataTypeDiff { DataType = s.Data_Type_Name, ColName = s.Column_Name, TableName = tableName });


            return source_col_type.Except(desc_col_type).Union(desc_col_type.Except(source_col_type)).ToList();
        }

        public static List<ContraintDiff> GetContraintDiff(string tableName)
        {
            var source_contraint = table_contraint_source.Where(w => w.TableName == tableName)
                  .Select(s => new { ContraintType = s.ConstraintType, TableName = tableName });

            var dest_contraint = table_contraint_dest.Where(w => w.TableName == tableName)
                  .Select(s => new { ContraintType = s.ConstraintType, TableName = tableName });

            //var a = source_contraint.Except(dest_contraint).ToList();
            //var b = dest_contraint.Except(source_contraint).ToList();
            //var c = a.Union(b).ToList();
            return source_contraint.Except(dest_contraint).Union(dest_contraint.Except(source_contraint)).GroupBy(x => x.ContraintType).Select(x => new ContraintDiff { ContraintType = x.Key }).ToList();
        }

        public static List<ContraintDiff> GetReferenceContraintDiff(string tableName)
        {
            var source_contraint = table_contraint_source.Where(w => w.ConstraintTableName == tableName)
                  .Select(s => new { ContraintTableName = s.ConstraintTableName, UniqContraintTableName = s.UniqConstraintName });

            var dest_contraint = table_contraint_dest.Where(w => w.ConstraintTableName == tableName)
                  .Select(s => new { ContraintTableName = s.ConstraintTableName, UniqContraintTableName = s.UniqConstraintName });


            return source_contraint.Except(dest_contraint).Union(dest_contraint.Except(source_contraint)).GroupBy(x => x.UniqContraintTableName).Select(x => new ContraintDiff { UniqContraintTableName = x.Key }).ToList();
        }
    }
}
