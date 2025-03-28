﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace B8
{
    class konekcija
    {
        public static SqlConnection getConnection()
        {
            string connectionstring = @"Data Source = DESKTOP-R0GJOHE\SQLEXPRESS;Initial Catalog = B8; Integrated Security = SSPI; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            SqlConnection cnn = new SqlConnection(connectionstring);
            cnn.Open();
            return cnn;
        }
        public static void newTableRecord(string table, Dictionary<string, object> parameters)
        {
            string sql = "INSERT INTO " + table;
            string columns = "";
            string values = "";
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if ("".Equals(columns))
                {
                    columns = "(" + parameter.Key;
                    values = "(@" + parameter.Key;
                }
                else
                {
                    columns = columns + "," + parameter.Key;
                    values = values + ",@" + parameter.Key;
                }

            }

            columns = columns + ")";
            values = values + ")";
            sql = sql + " " + columns + " VALUES " + values;
            Console.WriteLine("sql - " + sql);
            SqlConnection conn = getConnection();
            SqlCommand command = new SqlCommand(sql, conn);
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                command.Parameters.Add("@" + parameter.Key, parameter.Value);
            }
            command.ExecuteNonQuery();
            conn.Close();
        }
        public static List<snimi> getRecordsFromDB(string sql, Dictionary<string, object> parameters)
        {
            List<snimi> result = new List<snimi>();
            SqlConnection conn = getConnection();
            SqlCommand command = new SqlCommand("@" + sql, conn);
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.Add(parameter.Key, parameter.Value);
                }
            }

            SqlDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    snimi r = new snimi();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        r.addColumn(dr.GetName(i), dr.GetValue(i));
                    }

                }
            }
            return result;
        }
    }
}
