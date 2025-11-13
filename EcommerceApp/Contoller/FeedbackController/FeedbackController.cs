using EcommerceApp.DTOs;
using EcommerceApp.DTOs.FeedbackDTO;
using EcommerceApp.Services.FeedbackService;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.FeedbackDTOs;
using ECommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        




        [HttpPost("SubmitFeedback")]
        public async Task<ActionResult<ApiResponse<FeedbackResponseDTO>>> SubmitFeedback([FromBody] FeedbackCreateDTO feedbackCreateDTO)
        {
            var response = await _feedbackService.SubmitFeedbackAsync(feedbackCreateDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }







        [HttpGet("GetFeedbackForProductSpecifically/{productId}")]
        public async Task<ActionResult<ApiResponse<ProductFeedbackResponseDTO>>> GetFeedbackForProduct(int productId)
        {
            var response = await _feedbackService.GetFeedbackForProductAsync(productId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
      


        //admin
        [HttpGet("GetAllFeedback")]
        public async Task<ActionResult<ApiResponse<List<FeedbackResponseDTO>>>> GetAllFeedback()
        {
            var response = await _feedbackService.GetAllFeedbackAsync();
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }







        [HttpPut("UpdateFeedback")]
        public async Task<ActionResult<ApiResponse<FeedbackResponseDTO>>> UpdateFeedback([FromBody] FeedbackUpdateDTO feedbackUpdateDTO)
        {
            var response = await _feedbackService.UpdateFeedbackAsync(feedbackUpdateDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }







        [HttpDelete("DeleteFeedback")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteFeedback([FromBody] FeedbackDeleteDTO feedbackDeleteDTO)
        {
            var response = await _feedbackService.DeleteFeedbackAsync(feedbackDeleteDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);


        }

        [HttpGet("GetFeedbackByCustomerId/{customerId}")]
        public async Task<ActionResult<ApiResponse<List<FeedbackResponseDTO>>>> GetFeedbackByCustomerId(int customerId)
        {
            var response = await _feedbackService.GetFeedbackForProductAsync(customerId);

            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }






    }
}