using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class DeleteDTO
    {
        public int? Id { get; set; }
        public string? email { get; set; }
        public string? username { get; set; }

    }
}