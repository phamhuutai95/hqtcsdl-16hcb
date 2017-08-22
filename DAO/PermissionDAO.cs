using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class PermissionDAO
    {
        public DataTable SearchPermission(string strKeyWord, int intPageIndex, int intPageSize)
        {
            object[] objKeyWords = new object[] {
                "@PAGESIZE", intPageSize,
                "@PAGEINDEX", intPageIndex,
                "@KEYWORD", strKeyWord
            };

            DataTable dtbPermission = new DataConnect().callUSP("USP_PERMISSIONS_SHR", objKeyWords);
            if (dtbPermission != null && dtbPermission.Rows.Count != 0)
            {
                return dtbPermission;
            }

            return null;
        }
    }
}
