using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using memegeumApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace memegeumApp.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IMemeRespository _memeRespository;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IMemeRespository memeRespository, ILogger<HomeController> logger)
        {
            _memeRespository = memeRespository;
            _logger = logger;
        }
        [Route("~/")]
        public IActionResult Index()
        {
            return RedirectToAction("Page", new { numberOfPage = _memeRespository.GetMaxPageNumber() });
        }

        [Route("~/page/{numberOfPage}")]
        [Route("~/strona/{numberOfPage}")]
        public IActionResult Page(int numberOfPage)
        {
            numberOfPage = numberOfPage < 1 ? _memeRespository.GetMaxPageNumber() : numberOfPage;

            _logger.LogDebug($"Given number of page: {numberOfPage}");

            try
            {
                var tags = _memeRespository.GetAllMemesByNewest().SelectMany(meme => meme.Tags).Distinct().ToList();
                ViewBag.Tags = tags;

                var memesByPage = _memeRespository.GetMemesByPage(numberOfPage);
                ViewBag.PageRange = _memeRespository.GetPageNumberRange(numberOfPage);
                ViewBag.SelectedPage = numberOfPage;

                ViewBag.WhiteListTags = HttpContext.Session.GetString("_whiteListTags");
                ViewBag.BlackListTags = HttpContext.Session.GetString("_blackListTags");

                return View(memesByPage);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return View("~/Views/Error.cshtml", new string[] { "ArgumentOutOfRangeException", e.Message });
            }
        }

        [HttpPost]
        [Route("~/sort/")]
        public IActionResult SortByTags(string whiteListTagsText, string blackListTagsText)
        { 
            HttpContext.Session.SetString("_whiteListTags", whiteListTagsText??"");
            HttpContext.Session.SetString("_blackListTags", blackListTagsText??"");

            return RedirectToAction(nameof(Page), new {numberOfPage = _memeRespository.GetMaxPageNumber() });
        }
    }
}
