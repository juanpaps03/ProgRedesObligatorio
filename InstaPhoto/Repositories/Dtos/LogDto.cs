using System;
using Dapper.Contrib.Extensions;

namespace Repositories.Dtos
{
    [Dapper.Contrib.Extensions.Table("Log")]
    
    public class LogDto
    {
        [Key] 
        public int Id { get; set;  }
        public long date { get; set; } 
        public string Message { get; set;  }
    }
}