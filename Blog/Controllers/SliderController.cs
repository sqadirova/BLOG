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
    public class SliderController : Controller
    {
        BlogDBContext db = new BlogDBContext();

        // GET: Slider
        public ActionResult Index()
        {
            return View(db.Slider.ToList());
        }


        // Create: Slider
        public ActionResult Create()
        {

            return View();
        }

        // Create Post: Slider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider slider,HttpPostedFileBase FotoURL)
        {
            if (ModelState.IsValid)
            {
                if (FotoURL != null)
                {
                    WebImage img = new WebImage(FotoURL.InputStream);
                    FileInfo imginfo = new FileInfo(FotoURL.FileName);
                    string fotoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(1100, 400);

                    img.Save("~/Uploads/Slider/" + fotoname);
                    slider.FotoURL = "/Uploads/Slider/" + fotoname;
                }
                db.Slider.Add(slider);
                db.SaveChanges();

                return RedirectToAction("Index");
            }


            return View(slider);
        }

        
        //GET: EDIT
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var slider = db.Slider.Where(x => x.SliderId == id).SingleOrDefault();
            if (slider == null)
            {
                return HttpNotFound();
            }

            return View(slider);
        }

        //Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,Slider slider,HttpPostedFileBase FotoURL)
        {
            if (ModelState.IsValid)
            {
                var s = db.Slider.Where(x => x.SliderId == id).SingleOrDefault();

                if (FotoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(s.FotoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(s.FotoURL));
                    }

                    WebImage img = new WebImage(FotoURL.InputStream);
                    FileInfo imginfo = new FileInfo(FotoURL.FileName);
                    string fotoname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(1060, 400);

                    img.Save("~/Uploads/Slider/" + fotoname);
                    s.FotoURL = "/Uploads/Slider/" + fotoname;
                }
                s.Basliq = slider.Basliq;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(slider);
        }

        public ActionResult Delete(int id)
        {
            var s = db.Slider.Where(x => x.SliderId == id).SingleOrDefault();

            return View(s);
        }

        [HttpPost]
       
        public ActionResult Delete(int id,HttpPostedFileBase FotoURL)
        {
            try
            {
                var s = db.Slider.Where(x => x.SliderId == id).SingleOrDefault();
                if (s==null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(s.FotoURL)))
                {
                    System.IO.File.Delete(Server.MapPath(s.FotoURL));
                }

                db.Slider.Remove(s);
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