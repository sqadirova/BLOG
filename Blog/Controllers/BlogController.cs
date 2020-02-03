using Blog.Models.DataContext;
using Blog.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.IO;
using System.Data.Entity;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        private BlogDBContext db = new BlogDBContext();

        // GET: Blog
        public ActionResult Index()
        {
            // ViewBag.UserId = new SelectList(db.User, "UserId", "UserName");
       
            db.Configuration.LazyLoadingEnabled = false;
            var blogs = db.Blogs.Include("Category").ToList().OrderByDescending(x => x.BlogId);

            //var blogs = db.Blogs.Include(b=>b.User.UserName).Include(c=>c.Category).ToList().OrderByDescending(x=>x.BlogId);
            return View(blogs);

        }

        //GET: Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Category, "KategoriId", "KategoriAd");
            //ViewBag.UserId = new SelectList(db.User, "UserId", "UserName");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Blogs blog,string EtiketAdi, HttpPostedFileBase FotoURL)
        {
            if (FotoURL != null)
            {
                WebImage img = new WebImage(FotoURL.InputStream);
                FileInfo imginfo = new FileInfo(FotoURL.FileName);
                string fotoname = Guid.NewGuid().ToString() + imginfo.Extension;
                img.Resize(730, 487);

                img.Save("~/Uploads/Blogs/" + fotoname);
                blog.FotoURL = "/Uploads/Blogs/" + fotoname;
            }

            if (EtiketAdi != null)
            {
                string[] etiketmassiv = EtiketAdi.Split(',');
                foreach (var i in etiketmassiv)
                {
                    var yenietiket = new Tag { EtiketAdi = i };
                    db.Tag.Add(yenietiket);
                    blog.Tags.Add(yenietiket);
                }
            }
            //session yaradanda acarsan
            blog.UserId = Convert.ToInt32(Session["userid"]);
            blog.Oxunma = 1;
            db.Blogs.Add(blog);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //GET:
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();

            if (b == null)
            {
                return HttpNotFound();
            }

            ViewBag.KategoriId = new SelectList(db.Category, "KategoriId", "KategoriAd");
            ViewBag.UserId = new SelectList(db.User, "UserId", "UserName");
            return View(b);
        }

        //POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]                           //ckeditor deyise bilmesi ucun
        public ActionResult Edit(int id,Blogs blog,HttpPostedFileBase FotoURL)
        {
            if (ModelState.IsValid)
            {
                var b = db.Blogs.Where(x => x.BlogId==id).SingleOrDefault();

                if (FotoURL!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(b.FotoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(b.FotoURL));
                    }

                    WebImage img = new WebImage(FotoURL.InputStream);
                    FileInfo imginfo = new FileInfo(FotoURL.FileName);
                    string fotoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(730, 487);

                    img.Save("~/Uploads/Blogs/" + fotoname);
                    b.FotoURL = "/Uploads/Blogs/" + fotoname;
                }
                b.Baslik = blog.Baslik;
                b.Icerik = blog.Icerik;
                b.Tarih = blog.Tarih;
                b.KategoriId = blog.KategoriId;
                b.UserId = blog.UserId;

                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        //GET:
        public ActionResult Delete(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var b = db.Blogs.Include("Category").Include("User").Where(x => x.BlogId == id).SingleOrDefault();

            return View(b);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(int id, HttpPostedFileBase FotoURL)
        {
           
            try
            {
                var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();

                if (b == null)
                {
                    return HttpNotFound();
                }

                if (System.IO.File.Exists(Server.MapPath(b.FotoURL)))
                {
                    System.IO.File.Delete(Server.MapPath(b.FotoURL));
                }


                //yorumlari silmek lazimdi
                //here


                foreach (var i in b.Tags.ToList())
                {
                    db.Tag.Remove(i);
                }

                db.Blogs.Remove(b);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
            
        }





           
        }




    }
