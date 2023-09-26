using JwtStore.Core.SharedContext.Entities;

namespace JwtStore.Core.AccountContext.Entities;
public class Role : Entity
{
  public string Name { get; set; } = string.Empty;
  public List<User> Users { get; set; } = new();
}
