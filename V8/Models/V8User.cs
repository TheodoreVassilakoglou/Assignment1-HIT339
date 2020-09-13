using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace V8.Models
{
    // Add profile data for application users by adding properties to the V8User class
    public class V8User : IdentityUser
    {
        public string Name { get; set; }
    }
}
