using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Repositories.Dtos
{
    [Dapper.Contrib.Extensions.Table("Comments")]
    public class CommentDto
    {
        [ExplicitKey]
        [ForeignKey("Photo")]
        public string NamePhoto { get; set; }
        
        [ExplicitKey]
        [ForeignKey("User")]
        public string UserName { get; set; }
        public string Text { get; set; }
    }
}