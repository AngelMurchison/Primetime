using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Primetime.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? genreID { get; set; }
        public bool? isCheckedOut { get; set; }
        public string genreName { get; set; }
    }
}