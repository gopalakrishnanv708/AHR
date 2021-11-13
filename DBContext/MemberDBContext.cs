using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using AHR.Models;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AHR.DBContext
{
    public class MemberDBContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment hostEnvironment;
        AccountDBContext AccountDBContext;
        private static IConfiguration Configuration = new Config().Configuration;
        private DataTable FundsReceived;
        private DataTable FundsSpent;
        private DataTable FamilyDetails;
        private DataTable FamilyActivity;

        public MemberDBContext(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            AccountDBContext = new AccountDBContext();
            _httpContextAccessor = httpContextAccessor;
            hostEnvironment = webHostEnvironment;
        }

        public DataTable GetFundsReceived()
        {
            FundsReceived = new DataTable();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_g_income", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int, 50));
                cmd.Parameters["@user_sk"].Value = AccountDBContext.GetUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                adapter.Fill(FundsReceived);
            }

            return FundsReceived;
        }

        public DataTable GetFundsSpent()
        {
            FundsSpent = new DataTable();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_g_expense", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int, 50));
                cmd.Parameters["@user_sk"].Value = AccountDBContext.GetUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                adapter.Fill(FundsSpent);
            }

            return FundsSpent;
        }

        public DataTable GetFamilyDetails()
        {
            FamilyDetails = new DataTable();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_g_family", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int, 50));
                cmd.Parameters["@user_sk"].Value = AccountDBContext.GetUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                adapter.Fill(FamilyDetails);
            }

            return FamilyDetails;
        }

        public DataTable GetFamilyActivity()
        {
            FamilyActivity = new DataTable();

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_g_family_activity", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@user_sk", SqlDbType.Int, 50));
                cmd.Parameters["@user_sk"].Value = AccountDBContext.GetUserId(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                adapter.Fill(FamilyActivity);
            }

            return FamilyActivity;
        }

        public string GetPaymentMethodName(int id)
        {
            DataTable dt = new DataTable();

            string payment_id = "";

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_payment_method", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@method_sk", SqlDbType.Int));
                cmd.Parameters["@method_sk"].Value = id;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                payment_id = Convert.ToString(dataRow.ItemArray[0]);
            }

            return payment_id;
        }

        public string GetFamilyName(int id)
        {
            DataTable dt = new DataTable();

            string payment_id = "";

            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("AHRDB")))
            using (SqlCommand cmd = new SqlCommand("p_get_family_name_for_sk", conn))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@method_sk", SqlDbType.Int));
                cmd.Parameters["@method_sk"].Value = id;

                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                payment_id = Convert.ToString(dataRow.ItemArray[0]);
            }

            return payment_id;
        }

        public string GetFundsReceivedTotal(DataTable data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("amount", typeof(decimal));

            int[] income_sk = new int[data.Rows.Count];

            for (int i = 0; i < data.Rows.Count; i++)
            {
                income_sk[i] = Convert.ToInt32(data.Rows[i][0]);

            }

            income_sk = income_sk.ToList().Distinct().ToArray();

            for (int i = 0; i < income_sk.Length; i++)
            {
                DataRow[] dr = data.Select("income_sk = " + income_sk[i]);

                DateTime date = Convert.ToDateTime(dr[0].ItemArray[2]);

                dt.Rows.Add(Convert.ToDecimal(Convert.ToString(dr[0].ItemArray[3]).Remove(Convert.ToString(dr[0].ItemArray[3]).Length - 2)));
            }

            string total = Convert.ToString(dt.Compute("SUM(amount)", string.Empty));

            return total;

        }

        public string GetFundsSpentTotal(DataTable data)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("amount", typeof(decimal));

            int[] income_sk = new int[data.Rows.Count];

            for (int i = 0; i < data.Rows.Count; i++)
            {
                income_sk[i] = Convert.ToInt32(data.Rows[i][0]);

            }

            income_sk = income_sk.ToList().Distinct().ToArray();

            for (int i = 0; i < income_sk.Length; i++)
            {
                DataRow[] dr = data.Select("expense_sk = " + income_sk[i]);

                DateTime date = Convert.ToDateTime(dr[0].ItemArray[2]);

                dt.Rows.Add(Convert.ToDecimal(Convert.ToString(dr[0].ItemArray[3]).Remove(Convert.ToString(dr[0].ItemArray[3]).Length - 2)));
            }

            string total = Convert.ToString(dt.Compute("SUM(amount)", string.Empty));

            return total;

        }

        public List<FundsReceived> ProcessFundsReceived(DataTable data)
        {
            List<FundsReceived> res = new List<FundsReceived>();

            int[] income_sk = new int[data.Rows.Count];

            for (int i = 0; i < data.Rows.Count; i++)
            {
                income_sk[i] = Convert.ToInt32(data.Rows[i][0]);

            }

            income_sk = income_sk.ToList().Distinct().ToArray();

            for (int i = 0; i < income_sk.Length; i++)
            {
                DataRow[] dr = data.Select("income_sk = " + income_sk[i]);

                DateTime date = Convert.ToDateTime(dr[0].ItemArray[2]);

                FundsReceived funds = new FundsReceived();
                {
                    funds.PaymentDate = date.ToString("dd-MMM-yyyy");
                    funds.PaymentAmount = Convert.ToString(dr[0].ItemArray[3]).Remove(Convert.ToString(dr[0].ItemArray[3]).Length - 2);
                    funds.PaymentMethod = GetPaymentMethodName(Convert.ToInt32(dr[0].ItemArray[1]));
                }

                res.Add(funds);
            }

            return res;
        }

        public List<FundsSpent> ProcessFundsSpent(DataTable data)
        {
            List<FundsSpent> res = new List<FundsSpent>();

            int[] expense_sk = new int[data.Rows.Count];

            for (int i = 0; i < data.Rows.Count; i++)
            {
                expense_sk[i] = Convert.ToInt32(data.Rows[i][0]);

            }

            expense_sk = expense_sk.ToList().Distinct().ToArray();

            for (int i = 0; i < expense_sk.Length; i++)
            {

                DataRow[] dr = data.Select("expense_sk = " + expense_sk[i]);

                DateTime date = Convert.ToDateTime(dr[0].ItemArray[2]);


                FundsSpent funds = new FundsSpent();
                {
                    funds.FamilyNo = Convert.ToInt32(dr[0].ItemArray[1]);
                    funds.FamilyName = GetFamilyName(Convert.ToInt32(dr[0].ItemArray[1]));
                    funds.ExpenseDate = date.ToString("dd-MMM-yyyy");
                    funds.ExpenseAmount = Convert.ToString(dr[0].ItemArray[3]).Remove(Convert.ToString(dr[0].ItemArray[3]).Length - 2);
                    funds.ExpensePurpose = Convert.ToString(dr[0].ItemArray[4]);
                    funds.ProofDocument = new List<string>();
                    {
                        for (int j = 0; j < dr.Length; j++)
                        {
                            string wwwRootPath = hostEnvironment.WebRootPath;
                            string path = Path.Combine(wwwRootPath + "/images/user_images/");
                            string filename = Path.GetFileNameWithoutExtension(path + Convert.ToString(dr[j].ItemArray[8]));
                            string ext = Path.GetExtension(path + Convert.ToString(dr[j].ItemArray[8]));
                            funds.ProofDocument.Add(filename.Remove(filename.Length - 9) + ext);

                        }
                    }

                }

                res.Add(funds);
            }

            return res;
        }


    }
}
