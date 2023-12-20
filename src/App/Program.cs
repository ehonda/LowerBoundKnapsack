using System.Globalization;
using System.Text;
using Core;
using Spectre.Console;
using Spectre.Console.Rendering;

// S3878: Arrays should not be created for params parameters
// Justification: False positive, we're not passing elements but using C# 12's list patterns
#pragma warning disable S3878

Item[] items = [new(10, 5), new(40, 4), new(30, 6), new(50, 3)];
const int maxCapacity = 9;

// Make Emoji work in PowerShell, see: https://github.com/spectreconsole/spectre.console/issues/113
Console.OutputEncoding = Encoding.UTF8;

var solutions = Knapsack.Solve(items, maxCapacity);

// We need columns as follows:
//
//      [Item Number Labels, Capacity 1, ... Capacity Max]
//
// Which gives us a total of maxCapacity + 1 columns.
// TODO: Use distinct symbols for item number and number of items in a solution
// TODO: Make borders configurable so we can print markdown tables
var table = new Table()
    .AddColumns(Enumerable
        .Range(0, maxCapacity + 1)
        .Select(i => i == 0 ? @"📦 \ 🚚" : i.ToString(CultureInfo.InvariantCulture))
        .ToArray())
    .RoundedBorder();

// We start at item number 1 because we hide the trivial solutions for item number 0 / capacity 0.
for (var itemNumber = 1; itemNumber <= solutions.ItemCount; itemNumber++)
{
    // The first cell in each row is the item number label.
    var itemNumberLabel = new Text($"≤ {itemNumber}") as IRenderable;
    
    var solutionsForItemNumber = Enumerable
        .Range(1, solutions.MaximumCapacity)
        .Select(capacity =>
        {
            var solution = solutions[itemNumber, capacity];

            var style = solution.Items.Count > 0 ? Style.Plain : new(Color.Grey23);

            var rows = new Rows(
                new Text($"💲: {solution.Value.ToString("0.00", CultureInfo.InvariantCulture)}", style),
                new Text($"🚚: {solution.Weight}", style),
                new Text($"📦: {solution.Items.Count}", style));

            return new Panel(rows).RoundedBorder().BorderStyle(style) as IRenderable;
        })
        .ToArray();

    table.AddRow([itemNumberLabel, ..solutionsForItemNumber]);
}

AnsiConsole.Write(table);
