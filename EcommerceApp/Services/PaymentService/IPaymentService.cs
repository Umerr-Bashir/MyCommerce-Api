using EcommerceApp.DTOs;
using ECommerceApp.DTOs.PaymentDTOs;
using ECommerceApp.Models;
using System.Threading.Tasks;

namespace EcommerceApp.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentResponseDTO>> ProcessPaymentAsync(PaymentRequestDTO paymentRequest);
        Task<ApiResponse<PaymentResponseDTO>> GetPaymentByIdAsync(int paymentId);
        Task<ApiResponse<PaymentResponseDTO>> GetPaymentByOrderIdAsync(int orderId);
        Task<ApiResponse<ConfirmationResponseDTO>> UpdatePaymentStatusAsync(PaymentStatusUpdateDTO statusUpdate);
        Task<ApiResponse<ConfirmationResponseDTO>> CompleteCODPaymentAsync(CODPaymentUpdateDTO codPaymentUpdateDTO);
        //Task<PaymentStatus> SimulatePaymentGateway();
        //Task SendOrderConfirmationEmailAsync(int orderId);


    }
}
