namespace OnlineDiscussionForum.Controllers
{
    internal class FormsAuthenticationTicket
    {
        private int version;
        private string name;
        private DateTime issueDate;
        private DateTime expiration;
        private bool isPersistent;
        private object userData;

        public FormsAuthenticationTicket(int version, string name, DateTime issueDate, DateTime expiration, bool isPersistent, object userData)
        {
            this.version = version;
            this.name = name;
            this.issueDate = issueDate;
            this.expiration = expiration;
            this.isPersistent = isPersistent;
            this.userData = userData;
        }
    }
}