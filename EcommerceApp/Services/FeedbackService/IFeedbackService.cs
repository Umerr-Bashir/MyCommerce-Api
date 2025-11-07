using EcommerceApp.DTOs;
using ECommerceApp.DTOs.FeedbackDTOs;

namespace EcommerceApp.Services.FeedbackService
{
    public interface IFeedbackService
    {

        Task<ApiResponse<FeedbackResponseDTO>> SubmitFeedbackAsync(FeedbackCreateDTO feedbackCreateDTO);
        Task<ApiResponse<ProductFeedbackResponseDTO>> GetFeedbackForProductAsync(int productId);
        Task<ApiResponse<List<FeedbackResponseDTO>>> GetAllFeedbackAsync();

        Task<ApiResponse<FeedbackResponseDTO>> UpdateFeedbackAsync(FeedbackUpdateDTO feedbackUpdateDTO);

        Task<ApiResponse<ConfirmationResponseDTO>> DeleteFeedbackAsync(FeedbackDeleteDTO feedbackDeleteDTO);



    }
}
