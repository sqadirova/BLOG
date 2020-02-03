using Blog.Models.DataContext;
using Blog.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace Blog.Controllers
{
    public class AdminController : Controller
    {
        BlogDBContext db = new BlogDBContext();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.BlogCount = db.Blogs.Count();
            ViewBag.CommentCount = db.Comment.Count();
            ViewBag.CategoryCount = db.Category.Count();
            ViewBag.UserCount = db.User.Count();

            ViewBag.CommentOk = db.Comment.Where(x => x.Onay == false).Count();
            return View();
        }

       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var login = db.User.Include("Role").Where(x => x.Email == user.Email).SingleOrDefault();

            if (login.Email == user.Email && login.Sifre ==Crypto.Hash( user.Sifre,"MD5"))
            {
                Session["userId"] = login.UserId;
                Session["email"] = login.Email;
                Session["username"] = login.UserName;
                Session["userfoto"] = login.FotoURL;
                Session["adsoyad"] = login.AdSoyad;
                Session["yetki"] = login.Role.Yetki;

                return RedirectToAction("Index", "Admin");

            }

            ViewBag.Warning = "Email və ya şifrə yalnışdır!";

            return View(user);
        }

        public ActionResult Logout()
        {
            Session["userId"] = null;
            Session["email"] = null;
            Session["username"] = null;
            Session.Abandon();

            return RedirectToAction("Login", "Admin");
        }

       
        //GET:
        public ActionResult ForGotPassword()
        {
            return View();
        }

        //POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForGotPassword(string Email)
        {
            try
            {
                var mail = db.User.Where(x => x.Email == Email).SingleOrDefault();

                if (mail != null)
                {
                    Random rnd = new Random();
                    int yenisifre = rnd.Next();
                    mail.Sifre =Crypto.Hash( Convert.ToString(yenisifre),"MD5");
                    db.SaveChanges();

                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "kurumsalwebsidiqa@gmail.com";
                    WebMail.Password = "Kurumsalweb16";
                    WebMail.SmtpPort = 587;
                    WebMail.Send(Email, "Yeni istifadəçi şifrəniz", "Hörmətli istifadəçi, sizin yeni şifrəniz:" + yenisifre);

                    ViewBag.Mesaj = "Yeni Şifrəniz Göndərildi!";
                }
                else
                {
                    ViewBag.Mesaj = "Şifrəniz göndərilərkən xəta baş verdi!";
                }

                return View();
            }
            catch (Exception)
            {

                throw;
            }
           
        }


        //GET:  ---Create User.
        public ActionResult Signup()
        {

            return View();
        }

        //POST:  ---Create User
        [HttpPost]
        
        public ActionResult Signup(User user,HttpPostedFileBase FotoURL,string Sifre)
        {
            if (FotoURL != null)
            {
                WebImage img = new WebImage(FotoURL.InputStream);
                FileInfo imginfo = new FileInfo(FotoURL.FileName);
                string fotoname = Guid.NewGuid().ToString() + imginfo.Extension;
                img.Resize(40, 40);

                img.Save("~/Uploads/User/" + fotoname);
                user.FotoURL = "/Uploads/User/" + fotoname;
            }

            user.YetkiId = 2;
            user.Sifre = Crypto.Hash(Sifre, "MD5");

            db.User.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login","Admin");
        }





        public ActionResult UsersList()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var users = db.User.Include("Role").ToList();
            return View(users);
        }

        //GET:
        public ActionResult UsersEdit(int id)
        {
            //var u = db.User.Include("Role").Where(x => x.UserId == id).SingleOrDefault();
            var u = db.User.Where(x => x.UserId == id).SingleOrDefault();

            if (u == null)
            {
                return HttpNotFound();
            }

           ViewBag.YetkiId = new SelectList(db.Role, "YetkiId", "Yetki");

            return View(u);
        }

        //POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersEdit(int id, User user, HttpPostedFileBase FotoURL)
        {
            if (ModelState.IsValid)
            {
                var u = db.User.Where(x => x.UserId == id).SingleOrDefault();

                if (FotoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(u.FotoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(u.FotoURL));
                    }

                    WebImage img = new WebImage(FotoURL.InputStream);
                    FileInfo imginfo = new FileInfo(FotoURL.FileName);
                    string fotoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(40, 40);

                    img.Save("~/Uploads/User/" + fotoname);
                    u.FotoURL = "/Uploads/User/" + fotoname;
                }
                u.UserName = user.UserName;
                u.Email = user.Email;
                u.Sifre = Crypto.Hash(user.Sifre, "MD5");
                u.AdSoyad = user.AdSoyad;
                u.YetkiId = user.YetkiId;

                db.SaveChanges();

                return RedirectToAction("UsersList");
            }
            return View(user);
        }

        //GET:
        public ActionResult UsersDelete(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var u = db.User.Include("Role").Where(x => x.UserId == id).SingleOrDefault();

            if (u==null)
            {
                return HttpNotFound();
            }

            return View(u);
        }

        //POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersDelete(int id,HttpPostedFileBase FotoURL)
        {
            try
            {
                var u = db.User.Where(x => x.UserId == id).SingleOrDefault();

                if (u == null)
                {
                    return HttpNotFound();
                }

                if (FotoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(u.FotoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(u.FotoURL));
                    }
                }

                db.User.Remove(u);
                db.SaveChanges();

                return RedirectToAction("UsersList");
            }
            catch (Exception)
            {

                throw;
            }

        }


        public ActionResult MyProfile()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            //var user = db.User.Include("Role").Where(x=>x.UserId==Convert.ToInt32(Session["userId"])).SingleOrDefault();
            //return View(user);

            if (Session["userId"] != null)
            {
                //ViewBag.UserId = Session["userId"];

                ViewBag.AdSoyad = Session["adsoyad"];
                ViewBag.UserName = Session["username"];
                ViewBag.Foto = Session["userfoto"];
                ViewBag.Email = Session["email"];
                ViewBag.Yetki = Session["yetki"];

                return View();
            }
            else
            {
                return RedirectToAction("Login","Admin");;
            }
        }
            



    }
}