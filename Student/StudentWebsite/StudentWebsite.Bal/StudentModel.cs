using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentWebsite.Bal
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Address { get; set; }
        public int Class { get; set; }
        public int Age { get; set; }
        public string Hobbies { get; set; }
        public string Gender { get; set; }
        public int CityId { get; set; }
        public string Pincode { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public string Photo { get; set; }
        public bool IsDeleted { get; set; }

        public int StateId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
    }
}
