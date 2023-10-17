using JwtStore.Core.Contexts.SharedContext.Entities;

namespace JwtStore.Core.Contexts.AccountContext.Entities;
public class Role : Entity
{
  public string Name { get; set; } = string.Empty;
  public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>();
}
