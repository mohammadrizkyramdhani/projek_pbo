using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace projek_kyyme
{
    internal class CRUD
    {
        private static string getConnectionString()
        {
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string db = "Database=crud;";
            string user = "Username= postgres;";
            string pass = "Password=mafia0890;";

            string conString = string.Format("{0}{1}{2}{3}{4}", host, port, db, user, pass);

            return conString;
        }

        public static NpgsqlConnection con = new NpgsqlConnection(getConnectionString());
        public static NpgsqlCommand cmd = default(NpgsqlCommand);
        public static string sql = string.Empty;

        public static DataTable PerformCRUD(NpgsqlCommand com)
        {
            NpgsqlDataAdapter da = default(NpgsqlDataAdapter);
            DataTable dt = new DataTable();

            try
            {
                da = new NpgsqlDataAdapter();
                da.SelectCommand = com;
                con.Open();
                da.Fill(dt);
                con.Close();


                return dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message, "Perform CRUD Operation Failed : Rizky Ramdhani",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                dt = null;
            }

            return dt;

        }
    }
}
