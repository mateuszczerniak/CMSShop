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
    }
}