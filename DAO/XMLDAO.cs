using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Xml;
using System.Data;

namespace DAO
{
    public class XMLDAO
    {
        /// <summary>
        /// Hàm load menu trái động
        /// </summary>
        /// <param name="strpathXML">Đường dẫn file xml</param>
        /// <param name="strUsername">Mã người dùng</param>
        /// <returns></returns>
        public string DynamicXMLMenu(string strpathXML, string strUsername)
        {
            LoginDAO DLogin = new LoginDAO();
            DataTable PremissionKeyList = DLogin.GetKeyPermission(strUsername);
            XmlDocument xmlDoc = new XmlDocument();
            string htmlString = "";

            xmlDoc.Load(strpathXML);
            craftXML(xmlDoc.DocumentElement.ChildNodes, PremissionKeyList, ref htmlString, 1, null);

            return htmlString;
        }

        /// <summary>
        /// Hàm gen HTML menu trái
        /// </summary>
        /// <param name="xmlNote"></param>
        /// <param name="dtbPremissionKeyList"></param>
        /// <param name="strhtmlString"></param>
        /// <param name="intnavlevel"></param>
        private void craftXML(XmlNodeList xmlNote, DataTable dtbPremissionKeyList, ref string strhtmlString, int intnavlevel,List<string> ContainerHtml)
        {
            string snavlevel = "second";
            if (intnavlevel != 1)
            {
                snavlevel = "third";
            }

            foreach (XmlNode Node in xmlNote)
            {
                if (checkPermission(dtbPremissionKeyList, Node.Attributes["permissionkey"].Value) && Node.ChildNodes.Count != 0)
                {
                    List<string> ContainerHtml2 = new List<string>();
                    string strhtmlString2 = "";
                    craftXML(Node.ChildNodes, dtbPremissionKeyList, ref strhtmlString, 2, ContainerHtml2);
                    int iCountContainerHtml2 = ContainerHtml2.Count;
                    if (iCountContainerHtml2 > 0)
                    {
                        if (ContainerHtml != null)
                        {
                            strhtmlString2 += " <li> ";
                            strhtmlString2 += " <a href='#'><i class='fa fa-sitemap fa-fw'></i>" + Node.Attributes["text"].Value + "<span class='fa arrow'></span></a> ";
                            strhtmlString2 += " <ul class='nav nav-" + snavlevel + "-level'> ";
                            for (int i = 0; i < iCountContainerHtml2; i++)
                            {
                                strhtmlString2 += ContainerHtml2[i];
                            }
                            strhtmlString2 += "  </ul> ";
                            strhtmlString2 += " </li> ";
                            ContainerHtml.Add(strhtmlString2);
                        }
                        else
                        {
                            strhtmlString += " <li> ";
                            strhtmlString += " <a href='#'><i class='fa fa-sitemap fa-fw'></i>" + Node.Attributes["text"].Value + "<span class='fa arrow'></span></a> ";
                            strhtmlString += " <ul class='nav nav-" + snavlevel + "-level'> ";
                            for (int i = 0; i < iCountContainerHtml2; i++)
                            {
                                strhtmlString += ContainerHtml2[i];
                            }
                            strhtmlString += "  </ul> ";
                            strhtmlString += " </li> ";
                        }
                    }
                    
                }
                else if (checkPermission(dtbPremissionKeyList, Node.Attributes["permissionkey"].Value))
                {
                    if (ContainerHtml != null)
                    {
                        ContainerHtml.Add(" <li><a href=' " 
                                                            + Node.Attributes["url"].Value 
                                                            + " '><i class='fa fa-dashboard fa-fw'></i> " 
                                                            + Node.Attributes["text"].Value + "</a></li> ");
                    }
                    else
                    {
                        strhtmlString += " <li><a href=' " 
                                        + Node.Attributes["url"].Value 
                                        + " '><i class='fa fa-dashboard fa-fw'></i> " 
                                        + Node.Attributes["text"].Value + "</a></li> ";
                    }
                    
                }
            }

        }

        /// <summary>
        /// Hàm kiểm tra key quyền của người dùng
        /// </summary>
        /// <param name="dtbPremissionKeyList"></param>
        /// <param name="strcurrKey"></param>
        /// <returns></returns>
        public bool checkPermission(DataTable dtbPremissionKeyList, string strcurrKey)
        {
            if (strcurrKey == "0")
            {
                return true;
            }

            if (dtbPremissionKeyList != null && dtbPremissionKeyList.Rows.Count > 0)
            {
                int sizeListKey = dtbPremissionKeyList.Rows.Count;
                for (int i = 0; i < sizeListKey; i++)
                {
                    if ((string)dtbPremissionKeyList.Rows[i][0] == strcurrKey)
                    {
                        return true;
                    }
                }
            }

            return false;
        }





 

    }


   
}
