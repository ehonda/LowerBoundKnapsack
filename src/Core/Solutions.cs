using JetBrains.Annotations;

namespace Core;

[PublicAPI]
public class Solutions(int itemCount, int maximumCapacity)
{
    public int ItemCount { get; } = itemCount;
    public int MaximumCapacity { get; } = maximumCapacity;
    
    private readonly Solution[,] _solutions = new Solution[itemCount, maximumCapacity];
    
    // Counting for itemNumber is 1-based, i.e. if there are 3 items, the itemNumber is 1, 2, or 3.
    public Solution this[int itemNumber, int capacity]
    {
        get => itemNumber == 0 || capacity == 0 ? Solution.Empty : _solutions[itemNumber - 1, capacity - 1];
        // TODO: Better error if itemNumber or capacity is out of range
        set => _solutions[itemNumber - 1, capacity - 1] = value;
    }
}
