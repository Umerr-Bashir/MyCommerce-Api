using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class Address
    {
        public int Id { get; set; }


        [Required]
        public int CustomerId { get; set; }


        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 15, ErrorMessage = "Present Address should be between 15 to 40 character")]
        public string PresentAddress { get; set; }



        [Required]
        [StringLength(40,MinimumLength =15, ErrorMessage = "Permanent Address should be between 15 to 40 character")]
        public string  PermanentAddress { get; set; }




        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "State is required")]
        public string State { get; set; }




        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "City is required")]
        public string  City { get; set; }




        [Required]
        [StringLength(8,MinimumLength = 4 , ErrorMessage ="Postal code is required")]
        public string  PostalCode { get; set; }


    }
}
