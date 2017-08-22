using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TeacherDTO
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleID { get; set; }
        public string SubjectID { get; set; }
        public string ManagerID { get; set; }
        public string Password { get; set; }
        public bool IsBlock { get; set; }

    }
}
