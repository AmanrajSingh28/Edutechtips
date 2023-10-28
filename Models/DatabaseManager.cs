using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ProgrammingZone.Models
{
    public class DatabaseManager
    {
        SqlConnection con = new SqlConnection(@"Data Source=AYUSH-SINGH-CHA;Initial Catalog=ProgramminZone;Integrated Security=True");
        public bool InsertUpdateAndDelete(string command)
        {
            SqlCommand cmd = new SqlCommand(command, con);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataTable GetAllRecord(string command)
        {
            SqlDataAdapter sa = new SqlDataAdapter(command,con);
            DataTable dt = new DataTable();
            sa.Fill(dt);
            return dt;
        }
    }
}
