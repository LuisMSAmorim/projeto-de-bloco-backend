using FisioFinancials.Domain.Model.DTOs;
using FisioFinancials.Domain.Model.Entities;
using FisioFinancials.Domain.Model.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FisioFinancials.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReceivedsController : ControllerBase
{
    private readonly IReceivedService _receivedService;

    public ReceivedsController
    (
        IReceivedService receivedService
    )
    {
        _receivedService = receivedService;
    }

    // GET: api/<ReceivedsController>
    [HttpGet]
    public async Task<List<ReceivedDTO>> Get()
    {
        return await _receivedService.GetAllAsync(User);
    }

    // GET api/<ReceivedsController>/5
    [HttpGet("{id}")]
    public async Task<ReceivedDTO> Get(int id)
    {
        return await _receivedService.GetByIdAsync(id, User);
    }

    // POST api/<ReceivedsController>
    [HttpPost]
    public async Task<CreatedResult> Post([FromBody] ReceivedDTO receivedDTO)
    {
        ReceivedDTO received = await _receivedService.CreateAsync(receivedDTO, User);
        return Created("receiveds", received);
    }

    // PUT api/<ReceivedsController>/5
    [HttpPut("{id}")]
    public async Task<NoContentResult> Put(int id, [FromBody] ReceivedDTO receivedDTO)
    {
        await _receivedService.UpdateAsync(id, receivedDTO, User);
        return NoContent();
    }

    // DELETE api/<ReceivedsController>/5
    [HttpDelete("{id}")]
    public async Task<NoContentResult> Delete(int id)
    {
        await _receivedService.DeleteAsync(id, User);
        return NoContent();
    }
}
