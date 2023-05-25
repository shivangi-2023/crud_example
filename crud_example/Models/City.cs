using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crud_example.Models
{
    public class City
    {
       // public IEnumerable<City> Cities { get; internal set; }
       public int CityId { get; set; }
        public string CityName { get; set; }
    }
}