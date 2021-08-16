using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models.DTO
{
    public class TodoDTO
    {
        public string Name { get; set; }
        public bool Completed { get; set; }
    }
}
