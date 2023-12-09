using FisioFinancials.Domain.Model.DTOs;
using FisioFinancials.Domain.Model.Entities;
using FisioFinancials.Domain.Model.Exceptions;
using FisioFinancials.Domain.Model.Interfaces.Repositories;
using FisioFinancials.Domain.Model.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FisioFinancials.Domain.Services.Services;

public sealed class ReceivedService : IReceivedService
{
    private readonly IReceivedRepository _repository;
    private UserManager<User> _userManager;

    public ReceivedService
    (
        IReceivedRepository repository,
        UserManager<User> userManager
    )
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<ReceivedDTO> CreateAsync(ReceivedDTO receivedDTO, ClaimsPrincipal userClaims)
    {
        var (PatientName, Value, City, Local, Date) = receivedDTO;

        var user = await CurrentUser(userClaims);

        Received received = new(PatientName, Value, City, Local, Date) {
            User = user
        };

        return await _repository.AddAsync(received);
    }

    public async Task DeleteAsync(int id, ClaimsPrincipal userClaims)
    {
        var user = await CurrentUser(userClaims);

        await _repository.DeleteAsync(id, user.Id);
    }

    public async Task<List<ReceivedDTO>> GetAllAsync(ClaimsPrincipal userClaims)
    {
        var user = await CurrentUser(userClaims);

        return await _repository.GetAllAsync(user.Id);
    }

    public async Task<ReceivedDTO> GetByIdAsync(int id, ClaimsPrincipal userClaims)
    {
        var user = await CurrentUser(userClaims);

        return await _repository.GetByIdAsync(id, user.Id);
    }

    public async Task<ReceivedDTO> UpdateAsync(int id, ReceivedDTO receivedDTO, ClaimsPrincipal userClaims)
    {
        var user = await CurrentUser(userClaims);

        ReceivedDTO savedReceived = await _repository.GetByIdAsync(id, user.Id);

        if (savedReceived == null)
        {
            throw new ResourceNotFoundException("Resource not found");
        }

        return await _repository.UpdateAsync(id, receivedDTO);
    }

    private async Task<User> CurrentUser(ClaimsPrincipal userClaims)
    {
        return await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == userClaims.Identity.Name);
    }
}
