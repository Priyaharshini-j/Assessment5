namespace OnlineDiscussionForum.Models
{
    public class DiscussionModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string topic { get; set; }
        public string description { get; set; }
        public DateTime forumCreated { get; set; }
    }
}
