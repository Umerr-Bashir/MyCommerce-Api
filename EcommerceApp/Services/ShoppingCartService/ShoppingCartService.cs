using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.Services.ShoppingCartService;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.ShoppingCartDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;
namespace ECommerceApp.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<ApiResponse<CartResponseDTO>> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                
                var cart = await _context.Carts
                .Include(c => c.CartItems) 
                .ThenInclude(ci => ci.Product) 
                .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsCheckedOut);
                
                if (cart == null)
                {
                    var emptyCartDTO = new CartResponseDTO
                    {
                        CustomerId = customerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItemResponseDTO>(),
                        TotalBasePrice = 0,
                        TotalDiscount = 0,
                        TotalAmount = 0
                    };
                    
                    return new ApiResponse<CartResponseDTO>(200, emptyCartDTO);
                }
                
                var cartDTO = MapCartToDTO(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
       






        public async Task<ApiResponse<CartResponseDTO>> AddToCartAsync(AddToCartDTO addToCartDTO)
        {
            try
            {
                var product = await _context.Products.FindAsync(addToCartDTO.ProductId);
                if (product == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Product not found.");
                }
                if (addToCartDTO.Quantity > product.StockQuantity)
                {
                    return new ApiResponse<CartResponseDTO>(400, $"Only {product.StockQuantity} units of {product.Name} are available.");
                }
                var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == addToCartDTO.CustomerId && !c.IsCheckedOut);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        CustomerId = addToCartDTO.CustomerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }
                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == addToCartDTO.ProductId);
                if (existingCartItem != null)
                {
                    if (existingCartItem.Quantity + addToCartDTO.Quantity > product.StockQuantity)
                    {
                        return new ApiResponse<CartResponseDTO>(400, $"Adding {addToCartDTO.Quantity} exceeds available stock.");
                    }
                    existingCartItem.Quantity += addToCartDTO.Quantity;
                    existingCartItem.TotalPrice = (existingCartItem.UnitPrice - existingCartItem.Discount) * existingCartItem.Quantity;
                    existingCartItem.UpdatedAt = DateTime.UtcNow;
                    _context.CartItems.Update(existingCartItem);
                }
                else
                {
                    var discount = product.DiscountPercentage > 0 ? product.Price * product.DiscountPercentage / 100 : 0;
                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = product.Id,
                        Quantity = addToCartDTO.Quantity,
                        UnitPrice = product.Price,
                        Discount = discount,
                        TotalPrice = (product.Price - discount) * addToCartDTO.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.CartItems.Add(cartItem);
                }
                cart.UpdatedAt = DateTime.UtcNow;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
                cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id) ?? new Cart();
                var cartDTO = MapCartToDTO(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }


        public async Task<ApiResponse<CartResponseDTO>> UpdateCartItemAsync(UpdateCartItemDTO updateCartItemDTO)
        {
            try
            {
                var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == updateCartItemDTO.CustomerId && !c.IsCheckedOut);
                if (cart == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Active cart not found.");
                }
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == updateCartItemDTO.CartItemId);
                if (cartItem == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Cart item not found.");
                }
                if (updateCartItemDTO.Quantity > cartItem.Product.StockQuantity)
                {
                    return new ApiResponse<CartResponseDTO>(400, $"Only {cartItem.Product.StockQuantity} units of {cartItem.Product.Name} are available.");
                }
                cartItem.Quantity = updateCartItemDTO.Quantity;
                cartItem.TotalPrice = (cartItem.UnitPrice - cartItem.Discount) * cartItem.Quantity;
                cartItem.UpdatedAt = DateTime.UtcNow;
                _context.CartItems.Update(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
                cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id) ?? new Cart();
                var cartDTO = MapCartToDTO(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }



        public async Task<ApiResponse<CartResponseDTO>> RemoveCartItemAsync(RemoveCartItemDTO removeCartItemDTO)
        {
            try
            {
                var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == removeCartItemDTO.CustomerId && !c.IsCheckedOut);
                if (cart == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Active cart not found.");
                }
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == removeCartItemDTO.CartItemId);
                if (cartItem == null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Cart item not found.");
                }
                _context.CartItems.Remove(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id) ?? new Cart();
                var cartDTO = MapCartToDTO(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }





        public async Task<ApiResponse<ConfirmationResponseDTO>> ClearCartAsync(int customerId)
        {
            try
            {
                // Retrieve the active cart along with its items.
                var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsCheckedOut);
                // Return 404 if no active cart is found.
                if (cart == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Active cart not found.");
                }
                // If there are any items in the cart, remove them.
                if (cart.CartItems.Any())
                {
                    _context.CartItems.RemoveRange(cart.CartItems);
                    cart.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                // Create a confirmation response DTO.
                var confirmation = new ConfirmationResponseDTO
                {
                    Message = "Cart has been cleared successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
            }
            catch (Exception ex)
            {
                // Return error response if an exception occurs.
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        


        private CartResponseDTO MapCartToDTO(Cart cart)
        {
            // Map each CartItem entity to its corresponding CartItemResponseDTO.
            var cartItemsDto = cart.CartItems?.Select(ci => new CartItemResponseDTO
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                Discount = ci.Discount,
                TotalPrice = ci.TotalPrice
            }).ToList() ?? new List<CartItemResponseDTO>();
            // Initialize totals for base price, discount, and amount after discount.
            decimal totalBasePrice = 0;
            decimal totalDiscount = 0;
            decimal totalAmount = 0;
            // Iterate through each cart item DTO to accumulate the totals.
            foreach (var item in cartItemsDto)
            {
                totalBasePrice += item.UnitPrice * item.Quantity;       
                totalDiscount += item.Discount * item.Quantity;         
                totalAmount += item.TotalPrice;                         
            }
            return new CartResponseDTO
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                IsCheckedOut = cart.IsCheckedOut,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cartItemsDto,
                TotalBasePrice = totalBasePrice,
                TotalDiscount = totalDiscount,
                TotalAmount = totalAmount
            };
        }
    }
}