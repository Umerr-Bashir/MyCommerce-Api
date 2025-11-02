using EcommerceApp.DTOs;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CategoryDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApp.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryCreateDTO categoryDto);
        Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO categoryDto);
        Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(int id);
        Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync();
    }
}
