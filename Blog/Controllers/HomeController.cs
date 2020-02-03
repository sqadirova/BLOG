using Blog.Models.DataContext;
using Blog.Models.Model;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        BlogDBContext db = new BlogDBContext();
        // GET: Home

        [Route("")]
        [Route("Anasehife")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x=>x.SliderId));
        }

        [Route("Haqqimda")]
        public ActionResult About()
        {

            return View(db.About.SingleOrDefault());
        }

        //about-partial
        public ActionResult Biopartial()
        {
            return View(db.About.SingleOrDefault());
        }

        public ActionResult TagsPartial()
        {
            return View(db.Tag.ToList().OrderByDescending(x=>x.EtiketId).Take(15));
        }

        public ActionResult MenuCategoryList()
        {
            return PartialView(db.Category.Include("Blogss").ToList().OrderBy(x => x.KategoriAd));
        }

        public ActionResult LastBlogsPartial()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var b = db.Blogs.Include("Category").ToList().OrderByDescending(x => x.BlogId).Take(3);
            return View(b);
        }

        public ActionResult BlogPostsPartial()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var b = db.Blogs.Include("Category").ToList().OrderByDescending(x => x.BlogId).Take(6);
            return View(b);
        }

       

        //butun(umumi) bloglarin siyahisi
        [Route("BlogPosts")]
        public ActionResult AllBlogs(int page=1)
        {
           // db.Configuration.LazyLoadingEnabled = false;
            var b = db.Blogs.Include("Category").ToList().OrderByDescending(x => x.BlogId).ToPagedList(page,5);
            return View(b);
        }

        //category list
        public ActionResult BlogCategoryPartial()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var b = db.Category.Include("Blogss").ToList().OrderBy(x => x.KategoriAd);
            return PartialView(b);
        }

        //kateqoriyaya gore bloglarin siyahisi
        [Route("BlogPosts/{kategoriad}/{id:int}")]
        public ActionResult BlogCategory(int id,int page=1)
        {
            // db.Configuration.LazyLoadingEnabled = false;
            //var b = db.Blogs.Include("Category").Where(x => x.Category.KategoriId == id).OrderByDescending(x => x.BlogId).ToPagedList(page, 5);

            var b = db.Blogs.Include("Category").Where(x => x.KategoriId == id).OrderByDescending(x => x.BlogId).ToPagedList(page,5);
            return View(b);
        }

        //Blog yazisi
        [Route("BlogPosts/{basliq}-{id:int}")]
        public ActionResult BlogSingle(int id)
        {
            var b = db.Blogs.Include("Category").Include("Comments").Where(x => x.BlogId == id).SingleOrDefault();
            return View(b);
        }
       
     
        [HttpPost]
        public ActionResult OxunmaArttir(int id)
        {
            var b = db.Blogs.Where(x => x.BlogId == id).SingleOrDefault();
            b.Oxunma += 1;
            db.SaveChanges();
            return View();
        }

        public ActionResult PopularPostPartial()
        {
            return View(db.Blogs.ToList().OrderByDescending(x=>x.BlogId).Take(4));
        }

        //public JsonResult YorumYap(string adsoyad, string eposta, string icerik, int blogid)
        //{
        //    if (icerik == null)
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    db.Yorum.Add(new Yorum { AdSoyad = adsoyad, Eposta = eposta, Icerik = icerik, BlogId = blogid, Onay = false });
        //    db.SaveChanges();

        //    return Json(false, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult PostComment(string adsoyad ,string email,DateTime tarih, string icerik, int blogid)
        {
            if (icerik == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Comment.Add(new Comment { AdSoyad=adsoyad, Email=email, Icerik=icerik, Tarih=tarih, BlogId=blogid, Onay=false});
            db.SaveChanges();


            return Json(false,JsonRequestBehavior.AllowGet);
        }



    }
}