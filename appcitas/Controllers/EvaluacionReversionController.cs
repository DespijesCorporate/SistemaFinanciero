using appcitas.Context;
using appcitas.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appcitas.Controllers
{
    public class EvaluacionReversionController : Controller
    {
        private AppcitasContext _context;

        public EvaluacionReversionController()
        {
            _context = new AppcitasContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: EvaluacionReversion
        public ActionResult Index()
        {
            var model = new ReversionDto() { Fecha = DateTime.Now.Date };
            ViewBag.TiposDeReversion = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "TRVSN").ToList();
            return View(model);
        }
    }
}