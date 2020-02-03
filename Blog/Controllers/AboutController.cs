using Blog.Models.DataContext;
using Blog.Models.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace Blog.Controllers
{
    public class AboutController : Controller
    {
        BlogDBContext db = new BlogDBContext();

        // GET: About
        public ActionResult Index()
        {
            return View(db.About.ToList());
        }

        // GET: About/Edit/id
        public ActionResult Edit(int id)
        {
            return View(db.About.Where(x=>x.HaqqimdaId==id).SingleOrDefault());
        }

        //POST: About/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id,About about,HttpPostedFileBase FotoURL)
        {
            if (ModelState.IsValid)
            {
                var a = db.About.Where(x => x.HaqqimdaId == id).SingleOrDefault();

                if (FotoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(a.FotoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(a.FotoURL));
                    }

                    WebImage img = new WebImage(FotoURL.InputStream);
                    FileInfo imginfo = new FileInfo(FotoURL.FileName);
                    string fotoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(730, 487);

                    img.Save("~/Uploads/About/"+fotoname);
                    a.FotoURL = "/Uploads/About/" + fotoname;
                }
                a.Aciqlama = about.Aciqlama;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(about);
        }


    }
}