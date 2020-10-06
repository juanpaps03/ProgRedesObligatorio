namespace Domain
{
    public class Comment
    {
        public Photo Photo { get; set; }
        public User User { get; set; }
        public string Text { get; set; }

        public Comment()
        {
        }
    }
}