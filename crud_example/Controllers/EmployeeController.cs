
using System.Web.Mvc;
using crud_example.Models.Repository;
using crud_example.Models;
using System.Collections.Generic;

namespace crud_example.Controllers
{

    public class EmployeeController : Controller
    {

        // GET: Employee/GetAllEmpDetails
        public ActionResult GetAllEmpDetails()
        {
            EmpRepository EmpRepo = new EmpRepository();
            return View(EmpRepo.GetAllEmployees());
        }
        // GET: Employee/AddEmployee
        public ActionResult AddEmployee()
        {
            EmpRepository EmpRepo = new EmpRepository();

            // Get the list of subjects from the repository
            List<SelectListItem> subjectList = EmpRepo.GetSubjects();

            // Assign the subject list to a property in the model
            EmpModel model = new EmpModel
            {
                SubjectList = subjectList
            };

            return View(model);

            //return View();
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
                    EmpRepo.AddEmployee(Emp);
                    ViewBag.Message = "Records added successfully.";
                    //TempData["SuccessMessage"] = "Records added successfully.";

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
                    ViewBag.AlertMsg = "Employee details deleted successfully";
                }
                return RedirectToAction("GetAllEmpDetails");
            }
            catch
            {
                return RedirectToAction("GetAllEmpDetails");
            }
        }
    }
}




