using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineDiscussionForum.Models;
using System.Data.SqlClient;
using System.Data;

namespace OnlineDiscussionForum.Controllers
{
    public class DiscussionController : Controller
    {

        IConfiguration _configuration;
        SqlConnection _Connection;
        public string user = "";
        public DiscussionController(IConfiguration configuration)
        {
            _configuration = configuration;
            _Connection = new SqlConnection(_configuration.GetConnectionString("Users"));
        }
        public List<DiscussionModel> GetDiscussion(string email)
        {
            List<DiscussionModel> alldiscussion = new();
            _Connection.Open();
            SqlCommand cmd1 = new SqlCommand("FetchDiscussion", _Connection);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@Email", email);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                DiscussionModel model = new DiscussionModel();
                model.Id = (int)dr1[0];
                model.Email = (string)dr1[1];
                model.topic = (string)(dr1[2]);
                model.description = dr1.GetString(3);
                model.forumCreated = (DateTime)dr1[4];
                alldiscussion.Add(model);
            }
            dr1.Close();
            _Connection.Close();
            return alldiscussion;
        }
        // GET: AdminController
        public ActionResult Index(string email)
        {
            return View(GetDiscussion(email));
        }

        // GET: DiscussionController/Details/5
        public IActionResult Details(int id)
        {

            var discussion = GetDiscussionById(id);
            var replies = GetRepliesByDiscussionId(id);

            if (discussion == null)
            {
                return NotFound();
            }

            ViewBag.Discussion = discussion;
            ViewBag.Replies = replies;

            var model = new DiscussionViewModel
            {
                Discussion = discussion,
                Replies = replies,
                NewReply = new ReplyModel { DiscussionId = id }
            };

            return View(model);
        }

        // POST: DiscussionController/Details/5
        [HttpPost]
        public ActionResult Details(ReplyModel newReply)
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

                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction("Details", new { id = newReply.DiscussionId });
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

        // GET: DiscussionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiscussionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiscussionModel discuss)
        {
            try
            {
                _Connection.Open();
                SqlCommand cmd = new SqlCommand("Insert_Discussion", _Connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", discuss.Email);
                cmd.Parameters.AddWithValue("@Title", discuss.topic);
                cmd.Parameters.AddWithValue("@Description", discuss.description);
                cmd.Parameters.AddWithValue("@Date", discuss.forumCreated);
                cmd.ExecuteNonQuery();
                _Connection.Close();
                return RedirectToAction("Index","Discussion",new { email = discuss.Email });
            }
            catch
            {
                return View();
            }
        }

        
        // GET: DiscussionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DiscussionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string email)
        {
            try
            {
                _Connection.Open();
                SqlCommand cmd = new SqlCommand($"DELETE FROM Discussion where id={id}", _Connection);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index", "Discussion", new {email="Ashley1234567@gmail.com"});
            }
            catch
            {
                return View();
            }
        }


        public List<DiscussionModel> GetAllDiscussion()
        {
            List<DiscussionModel> alldiscussion = new();
            _Connection.Open();
            SqlCommand cmd1 = new SqlCommand("FetchAllDiscussion", _Connection);
            cmd1.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                DiscussionModel model = new DiscussionModel();
                model.Id = (int)dr1[0];
                model.Email = (string)dr1[1];
                model.topic = (string)(dr1[2]);
                model.description = dr1.GetString(3);
                model.forumCreated = (DateTime)dr1[4];
                alldiscussion.Add(model);
            }
            dr1.Close();
            _Connection.Close();
            return alldiscussion;
        }
        // GET: AdminController
        public ActionResult List()
        {
            return View(GetAllDiscussion());
        }


    }
}
