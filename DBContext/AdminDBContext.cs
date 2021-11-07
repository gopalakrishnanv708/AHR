using AHR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace AHR.DBContext
{
    public class AdminDBContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        AccountDBContext AccountDBContext;
        private static IConfiguration Configuration = new Config().Configuration;

        public AdminDBContext(IHttpContextAccessor httpContextAccessor)
        {
            AccountDBContext = new AccountDBContext();
            _httpContextAccessor = httpContextAccessor;
        }

        public bool UpdateInfo(ContactUs contactUs)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                using (SqlCommand cmd = new SqlCommand("p_insert_enquiries"))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.Char, 50));
                    cmd.Parameters["@username"].Value = contactUs.Name;

                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 500));
                    cmd.Parameters["@email"].Value = contactUs.Email;

                    cmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.VarChar, 15));
                    cmd.Parameters["@PhoneNumber"].Value = contactUs.MobileNumber;

                    cmd.Parameters.Add(new SqlParameter("@message", SqlDbType.Char, 50));
                    cmd.Parameters["@message"].Value = contactUs.Message;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<string> GetPaymentMethod()
        {
            DataTable dt = new DataTable();

            List<string> payment_name_list = new List<string>();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_payment_method_name", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataRow = dt.Rows[i];
                    payment_name_list.Add(Convert.ToString(dataRow.ItemArray[0]));
                }


            }

            return payment_name_list;
        }

        public List<string> GetUserEmail()
        {
            DataTable dt = new DataTable();

            List<string> user_email_list = new List<string>();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_user_email", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataRow = dt.Rows[i];
                    user_email_list.Add(Convert.ToString(dataRow.ItemArray[0]));
                }


            }

            return user_email_list;
        }

        public int GetPaymentMethodId(string MethodName)
        {
            DataTable dt = new DataTable();

            int payment_id = 0;

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_payment_method_sk", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@method_name", SqlDbType.Char, 50));
                cmd.Parameters["@method_name"].Value = MethodName;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                payment_id = Convert.ToInt32(dataRow.ItemArray[0]);
            }

            return payment_id;
        }

        public int GetFamilyNameId(string FamilyName)
        {
            DataTable dt = new DataTable();

            int familyName_id = 0;

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_family_name_sk", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@family_name", SqlDbType.Char, 50));
                cmd.Parameters["@family_name"].Value = FamilyName;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                familyName_id = Convert.ToInt32(dataRow.ItemArray[0]);
            }

            return familyName_id;
        }

        public int GetUserIdWithFamilyId(int FamilyId)
        {
            DataTable dt = new DataTable();

            int user_id = 0;

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_user_sk", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@family_sk", SqlDbType.Int, 50));
                cmd.Parameters["@family_sk"].Value = FamilyId;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                user_id = Convert.ToInt32(dataRow.ItemArray[0]);
            }

            return user_id;
        }

        public bool ValidateOtherEmail(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                using (SqlCommand cmd = new SqlCommand("p_get_other_email", conn))
                {

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 200));
                    cmd.Parameters["@email"].Value = email;

                    adapter.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }

           
        }

        public List<string> GetFamilyName()
        {
            DataTable dt = new DataTable();

            List<string> family_name_list = new List<string>();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_family_name", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataRow = dt.Rows[i];
                    family_name_list.Add(Convert.ToString(dataRow.ItemArray[0]));
                }

            }

            return family_name_list;
        }


        public bool UpdateIncome(Income income, DataTable imgTable)
        {
            if (GetPaymentMethodId(income.PaymentMethod) > 0)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                    using (SqlCommand cmd = new SqlCommand("p_iud_income"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@operation_mode", SqlDbType.Char, 1));
                        cmd.Parameters["@operation_mode"].Value = "I";

                        cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int));
                        cmd.Parameters["@user_sk"].Value = AccountDBContext.GetUserId(income.DonorEmail);

                        cmd.Parameters.Add(new SqlParameter("@payment_method_sk", SqlDbType.Int));
                        cmd.Parameters["@payment_method_sk"].Value = GetPaymentMethodId(income.PaymentMethod);

                        cmd.Parameters.Add(new SqlParameter("@tran_date", SqlDbType.Date));
                        cmd.Parameters["@tran_date"].Value = income.PaymentDate;

                        cmd.Parameters.Add(new SqlParameter("@amount_recd", SqlDbType.Money));
                        cmd.Parameters["@amount_recd"].Value = income.PaymentAmount;

                        cmd.Parameters.Add(new SqlParameter("@entry_user_sk", SqlDbType.Int));
                        cmd.Parameters["@entry_user_sk"].Value = AccountDBContext.GetUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                        cmd.Parameters.Add(new SqlParameter("@udtt_image", SqlDbType.Structured));
                        cmd.Parameters["@udtt_image"].Value = imgTable;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool UpdateExpense(Expense expense, DataTable imgTable)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                using (SqlCommand cmd = new SqlCommand("p_iud_expense"))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@operation_mode", SqlDbType.Char, 1));
                    cmd.Parameters["@operation_mode"].Value = "I";

                    cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int));
                    cmd.Parameters["@user_sk"].Value = GetUserIdWithFamilyId(GetFamilyNameId(expense.FamilyName));

                    cmd.Parameters.Add(new SqlParameter("@family_sk", SqlDbType.Int));
                    cmd.Parameters["@family_sk"].Value = GetFamilyNameId(expense.FamilyName);

                    cmd.Parameters.Add(new SqlParameter("@tran_date", SqlDbType.Date));
                    cmd.Parameters["@tran_date"].Value = expense.ExpenseDate;

                    cmd.Parameters.Add(new SqlParameter("@amount_expense", SqlDbType.Money));
                    cmd.Parameters["@amount_expense"].Value = expense.ExpenseAmount;

                    cmd.Parameters.Add(new SqlParameter("@purpose_desc", SqlDbType.VarChar));
                    cmd.Parameters["@purpose_desc"].Value = expense.ExpensePurpose;

                    cmd.Parameters.Add(new SqlParameter("@entry_user_sk", SqlDbType.Int));
                    cmd.Parameters["@entry_user_sk"].Value = AccountDBContext.GetUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    cmd.Parameters.Add(new SqlParameter("@udtt_image", SqlDbType.Structured));
                    cmd.Parameters["@udtt_image"].Value = imgTable;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateActivity(AhrActivity ahrActivity, DataTable imgTable, DataTable vidTable)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                using (SqlCommand cmd = new SqlCommand("p_iud_family_activity"))
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@operation_mode", SqlDbType.Char, 1));
                    cmd.Parameters["@operation_mode"].Value = "I";

                    cmd.Parameters.Add(new SqlParameter("@family_sk", SqlDbType.Int));
                    cmd.Parameters["@family_sk"].Value = GetFamilyNameId(ahrActivity.FamilyName);

                    cmd.Parameters.Add(new SqlParameter("activity_date", SqlDbType.Date));
                    cmd.Parameters["activity_date"].Value = ahrActivity.ActivityDate;

                    cmd.Parameters.Add(new SqlParameter("@activity_desc", SqlDbType.VarChar));
                    cmd.Parameters["@activity_desc"].Value = ahrActivity.ActivityDesc;

                    cmd.Parameters.Add(new SqlParameter("@entry_user_sk", SqlDbType.Int));
                    cmd.Parameters["@entry_user_sk"].Value = AccountDBContext.GetUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    cmd.Parameters.Add(new SqlParameter("@udtt_image", SqlDbType.Structured));
                    cmd.Parameters["@udtt_image"].Value = imgTable;

                    cmd.Parameters.Add(new SqlParameter("@udtt_video", SqlDbType.Structured));
                    cmd.Parameters["@udtt_video"].Value = vidTable;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool AddUser(AddUser addUser)
        {
            try
            {
                if(ValidateOtherEmail(addUser.EmailFirst) && ValidateOtherEmail(addUser.EmailSecond))
                {
                    using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                    using (SqlCommand cmd = new SqlCommand("p_insert_customer"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.Char, 50));
                        cmd.Parameters["@username"].Value = addUser.Name;

                        cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 500));
                        cmd.Parameters["@email"].Value = addUser.Email;

                        cmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.VarChar, 15));
                        cmd.Parameters["@PhoneNumber"].Value = addUser.MobileNumber;

                        cmd.Parameters.Add(new SqlParameter("@usertype", SqlDbType.Char, 1));
                        cmd.Parameters["@usertype"].Value = addUser.UserType;

                        cmd.Parameters.Add(new SqlParameter("@userpasswd", SqlDbType.Char, 50));
                        cmd.Parameters["@userpasswd"].Value = addUser.Password;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }

                    using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                    using (SqlCommand cmd = new SqlCommand("p_insert_other_emails"))
                    {
                        if (!String.IsNullOrEmpty(addUser.EmailFirst))
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int));
                            cmd.Parameters["@user_sk"].Value = AccountDBContext.GetUserId(addUser.Email);

                            cmd.Parameters.Add(new SqlParameter("@email_id", SqlDbType.VarChar, 200));
                            cmd.Parameters["@email_id"].Value = addUser.EmailFirst;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }

                    }

                    using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
                    using (SqlCommand cmd = new SqlCommand("p_insert_other_emails"))
                    {

                        if (!String.IsNullOrEmpty(addUser.EmailSecond))
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int));
                            cmd.Parameters["@user_sk"].Value = AccountDBContext.GetUserId(addUser.Email);

                            cmd.Parameters.Add(new SqlParameter("@email_id", SqlDbType.VarChar, 200));
                            cmd.Parameters["@email_id"].Value = addUser.EmailSecond;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
