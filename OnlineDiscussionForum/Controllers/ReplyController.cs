using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using OnlineDiscussionForum.Models;
using System.Data.SqlClient;

namespace OnlineDiscussionForum.Controllers
{
    public class ReplyController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _Connection;
        public string user = "";
        public ReplyController(IConfiguration configuration)
        {
            _configuration = configuration;
            _Connection = new SqlConnection(_configuration.GetConnectionString("Users"));
        }
       
        private DiscussionModel GetDiscussionById(int id)
        {
            DiscussionModel discussion = null;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Users")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Discussion WHERE Id=@Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    discussion = new DiscussionModel
                    {
                        Id = (int)dr["Id"],
                        Email = (string)dr["Email"],
                        topic = (string)dr["Title"],
                        description = (string)dr["Description"],
                        forumCreated = (DateTime)dr["DateCreated"]
                    };
                }
                dr.Close();
            }

            return discussion;
        }

        private List<ReplyModel> GetRepliesByDiscussionId(int id)
        {
            List<ReplyModel> replies = new List<ReplyModel>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Users")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Reply WHERE DiscussionId=@DiscussionId ORDER BY ReplyCreated ASC", connection);
                cmd.Parameters.AddWithValue("@DiscussionId", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ReplyModel reply = new ReplyModel
                    {
                        Id = (int)dr["Id"],
                        DiscussionId = (int)dr["DiscussionId"],
                        Email = (string)dr["Email"],
                        Content = (string)dr["Content"],
                        ReplyCreated = (DateTime)dr["ReplyCreated"]
                    };
                    replies.Add(reply);
                }
                dr.Close();
            }

            return replies;
        }
        [HttpPost]
        public IActionResult CreateReply(ReplyModel newReply)
        {
            if (ModelState.IsValid)
            {
                var connection = new SqlConnection(_configuration.GetConnectionString("Users"));
                connection.Open();
                string query = "INSERT INTO dbo.Reply (DiscussionId, Email, Content, ReplyCreated) VALUES (@DiscussionId, @Email, @Content, @ReplyCreated)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@DiscussionId", newReply.DiscussionId);
                cmd.Parameters.AddWithValue("@Email", newReply.Email);
                cmd.Parameters.AddWithValue("@Content", newReply.Content);
                cmd.Parameters.AddWithValue("@ReplyCreated", DateTime.Now);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction("Details", "Discussion", new { id = newReply.DiscussionId });
            }



            // If model state is invalid, return to the same page with the model to show validation errors
            var discussion = GetDiscussionById(newReply.DiscussionId);
            var replies = GetRepliesByDiscussionId(newReply.DiscussionId);

            var model = new DiscussionViewModel
            {
                Discussion = discussion,
                Replies = replies,
                NewReply = newReply
            };

            return View("Details", model);
        }
    }

}
