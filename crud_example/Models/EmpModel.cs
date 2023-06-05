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
        [Required(ErrorMessage = "Email is required and Please enter a valid email address.")]

        public string Email { get; set; }

        [Required(ErrorMessage = "BirthDate is required.")]


        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Gender is required.")]


        public string Gender { get; set; }
        [Required(ErrorMessage = "City is required.")]

        public int CityId { get; set; }

        public String CityName { get; set; }
        //public bool IsDeleted { get; set; }
        public bool isActive { get; set; }
        public DateTime? DeletedAt { get; set; }


    }
    public interface ICustomSoftDelete
    {
        bool isActive { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}