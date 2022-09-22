using Contracts.Product;
using Contracts.UI.Recommendation;
using Domain.Entities;

namespace Services.Abstractions
{
    public interface IRecommendService
    {
        Task<RecommendationResponseDto> Recommended(RecommendationRequestDto recommendationRequestDto);

        Task<string> SerializeRecommendationRequestDto(RecommendationRequestDto recommendationRequestDto);

        public ICollection<ProductRecommendResponseDto>? DeserializeRecommendationRequestDto(string productResponseJson);

        public RecommendationResponseDto GetInvoiceItemsOfUserFromDatabase(RecommendationRequestDto recommendationRequestDto);

        public IEnumerable<Invoice?> GetOrderAndReturnInvoiceOfUser(RecommendationRequestDto recommendationRequestDto);

        public RecommendationResponseDto GetIsDeletedProductItemsOfUser(IEnumerable<Invoice?> invoices);
    }
}
