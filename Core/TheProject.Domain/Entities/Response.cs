using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheProject.Domain.Entities
{
    public class Response<T>
    {

        //Genérico para ser usado com varias models e ainda por cima nulo fi
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;

        public bool Status { get; set; } = true;
    }
}