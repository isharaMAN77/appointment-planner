using System.ComponentModel.DataAnnotations;

namespace AppointmentPlanner.Models
{
    public class Depot
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter a depot name.")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Enter a valid postal code.")]
        public string PostCode { get; set; }
    }
    public class Fleet
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Enter a valid registration.")]
        public string Registration { get; set; }
        [Required(ErrorMessage = "Enter a make name.")]
        public string Make { get; set; }
        [Required(ErrorMessage = "Enter a valid model.")]
        public string Model { get; set; }
        public string Year { get; set; }
        public bool Rented { get; set; }
        public int DepotID { get; set; }

    }
}
