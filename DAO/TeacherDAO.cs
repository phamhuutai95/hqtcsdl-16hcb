using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class TeacherDAO
    {

        public DataTable SearchTeacher(string strKeyWord, int intPageIndex, int intPageSize, int intSubjectID, bool bolIsBlock)
        {
            object[] objKeyWords = new object[] {
                "@PAGESIZE", intPageSize,
                "@PAGEINDEX", intPageIndex,
                "@KEYWORD", strKeyWord,
                "@SUBJECTID", intSubjectID,
                "@ISBLOCK", bolIsBlock
            };

            DataTable dtbTeacher = new DataConnect().callUSP("USP_USER_SHR", objKeyWords);
            if (dtbTeacher != null && dtbTeacher.Rows.Count != 0)
            {
                return dtbTeacher;
            }

            return null;
        }

        //public bool DeleteEmployees(string strEmployeeID)
        //{
        //    object[] objKeyWords = new object[] {
        //        "@EmployeeID", strEmployeeID
        //    };

        //    DataTable dtbEmployees = new DataConnect().callUSP("uspDeleteEmployees", objKeyWords);
        //    if (dtbEmployees != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}


        //public DataTable GetRoleList()
        //{
        //    DataTable dtbQuery = new DataConnect().callUSP("uspGetRoleList", null);
        //    if (dtbQuery != null && dtbQuery.Rows.Count != 0)
        //    {
        //        return dtbQuery;
        //    }

        //    return null;
        //}

        public bool InsertTeacher(string strFullName, string strAddress, DateTime dtmBirthDate, string strPhoneNumber, string strRoleID, 
            string strSubjectID, string strManagerID)
        {
            object[] objKeyWords = new object[] {
                "@FULLNAME", strFullName,
                "@ADDRESS", strAddress,
                "@BIRTHDATE", dtmBirthDate,
                "@PHONENUMBER", DBNull.Value,
                //"@ROLEID", strRoleID,
                //"@SUBJECTID", strSubjectID,
                //"@MANAGERID", strManagerID
                "@ROLEID", "R001",
                "@SUBJECTID", "S001"
                //"@MANAGERID", "U0002"
            };

            DataTable dtbQuery = new DataConnect().callUSP("USP_USERS_INS", objKeyWords);
            //Nếu tồn tại dòng dữ liệu chứng tỏ đăng nhập thành công
            if (dtbQuery != null && dtbQuery.Rows.Count != 0)
            {
                return true;
            }
            return false;
        }

        public bool InsertTeacher(string strFullName, string strAddress, DateTime dtmBirthDate, string strPhoneNumber, string strRoleID,
            string strSubjectID, string strManagerID, List<PermissionDTO> LstPermission)
        {
            List<string> Lstusp_name = new List<string>() { "USP_USERS_INS", "USP_USERPERMISSIONS_INS" };
            List<object[]> LstobjKeyWords = new List<object[]>();

            //Tạo list keywords
            object[] objKeyWords1 = new object[] {
                "@FULLNAME", strFullName,
                "@ADDRESS", strAddress,
                "@BIRTHDATE", dtmBirthDate,
                "@PHONENUMBER", strPhoneNumber,
                "@ROLEID", strRoleID,
                "@SUBJECTID", strSubjectID,
                "@MANAGERID", strManagerID
            };
            LstobjKeyWords.Add(objKeyWords1);

            StringBuilder strPermissionID = new StringBuilder();
            if (LstPermission != null && LstPermission.Count > 0)
            {
                for (int i = 0; i < LstPermission.Count; i++)
                {
                    strPermissionID.Append(LstPermission[i].PermissionID);

                    if (i != LstPermission.Count - 1)
                        strPermissionID.Append(",");
                }
            }
            object[] objKeyWords2 = new object[] {
                "@USERID", "INFINITY",
                "@LSTPERMISSIONID", Convert.ToString(strPermissionID)
            };
            LstobjKeyWords.Add(objKeyWords2);

            return new DataConnect().exeUSP(Lstusp_name, LstobjKeyWords, true);
        }


        //public bool UpdateEmployees(string strEmployeeID, string strLastName, string strFirstName, string strAddress, string strDistrict, string strCity, string strSDT, string strEmail, string strRole)
        //{

        //    object[] objKeyWords = new[] {
        //        "@EmployeeID",strEmployeeID,
        //        "@FirstName", strFirstName,
        //        "@LastName", strLastName,
        //        "@Address", strAddress,
        //        "@Distric", strDistrict,
        //        "@City", strCity,
        //        "@Phone", strSDT,
        //        "@Email", strEmail,
        //        "@Role", strRole,
        //    };

        //    DataTable dtbQuery = new DataConnect().callUSP("uspUpdateEmployee", objKeyWords);
        //    //Nếu tồn tại dòng dữ liệu chứng tỏ đăng nhập thành công
        //    if (dtbQuery != null && dtbQuery.Rows.Count != 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public TeacherDTO GetEmployeesByID(string strEmployeeID)
        //{
        //    TeacherDTO EDTO = new TeacherDTO();
        //    object[] objKeyWords = new object[] {
        //        "@EmployeeID", strEmployeeID
        //    };
        //    DataTable dtbQuery = new DataConnect().callUSP("uspGetDetailEmployeesByID", objKeyWords);
        //    if (dtbQuery != null && dtbQuery.Rows.Count != 0)
        //    {
        //        EDTO.EmployeesID = (string)dtbQuery.Rows[0]["EmployeeID"];
        //        EDTO.strLastName = (string)dtbQuery.Rows[0]["LastName"];
        //        EDTO.strFirstName = (string)dtbQuery.Rows[0]["FirstName"];
        //        EDTO.strAddress = (string)dtbQuery.Rows[0]["Address"];
        //        EDTO.strDistrict = (string)dtbQuery.Rows[0]["District"];
        //        EDTO.strCity = (string)dtbQuery.Rows[0]["CityorProvince"];
        //        EDTO.strSDT = (string)dtbQuery.Rows[0]["PhoneNumber"];
        //        EDTO.strEmail = (string)dtbQuery.Rows[0]["EmailAddress"];
        //        EDTO.strRoleID = (string)dtbQuery.Rows[0]["RoleID"];
        //        EDTO.strRole = (string)dtbQuery.Rows[0]["RoleName"];
        //        return EDTO;
        //    }

        //    return null;
        //}
    }
}
