using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DTO;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Configuration;

namespace DAO
{
    public class DataConnect
    {
        SqlConnection Connection { get; set; }
        private string connectionstring = ConfigurationManager.ConnectionStrings["WebConnection"].ConnectionString;
        private SqlTransaction tran = null;

        /// <summary>
        /// Hàm mở kết nối data
        /// </summary>
        void Connect(bool IsHasTran = false)
        {
            try
            {
                if (Connection == null)
                    Connection = new SqlConnection(connectionstring);
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                Connection.Open();
                if (IsHasTran == true)
                    tran = Connection.BeginTransaction();
                else
                    tran = null;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm đóng kết nối data
        /// </summary>
        void Disconnect()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
                Connection.Close();
        }

        /// <summary>
        /// Hàm thực thi usp
        /// </summary>
        /// <param name="strusp_name">Tên usp</param>
        /// <param name="objKeyWords">Danh sách tham số</param>
        /// <returns></returns>
        public DataTable callUSP(string strusp_name, object[] objKeyWords)
        {
            DataTable dtbResult = new DataTable();
            Connect();
            try
            {
                //usp_name bỏ vào command
                SqlCommand command = new SqlCommand(strusp_name, Connection);
                command.CommandType = CommandType.StoredProcedure;

                //Vòng lặp danh sách thông số
                if(objKeyWords != null)
                {
                    int sizeParameter = objKeyWords.Count();
                    for (int i = 0; i < sizeParameter; i += 2)
                    {
                        command.Parameters.AddWithValue(Convert.ToString(objKeyWords[i]), Convert.ToString(objKeyWords[i + 1]));
                    }
                }
                
                
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dtbResult);
            }
            catch (SqlException ex)
            {
                return null;
            }
            finally
            {
                Disconnect();
            }

            return dtbResult;
        }

        /// <summary>
        /// Thực thi list store (có kèm theo tran)
        /// </summary>
        /// <param name="Lstusp_name">DS tên store</param>
        /// <param name="LstobjKeyWords">DS tham số</param>
        /// <param name="IsReturnKey">Đánh dấu có dùng tham số trả ra hay không</param>
        /// <returns></returns>
        public bool exeUSP(List<string> Lstusp_name, List<object[]> LstobjKeyWords, bool IsReturnKey = false)
        {
            Connect(true);
            string ID = string.Empty;
            try
            {
                if (Lstusp_name.Count != LstobjKeyWords.Count)
                {
                    return false;
                }
                else
                {
                    int intCountLstusp_name = Lstusp_name.Count;
                    for (int i = 0; i < intCountLstusp_name; i++)
                    {
                        //usp_name bỏ vào command
                        SqlCommand command = new SqlCommand(Lstusp_name[i], Connection, tran);
                        command.CommandType = CommandType.StoredProcedure;

                        //Vòng lặp danh sách thông số
                        int sizeParameter = LstobjKeyWords[i].Count();
                        for (int j = 0; j < sizeParameter; j += 2)
                        {
                            command.Parameters.AddWithValue(Convert.ToString(LstobjKeyWords[i][j]), (Convert.ToString(LstobjKeyWords[i][j + 1]) == "INFINITY" ? ID : Convert.ToString(LstobjKeyWords[i][j + 1])));
                        }

                        if (IsReturnKey == true)
                        {
                            var temp = command.ExecuteScalar();
                            ID = temp.ToString();
                            IsReturnKey = false;
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                tran.Commit();
                Disconnect();
            }
            return true;
        }

        public bool uspExecuteNonQuery(string strusp_name, object[] objKeyWords)
        {
            Connect(true);
            string ID = string.Empty;
            try
            {
                //usp_name bỏ vào command
                SqlCommand command = new SqlCommand(strusp_name, Connection,tran);
                command.CommandType = CommandType.StoredProcedure;

                //Vòng lặp danh sách thông số
                if (objKeyWords != null)
                {
                    int sizeParameter = objKeyWords.Count();
                    for (int i = 0; i < sizeParameter; i += 2)
                    {
                        command.Parameters.AddWithValue(Convert.ToString(objKeyWords[i]), Convert.ToString(objKeyWords[i + 1]));
                    }
                }
                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                tran.Commit();
                Disconnect();
            }
            return true;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Phát sinh chuỗi ngẫu nhiêm
        /// </summary>
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool SendMail(string strToEmail, string strSubject, string strBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(strToEmail);
                mail.From = new MailAddress("phamhuutai95.test@gmail.com");
                mail.Subject = strSubject;
                mail.Body = strBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential
                     ("phamhuutai95.test@gmail.com", "abc123456789");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
