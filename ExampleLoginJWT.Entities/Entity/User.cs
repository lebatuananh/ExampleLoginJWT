using System;
using System.ComponentModel.DataAnnotations;
using ExampleLoginJWT.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExampleLoginJWT.Domain.Entity
{
    public class User : IdentityUser, IDateTracking
    {
        public User()
        {
        }

        public User(string id, string userName, string firstName, string lastName,
            string email, string phoneNumber, DateTime dob)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Dob = dob;
        }

        [MaxLength(50)] [Required] public string FirstName { get; set; }
        [MaxLength(50)] [Required] public string LastName { get; set; }
        [MaxLength(50)] public string PersonalCode { get; set; }
        [MaxLength(50)] public string UnitCode { get; set; }
        [MaxLength(50)] public string Code { get; set; }
        [MaxLength(50)] public string BlockCode { get; set; }
        [Required] public DateTime Dob { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}