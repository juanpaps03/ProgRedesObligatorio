using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Repositories.Dtos
{
    [Dapper.Contrib.Extensions.Table("Comments")]
    public class CommentDto
    {
        [ExplicitKey] 
        private int Id { get; set;  }
        
        [ForeignKey("Photo")]
        public string NamePhoto { get; set; }

        [ForeignKey("User")]
        public string UserName { get; set; }
        public string Text { get; set; }
    }
}