using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Text;

public class DB : MonoBehaviour
{
    public static string conStr = "Data Source=145.239.80.7;Initial Catalog=DB_SZYRROGI;User ID=szyrrogi;Password=szyrrogi;Connect Timeout=10;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    void Start()
    {
        // Zarejestruj dostawcę stron kodowych
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // Połącz się z bazą danych
        Connect(conStr);
    }

    public static SqlConnection Connect(string conStr)
    {
        SqlConnection c = new SqlConnection(conStr);
        c.Open();
        return c;
    }

    private static SqlConnection _con;
    public static SqlConnection con
    {
        get
        {
            if (_con == null)
            {
                DoConnect(ref _con);
            }
            return _con;
        }
    }

    private static void DoConnect(ref SqlConnection con)
    {
        con = new SqlConnection(conStr);
        con.Open();
    }

    public static bool execSQL(SqlCommand sqlCmd)
    {
        int success = 0;
        success = sqlCmd.ExecuteNonQuery();  // 1 sukces
        return success >= 1;
    }

    public static DataSet selectSQL(SqlCommand cmd)
    {
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        adapter.Fill(ds);
        return ds;
    }

    public static SqlCommand commandSQL(SqlConnection con, string sql, params object[] values)
    {
        SqlCommand cmd = new SqlCommand(sql, con);
        if (values != null)
            for (int i = 0; i < values.Length; i++)
                if (values[i] == null)
                    cmd.Parameters.AddWithValue("p" + i.ToString(), DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("p" + i.ToString(), values[i]);
        return cmd;
    }
}