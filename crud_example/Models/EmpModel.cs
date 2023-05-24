using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace crud_example.Models
{
    public class EmpModel
    {
        [Display(Name = "Id")]
        public int Userid { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]


        public string DOB { get; set; }
        [Required(ErrorMessage = "Gender is required.")]


        public string Gender { get; set; }

        //[Required(ErrorMessage = "Skills are required.")]
        ///public List<string> Skills { get; set; }
        ///
        public EmpModel()
        {
            SubjectList = new List<SelectListItem>();
        }
        [DisplayName("City")]
        public List<SelectListItem> SubjectList
        {
            get;
            set;
        }
    }

}