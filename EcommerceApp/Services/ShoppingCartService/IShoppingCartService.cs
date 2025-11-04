using EcommerceApp.DTOs;
using ECommerceApp.DTOs.ShoppingCartDTOs;
using System.Threading.Tasks;

namespace EcommerceApp.Services.ShoppingCartService
{
    public interface IShoppingCartService
    {
        Task<ApiResponse<CartResponseDTO>> GetCartByCustomerIdAsync(int customerId);
        Task<ApiResponse<CartResponseDTO>> AddToCartAsync(AddToCartDTO addToCartDTO);
        Task<ApiResponse<ConfirmationResponseDTO>> ClearCartAsync(int customerId);

        Task<ApiResponse<CartResponseDTO>> UpdateCartItemAsync(UpdateCartItemDTO updateCartItemDTO);
        Task<ApiResponse<CartResponseDTO>> RemoveCartItemAsync(RemoveCartItemDTO removeCartItemDTO);

    }
}
