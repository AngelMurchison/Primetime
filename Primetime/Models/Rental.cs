using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Primetime.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int customerID { get; set; }
        public int movieID { get; set; }
        public DateTime dateCheckedOut { get; set; }
        public DateTime dueDate { get; set; }
        public bool checkedOut { get; set; } // Don't know if should be in movie or Rental or viewModel
    }
}