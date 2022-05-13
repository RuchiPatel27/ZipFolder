using StudentWebsite.Bal;
using StudentWebsite.Dal;
using StudentWebsite.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentWebsite.Repository
{
    public class StudentRepository : IStudentRepository
    {
        readonly StudentDBEntities _entities = new StudentDBEntities();

        public IList<StudentModel> GetStudentList()
        {
            var studentList = _entities.Students.Select(x=>new StudentModel { 
                StudentName=x.StudentName,
                Class=x.Class,
                Age=x.Age,
                Gender=x.Gender,
                Hobbies=x.Hobbies,
                Photo=x.Photo,
                StateName=x.City.State.StateName,
                CityName=x.City.CityName,
                Address=x.Address,
                Pincode=x.Pincode
            }).ToList();
            return studentList;
        }

        public Tuple<List<StudentModel>, int> GetAllStudent(int startno, int perpagecount, string orderby, string order, string search)
        {
            try
            {
                int totalrec = 0;
                var student = from students in _entities.Students
                           select new StudentModel
                           {
                              RegistrationDate=students.RegistrationDate,
                              Address=students.Address,
                              Age=students.Age,
                              CityName=students.City.CityName,
                              StateName=students.City.State.StateName,
                              Class=students.Class,
                              Gender=students.Gender,
                              Hobbies=students.Hobbies,
                              Pincode=students.Pincode,
                              StudentName=students.StudentName,
                              IsDeleted=students.IsDeleted,
                              Id=students.Id,
                              Photo=students.Photo
                           };

                if (!String.IsNullOrEmpty(search))
                {
                    student = student.Where(x => x.StudentName.Contains(search) || x.Address.Contains(search) || x.Age.ToString().Contains(search) 
                    || x.CityName.Contains(search) || x.StateName.Contains(search) || x.Class.ToString().Contains(search)
                    || x.Gender.Contains(search) || x.Hobbies.Contains(search) || x.Pincode.ToString().Contains(search));
                }

                totalrec = student.Count();

                if (order.Equals("asc", comparisonType: StringComparison.OrdinalIgnoreCase))
                {
                    switch (orderby)
                    {
                        case "RegistrationDate":
                            student = student.OrderBy(x => x.RegistrationDate);
                            break;

                        case "StudentName":
                            student = student.OrderBy(x => x.StudentName);
                            break;

                        case "Address":
                            student = student.OrderBy(x => x.Address);
                            break;

                        case "Class":
                            student = student.OrderBy(x => x.Class);
                            break;

                        case "Age":
                            student = student.OrderBy(x => x.Age);
                            break;

                        case "Hobbies":
                            student = student.OrderBy(x => x.Hobbies);
                            break;

                        case "Gender":
                            student = student.OrderBy(x => x.Gender);
                            break;

                        case "State":
                            student = student.OrderBy(x => x.StateName);
                            break;

                        case "City":
                            student = student.OrderBy(x => x.CityName);
                            break;

                        case "Pincode":
                            student = student.OrderBy(x => x.Pincode);
                            break;

                        default:
                            student = student.OrderBy(x => x.RegistrationDate);
                            break;
                    }
                }
                else
                {
                    switch (orderby)
                    {
                        case "RegistrationDate":
                            student = student.OrderByDescending(x => x.RegistrationDate);
                            break;

                        case "StudentName":
                            student = student.OrderByDescending(x => x.StudentName);
                            break;

                        case "Address":
                            student = student.OrderByDescending(x => x.Address);
                            break;

                        case "Class":
                            student = student.OrderByDescending(x => x.Class);
                            break;

                        case "Age":
                            student = student.OrderByDescending(x => x.Age);
                            break;

                        case "Hobbies":
                            student = student.OrderByDescending(x => x.Hobbies);
                            break;

                        case "Gender":
                            student = student.OrderByDescending(x => x.Gender);
                            break;

                        case "State":
                            student = student.OrderByDescending(x => x.StateName);
                            break;

                        case "City":
                            student = student.OrderByDescending(x => x.CityName);
                            break;

                        case "Pincode":
                            student = student.OrderByDescending(x => x.Pincode);
                            break;

                        default:
                            student = student.OrderByDescending(x => x.RegistrationDate);
                            break;
                    }
                }

                List<StudentModel> result = null;
                if (perpagecount > 0)
                {
                    result = student.Skip(startno).Take(perpagecount).ToList();
                }
                else
                {
                    result = student.Skip(startno).ToList();
                }

                return new Tuple<List<StudentModel>, int>(result, totalrec);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ToogleDeletedStudentState(int id)
        {
            try
            {
                Student student = _entities.Students.Where(x => x.Id == id).FirstOrDefault();
                if (student != null)
                {
                    student.IsDeleted = !student.IsDeleted;
                    _entities.Entry(student).State = EntityState.Modified;
                    _entities.SaveChanges();
                    return "";
                }
                else
                {
                    return "Not Found";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StateModel> GetStateList()
        {
            try
            {
                var stateModel = _entities.States.Select(x=>new StateModel { 
                    Id=x.Id,
                    StateName=x.StateName
                }).ToList();
                return stateModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CityModel> GetCityList(int state)
        {
            try
            {
                var cityModel = _entities.Cities.Where(x => x.StateId == state).Select(x=>new CityModel { 
                    Id=x.Id,
                    CityName=x.CityName
                }).ToList();
                return cityModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool AddStudent(StudentModel studentModel)
        {
            try
            {
                var student = new Student
                {
                    StudentName=studentModel.StudentName,
                    Address=studentModel.Address,
                    Age=studentModel.Age,
                    CityId=studentModel.CityId,
                    Gender=studentModel.Gender,
                    Class=studentModel.Class,
                    Hobbies=studentModel.Hobbies,
                    Photo=studentModel.Photo,
                    Pincode=studentModel.Pincode,
                    RegistrationDate=DateTime.UtcNow,
                };

                _entities.Students.Add(student);
                _entities.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public StudentModel GetStudentById(int id)
        {

            var student = _entities.Students.FirstOrDefault(x => x.Id == id);
            if (student != null)
            {
                StudentModel objStudentModel = new StudentModel
                {
                    Address=student.Address,
                    Age=student.Age,
                    CityId=student.CityId,
                    StateId=student.City.StateId,
                    Class=student.Class,
                    Gender=student.Gender,
                    Pincode=student.Pincode,
                    StudentName=student.StudentName,
                    Hobbies=student.Hobbies,
                    Photo=student.Photo,
                };
                return objStudentModel;
            }
            return null;
        }

        public bool UpdateStudent(StudentModel studentModel)
        {
            try
            {
                Student student = _entities.Students.Where(x => x.Id == studentModel.Id).FirstOrDefault();
                if (student != null)
                {
                    student.StudentName = studentModel.StudentName;
                    student.Pincode = studentModel.Pincode;
                    student.Class = studentModel.Class;
                    student.Gender = studentModel.Gender;
                    student.Hobbies = studentModel.Hobbies;
                    student.Photo = studentModel.Photo;
                    student.Address = studentModel.Address;
                    student.Age = studentModel.Age;
                    student.CityId = studentModel.CityId;

                    _entities.Entry(student).State = EntityState.Modified;
                    _entities.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
