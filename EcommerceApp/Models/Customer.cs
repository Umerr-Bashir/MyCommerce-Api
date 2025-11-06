using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EcommerceApp.Models
{   //Unique Email
    [Index (nameof(Email) , IsUnique = true)]
    public class Customer
    {
        public int  Id { get; set; }

        //Data Annotations

        [Required(ErrorMessage = "First Name Is Required")]
        [StringLength(6 ,MinimumLength = 3 , ErrorMessage ="First name must Be between 3 And 6 characters")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name Is Required")]
        [StringLength(6 , MinimumLength = 3 , ErrorMessage = "Last name must be between 3 and 6 character")]
        public string LastName { get; set; }



        [Required(ErrorMessage = "Date Of Birth Is Required")]
        public DateTime DateOfBirth { get; set; }


        [StringLength(12 , MinimumLength = 15 , ErrorMessage = "Phone number must be between 12 and 15 character")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password Is Required")]
        public string  Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }

        public string Roles { get; set; }
        //Relationships
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }


    }
}
