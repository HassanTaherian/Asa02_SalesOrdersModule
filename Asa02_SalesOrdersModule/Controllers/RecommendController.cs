using Contracts.UI.Recommendation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("/api/[controller]")]
    public class RecommendController : Controller
    {
        private readonly IRecommendService _recommendService;

        public RecommendController(IRecommendService recommendService)
        {
            _recommendService = recommendService;
        }

        // POST: RecommendController/UI
        [HttpPost]
        public IActionResult RecommendUI(RecommendationRequestDto recommendationRequestDto)
        {
            var RelatedItems = _recommendService.
        }

        // GET: RecommendController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RecommendController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RecommendController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RecommendController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
