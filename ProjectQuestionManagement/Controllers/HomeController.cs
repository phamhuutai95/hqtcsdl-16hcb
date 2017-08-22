using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectQuestionManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        #region Login
        public ActionResult Login()
        {
            if (Session["Username"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginCheck(string strUserName, string strPassword)
        {
            try
            {
                UserDTO objUser = new UserDTO();
                objUser.Username = strUserName;
                objUser.Password = strPassword;

                LoginDAO objLoginDAO = new LoginDAO();
                bool bolIsLoginSucc = objLoginDAO.loginCheck(ref objUser);

                if (bolIsLoginSucc == true)
                {
                    Session["Fullname"] = objUser.FullName;
                    Session["Username"] = objUser.Username;

                    return Json(new { iserror = false, chkUser = true });
                }
                return Json(new { iserror = false, chkUser = false });
            }
            catch
            {
                return Json(new { iserror = true });
            }
        }
        #endregion

        #region XmlMenu
        [HttpPost]
        public ActionResult craftXmlmenu()
        {
            XMLDAO objXMLDAO = new XMLDAO();
            return Content(objXMLDAO.DynamicXMLMenu(Server.MapPath(@"\Menu.xml"), (string)Session["Username"]));
        }
        #endregion
    }
}