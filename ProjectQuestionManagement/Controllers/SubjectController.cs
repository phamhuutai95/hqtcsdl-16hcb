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

namespace ProjectStudentManagement.Controllers
{
    public class SubjectController : Controller
    {
        #region Quản lý
        public ActionResult Index()
        {
            //Kiểm tra xem người dùng có được xem(truy cập vào chức năng)
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLMH_V") == false)
            {
                return RedirectToAction("Login", "Home");
            }

            //Kiểm tra xem người dùng có được phép thêm
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLMH_I") == false)
            {
                ViewBag.InsertPermission = false;
            }

            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);
            return View();
        }

        [HttpPost]
        public ActionResult SearchSubject(string strKeyWord, int intPageIndex = 0)
        {
            try
            {
                //Lấy cấu hình số dòng trên trang
                //int intPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["RowPerPage"]);
                //Lấy config số trang hiển thị trên thanh phân trang
                int intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);
                int intTotalPages = 0;
                int intTotalRows = 0;

                //Gen HTML
                StringBuilder strHTMLSubject = new StringBuilder();

                DataTable dtbSubject = new SubjectDAO().SearchSubject(strKeyWord, intPageIndex, intVisiblePages);

                if (dtbSubject != null && dtbSubject.Rows.Count > 0)
                {
                    strHTMLSubject = GenHTMLSubject(dtbSubject);
                    //Lấy tổng số lưởng dòng
                    intTotalRows = Convert.ToInt32(dtbSubject.Rows[0]["TotalRows"]);
                    //Lấy tổng số lượng trang
                    intTotalPages = intTotalRows / intVisiblePages;
                    //Trường hợp số phần tử không chia hết cho intVisiblePages
                    if (intTotalRows % intVisiblePages != 0)
                        intTotalPages = intTotalPages + 1;
                }
                else
                {
                    strHTMLSubject.Append("<tr><td colspan='12' class='table-content-null'>Hiện không có nội dung</td></tr>");
                }

                return Json(new
                {
                    iserror = false,
                    content = Convert.ToString(strHTMLSubject),
                    totalpages = intTotalPages,
                    totalrows = intTotalRows
                });
            }
            catch
            {
                return Json(new { iserror = true });
            }
        }

        private StringBuilder GenHTMLSubject(DataTable dtbSubject)
        {
            bool bPerEdit = true;
            bool bPerDelete = true;
            //Kiểm tra người dùng được xóa
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLMH_D") == false)
            {
                bPerDelete = false;
            }

            //Kiểm tra người dùng được cập nhật
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLMH_U") == false)
            {
                bPerEdit = false;
            }

            int intCountRow = dtbSubject.Rows.Count;
            StringBuilder strHTMLSubject = new StringBuilder();

            for (int i = 0; i < intCountRow; i++)
            {
                string strSubjectID = Convert.ToString(dtbSubject.Rows[i]["SubjectID"]).Trim();

                strHTMLSubject.Append("<tr>");
                strHTMLSubject.AppendFormat("<td>{0}</td>", dtbSubject.Rows[i]["STT"]);
                strHTMLSubject.AppendFormat("<td>{0}</td>", strSubjectID);
                strHTMLSubject.AppendFormat("<td>{0}</td>", dtbSubject.Rows[i]["SubjectName"]);
                string sEdit = "";
                string sDelete = "";
                if (bPerEdit != false)
                {
                    sEdit += "<span class='glyphicon glyphicon-pencil table-icon' data-name='Detail'></span>";
                }

                if (bPerDelete != false)
                {
                    sDelete += "<span class='glyphicon glyphicon-trash table-icon' data-name='Delete'></span>";
                }
                strHTMLSubject.AppendFormat("<td data-subjectid='{0}'>" + sEdit + sDelete + "</td>", strSubjectID);

                strHTMLSubject.Append("</tr>");
            }

            return strHTMLSubject;
        }

        public ActionResult SubjectInsert()
        {
            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

            return View();
        }

        public ActionResult SubjectUpdate(string strSubjectID)
        {
            //Kiểm tra người dùng được sửa
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLMH_U") == false)
            {
                return RedirectToAction("Login", "Home");
            }
            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

            SubjectDTO objSubject = new SubjectDAO().GetSubjectByID(strSubjectID);
            return View(objSubject);
        }

        public ActionResult SubjectDetail(string strSubjectID)
        {
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLMH_V") == false)
            {
                return RedirectToAction("Login", "Home");
            }

            //Kiểm tra người dùng được sửa
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QLMH_U") == false)
            {
                ViewBag.UpdatePermission = false;
            }
            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

            SubjectDTO objSubject = new SubjectDAO().GetSubjectByID(strSubjectID);
            return View(objSubject);
        }

        //[HttpPost]
        //public JsonResult DeleteSubject(string strSubjectID)
        //{
        //    try
        //    {
        //        string strMessageError = string.Empty;
        //        bool bolSuccess = new SubjectDAO().DeleteSubject(strSubjectID, ref strMessageError);

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
        public JsonResult InsertSubject(FormCollection frm)
        {
            try
            {
                string strSubjectName = Convert.ToString(frm["txtSubjectName"]);

                bool bolSuccess = new SubjectDAO().InsertSubject(strSubjectName);

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

        #region Cập nhật
        [HttpPost]
        public JsonResult UpdateSubject(FormCollection frm)
        {
            try
            {
                string strSubjectID = Convert.ToString(frm["hdSubjectID"]);
                string strSubjectName = Convert.ToString(frm["txtSubjectName"]);

                bool bolSuccess = new SubjectDAO().UpdateSubject(strSubjectID, strSubjectName);

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
	}
}