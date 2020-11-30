using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Repositories.Dtos
{
    [Dapper.Contrib.Extensions.Table("Comments")]
    public class CommentDto
    {
        [Key] 
        private int Id { get; set;  }
        
        [ForeignKey("Photos")]
        public string Username { get; set; }
        
        [ForeignKey("Photos")]
        public string PhotoName { get; set; }
        public string Text { get; set; }
    }
}