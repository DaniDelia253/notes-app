using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Responses
{
    public class UserExisitsResponse
    {
        public bool userAlreadyExists { get; set; }
        public string? message { get; set; }
    }
}