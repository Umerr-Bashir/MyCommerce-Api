using EcommerceApp.Data;
using EcommerceApp.DTOs;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CategoryDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryCreateDTO categoryDto)
        {
            try
            {
                bool exists = await _context.Categories
                    .AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower());

                if (exists)
                    return new ApiResponse<CategoryResponseDTO>(400, "Category name already exists.");

                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    IsActive = true
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var response = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive
                };

                return new ApiResponse<CategoryResponseDTO>(200, response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"Error creating category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return new ApiResponse<CategoryResponseDTO>(404, "Category not found.");

                var response = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive
                };

                return new ApiResponse<CategoryResponseDTO>(200, response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"Error fetching category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO categoryDto)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryDto.Id);
                if (category == null)
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");

                bool nameExists = await _context.Categories
                    .AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower() && c.Id != categoryDto.Id);

                if (nameExists)
                    return new ApiResponse<ConfirmationResponseDTO>(400, "Another category with the same name already exists.");

                category.Name = categoryDto.Name;
                category.Description = categoryDto.Description;

                await _context.SaveChangesAsync();

                var confirmation = new ConfirmationResponseDTO
                {
                    Message = $"Category with ID {category.Id} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"Error updating category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");

                category.IsActive = false;
                await _context.SaveChangesAsync();

                var confirmation = new ConfirmationResponseDTO
                {
                    Message = $"Category with ID {id} deleted successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"Error deleting category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .Where(c => c.IsActive)
                    .AsNoTracking()
                    .ToListAsync();

                var responseList = categories.Select(c => new CategoryResponseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive
                }).ToList();

                return new ApiResponse<List<CategoryResponseDTO>>(200, responseList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoryResponseDTO>>(500, $"Error fetching categories: {ex.Message}");
            }
        }
    }
}
