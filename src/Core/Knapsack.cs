using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Core;

/// <summary>
/// Solves the knapsack problem using dynamic programming.
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
[PublicAPI]
public static class Knapsack
{
    public static Solutions Solve(IEnumerable<Item> items, int maxCapacity)
        => Solve(items.ToArray(), maxCapacity);
    
    [SuppressMessage(
        "SonarLint",
        "S1116: Empty statements should be removed",
        Justification = "False positive (C# 12 List pattern)")]
    [SuppressMessage(
        "SonarLint",
        "S1481: Unused local variables should be removed",
        Justification = "False positive (C# 12 List pattern)")]
    public static Solutions Solve(Item[] items, int maxCapacity)
    {
        var solutions = new Solutions(items.Length, maxCapacity);
        
        // We start at 1 in both loops because row and column 0 are empty (no items or no capacity). This effectively
        // means we "1-based" the items array.
        for (var itemNumber = 1; itemNumber <= items.Length; itemNumber++)
        {
            for (var capacity = 1; capacity <= maxCapacity; capacity++)
            {
                var item = items[itemNumber - 1];
                var solutionWithoutItem = solutions[itemNumber - 1, capacity];
                
                // See if we can include the item in the solution (i.e. there is enough capacity for it). If we can't,
                // the solutions is the same as the solution without the item.
                if (item.Weight > capacity)
                {
                    solutions[itemNumber, capacity] = solutionWithoutItem;
                    continue;
                }
                
                // If we choose to include the item, we need to subtract its weight from the capacity, which gives
                // us the remaining capacity. We then look up the solution for the remaining capacity (without using
                // the item) and add the item's value to it.
                var solutionWithRemainingCapacity = solutions[itemNumber - 1, capacity - item.Weight];
                    
                // We now compare the solution with the item (combined with the solution for the remaining capacity)
                // to the solution without the item. If the solution with the item is better, we use it, otherwise
                // we use the solution without the item.
                var solutionWithItem = new Solution([..solutionWithRemainingCapacity.Items, item]);
                    
                solutions[itemNumber, capacity] = solutionWithItem.Value > solutionWithoutItem.Value
                    ? solutionWithItem
                    : solutionWithoutItem;
            }
        }

        return solutions;
    }
}
