using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProjectQuestionManagement.Controllers
{
    public class TeacherController : Controller
    {
        #region Quản lý
        public ActionResult Index()
        {
            string strUserName = Convert.ToString(Session["Username"]);

            //Kiểm tra xem người dùng có được xem(truy cập vào chức năng)
            if (LoginDAO.CheckKeyPermission(strUserName, "") == false && strUserName != "U0001")
            {
                return RedirectToAction("Login", "Home");
            }

            //Gen HTML combo môn học
            DataTable dtbSubject = new SubjectDAO().SearchSubject(null, -1, -1);
            ViewBag.HTMLComboSubject = new SubjectDAO().genHTMLComboSubject(dtbSubject);

            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

            return View();
        }

        [HttpPost]
        public ActionResult SearchTeacher(string strKeyWord, int intSubjectID, bool bolIsBlock, int intPageIndex = 0)
        {
            try
            {
                //Lấy config số trang hiển thị trên thanh phân trang
                int intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);
                int intTotalPages = 0;
                int intTotalRows = 0;

                //Gen HTML
                StringBuilder strHTMLTeacher = new StringBuilder();

                DataTable dtbTeacher = new TeacherDAO().SearchTeacher(strKeyWord, intPageIndex, intVisiblePages, intSubjectID, bolIsBlock);

                if (dtbTeacher != null && dtbTeacher.Rows.Count > 0)
                {
                    strHTMLTeacher = GenHTMLTeacher(dtbTeacher);
                    //Lấy tổng số lưởng dòng
                    intTotalRows = Convert.ToInt32(dtbTeacher.Rows[0]["TotalRows"]);
                    //Lấy tổng số lượng trang
                    intTotalPages = intTotalRows / intVisiblePages;
                    //Trường hợp số phần tử không chia hết cho intVisiblePages
                    if (intTotalRows % intVisiblePages != 0)
                        intTotalPages = intTotalPages + 1;
                }
                else
                {
                    strHTMLTeacher.Append("<tr><td colspan='12' class='table-content-null'>Hiện không có nội dung</td></tr>");
                }

                return Json(new
                {
                    iserror = false,
                    content = Convert.ToString(strHTMLTeacher),
                    totalpages = intTotalPages,
                    totalrows = intTotalRows
                });
            }
            catch
            {
                return Json(new { iserror = true });
            }
        }

        private StringBuilder GenHTMLTeacher(DataTable dtbTeacher)
        {
            string strUserName = Convert.ToString(Session["Username"]);
            StringBuilder strHTMLTeacher = new StringBuilder();

            if (dtbTeacher != null && dtbTeacher.Rows.Count > 0)
            {
                bool bolIsDelted = true;
                //Kiểm tra quyền xóa
                if (LoginDAO.CheckKeyPermission(strUserName, "") == false && strUserName != "U0001")
                {
                    bolIsDelted = false;
                }

                int intCountRow = dtbTeacher.Rows.Count;

                for (int i = 0; i < intCountRow; i++)
                {
                    string strTeacherID = Convert.ToString(dtbTeacher.Rows[i]["USERID"]).Trim();

                    strHTMLTeacher.Append("<tr>");
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", dtbTeacher.Rows[i]["STT"]);
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", strTeacherID);
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", dtbTeacher.Rows[i]["FULLNAME"]);
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", String.Format("{0:dd/MM/yyyy}", dtbTeacher.Rows[i]["BIRTHDATE"]));
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", dtbTeacher.Rows[i]["PHONENUMBER"]);
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", dtbTeacher.Rows[i]["SUBJECTNAME"]);
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", dtbTeacher.Rows[i]["MANAGER"]);
                    strHTMLTeacher.AppendFormat("<td>{0}</td>", dtbTeacher.Rows[i]["ISBLOCK"]);
                    strHTMLTeacher.AppendFormat("<td data-teacherid='{0}'>", strTeacherID);
                    strHTMLTeacher.Append("<span class='glyphicon glyphicon-pencil table-icon' data-name='Detail'></span>");
                    if (bolIsDelted == true)
                    {
                        strHTMLTeacher.Append("<span class='glyphicon glyphicon-trash table-icon' data-name='Delete'></span>");
                    }
                    strHTMLTeacher.Append("</td>");
                }
            }

            return strHTMLTeacher;
        }

        public ActionResult TeacherInsert()
        {
            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

            return View();
        }

        //public ActionResult TeacherUpdate(string strTeacherID)
        //{
        //    //Kiểm tra người dùng được sửa
        //    if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLNND_U") == false)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    //Lấy config số trang hiển thị trên thanh phân trang
        //    ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

        //    EmloyeeRolesDTO EmloyeeRoles = new EmloyeeRolesDAO().GetEmloyeeRolesByID(strTeacherID);
        //    return View(EmloyeeRoles);
        //}

        //public ActionResult TeacherDetail(string strTeacherID)
        //{
        //    if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLNND_V") == false)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    //Kiểm tra người dùng được sửa
        //    if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLNND_U") == false)
        //    {
        //        ViewBag.UpdatePermission = false;
        //    }
        //    //Lấy config số trang hiển thị trên thanh phân trang
        //    ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

        //    EmloyeeRolesDTO EmloyeeRoles = new EmloyeeRolesDAO().GetEmloyeeRolesByID(strTeacherID);
        //    return View(EmloyeeRoles);
        //}

        //[HttpPost]
        //public JsonResult DeleteTeacher(string strUserID)
        //{
        //    try
        //    {
        //        string strMessageError = string.Empty;
        //        bool bolSuccess = new TeacherDAO().DeleteTeacher(strUserID, ref strMessageError);

        //        if (bolSuccess == false)
        //            return Json(new { iserror = true, message = strMessageError });

        //        return Json(new { iserror = false });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { iserror = true, message = "" });
        //    }
        //}
        #endregion

        #region Thêm
        [HttpPost]
        public ActionResult SearchPermission(string strKeyWord, int intPageIndex = 0)
        {
            try
            {
                //Lấy config số trang hiển thị trên thanh phân trang
                int intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);
                int intTotalPages = 0;
                int intTotalRows = 0;

                //Gen HTML
                StringBuilder strHTMLPermission = new StringBuilder();

                DataTable dtbPermission = new PermissionDAO().SearchPermission(strKeyWord, intPageIndex, intVisiblePages);

                if (dtbPermission != null && dtbPermission.Rows.Count > 0)
                {
                    strHTMLPermission = GenHTMLPermission(dtbPermission);
                    //Lấy tổng số lưởng dòng
                    intTotalRows = Convert.ToInt32(dtbPermission.Rows[0]["TotalRows"]);
                    //Lấy tổng số lượng trang
                    intTotalPages = intTotalRows / intVisiblePages;
                    //Trường hợp số phần tử không chia hết cho intVisiblePages
                    if (intTotalRows % intVisiblePages != 0)
                        intTotalPages = intTotalPages + 1;
                }
                else
                {
                    strHTMLPermission.Append("<tr><td colspan='12' class='table-content-null'>Hiện không có nội dung</td></tr>");
                }

                return Json(new
                {
                    iserror = false,
                    content = Convert.ToString(strHTMLPermission),
                    totalpages = intTotalPages,
                    totalrows = intTotalRows
                });
            }
            catch
            {
                return Json(new { iserror = true });
            }
        }

        private StringBuilder GenHTMLPermission(DataTable dtbPermission)
        {
            int intCountRow = dtbPermission.Rows.Count;
            StringBuilder strHTMLPermission = new StringBuilder();

            for (int i = 0; i < intCountRow; i++)
            {
                string strPermissionID = Convert.ToString(dtbPermission.Rows[i]["PERMISSIONID"]).Trim();

                strHTMLPermission.AppendFormat("<tr id='tr_{0}'>", strPermissionID);
                strHTMLPermission.AppendFormat("<td>{0}</td>", dtbPermission.Rows[i]["STT"]);
                strHTMLPermission.AppendFormat("<td data-name='idkey'>{0}</td>", strPermissionID);
                strHTMLPermission.AppendFormat("<td data-name='description'>{0}</td>", dtbPermission.Rows[i]["PERMISSIONNAME"]);
                strHTMLPermission.AppendFormat("<td><span class='glyphicon glyphicon-plus table-icon' data-name='btnAddPermission' data-idkey='{0}'></span></td>", strPermissionID);
                strHTMLPermission.Append("</tr>");
            }

            return strHTMLPermission;
        }

        [HttpPost]
        public ActionResult SearchPermissionAll(string strKeyWord, int intPageIndex = 0)
        {
            try
            {
                StringBuilder strHTMLPermission = new StringBuilder();

                DataTable dtbPermission = new PermissionDAO().SearchPermission(strKeyWord, -1, -1);

                if (dtbPermission != null && dtbPermission.Rows.Count > 0)
                {
                    strHTMLPermission = GenHTMLPermissionAll(dtbPermission);
                }
                else
                {
                    strHTMLPermission.Append("<tr><td colspan='12' class='table-content-null'>Hiện không có nội dung</td></tr>");
                }

                return Json(new
                {
                    iserror = false,
                    content = Convert.ToString(strHTMLPermission)
                });
            }
            catch
            {
                return Json(new { iserror = true });
            }
        }

        private StringBuilder GenHTMLPermissionAll(DataTable dtbPermission)
        {
            int intCountRow = dtbPermission.Rows.Count;
            StringBuilder strHTMLPermission = new StringBuilder();

            for (int i = 0; i < intCountRow; i++)
            {
                string strPermissionID = Convert.ToString(dtbPermission.Rows[i]["PERMISSIONID"]).Trim();

                strHTMLPermission.AppendFormat("<tr id='tr_{0}'>", strPermissionID);
                strHTMLPermission.AppendFormat("<td data-name='idkey'>{0}</td>", strPermissionID);
                strHTMLPermission.AppendFormat("<td data-name='description'>{0}</td>", dtbPermission.Rows[i]["PERMISSIONNAME"]);
                strHTMLPermission.AppendFormat("<td><span class='glyphicon glyphicon-remove table-icon' data-name='btnRemovePermission' data-idkey='{0}'></span></td>", strPermissionID);
                strHTMLPermission.Append("</tr>");
            }

            return strHTMLPermission;
        }

        [HttpPost]
        public JsonResult InsertTeacher(FormCollection frm)
        {
            try
            {
                List<PermissionDTO> lstPermission = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PermissionDTO>>(frm["hdListPermission"]);

                string strFullName = Convert.ToString(frm["txtFullName"]);
                string strAddress = Convert.ToString(frm["txaAddress"]);
                string strBirthDate = Convert.ToString(frm["dtmBirthDate"]);
                string strPhoneNumber = Convert.ToString(frm["txtPhoneNumber"]);
                string strRoleID = Convert.ToString(frm["cboRole"]);
                string strSubjectID = Convert.ToString(frm["cboSubject"]);
                string strManagerID = Convert.ToString(frm["cboManage"]);

                System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                dateInfo.ShortDatePattern = "dd/MM/yyyy";

                DateTime dtmBirthDate = DateTime.Now;
                if (!string.IsNullOrEmpty(strBirthDate))
                {
                    dtmBirthDate = Convert.ToDateTime(strBirthDate, dateInfo);
                }

                bool bolSuccess = new TeacherDAO().InsertTeacher(strFullName, strAddress, dtmBirthDate, strPhoneNumber, strRoleID, strSubjectID, strManagerID);

                if (bolSuccess == false)
                    return Json(new { iserror = true });

                return Json(new { iserror = false });
            }
            catch (Exception)
            {
                return Json(new { iserror = true });
            }
        }
        #endregion

        //#region Cập nhật
        //[HttpPost]
        //public JsonResult UpdateEmloyeeRoles(FormCollection frm)
        //{
        //    try
        //    {
        //        List<PermissionDTO> lstSubjectExemptRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PermissionDTO>>(frm["hdListPermission"]);
        //        string strRoleID = Convert.ToString(frm["hdRoleID"]);
        //        string strRoleName = Convert.ToString(frm["txtRoleName"]);

        //        bool bolSuccess = new EmloyeeRolesDAO().UpdateEmloyeeRoles(strRoleID, strRoleName, lstSubjectExemptRequest);

        //        if (bolSuccess == false)
        //            return Json(new { iserror = true });

        //        return Json(new { iserror = false });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { iserror = true });
        //    }
        //}
        //#endregion
    }
}