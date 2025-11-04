using EcommerceApp.DTOs;
using ECommerceApp.DTOs.OrderDTOs;

namespace EcommerceApp.Services.OrderService
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderResponseDTO>> CreateOrderAsync(OrderCreateDTO orderDto);
        Task<ApiResponse<OrderResponseDTO>> GetOrderByIdAsync(int orderId);
        Task<ApiResponse<ConfirmationResponseDTO>> UpdateOrderStatusAsync(OrderStatusUpdateDTO statusDto);
        Task<ApiResponse<List<OrderResponseDTO>>> GetAllOrdersAsync();
        Task<ApiResponse<List<OrderResponseDTO>>> GetOrdersByCustomerAsync(int customerId);


    }
}
