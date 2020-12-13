using System;
using ExampleLoginJWT.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExampleLoginJWT.Domain.Entity
{
    public class Role: IdentityRole, IDateTracking
    {
        public Role()
        {
            
        }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}