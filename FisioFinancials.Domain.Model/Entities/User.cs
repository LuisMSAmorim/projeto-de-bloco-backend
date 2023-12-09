using Microsoft.AspNetCore.Identity;

namespace FisioFinancials.Domain.Model.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<Received> Receiveds { get; set; }

    public User() : base() { }

    public string FullName()
    {
        return $"{FirstName} + {LastName}";
    }

    public static implicit operator Task<object>(User v)
    {
        throw new NotImplementedException();
    }
}
