using CompareSDF_3_5.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace CompareSDF_3_5
{
    public class SchemaInfo
    {
        public static string GetDatabaseSourcePath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["sdf_1"];
        }

        public static string GetDatabaseDescPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["sdf_2"];
        }
        public static List<TableStructureModel> GetSourceTableSchemas()
        {
            string sdf_1 = System.Configuration.ConfigurationManager.AppSettings["sdf_1"];

            using (SqlCeConnection conn_sdf1 = new SqlCeConnection(sdf_1))
            {
                if (conn_sdf1.State != System.Data.ConnectionState.Open)
                {
                    conn_sdf1.Open();
                    Console.WriteLine("Open connection sdf 1");
                    using (var cmd = new SqlCeCommand("select * from INFORMATION_SCHEMA.COLUMNS order by Table_name", conn_sdf1))
                    {
                        SqlCeDataReader reader = cmd.ExecuteReader();
                        List<TableStructureModel> tables = new List<TableStructureModel>();
                        while (reader.Read())
                        {
                            var model = new TableStructureModel();

                            model.Table_Name = reader.GetString(2);
                            model.Column_Name = reader.GetString(3);
                            model.Is_Nullable = (reader.GetString(10) == "YES") ? true : false;
                            model.Data_Type_Name = reader.GetString(11);

                            tables.Add(model);
                            //model.Data_Type = reader.GetString(2);
                            //Console.WriteLine(table_name);
                        }
                        return tables;
                    }
                }
                else
                {
                    Console.WriteLine("Cannot open connection");
                }
            }
            return null;
        }
        public static List<TableStructureModel> GetDescTableSchemas()
        {
            string sdf_2 = System.Configuration.ConfigurationManager.AppSettings["sdf_2"];

            using (SqlCeConnection conn_sdf2 = new SqlCeConnection(sdf_2))
            {
                if (conn_sdf2.State != System.Data.ConnectionState.Open)
                {
                    conn_sdf2.Open();
                    Console.WriteLine("Open connection sdf 2");
                    using (var cmd = new SqlCeCommand("select * from INFORMATION_SCHEMA.COLUMNS order by Table_name", conn_sdf2))
                    {
                        SqlCeDataReader reader = cmd.ExecuteReader();
                        List<TableStructureModel> tables = new List<TableStructureModel>();
                        while (reader.Read())
                        {
                            var model = new TableStructureModel();

                            model.Table_Name = reader.GetString(2);
                            model.Column_Name = reader.GetString(3);
                            model.Is_Nullable = (reader.GetString(10) == "YES") ? true : false;
                            model.Data_Type_Name = reader.GetString(11);

                            tables.Add(model);
                            //model.Data_Type = reader.GetString(2);
                            //Console.WriteLine(table_name);
                        }
                        return tables;
                    }
                }
                else
                {
                    Console.WriteLine("Cannot open connection");
                }
            }
            return null;
        }
    }
}
