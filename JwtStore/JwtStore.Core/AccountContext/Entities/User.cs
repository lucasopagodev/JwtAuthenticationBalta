using JwtStore.Core.SharedContext.Entities;

namespace JwtStore.Core;

public class User : Entity
{
    public string Name { get; set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
}
