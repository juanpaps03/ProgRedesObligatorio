using Dapper.Contrib.Extensions;

namespace Repositories.Dtos
{
    [Table("Users")]
    public class UserDto
    {
        [ExplicitKey]
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set;  }
    }
}