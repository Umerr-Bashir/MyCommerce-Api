using EcommerceApp.Data;
using EcommerceApp.DTOs;
using EcommerceApp.DTOs.AddressDTO;
using EcommerceApp.Models;
using ECommerceApp.DTOs.AddressesDTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApp.Service.Address_Service
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _context;

        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create address
        public async Task<ApiResponse<ConfirmationResponseDTO>> CreateAddressAsync(AddressCreateDTO addressDto)
        {
            var customer = await _context.Customers.FindAsync(addressDto.CustomerId);
            if (customer == null)
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found.");

            var address = new Address
            {
                CustomerId = addressDto.CustomerId,
                PresentAddress = addressDto.PresentAddress,
                PermanentAddress = addressDto.PermanentAddress,
                City = addressDto.City,
                State = addressDto.State,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
            {
                Message = "Address created successfully."
            });
        }

        // Get address by ID
        public async Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id)
        {
            var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (address == null)
                return new ApiResponse<AddressResponseDTO>(404, "Address not found.");

            var dto = new AddressResponseDTO
            {
                Id = address.Id,
                CustomerId = address.CustomerId,
                PresentAddress = address.PresentAddress,
                PermanentAddress = address.PermanentAddress,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country

            };

            return new ApiResponse<AddressResponseDTO>(200, dto);
        }

        // Update address
        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressDto.AddressId && a.CustomerId == addressDto.CustomerId);

            if (address == null)
                return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");

            address.PresentAddress = addressDto.PresentAddress;
            address.PermanentAddress = addressDto.PermanentAddress;
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.PostalCode = addressDto.PostalCode;
            address.Country = addressDto.Country;

            await _context.SaveChangesAsync();

            return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
            {
                Message = "Address updated successfully."
            });
        }

        // Delete address
        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
            if (address == null)
                return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found.");

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return new ApiResponse<ConfirmationResponseDTO>(200, new ConfirmationResponseDTO
            {
                Message = "Address deleted successfully."
            });
        }

        // Get all addresses for a customer
        public async Task<ApiResponse<List<AddressResponseDTO>>> GetAllAddressesByCustomerIdAsync(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                return new ApiResponse<List<AddressResponseDTO>>(404, "Customer not found.");

            var list = customer.Addresses.Select(a => new AddressResponseDTO
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                PresentAddress = a.PresentAddress,
                PermanentAddress = a.PermanentAddress,
                City = a.City,
                State = a.State,
                PostalCode = a.PostalCode,
                Country = a.Country
            }).ToList();

            return new ApiResponse<List<AddressResponseDTO>>(200, list);
        }
    }
}
