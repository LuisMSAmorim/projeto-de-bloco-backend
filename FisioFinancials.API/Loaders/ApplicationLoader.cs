using FisioFinancials.Domain.Loader;
using FisioFinancials.Domain.Model.Entities;
using FisioFinancials.Domain.Model.Interfaces.Services;
using FisioFinancials.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FisioFinancials.API.Loaders;

public class ApplicationLoader : Base
{
    private readonly UserManager<User> _userManager;
    private readonly FisioFinancialsDbContext _context;

    public ApplicationLoader 
    (
        UserManager<User> userManager,
        FisioFinancialsDbContext context
    )
    {
        _userManager = userManager;
        _context = context;
    }

    public async override Task ExecuteAsync()
    {
        var filePath = "./seed.txt";
        var lines = File.ReadAllLines(filePath);

        var userLine = lines.FirstOrDefault();
        if (!string.IsNullOrEmpty(userLine))
        {
            var userData = userLine.Split(',');

            var existingUser = await _userManager.FindByIdAsync(userData[0]);

            if (existingUser != null)
            {
                Console.WriteLine("User already exists");
            } else
            {
                var user = new User
                {
                    Id = userData[0],
                    FirstName = userData[1],
                    LastName = userData[2],
                    Email = userData[3],
                    UserName = userData[4],
                    NormalizedUserName = userData[5]
                };

                await _userManager.CreateAsync(user, userData[6]);
            }
        }
        bool alreadyHasReceivedsRegistered = _context.Receiveds.ToList().Count > 0;

        if (alreadyHasReceivedsRegistered)
        {
            Console.WriteLine("Database already populated");
            return;
        }

        foreach (var line in lines.Skip(1))
        {
            var data = line.Split(',');
   
            if (data.Length == 7)
            {
                string patientName = data[1];
                int value = int.Parse(data[2]);
                string city = data[3];
                string local = data[4];
                DateTime date = DateTime.Parse(data[5]);
                string userId = data[6];
                Console.WriteLine(userId);
                Received received = new(patientName, value, city, local, date)
                {
                    UserId = userId
                };

                await _context.Receiveds.AddAsync(received);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Ignorando linha inválida: {line}");
            }
        };

    }
}
