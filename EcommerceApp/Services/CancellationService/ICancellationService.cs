using EcommerceApp.DTOs;
using ECommerceApp.DTOs.CancellationDTOs;
using ECommerceApp.Models;

namespace EcommerceApp.Services.CancellationService
{
    public interface ICancellationService
    {
        Task<ApiResponse<CancellationResponseDTO>> RequestCancellationAsync(CancellationRequestDTO cancellationRequest);
        Task<ApiResponse<CancellationResponseDTO>> GetCancellationByIdAsync(int id);
        Task<ApiResponse<ConfirmationResponseDTO>> UpdateCancellationStatusAsync(CancellationStatusUpdateDTO statusUpdate);
        Task<ApiResponse<List<CancellationResponseDTO>>> GetAllCancellationsAsync();
        //Task NotifyCancellationAcceptedAsync(Cancellation cancellation);

        //Task NotifyCancellationRejectionAsync(Cancellation cancellation);
        }
}
