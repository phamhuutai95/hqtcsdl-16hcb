using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class SubjectDAO
    {
        public DataTable SearchSubject(string strKeyWord, int intPageIndex, int intPageSize)
        {
            object[] objKeyWords = new object[] {
                "@PAGESIZE", intPageSize,
                "@PAGEINDEX", intPageIndex,
                "@KEYWORD", strKeyWord
            };

            DataTable dtbSubject = new DataConnect().callUSP("USP_SUBJECT_SHR", objKeyWords);
            if (dtbSubject != null && dtbSubject.Rows.Count != 0)
            {
                return dtbSubject;  
            }

            return null;
        }

        public StringBuilder genHTMLComboSubject(DataTable dtbSubject, string strSubjectID = "")
        {
            StringBuilder strHTMLComboSubject = new StringBuilder();

            if (dtbSubject != null && dtbSubject.Rows.Count > 0)
            {
                for (int i = 0; i < dtbSubject.Rows.Count; i++)
                {
                    strHTMLComboSubject.AppendFormat("<option value='{0}' {1}>{2}</option>",
                            Convert.ToString(dtbSubject.Rows[i]["SUBJECTID"]).Trim(),
                            Convert.ToString(dtbSubject.Rows[i]["SUBJECTID"]).Trim() == strSubjectID ? "selected" : "",
                            Convert.ToString(dtbSubject.Rows[i]["SUBJECTNAME"]).Trim());
                }
                
            }

            return strHTMLComboSubject;
        }

        //public DataTable GetSubjectList()
        //{
        //    DataTable dtb = new DataConnect().callUSP("uspGetSubjectList", null);
        //    if (dtb != null && dtb.Rows.Count != 0)
        //    {
        //        return dtb;
        //    }
        //    return null;
        //}

        public bool InsertSubject(string strSubjectName)
        {
            object[] objKeyWords = new object[] {
                "@SUBJECTNAME", strSubjectName
            };

            return new DataConnect().uspExecuteNonQuery("USP_SUBJECTS_ADD", objKeyWords);
        }
        public SubjectDTO GetSubjectByID(string strSubjectID)
        {
            SubjectDTO objResult = new SubjectDTO();

            object[] objKeyWords = new object[] {
                "@SUBJECTID", strSubjectID
            };

            DataTable dtbSubject = new DataConnect().callUSP("USP_SUBJECTS_SEL", objKeyWords);

            if (dtbSubject != null && dtbSubject.Rows.Count > 0)
            {
                objResult.SubjectID = Convert.ToString(dtbSubject.Rows[0]["SubjectID"]).Trim();
                objResult.SubjectName = Convert.ToString(dtbSubject.Rows[0]["SubjectName"]).Trim();
            }

            return objResult;
        }
        public bool UpdateSubject(string strSubjectID, string strSubjectName)
        {
            object[] objKeyWords = new object[] {
                "@SUBJECTID", strSubjectID,
                "@SUBJECTNAME", strSubjectName
            };

            return new DataConnect().uspExecuteNonQuery("USP_SUBJECTS_UPD", objKeyWords);
        }
        //public bool DeleteSubject(string strSubjectID, ref string strMessage)
        //{
        //    //Kiểm tra nhóm người dùng có đang tồn tại người dùng nào không
        //    DataTable dtbScoreBySubject = new ScoresDAO().GetScoreBySubject(strSubjectID);
        //    if (dtbScoreBySubject != null && dtbScoreBySubject.Rows.Count > 0)
        //    {
        //        strMessage = "Tồn tại điểm thuộc môn học!";
        //    }
        //    else
        //    {
        //        object[] objKeyWords = new object[] {
        //        "@SubjectID", strSubjectID
        //         };

        //        return new DataConnect().uspExecuteNonQuery("uspSubjects_DEL", objKeyWords);
        //    }

        //    return false;
        //}
    }
}
