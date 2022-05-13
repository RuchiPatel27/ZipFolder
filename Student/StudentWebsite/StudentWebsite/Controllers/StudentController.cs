using StudentWebsite.Repository;
using StudentWebsite.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using StudentWebsite.Bal;

namespace StudentWebsite.Controllers
{
    public class StudentController : Controller
    {
        IStudentRepository objStudentRepository = new StudentRepository();
        public ActionResult Student()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentList()
        {
            try
            {
                List<string> columns = new List<string>();

                columns.Add("RegistrationDate");
                columns.Add("StudentName");
                columns.Add("Address");
                columns.Add("Class");
                columns.Add("Age");
                columns.Add("Hobbies");
                columns.Add("Gender");
                columns.Add("State");
                columns.Add("City");
                columns.Add("Pincode");

                int ajaxDraw = Convert.ToInt32(Request.Form["draw"]);

                int OffsetValue = Convert.ToInt32(Request.Form["start"]);

                int PagingSize = Convert.ToInt32(Request.Form["length"]);

                string searchby = Request.Form["search[value]"];

                string sortColumn = Request.Form["order[0][column]"];

                int colindex = Convert.ToInt32(sortColumn);
                sortColumn = columns[colindex == 0 ? colindex : colindex - 1];
                
                string sortDirection = Request.Form["order[0][dir]"];

                var reult = objStudentRepository.GetAllStudent(OffsetValue, PagingSize, sortColumn, sortDirection, searchby);
                return Json(new
                {
                    data = reult.Item1,
                    draw = ajaxDraw,
                    recordsFiltered = reult.Item2,
                    recordsTotal = reult.Item2,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Something Went Wrong.");
            }
        }

        [HttpPost]
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                var reult = objStudentRepository.ToogleDeletedStudentState(id);
                return Json(reult);
            }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Something Went Wrong.");
            }
        }

        public ActionResult ExportStudentData()
        {
            try
            {
                var gv = new GridView();
                gv.DataSource = objStudentRepository.GetStudentList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=StudentExcel.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View("Student");
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public JsonResult GetStateList()
        {
            var state = objStudentRepository.GetStateList();
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityByState(int state)
        {
            var city = objStudentRepository.GetCityList(state);
            return Json(city, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RegisterStudent(StudentModel studentModel)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if(file!=null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string _fileName = DateTime.Now.ToString("yymmssfff") + fileName;
                        string extension = Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/StudentPhoto"), _fileName);
                        studentModel.Photo = _fileName;

                        bool isInserted = objStudentRepository.AddStudent(studentModel);
                        if (isInserted)
                        {
                            file.SaveAs(path);
                        }
                        return Json(new {Status=true,Message= "inserted" });
                    }
                }
                return Json(new { Status = false, Message = "error" });
                
            }
            catch(Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Status = false, Message = "error" });
            }
            
        }

        [HttpGet]
        public ActionResult GetStudentById(int id)
        {
            try
            {
                var reult = objStudentRepository.GetStudentById(id);
                Session["Photo"] = reult.Photo;
                return Json(reult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Something Went Wrong.");
            }
        }

        [HttpPost]
        public ActionResult UpdateStudent(StudentModel studentModel)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string _fileName = DateTime.Now.ToString("yymmssfff") + fileName;
                        string extension = Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/StudentPhoto"), _fileName);
                        studentModel.Photo = _fileName;

                        bool isUpdated = objStudentRepository.UpdateStudent(studentModel);
                        string oldPath = Path.Combine(Server.MapPath("~/Content/StudentPhoto"),Session["Photo"].ToString());
                        if (isUpdated)
                        {
                            file.SaveAs(path);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                            
                        }
                        return Json(new { Status = true, Message = "updated" });
                    }
                }
                else
                {
                    studentModel.Photo=Session["Photo"].ToString();
                    bool isUpdated = objStudentRepository.UpdateStudent(studentModel);
                    return Json(new { Status = true, Message = "updated" });
                }

                return Json(new { Status = false, Message = "Something went wrong" });
            }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Something Went Wrong.");
            }
        }

    }
}