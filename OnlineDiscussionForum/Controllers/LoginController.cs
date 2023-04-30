using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineDiscussionForum.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;

namespace OnlineDiscussionForum.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _Connection;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _Connection = new SqlConnection(_configuration.GetConnectionString("Users"));
        }
        // GET: LoginController/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login)
        {
            try
            {
                
                    return RedirectToAction("Index", "Discussion", new { email = login.Email});
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return View();
            }
            return View();
        }



        //For SignUp Page
        // GET: AdminController/Create
        public ActionResult Signup()
        {
            return View();
        }

        void CreateUser(LoginModel login_user)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("Insert_User", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@User_name", login_user.UserName);
            cmd.Parameters.AddWithValue("@Email", login_user.Email);
            cmd.Parameters.AddWithValue("@Password", login_user.Password);
            cmd.Parameters.AddWithValue("@Hobbies", login_user.Hobbies);

            cmd.ExecuteNonQuery();
            _Connection.Close();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(LoginModel login)
        {
            try
            {
                CreateUser(login);
                return RedirectToAction(nameof(Login));
            }
            catch (Exception e)
            {
                Console.WriteLine($"We have faced some issues {e}");
                return View();
            }
        }

        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
