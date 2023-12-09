using FisioFinancials.Domain.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FisioFinancials.Infrastructure.Data.Context;

public class FisioFinancialsDbContext : IdentityDbContext<User>
{
     public FisioFinancialsDbContext(DbContextOptions<FisioFinancialsDbContext> options) : base(options) { }

    public virtual DbSet<Received> Receiveds { get; set; }
}
