﻿using Microsoft.AspNetCore.Identity;

namespace WebBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }

    }
}
