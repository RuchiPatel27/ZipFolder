using StudentWebsite.Bal;
using StudentWebsite.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentWebsite.Repository.Interface
{
    public interface IStudentRepository
    {
        IList<StudentModel> GetStudentList();
        Tuple<List<StudentModel>, int> GetAllStudent(int startno, int perpagecount, string orderby, string order, string search);
        string ToogleDeletedStudentState(int id);
        List<StateModel> GetStateList();
        List<CityModel> GetCityList(int state);

        bool AddStudent(StudentModel studentModel);
        StudentModel GetStudentById(int id);
        bool UpdateStudent(StudentModel studentModel);
    }
}
