using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? email { get; set; }
        public string? username { get; set; }
        public string? passwordSalt { get; set; }
        public string? passwordHash { get; set; }

    }
}