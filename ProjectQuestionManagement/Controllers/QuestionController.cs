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
    public class QuestionController : Controller
    {
        #region Quản lý
        //
        // GET: /Question/
        public ActionResult Index()
        {
            //Kiểm tra xem người dùng có được xem(truy cập vào chức năng)
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QUE_V") == false)
            {
                return RedirectToAction("Login", "Home");
            }


            //Kiểm tra xem người dùng có được phép thêm
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QUE_I") == false)
            {
                ViewBag.InsertPermission = false;
            }


            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);


            //Gen HTML combo hệ môn học
            DataTable dtbTrainingSubject = new QuestionDAO().SearchTrainingSubject(null, -1, -1);
            ViewBag.HTMLComboTrainingSubject = new QuestionDAO().genHTMLComboTrainingSubject(dtbTrainingSubject);

            return View();
        }


        //Begin SearchQuestion
        [HttpPost]
        public ActionResult SearchQuestion(string strKeyWord, string strTrainingSubject, string strLevel, string strChoiceType, int intPageIndex = 0)
        {
            try
            {
                //Lấy config số trang hiển thị trên thanh phân trang
                int intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);
                int intTotalPages = 0;
                int intTotalRows = 0;

                if (strKeyWord.Equals(""))
                {
                    strKeyWord = null;
                }
                if (strTrainingSubject.Equals("-1"))
                {
                    strTrainingSubject = null;
                }
                if (strLevel.Equals("-1"))
                {
                    strLevel = null;
                }
                if (strChoiceType.Equals("-1"))
                {
                    strChoiceType = null;
                }

                //Gen HTML
                StringBuilder strHTML = new StringBuilder();

                DataTable dtbTable = new QuestionDAO().SearchQuestion(strKeyWord, strTrainingSubject, strLevel, strChoiceType, intPageIndex, intVisiblePages);

                if (dtbTable != null && dtbTable.Rows.Count > 0)
                {
                    strHTML = GenHTML(dtbTable);
                    //Lấy tổng số lưởng dòng
                    intTotalRows = Convert.ToInt32(dtbTable.Rows[0]["TotalRows"]);
                    //Lấy tổng số lượng trang
                    intTotalPages = intTotalRows / intVisiblePages;
                    //Trường hợp số phần tử không chia hết cho intVisiblePages
                    if (intTotalRows % intVisiblePages != 0)
                        intTotalPages = intTotalPages + 1;
                }
                else
                {
                    strHTML.Append("<tr><td colspan='12' class='table-content-null'>Hiện không có nội dung</td></tr>");
                }

                return Json(new
                {
                    iserror = false,
                    content = Convert.ToString(strHTML),
                    totalpages = intTotalPages,
                    totalrows = intTotalRows
                });
            }
            catch
            {
                return Json(new { iserror = true });
            }
        }
        //End SearchQuestion


        private StringBuilder GenHTML(DataTable dtbTable)
        {
            string strUserName = Convert.ToString(Session["Username"]);
            StringBuilder strHTML = new StringBuilder();

            if (dtbTable != null && dtbTable.Rows.Count > 0)
            {
                bool bolIsDelted = true;
                //Kiểm tra quyền xóa
                if (LoginDAO.CheckKeyPermission(strUserName, "PK_QUE_D") == false)
                {
                    bolIsDelted = false;
                }

                int intCountRow = dtbTable.Rows.Count;

                for (int i = 0; i < intCountRow; i++)
                {
                    string strID = Convert.ToString(dtbTable.Rows[i]["QuestionID"]).Trim();

                    strHTML.Append("<tr>");
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["STT"]);
                    strHTML.AppendFormat("<td>{0}</td>", strID);
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["QuestionContent"]);
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["ProposalScore"]);
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["NumberAnswer"]);
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["RTrainningSubjectsName"]);
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["LevelName"]);
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["ChoiceTypeName"]);
                    strHTML.AppendFormat("<td>{0}</td>", dtbTable.Rows[i]["UserCreateID"]);
                    strHTML.AppendFormat("<td data-detailid='{0}'>", strID);
                    strHTML.Append("<span class='glyphicon glyphicon-pencil table-icon' data-name='Detail'></span>");
                    if (bolIsDelted == true)
                    {
                        strHTML.Append("<span class='glyphicon glyphicon-trash table-icon' data-name='Delete'></span>");
                    }
                    strHTML.Append("</td>");
                }
            }

            return strHTML;
        }



        #endregion


        #region Thêm
        //
        // GET: /Question/
        public ActionResult Insert()
        {
            //Kiểm tra xem người dùng có được xem(truy cập vào chức năng)
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QUE_I") == false)
            {
                return RedirectToAction("Login", "Home");
            }



            //Gen HTML combo hệ môn học
            DataTable dtbTrainingSubject = new QuestionDAO().SearchTrainingSubject(null, -1, -1);
            ViewBag.HTMLComboTrainingSubject = new QuestionDAO().genHTMLComboTrainingSubject(dtbTrainingSubject);


            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

            return View();
        }

        [HttpPost]
        public JsonResult InsertProcess(FormCollection frm)
        {
            try
            {

                string strScoreExpect = Convert.ToString(frm["txtScoreExpect"]);  
                string strchoicetype = Convert.ToString(frm["cbochoicetype"]);
                string strlevel = Convert.ToString(frm["cbolevel"]);
                string strtrainningid = Convert.ToString(frm["cboHMH"]);
                string strContent = Convert.ToString(frm["taContent"]);
                string strusercreate = (string)Session["Username"];
                string strNumbAnswer = Convert.ToString(frm["createAnswerhdnumberanswer"]);
                int intNumbAnswer = int.Parse(strNumbAnswer);
                List<AnswerDTO> lcontentAnswer = new List<AnswerDTO>();

                foreach (var key in frm.AllKeys.Where(k => k.StartsWith("HDInserttaAnswer")))
                {
                    string[] sSplit = key.Split(new char[]{ '_' } ,StringSplitOptions.RemoveEmptyEntries);
                    string _selectorp = "HDInserttaAnswer_" + sSplit[1];
                    AnswerDTO adto = new AnswerDTO();
                    adto.AnswerContent = Convert.ToString(frm[_selectorp]);
                    _selectorp = "HDInsertipTrueAnswer_" + sSplit[1];
                    adto.AnswerCheck = Convert.ToBoolean(frm[_selectorp]);
                    lcontentAnswer.Add(adto);
                }



                //for (int i = 1; i <= intNumbAnswer; i++ )
                //{
                //    string _selectorp = "HDInserttaAnswer" + i;
                //    AnswerDTO adto = new AnswerDTO();
                //    adto.AnswerContent = Convert.ToString(frm[_selectorp]);
                //    _selectorp = "HDInsertipTrueAnswer" + i;
                //    adto.AnswerCheck = Convert.ToBoolean(frm[_selectorp]);
                //    lcontentAnswer.Add(adto);

                //}

                bool bolSuccess = new QuestionDAO().Insert(strScoreExpect, strNumbAnswer, strchoicetype, strlevel, strtrainningid, strContent, strusercreate, lcontentAnswer);

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


        #region Xoá
        [HttpPost]
        public JsonResult Delete(string strKeyWord)
        {
            try
            {
                string strMessageError = string.Empty;
                bool bolSuccess = new QuestionDAO().Delete(strKeyWord);

                if (bolSuccess == false)
                    return Json(new { iserror = true, message = "" });

                return Json(new { iserror = false });
            }
            catch (Exception)
            {
                return Json(new { iserror = true, message = "" });
            }
        }
        #endregion


        #region Chi tiết
        public ActionResult Detail(string strID)
        {
            //Kiểm tra xem người dùng có được xem(truy cập vào chức năng)
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QUE_V") == false)
            {
                return RedirectToAction("Login", "Home");
            }

            StringBuilder strHTMLbtn = new StringBuilder();
            if (LoginDAO.CheckKeyPermission((string)Session["Username"], "PK_QUE_U") != false)
            {
                strHTMLbtn.AppendFormat(@" <button type='button' class='btn btn-primary' id='btnUpdateShow' onclick='UpdateShow();' data-loading-text=""<i class='fa fa-spinner fa-spin '></i> Đang xử lý""><i class='fa fa-plus'></i>Cập nhật</button> ");
                strHTMLbtn.AppendFormat(@" <button type='button' class='btn btn-primary' style='display:none;' id='btnUpdateConfirm' onclick='Update();' data-loading-text=""<i class='fa fa-spinner fa-spin '></i> Đang xử lý""><i class='fa fa-plus'></i>Xác nhận</button> ");
            }
            ViewBag.HTMLbtnUpdate = strHTMLbtn;



            //Gen HTML combo hệ môn học
            DataTable dtbTrainingSubject = new QuestionDAO().SearchTrainingSubject(null, -1, -1);
            ViewBag.HTMLComboTrainingSubject = new QuestionDAO().genHTMLComboTrainingSubject(dtbTrainingSubject);


            //Gen Datable thông số chi tiết
            DataTable dtbDetailParameter = new QuestionDAO().SearchQuestionDetail(strID);
            ViewBag.QuestionID = dtbDetailParameter.Rows[0]["QuestionID"];
            ViewBag.QuestionContent = dtbDetailParameter.Rows[0]["QuestionContent"];
            ViewBag.ProposalScore = dtbDetailParameter.Rows[0]["ProposalScore"];
            ViewBag.NumberAnswer = dtbDetailParameter.Rows[0]["NumberAnswer"];
            ViewBag.TrainningTypeSubjectID = dtbDetailParameter.Rows[0]["TrainningTypeSubjectID"];
            ViewBag.LevelID = dtbDetailParameter.Rows[0]["LevelID"];
            ViewBag.ChoiceTypeID = dtbDetailParameter.Rows[0]["ChoiceTypeID"];


            //Gen danh sách trả lời
            dtbDetailParameter = new QuestionDAO().SearchAnswerDetail(strID);
            ViewBag.strHTMLAnswerList = new QuestionDAO().genHTMLAnswerDetail(dtbDetailParameter, strID);




            //Lấy config số trang hiển thị trên thanh phân trang
            ViewBag.intVisiblePages = Convert.ToInt32(ConfigurationManager.AppSettings["visiblePages"]);

            return View();
        }


        #endregion


        #region Cập nhật

        [HttpPost]
        public JsonResult UpdateProcess(FormCollection frm)
        {
            try
            {
                string strID = Convert.ToString(frm["hdupdatestridtxt"]);
                string strScoreExpect = Convert.ToString(frm["txtScoreExpect"]);
                string strchoicetype = Convert.ToString(frm["cbochoicetype"]);
                string strlevel = Convert.ToString(frm["cbolevel"]);
                string strtrainningid = Convert.ToString(frm["cboHMH"]);
                string strContent = Convert.ToString(frm["taContent"]);
                string strusercreate = (string)Session["Username"];
                string strNumbAnswer = Convert.ToString(frm["createAnswerhdnumberanswer"]);
                int intNumbAnswer = int.Parse(strNumbAnswer);
                List<string> listoldanswerd = new List<string>();
                foreach (var key in frm.AllKeys.Where(k => k.StartsWith("HDoldAnswer")))
                {
                    string value = Convert.ToString(frm[key]);
                    listoldanswerd.Add(value);
                }



                List<AnswerDTO> lcontentAnswer = new List<AnswerDTO>();

                foreach (var key in frm.AllKeys.Where(k => k.StartsWith("HDInserttaAnswer")))
                {
                    var match = listoldanswerd.FirstOrDefault(stringToCheck => stringToCheck.Contains(key));
                    if(match == null)
                    {
                        string[] sSplit = key.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        string _selectorp = "HDInserttaAnswer_" + sSplit[1];
                        AnswerDTO adto = new AnswerDTO();
                        adto.AnswerContent = Convert.ToString(frm[_selectorp]);
                        _selectorp = "HDInsertipTrueAnswer_" + sSplit[1];
                        adto.AnswerCheck = Convert.ToBoolean(frm[_selectorp]);
                        lcontentAnswer.Add(adto);
                    }
                    
                }


                List<AnswerDTO> lupdatecontentAnswer = new List<AnswerDTO>();

                foreach (var key in frm.AllKeys.Where(k => k.StartsWith("HDUpdateAnswer")))
                {

                    string[] sSplit = key.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    string _selectorp = "HDInserttaAnswer_" + sSplit[1];
                    AnswerDTO adto = new AnswerDTO();
                    adto.AnswerContent = Convert.ToString(frm[_selectorp]);
                    _selectorp = "HDInsertipTrueAnswer_" + sSplit[1];
                    adto.AnswerCheck = Convert.ToBoolean(frm[_selectorp]);

                    adto.AnswerID = Convert.ToString(frm[key]);
                    lupdatecontentAnswer.Add(adto);

                }


                List<string> ldeletecontentAnswer = new List<string>();

                foreach (var key in frm.AllKeys.Where(k => k.StartsWith("HDDeleteAnswer")))
                {
                    ldeletecontentAnswer.Add(Convert.ToString(frm[key]));

                }


                bool bolSuccess = new QuestionDAO().Update(strID, strScoreExpect, strNumbAnswer, strchoicetype, strlevel, strtrainningid, strContent, lcontentAnswer, lupdatecontentAnswer, ldeletecontentAnswer);

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