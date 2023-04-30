
namespace OnlineDiscussionForum.Models
{
    public class DiscussionViewModel
    {
        public DiscussionModel Discussion { get; set; }
        public List<ReplyModel> Replies { get; set; }
        public ReplyModel NewReply { get; set; }
    }

}
