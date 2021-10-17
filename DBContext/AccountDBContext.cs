using AHR.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AHR.DBContext
{
    public class AccountDBContext
    {
        public string Role;
        private Login login;
        private static IConfiguration Configuration = new Config().Configuration;

        public AccountDBContext()
        {
            login = new Login();
        }

        public DataTable GetLoginDetails(String email)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_customer", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 500));
                adapter.SelectCommand.Parameters["@email"].Value = email;
              
                adapter.Fill(dt);
            }

            return dt;
        }

        public bool CreateUser(Register register)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                using (SqlCommand cmd = new SqlCommand("p_insert_customer"))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.Char, 50));
                    cmd.Parameters["@username"].Value = register.Name;

                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 500));
                    cmd.Parameters["@email"].Value = register.Email;

                    cmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.VarChar, 15));
                    cmd.Parameters["@PhoneNumber"].Value = register.MobileNumber;

                    cmd.Parameters.Add(new SqlParameter("@usertype", SqlDbType.Char, 1));
                    cmd.Parameters["@usertype"].Value = "D";

                    cmd.Parameters.Add(new SqlParameter("@userpasswd", SqlDbType.Char, 50));
                    cmd.Parameters["@userpasswd"].Value = register.Password;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        public bool ReadDataTable(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                login.Email = dataRow.ItemArray[3].ToString().Trim();
                login.Password = dataRow.ItemArray[5].ToString().Trim();
                Role = dataRow.ItemArray[1].ToString().Trim();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ValidateCredentials(Login loginfromuser)
        {

            if (ReadDataTable(GetLoginDetails(loginfromuser.Email)))
            {
                if (loginfromuser.Email.Equals(login.Email) && loginfromuser.Password.Equals(login.Password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
