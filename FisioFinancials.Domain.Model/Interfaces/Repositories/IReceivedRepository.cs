
using FisioFinancials.Domain.Model.DTOs;
using FisioFinancials.Domain.Model.Entities;

namespace FisioFinancials.Domain.Model.Interfaces.Repositories;

public interface IReceivedRepository
{
    Task<ReceivedDTO> AddAsync(Received received);
    Task<ReceivedDTO> GetByIdAsync(int id, string userId);
    Task<List<ReceivedDTO>> GetAllAsync(string userId);
    Task<ReceivedDTO> UpdateAsync(int id, ReceivedDTO receivedDTO);
    Task DeleteAsync(int id, string userId);
}
