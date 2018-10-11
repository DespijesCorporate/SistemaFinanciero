using appcitas.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appcitas.Controllers
{
    public class EvaluacionComboController : Controller
    {
        private AppcitasContext _context;

        public EvaluacionComboController()
        {
            _context = new AppcitasContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: EvaluacionCombo
        public ActionResult Index()
        {
            return View();
        }
    }
}