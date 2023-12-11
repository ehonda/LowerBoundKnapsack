using System.Globalization;
using FluentAssertions;
using Relaxdays.TestUtilities.Scenarios;

namespace Core.Tests;

[TestOf(typeof(Knapsack))]
public class KnapsackTests
{
    public static class Problems
    {
        public record Data(
            IReadOnlyList<Item> Items,
            int MaxCapacity,
            Solution ExpectedSolution);
        
        public static IEnumerable<Scenario<Data>> All => new[]
            {
                CreateData(
                    [Item(2, 1), Item(3, 1)],
                    1,
                    1),
                
                CreateData(
                    [Item(1, 1), Item(1, 1), Item(1, 2)],
                    2,
                    0, 1),
                
                CreateData(
                    [Item(10, 5), Item(40, 4), Item(30, 6), Item(50, 3)],
                    9,
                    1, 3)
            }
            .AsScenarios(data => $"{ItemsToString(data.Items)}, 🚚: {data.MaxCapacity} " +
                                 $"=> {ItemsToString(data.ExpectedSolution.Items)}");
        
        private static Data CreateData(IReadOnlyList<Item> items, int maxCapacity, params int[] solutionItemIndices) => 
            new(items, maxCapacity, new(solutionItemIndices.Select(i => items[i]).ToList()));
        
        private static Item Item(decimal value, int weight) => new(value, weight);
        
        private static string ItemToString(Item item)
            => $"(💲: {item.Value.ToString("0.00", CultureInfo.InvariantCulture)}, 🚚: {item.Weight})";

        private static string ItemsToString(IEnumerable<Item> items)
            => $"[{string.Join(", ", items.Select(ItemToString))}]";
    }

    [Test]
    [Description(
        """
        Scenario:
            The solve algorithm is invoked with a list of items and a maximum capacity.
            
        Expected result:
            The algorithm returns the solution with the highest value that fits in the maximum capacity.
        """)]
    public void Solve_works([ValueSource(typeof(Problems), nameof(Problems.All))] Scenario<Problems.Data> scenario)
    {
        // Arrange
        var items = scenario.Data.Items;
        var maxCapacity = scenario.Data.MaxCapacity;
        var expectedSolution = scenario.Data.ExpectedSolution;
        
        // Act
        var actualSolution = Knapsack.Solve(items, maxCapacity)[items.Count, maxCapacity];
        
        // Assert
        actualSolution.Should().BeEquivalentTo(expectedSolution);
    }
}
