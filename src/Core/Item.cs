using JetBrains.Annotations;

namespace Core;

[PublicAPI]
public class Item(decimal value, int weight)
{
    public decimal Value { get; } = value;
    public int Weight { get; } = weight;
}
