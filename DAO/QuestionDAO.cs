using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class QuestionDAO
    {
        #region Insert
        //Start Insert method
        public bool Insert(string strScoreExpect, string strNumbAnswer, string strchoicetype, string strlevel, string strtrainningid, string strContent, string strusercreate, List<AnswerDTO> laswdto)
        {
            object[] objKeyWords = new object[] {
                "@CONTENT", strContent,
                "@SCORE", strScoreExpect,
                "@NUMBANSWER", strNumbAnswer,
                "@TRAINNINGID", strtrainningid,
                "@LEVELID", strlevel,
                "@CHOICEID", strchoicetype,
                "@USERID", strusercreate,
                "@LISTANSWER", createTableFromlist(laswdto)
            };

            DataTable dtbQuery = new DataConnect().callUSPNullable("USP_QUE_INS", objKeyWords);
            //Nếu tồn tại dòng dữ liệu chứng tỏ đăng nhập thành công
            if (dtbQuery != null && dtbQuery.Rows.Count != 0)
            {
                return true;
            }
            return false;
        }
        //End Insert method

        //Start createTableFromlist method
        private DataTable createTableFromlist(List<AnswerDTO> adto)
        {
            DataTable table = new DataTable();
            table.Columns.Add("AnswerContent", typeof(string));
            table.Columns.Add("AnswerCheck", typeof(bool));
            int size = adto.Count;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    table.Rows.Add(adto[i].AnswerContent, adto[i].AnswerCheck);
                }
            }
            return table;
        }
        //End createTableFromlist method



        //Begin get/gen trainingsubject
        public DataTable SearchTrainingSubject(string strKeyWord, int intPageIndex, int intPageSize)
        {
            object[] objKeyWords = new object[] {
                "@PAGESIZE", intPageSize,
                "@PAGEINDEX", intPageIndex,
                "@KEYWORD", strKeyWord
            };

            DataTable dtbSubject = new DataConnect().callUSP("USP_TRAINNINGSUBJECT_SHR", objKeyWords);
            if (dtbSubject != null && dtbSubject.Rows.Count != 0)
            {
                return dtbSubject;
            }

            return null;
        }

        public StringBuilder genHTMLComboTrainingSubject(DataTable dtbSubject, string strSubjectID = "")
        {
            StringBuilder strHTMLTrainingComboSubject = new StringBuilder();

            if (dtbSubject != null && dtbSubject.Rows.Count > 0)
            {
                for (int i = 0; i < dtbSubject.Rows.Count; i++)
                {
                    strHTMLTrainingComboSubject.AppendFormat("<option value='{0}'>-- {1} - {2} --</option>",
                            Convert.ToString(dtbSubject.Rows[i]["TranningTypeSubjectID"]).Trim(),
                            Convert.ToString(dtbSubject.Rows[i]["TrainingTypeName"]).Trim(),
                            Convert.ToString(dtbSubject.Rows[i]["SUBJECTNAME"]).Trim());
                }

            }

            return strHTMLTrainingComboSubject;
        }

        //end get/gen trainingsubject
        #endregion

        #region Index
        //begin SearchQuestion
        public DataTable SearchQuestion(string strKeyWord, string strTrainingSubject, string strLevel, string strChoiceType, int intPageIndex, int intPageSize)
        {
            object[] objKeyWords = new object[] {
                "@PAGESIZE", intPageSize,
                "@PAGEINDEX", intPageIndex,
                "@KEYWORD", strKeyWord,
                "@TNSJID", strTrainingSubject,
                "@LVID", strLevel,
                "@CTID", strChoiceType
            };

            DataTable dtbTable = new DataConnect().callUSPNullable("USP_QUE_SHR", objKeyWords);
            if (dtbTable != null && dtbTable.Rows.Count != 0)
            {
                return dtbTable;
            }

            return null;
        }
        //end SearchQuestion

        #endregion

        #region Delete
        //begin Delete
        public bool Delete(string strKeyWord)
        {
            object[] objKeyWords = new object[] {
                "@KEYWORD", strKeyWord
            };

            DataTable dtbTable = new DataConnect().callUSPNullable("USP_QUE_DEL", objKeyWords);

            if (dtbTable == null || dtbTable.Rows.Count == 0)
            {
                return true;
            }

            return false;
        }
        //end Delete

        #endregion

        #region Chi tiết
        //begin SearchQuestion
        public DataTable SearchQuestionDetail(string strKeyWord)
        {
            object[] objKeyWords = new object[] {
                "@KEYWORD", strKeyWord
                };
            DataTable dtbTable = new DataConnect().callUSPNullable("USP_QUE_SHRDT", objKeyWords);
            if (dtbTable != null && dtbTable.Rows.Count != 0)
            {
                return dtbTable;
            }

            return null;
        }
        //end SearchQuestion



        //begin SearchAnswerDetail
        public DataTable SearchAnswerDetail(string strKeyWord)
        {
            object[] objKeyWords = new object[] {
                "@KEYWORD", strKeyWord
                };
            DataTable dtbTable = new DataConnect().callUSPNullable("USP_ANS_SHRL", objKeyWords);
            if (dtbTable != null && dtbTable.Rows.Count != 0)
            {
                return dtbTable;
            }

            return null;
        }




        public StringBuilder genHTMLAnswerDetail(DataTable dtbTable, string strID)
        {
            StringBuilder strHTML = new StringBuilder();

            if (dtbTable != null && dtbTable.Rows.Count > 0)
            {
                for (int i = dtbTable.Rows.Count; i > 0; i--)
                {
                    strHTML.AppendFormat(" <textarea id='HDInserttaAnswer_{0}' name='HDInserttaAnswer_{0}' style='display:none;' data-qid='{1}'>{2}</textarea> \n",
                            Convert.ToString(i).Trim(),
                            Convert.ToString(dtbTable.Rows[i - 1]["AnswerID"]).Trim(),
                            Convert.ToString(dtbTable.Rows[i - 1]["AnswerContent"]).Trim());

                    strHTML.AppendFormat(" <input id='HDoldAnswer_{0}' name='HDoldAnswer_{0}' type='hidden' value='HDInserttaAnswer_{0}'> \n",
                            Convert.ToString(i).Trim());

                    strHTML.AppendFormat(" <input id='HDInsertipTrueAnswer_{0}' name='HDInsertipTrueAnswer_{0}' type='hidden' value='{1}'> \n",
                            Convert.ToString(i).Trim(),
                            Convert.ToString(dtbTable.Rows[i - 1]["AnswerTrue"]).Trim());


                    strHTML.AppendFormat(" <button type='button' id='btnCheckAnswer{0}' class='btn btn-primary' onclick='CheckThisAnswer({0});'>Xem/Sửa câu trả lời {0}</button> \n",
                            Convert.ToString(i).Trim());
                 
                }

            }
            else
            {
                strHTML.AppendFormat(" <button type='button' id='btnNoAnswer' class='btn btn-primary'>Không có câu trả lời nào cho câu hỏi này</button> \n");
            }

            return strHTML;
        }

        //end SearchAnswerDetail

        #endregion

        #region Cập nhật
        //Start Insert method
        public bool Update(string strID, string strScoreExpect, string strNumbAnswer, string strchoicetype, string strlevel, string strtrainningid,
            string strContent, List<AnswerDTO> laddaswe, List<AnswerDTO> luppaswe, List<string> ldelaswe)
        {
            object[] objKeyWords = new object[] {
                "@QID", strID,
                "@CONTENT", strContent,
                "@SCORE", strScoreExpect,
                "@NUMBANSWER", strNumbAnswer,
                "@TRAINNINGID", strtrainningid,
                "@LEVELID", strlevel,
                "@CHOICEID", strchoicetype,
                "@LISTANSWERDEL", createTableFromlistDel(ldelaswe),
                "@LISTANSWERUPD", createTableFromlistUpdate(luppaswe),
                "@LISTANSWERADD", createTableFromlist(laddaswe)
            };

            DataTable dtbQuery = new DataConnect().callUSPNullable("USP_QUE_UPD", objKeyWords);
            //Nếu tồn tại dòng dữ liệu chứng tỏ đăng nhập thành công
            if (dtbQuery != null && dtbQuery.Rows.Count != 0)
            {
                return true;
            }
            return false;
        }
        //End Insert method


        //Start createTableFromlistUpdate method
        private DataTable createTableFromlistUpdate(List<AnswerDTO> adto)
        {
            DataTable table = new DataTable();
            table.Columns.Add("AnswerID", typeof(string));
            table.Columns.Add("AnswerContent", typeof(string));
            table.Columns.Add("AnswerCheck", typeof(bool));
            int size = adto.Count;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    table.Rows.Add(adto[i].AnswerID, adto[i].AnswerContent, adto[i].AnswerCheck);
                }
            }
            return table;
        }
        //End createTableFromlistUpdate method


        //Start createTableFromlistDel method
        private DataTable createTableFromlistDel(List<string> adto)
        {
            DataTable table = new DataTable();
            table.Columns.Add("AnswerID", typeof(string));
            int size = adto.Count;
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    table.Rows.Add(adto[i]);
                }
            }
            return table;
        }
        //End createTableFromlistDel method

        #endregion

    }//end class


    


}
