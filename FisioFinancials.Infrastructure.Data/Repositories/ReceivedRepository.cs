using FisioFinancials.Domain.Model.DTOs;
using FisioFinancials.Domain.Model.Entities;
using FisioFinancials.Domain.Model.Exceptions;
using FisioFinancials.Domain.Model.Interfaces.Repositories;
using FisioFinancials.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FisioFinancials.Infrastructure.Data.Repositories;

public sealed class ReceivedRepository : IReceivedRepository
{
    private readonly FisioFinancialsDbContext _context;

    public ReceivedRepository
    (
        FisioFinancialsDbContext context
    )
    {
        _context = context;
    }

    public async Task<ReceivedDTO> AddAsync(Received received)
    {
        _context.Receiveds.Add(received);
        await _context.SaveChangesAsync();

         return new ReceivedDTO()
        {
            ReceivedId = received.ReceivedId,
            City = received.City,
            Date = received.Date,
            Local = received.Local,
            PatientName = received.PatientName,
            UserId = received.UserId,
            Value = received.Value
        };
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var received = await _context
                             .Receiveds
                             .Where(x => x.UserId == userId)
                             .Where(x => x.ReceivedId == id)
                             .FirstOrDefaultAsync();

        if (received == null)
            throw new ResourceNotFoundException("Resource not found");

        _context.Receiveds.Remove(received);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ReceivedDTO>> GetAllAsync(string userId)
    {
        return await _context
                     .Receiveds
                     .Where(x => x.UserId == userId)
                     .Include(x => x.User)
                     .Select(x => new ReceivedDTO
                     {
                         ReceivedId = x.ReceivedId,
                         City = x.City,
                         Date = x.Date,
                         Local = x.Local,
                         PatientName = x.PatientName,
                         UserId = x.UserId,
                         Value = x.Value
                     })
                     .ToListAsync();
    }

    public async Task<ReceivedDTO> GetByIdAsync(int id, string userId)
    {
        Received received =  await _context
                                  .Receiveds
                                  .Where (x => x.UserId == userId)
                                  .Where(x => x.ReceivedId == id)
                                  .FirstOrDefaultAsync();
        if (received == null)
        {
            throw new ResourceNotFoundException("Resource not found");
        }

        return new ReceivedDTO()
        {
            ReceivedId = received.ReceivedId,
            City = received.City,
            Date = received.Date,
            Local = received.Local,
            PatientName = received.PatientName,
            UserId = received.UserId,
            Value = received.Value
        };
    }

    public async Task<ReceivedDTO> UpdateAsync(int id, ReceivedDTO receivedDTO)
    {
        Received received = await _context
                                  .Receiveds
                                  .FindAsync(id);

        received.PatientName = receivedDTO.PatientName;
        received.Date = receivedDTO.Date;
        received.Local = receivedDTO.Local;
        received.City = receivedDTO.City;
        received.Value = receivedDTO.Value;

        await _context.SaveChangesAsync();

        return receivedDTO;
    }
}
