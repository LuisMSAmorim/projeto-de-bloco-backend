using FisioFinancials.Domain.Model.DTOs;
using System.Security.Claims;

namespace FisioFinancials.Domain.Model.Interfaces.Services;

public interface IReceivedService
{
    Task<List<ReceivedDTO>> GetAllAsync(ClaimsPrincipal userClaims);
    Task<ReceivedDTO> GetByIdAsync(int id, ClaimsPrincipal userClaims);
    Task<ReceivedDTO> CreateAsync(ReceivedDTO receivedDTO, ClaimsPrincipal userClaims);
    Task<ReceivedDTO> UpdateAsync(int id, ReceivedDTO receivedDTO, ClaimsPrincipal userClaims);
    Task DeleteAsync(int id, ClaimsPrincipal userClaims);
}
