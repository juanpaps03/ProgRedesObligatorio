using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Repositories.Dtos
{
    [Dapper.Contrib.Extensions.Table("Photos")]
    public class PhotoDto
    {
        [ExplicitKey]
        public string Name { get; set;  }
        
        public string File { get; set; }

        [ExplicitKey]
        [ForeignKey("User")]
        public string Username { get; set;  }
    }
}