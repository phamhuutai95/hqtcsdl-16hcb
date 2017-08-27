using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectQuestionManagement
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Quản lý giáo viên
            routes.MapRoute(
                name: "quan-ly-giao-vien",
                url: "quan-ly-giao-vien",
                defaults: new { controller = "Teacher", action = "Index" }
            );

            routes.MapRoute(
                name: "quan-ly-giao-vien/them",
                url: "quan-ly-giao-vien/them",
                defaults: new { controller = "Teacher", action = "TeacherInsert" }
            );

            routes.MapRoute(
                name: "quan-ly-giao-vien/cap-nhat",
                url: "quan-ly-giao-vien/cap-nhat/{strEmployeeID}",
                defaults: new { controller = "Teacher", action = "TeacherUpdate", strEmployeeID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "quan-ly-giao-vien/chi-tiet",
                url: "quan-ly-giao-vien/chi-tiet/{strEmployeeID}",
                defaults: new { controller = "Teacher", action = "TeacherDetail", strEmployeeID = UrlParameter.Optional }
            );
            #endregion

            #region Quản lý môn học
            routes.MapRoute(
                name: "quan-ly-mon-hoc",
                url: "quan-ly-mon-hoc",
                defaults: new { controller = "Subject", action = "Index" }
            );

            routes.MapRoute(
                name: "quan-ly-mon-hoc/them",
                url: "quan-ly-mon-hoc/them",
                defaults: new { controller = "Subject", action = "SubjectInsert" }
            );

            routes.MapRoute(
                name: "quan-ly-mon-hoc/cap-nhat",
                url: "quan-ly-mon-hoc/cap-nhat/{strSubjectID}",
                defaults: new { controller = "Subject", action = "SubjectUpdate", strSubjectID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "quan-ly-mon-hoc/chi-tiet",
                url: "quan-ly-mon-hoc/chi-tiet/{strSubjectID}",
                defaults: new { controller = "Subject", action = "SubjectDetail", strSubjectID = UrlParameter.Optional }
            );
            #endregion

            #region Quản lý câu hỏi
            routes.MapRoute(
                name: "quan-ly-cau-hoi",
                url: "quan-ly-cau-hoi",
                defaults: new { controller = "Question", action = "Index" }
            );

            routes.MapRoute(
                name: "quan-ly-cau-hoi/them",
                url: "quan-ly-cau-hoi/them",
                defaults: new { controller = "Question", action = "Insert" }
            );

            routes.MapRoute(
                name: "quan-ly-cau-hoi/cap-nhat",
                url: "quan-ly-cau-hoi/cap-nhat/{strID}",
                defaults: new { controller = "Question", action = "Update", strSubjectID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "quan-ly-cau-hoi/chi-tiet",
                url: "quan-ly-cau-hoi/chi-tiet/{strID}",
                defaults: new { controller = "Question", action = "Detail", strID = UrlParameter.Optional }
            );
            #endregion



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
