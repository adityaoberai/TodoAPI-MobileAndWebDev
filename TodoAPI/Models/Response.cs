using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class Response
    {
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
    }

    public class TodoResponse : Response
    {
        public Todo Response { get; set; }
    }

    public class TodoListResponse : Response
    {
        public List<Todo> Response { get; set; }
    }
}
