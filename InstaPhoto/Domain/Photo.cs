using System.IO;

namespace Domain
{
    public class Photo
    {
        public User Client { get; set; }
        public string File { get; set; }
        public string Name { get; set;  }

        public Photo()
        {
        }
    }
}