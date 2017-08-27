
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;

namespace DAO
{
    public class LoginDAO
    {
        /// <summary>
        /// Hàm kiểm tra tài khoản đăng nhập
        /// </summary>
        /// <param name="objUserDTO">Thông tin người dùng</param>
        /// <returns></returns>
        public bool loginCheck(ref UserDTO objUserDTO)
        {
            
            object[] objKeyWords = new[] {
                "@USERNAME", objUserDTO.Username,
                "@PASSWORD", DataConnect.MD5Hash(objUserDTO.Password)
            };

            DataTable dtbLogin = new DataConnect().callUSP("USP_LOGIN", objKeyWords);
            //Nếu tồn tại dòng dữ liệu chứng tỏ đăng nhập thành công
            if (dtbLogin != null && dtbLogin.Rows.Count > 0)
            {
                //Lấy ra những thông tin về người dùng cần thiết
                objUserDTO.FullName = Convert.ToString(dtbLogin.Rows[0]["FULLNAME"]).Trim();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Lấy danh sách quyền theo người dùng
        /// </summary>
        /// <param name="strUsername">Mã người dùng</param>
        /// <returns></returns>
        public DataTable GetKeyPermission(string strUsername)
        {
            object[] objKeyWords = new[] {
                "@Username", strUsername
            };

            DataTable dtbKeyPermission = new DataConnect().callUSP("USP_KEYPERMISSIONDATA", objKeyWords);
            if (dtbKeyPermission != null && dtbKeyPermission.Rows.Count != 0)
            {
                return dtbKeyPermission;
            }
            return null;
        }


        public static bool CheckKeyPermission(string strUsername, string strKey)
        {
            if(strUsername == null | strUsername == "")
            {
                return false;
            }
            object[] objKeyWords = new[] {
                "@USERNAME", strUsername,
                "@KEY", strKey
            };

            DataTable dtbKeyPermission = new DataConnect().callUSP("USP_CHECKKEYPERMISSION", objKeyWords);
            if (dtbKeyPermission != null && dtbKeyPermission.Rows.Count != 0)
            {
                return true;
            }
            return false;
        }


        public string NameLoginAcc(string useName)
        {

            object[] objKeyWords = new[] {
                "@USERNAME", useName
            };

            DataTable dtbLoginData = new DataConnect().callUSP("usp_loginname", objKeyWords);
            //Nếu tồn tại dòng dữ liệu chứng tỏ đăng nhập thành công
            if (dtbLoginData != null && dtbLoginData.Rows.Count != 0)
            {
                string sNameAcc = (string)dtbLoginData.Rows[0][1];
                sNameAcc += " ";
                sNameAcc += (string)dtbLoginData.Rows[0][0];
                return sNameAcc;
            }
            return null;
        }
    }
}
