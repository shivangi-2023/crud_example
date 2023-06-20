using System.Web.Mvc;
using crud_example.Models.Repository;
using crud_example.Models;
using System.Collections.Generic;
using Rotativa;
using OfficeOpenXml;
using System.Linq;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using PagedList.Mvc;
using System.Web.UI;
using Microsoft.Graph;


namespace crud_example.Controllers
{  
    public class EmployeeController : Controller
    {        
        public ActionResult GetAllEmpDetails(string sortingOrder, string searchString, string Filter_Value, int? page,int? status)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            ViewBag.SortingName = String.IsNullOrEmpty(sortingOrder) ? "Username" : "";
            ViewBag.SortingDate = sortingOrder == "Email" ? "DOB" : "Gender";   
            EmpRepository EmpRepo = new EmpRepository();
            var employees = EmpRepo.GetAllEmployees();
            // Apply search filter if a search string is provided
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = Filter_Value;
            }

            ViewBag.FilterValue = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {   
                employees = employees.Where(e =>
                    e.Username.ToUpper().Contains(searchString.ToUpper()) || e.Email.ToUpper().Contains(searchString.ToUpper())||
                    e.Gender.ToUpper().Contains(searchString.ToUpper())|| e.CityName.ToUpper().Contains(searchString.ToUpper())
                  ).ToList();
            }

            //List<SelectListItem> statusList = new List<SelectListItem>
            //{
            //    new SelectListItem { Value = "", Text = "Show All" },
            //    new SelectListItem { Value = "1", Text = "Active" },
            //    new SelectListItem { Value = "0", Text = "Inactive" }
            //};8

            //ViewBag.StatusList = statusList;

            //}
            //Apply filtering based on status if provided
                if (status == 1)
                {
                //employees = employees.Where(e => e.Status == status.ToString()).ToList(); // Show active records
                employees = EmpRepo.Softdeletefilter(status);//add status para
                 }
                else if (status == 0)
                {
                //employees = employees.Where(e => e.Status == status.ToString()).ToList(); // Show deleted records
                employees = EmpRepo.Softdeletefilter(status);
            }

            // Apply sorting order
            switch (sortingOrder)
            {
                case "Username":
                    employees = employees.OrderBy(e => e.Username).ToList();
                    break;
                case "Email":
                    employees = employees.OrderBy(e => e.Email).ToList();
                    break;
                case "DOB":
                    employees = employees.OrderBy(e => e.DOB).ToList();
                    break;
                case "Gender":
                    employees = employees.OrderBy(e => e.Gender).ToList();
                    break;
                case "CityName":
                    employees = employees.OrderBy(e => e.CityName).ToList();
                    break;
                default:
                    employees = employees.OrderBy(e => e.Username).ToList();
                    break;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            //////ViewBag.FilterValue = searchString;
             return View(employees.ToPagedList(pageNumber, pageSize));            
        }
        //[HttpPost]
        //public ActionResult GetAllEmpDetails(string sortingOrder, string searchString, string Filter_Value, int? page,string status)
        //{
        //    ViewBag.CurrentSortOrder = sortingOrder;
        //    ViewBag.SortingName = String.IsNullOrEmpty(sortingOrder) ? "Username" : "";
        //    ViewBag.SortingDate = sortingOrder == "Email" ? "DOB" : "Gender";
        //    EmpRepository EmpRepo = new EmpRepository();
        //    var employees = EmpRepo.GetAllEmployees();
        //    // Apply search filter if a search string is provided
        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = Filter_Value;
        //    }

        //    ViewBag.FilterValue = searchString;
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        employees = employees.Where(e =>
        //            e.Username.ToUpper().Contains(searchString.ToUpper()) || e.Email.ToUpper().Contains(searchString.ToUpper()) ||
        //            e.Gender.ToUpper().Contains(searchString.ToUpper()) || e.CityName.ToUpper().Contains(searchString.ToUpper())
        //          ).ToList();
        //    }
        //    // Apply sorting order
        //    switch (sortingOrder)
        //    {
        //        case "Username":
        //            employees = employees.OrderBy(e => e.Username).ToList();
        //            break;
        //        case "Email":
        //            employees = employees.OrderBy(e => e.Email).ToList();
        //            break;
        //        case "DOB":
        //            employees = employees.OrderBy(e => e.DOB).ToList();
        //            break;
        //        case "Gender":
        //            employees = employees.OrderBy(e => e.Gender).ToList();
        //            break;
        //        case "CityName":
        //            employees = employees.OrderBy(e => e.CityName).ToList();
        //            break;
        //        default:
        //            employees = employees.OrderBy(e => e.Username).ToList();
        //            break;
        //    }
        //    int pageSize = 6;
        //    int pageNumber = (page ?? 1);
        //    //////ViewBag.FilterValue = searchString;
        //    return View(employees.ToPagedList(pageNumber, pageSize));           
        //}



        //GET: Employee/AddEmployee
        public ActionResult AddEmployee()
        {
            EmpRepository EmpRepo = new EmpRepository();
            ViewData["CityList"] = EmpRepo.GetCities();
            return View();

        }
        // POST: Employee/AddEmployee
        [HttpPost]
        public ActionResult AddEmployee(EmpModel Emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    ViewData["CityList"] = EmpRepo.GetCities();
                    EmpRepo.AddEmployee(Emp);
                    TempData["SuccessMessage"] = "Records added successfully.";
                    ModelState.Clear();
                    return View();
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditEmpDetails(int id)
        {
            EmpRepository EmpRepo = new EmpRepository();
            ViewData["CityList"] = EmpRepo.GetCities();
            ModelState.Clear();

            return View(EmpRepo.GetAllEmployees().Find(Emp => Emp.Userid == id));
           
        }
        // POST:Update the details into database
        [HttpPost]
        public ActionResult EditEmpDetails( EmpModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    ViewData["CityList"] = EmpRepo.GetCities();
                    EmpRepo.UpdateEmployee(obj);
                    TempData["SuccessMessage"] = "Records Updated successfully.";
                    return RedirectToAction("EditEmpDetails");
                }
                else
                {
                    return View(obj);
                }

            }
            catch
            {
                return View();
            }
        }
        // GET: Delete  Employee details by id
        public ActionResult DeleteEmp(int id)
        {
            try
            {   
                EmpRepository EmpRepo = new EmpRepository();
                if (EmpRepo.DeleteEmployee(id))
                {
                    TempData["SuccessMessage"] = "Records Deleted successfully.";

                }

                return RedirectToAction("GetAllEmpDetails");
            }
            catch
            {
                return RedirectToAction("GetAllEmpDetails");
            }
        }
        public ActionResult Details(int id)
        {
            try
            {
                EmpRepository EmpRepo = new EmpRepository();
                EmpModel Emp = EmpRepo.GetEmployeeDetails(id);
                return View("Details", Emp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult RestoredEmp()
        //{
        //    try
        //    {   
        //        EmpRepository EmpRepo = new EmpRepository();
        //        // Get the employee details
        //        var employee = EmpRepo.GetDeletedEmployees();
        //        if (employee == null)
        //        {
        //            ViewBag.ErrorMsg = "No deleted record found";
        //            return RedirectToAction("GetAllEmpDetails");
        //        }
        //        return View(employee);
        //    }
        //    catch
        //    {
        //        return RedirectToAction("GetAllEmpDetails");
        //    }
        //}


        public ActionResult PrintPDF()
        {
            EmpRepository EmpRepo = new EmpRepository();
            var employees = EmpRepo.GetAllEmployees();

            var pdf = new ViewAsPdf("Userdata", employees)
            {
                FileName = "GetUserdetails.pdf",
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4
            };

            // Set the custom options for prompting the user to print
            pdf.CustomSwitches = "--print-media-type --header-html \"\" --footer-html \"\"";
            // Show the PDF in the browser
            return pdf;
        }



        public void ExportListUsingEPPlus()
        {
            EmpRepository EmpRepo = new EmpRepository();
            var data = EmpRepo.GetAllEmployees();
            //INSTALL ExcelPackage
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            // Load data into the worksheet, excluding the CityId property
            var properties = typeof(EmpModel).GetProperties().Where(p => p.Name != "CityId" && p.Name != "Status" ).ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = properties[i].Name;
            }

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < properties.Count; j++)
                {
                    workSheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(data[i]);
                }
            }

            using (var memoryStream = new System.IO.MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Users_List.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}


