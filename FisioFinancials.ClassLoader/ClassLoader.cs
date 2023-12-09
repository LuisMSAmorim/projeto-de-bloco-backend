using FisioFinancials.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FisioFinancials.ClassLoader;

public static class ClassLoader
{
    public static void PopulateDatabase(DbContext context)
    {
        Received received = new("Maria da Silva", 1500, "Florianópolis", "Clínica", new DateTime());
        Received received1 = new("Ana de Souza", 1500, "Biguaçu", "hospital", new DateTime());
        Received received2 = new("Pedro dos Santos", 1500, "São José", "Clínica", new DateTime());
        Received received3 = new("José das palmeiras", 1500, "Imbituba", "Hospital", new DateTime());

        context.Add(received);
        context.Add(received1);
        context.Add(received2);
        context.Add(received3);

        context.SaveChanges();
    }
}
