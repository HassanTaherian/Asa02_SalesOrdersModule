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
            var RelatedItems = _recommendService.Recommended(recommendationRequestDto);
            return Ok(RelatedItems);
        }
    }
}
