using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Primetime.Models
{
    public class RentalViewModel
    {
        public int? Id { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; }
        public string movieName { get; set; }
        public DateTime? rentalDate { get; set; }
        public DateTime? dueDate { get; set; }
        public bool? checkedOut { get; set; } // Don't know if should be in movie or Rental or viewModel
    }
}