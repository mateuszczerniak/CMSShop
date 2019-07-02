using CMSShop.Models.Data;
using CMSShop.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSShop.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //deklaracja listy pagevm
            List<PageVM> pagesList;

            
            using(Db db = new Db())
            {
                //inicjalizacja listy
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //zwracamy strony do widoku
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //sprawdzanie model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using(Db db = new Db())
            {
                string slug;

                //inicjalizacja PageDTO
                PageDTO dto = new PageDTO();
                
                //gdy niemamy adresu strony to przypisujemy tytul
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-");
                }

                //zapobiegamy dodaniu takiej samej nazwy strony

                if (db.Pages.Any(x=>x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Ten tytuł lub adres strony już istnieje.");
                    return View(model);
                }

                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 1000;

                //zapis dto
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            TempData["SM"] = "Dodałeś nową stronę.";

            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM model;

            using (Db db = new Db())
            {
                //pobieram strone z bazy o przekazanym id
                PageDTO dto = db.Pages.Find(id);

                //sprawdzam czy taka strona istnieje
                if(dto == null)
                {
                    Content("Strona nieistnieje.");
                }

                model = new PageVM(dto);

            }

            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //pobranie id strony do edycji
                int id = model.Id;

                string slug = "home";

                //pobranie strony do edycji
                PageDTO dto = db.Pages.Find(id);
                
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //sprawdzenie unikalnosc strony/adresu
                if (db.Pages.Where(x=>x.Id!=id).Any(x=>x.Title==model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Strona lub tytuł już istnieje.");
                }

                //modyfikacja dto
                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //zapis do bazy
                db.SaveChanges();
            }

            TempData["SM"] = "Edytowałeś stronę.";

            return RedirectToAction("EditPage");
        }

        public ActionResult Details(int id)
        {
            //deklaracja PageVM
            PageVM model;

            using (Db db = new Db())
            {
                //pobranie strony o id
                PageDTO dto = db.Pages.Find(id);

                //sprawdzenie czy strona istnieje
                if (dto == null)
                {
                    return Content("Strona nie istnieje.");
                }

                //inicjalizacja PageVM
                model = new PageVM(dto);
            }
            return View(model);
        }
    }
}