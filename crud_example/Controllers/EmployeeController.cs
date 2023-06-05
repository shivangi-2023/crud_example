using System.Web.Mvc;
using crud_example.Models.Repository;
using crud_example.Models;
using System.Collections.Generic;
using Rotativa;
using OfficeOpenXml;
using System.Linq;
using PagedList;
using System;

namespace crud_example.Controllers
{

    public class EmployeeController : Controller
    {        
        public ActionResult GetAllEmpDetails(string Sorting_Order, string searchString, int Page = 1)
        {
            EmpRepository EmpRepo = new EmpRepository();
            var employees = EmpRepo.GetAllEmployees();
            ViewBag.TotalPages = Math.Ceiling(employees.Count() / 10.0);
            employees = employees.Skip((Page - 1) * 10).Take(10).ToList();


            ViewBag.SortingUsername = String.IsNullOrEmpty(Sorting_Order) ? "Username" : "";
            ViewBag.SortingEmail = String.IsNullOrEmpty(Sorting_Order) ? "Email" : "";
            ViewBag.SortingDOB = String.IsNullOrEmpty(Sorting_Order) ? "DOB" : "";
            ViewBag.SortingGender = String.IsNullOrEmpty(Sorting_Order) ? "Gender" : "";
            ViewBag.SortingCityName = String.IsNullOrEmpty(Sorting_Order) ? "CityName" : "";
            if (!String.IsNullOrEmpty(searchString))
            {
                string lowerSearchString = searchString.ToLower(); // Convert the search string to lowercase
                employees = employees.Where(e =>
                    e.Username.ToLower().StartsWith(lowerSearchString) || // Check if the username starts with the search string
                    e.Email.ToLower().StartsWith(lowerSearchString)|| // Check if the email starts with the search string
                      e.CityName.ToLower().StartsWith(lowerSearchString) || // Check if the city name starts with the search string
                      e.Gender.ToLower().StartsWith(lowerSearchString) 
                    ).ToList();
            }
                       
            switch (Sorting_Order)
            {
                case "Username":
                    employees = employees.OrderByDescending(e => e.Username).ToList();
                    break;
                case "Email":
                    employees = employees.OrderByDescending(e => e.Email).ToList();
                    break;

                case "DOB":
                    employees = employees.OrderByDescending(e => e.DOB).ToList();
                    break;
                case "Gender":
                    employees = employees.OrderByDescending(e => e.Gender).ToList();
                    break;
                case "CityName":
                    employees = employees.OrderBy(e => e.CityName).ToList();
                    break;
                default:
                    employees = employees.OrderBy(e => e.Username).ToList();
                    break;

            }
            return View(employees);
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
            var properties = typeof(EmpModel).GetProperties().Where(p => p.Name != "CityId").ToList();
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


