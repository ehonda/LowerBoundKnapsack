using JetBrains.Annotations;

namespace Core;

[PublicAPI]
public record Solution(IReadOnlyList<Item> Items)
{
    public decimal Value => Items.Sum(i => i.Value);
    public int Weight => Items.Sum(i => i.Weight);
    
    public static Solution Empty => new(Array.Empty<Item>());
}
