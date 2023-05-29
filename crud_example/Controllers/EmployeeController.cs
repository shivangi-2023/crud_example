using System.Web.Mvc;
using crud_example.Models.Repository;
using crud_example.Models;
using System.Collections.Generic;
using Rotativa;
using OfficeOpenXml;
using System.Linq;

namespace crud_example.Controllers
{

    public class EmployeeController : Controller
    {

        // GET: Employee/GetAllEmpDetails
        public ActionResult GetAllEmpDetails(string searchString, int Page = 1)
        {
            EmpRepository EmpRepo = new EmpRepository();

            return View(EmpRepo.GetAllEmployees());

        }
        // GET: Employee/AddEmployee
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
                    ViewBag.Message = "Records added successfully.";
                    ModelState.Clear();
                    //TempData["SuccessMessage"] = "Records added successfully.";
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
        public ActionResult EditEmpDetails(int id, EmpModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    ViewData["CityList"] = EmpRepo.GetCities();

                    EmpRepo.UpdateEmployee(obj);
                    return RedirectToAction("GetAllEmpDetails");
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
                    ViewBag.Message = "Employee details deleted successfully";
                    //ViewBag.AlertMsg = "Employee details deleted successfully";
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

            return new PartialViewAsPdf("Userdata", employees)
            {
                FileName = "GetUserdetails.pdf"
            };
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




