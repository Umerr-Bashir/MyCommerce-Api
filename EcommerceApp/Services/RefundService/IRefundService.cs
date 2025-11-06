using EcommerceApp.DTOs;
using ECommerceApp.DTOs.RefundDTOs;
using ECommerceApp.Models;
using System.Threading.Tasks;

namespace EcommerceApp.Services.RefundService
{
    public interface IRefundService
    {

        Task<ApiResponse<List<PendingRefundResponseDTO>>> GetEligibleRefundsAsync();
        Task<ApiResponse<RefundResponseDTO>> ProcessRefundAsync(RefundRequestDTO refundRequest);
        Task<ApiResponse<ConfirmationResponseDTO>> UpdateRefundStatusAsync(RefundStatusUpdateDTO statusUpdate);
        Task<ApiResponse<RefundResponseDTO>> GetRefundByIdAsync(int id);
        Task<ApiResponse<List<RefundResponseDTO>>> GetAllRefundsAsync();
        Task<PaymentGatewayRefundResponseDTO> ProcessRefundPaymentAsync(Refund refund);






    }
}
