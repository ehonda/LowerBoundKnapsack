namespace Core;

// TODO: Improve readability and comments for everything
public static class Knapsack
{
    // Class because we want reference semantics, i.e. items are different even if they have the same value and weight
    public class Item(int value, int weight)
    {
        public int Value { get; } = value;
        public int Weight { get; } = weight;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Sources:
    /// <list type="bullet">
    ///     <item>
    ///         <a href="https://en.wikipedia.org/wiki/Knapsack_problem#0-1_knapsack_problem">Wikipedia</a>
    ///     </item>
    ///     <item>
    ///         <a href="https://medium.com/@fabianterh/how-to-solve-the-knapsack-problem-with-dynamic-programming-eb88c706d3cf">
    ///             Medium (Fabian Terh)
    ///         </a>
    ///     </item>
    /// </list>
    /// </remarks>
    /// <param name="values"></param>
    /// <param name="weights"></param>
    /// <param name="knapsackCapacity"></param>
    /// <returns></returns>
    public static int[,] MaxValueSolutions(IEnumerable<Item> items, int knapsackCapacity)
    {
        // TODO: Add comments and improve readability
        var itemsArray = items.ToArray();
        
        var solutions = new int[itemsArray.Length + 1, knapsackCapacity + 1];

        for (var i = 0; i <= itemsArray.Length; i++)
        {
            for (var c = 0; c <= knapsackCapacity; c++)
            {
                if (i == 0 || c == 0)
                {
                    solutions[i, c] = 0;
                }
                else if (itemsArray[i - 1].Weight <= c)
                {
                    solutions[i, c] = Math.Max(itemsArray[i - 1].Value + solutions[i - 1, c - itemsArray[i - 1].Weight], solutions[i - 1, c]);
                }
                else
                {
                    solutions[i, c] = solutions[i - 1, c];
                }
            }
        }

        return solutions;
    }

    public static (int i, int j) LowerBoundItems(IEnumerable<Item> items, int knapsackCapacity)
    {
        // TODO: Implement
        return (0, 0);
    }

    public static IReadOnlyList<Item> KnapsackItems(IEnumerable<Item> items, int i, int j)
    {
        var itemsArray = items.ToArray();
        var solutions = MaxValueSolutions(itemsArray, j);
        return KnapsackItemsRecursive(itemsArray, solutions, i, j);
    }
    
    private static IReadOnlyList<Item> KnapsackItemsRecursive(Item[] items, int[,] solutions, int i, int j)
    {
        if (i == 0 || j == 0)
        {
            return Array.Empty<Item>();
        }
        
        if (solutions[i, j] > solutions[i - 1, j])
        {
            var item = items[i];
            return KnapsackItemsRecursive(items, solutions, i - 1, j - item.Weight).Append(item).ToList();
        }
        
        return KnapsackItemsRecursive(items, solutions, i - 1, j);
    }
}
