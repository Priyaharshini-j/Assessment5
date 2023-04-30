namespace OnlineDiscussionForum.Models
{
    public class ReplyModel
    {
        public int Id { get; set; }
        public int DiscussionId { get; set; }
        public string Email { get; set; }

        public string Content { get; set; }
        public DateTime ReplyCreated { get; set; }
    }
}
