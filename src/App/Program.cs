using System.Globalization;
using Core;
using Spectre.Console;
using Spectre.Console.Rendering;

Item[] items = [new(10, 5), new(40, 4), new(30, 6), new(50, 3)];
const int maxCapacity = 9;

var solutions = Knapsack.Solve(items, maxCapacity);

var innerTable = new Table()
    .AddColumns(Enumerable
        .Range(0, maxCapacity + 1)
        .Select(i => i.ToString(CultureInfo.InvariantCulture))
        .ToArray());

for (var itemNumber = 0; itemNumber <= solutions.ItemCount; itemNumber++)
{
    var cells = Enumerable
        .Range(0, solutions.MaximumCapacity + 1)
        .Select(capacity =>
        {
            var solution = solutions[itemNumber, capacity];

            return new Rows(
                new Text($"💲: {solution.Value.ToString("0.00", CultureInfo.InvariantCulture)}"),
                new Text($"🚚: {solution.Weight}"),
                new Text($"📦: {solution.Items.Count}")) as IRenderable;
        })
        .ToArray();

    innerTable.AddRow(cells);
}

var outerTable = new Table()
    .AddColumns("", "🚚 - Weight")
    .AddRow(new Text("💲 - Value"), innerTable);

AnsiConsole.Write(outerTable);
