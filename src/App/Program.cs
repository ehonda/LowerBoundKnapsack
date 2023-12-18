﻿using System.Globalization;
using System.Text;
using Core;
using Spectre.Console;
using Spectre.Console.Rendering;

// S1116: Empty statements should be removed
// Justification: False positive (See e.g.: https://community.sonarsource.com/t/105285)
#pragma warning disable S1116

Item[] items = [new(10, 5), new(40, 4), new(30, 6), new(50, 3)];
const int maxCapacity = 9;

// Make Emoji work in PowerShell, see: https://github.com/spectreconsole/spectre.console/issues/113
Console.OutputEncoding = Encoding.UTF8;

var solutions = Knapsack.Solve(items, maxCapacity);

// We need columns as follows:
//
//      [Item Number Labels, Capacity 1, ... Capacity Max]
//
// Which gives us a total of maxCapacity + 2 columns.
// TODO: Use distinct symbols for item number and number of items in a solution
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

            var rows = new Rows(
                new Text($"💲: {solution.Value.ToString("0.00", CultureInfo.InvariantCulture)}"),
                new Text($"🚚: {solution.Weight}"),
                new Text($"📦: {solution.Items.Count}"));

            return new Panel(rows).RoundedBorder() as IRenderable;
        })
        .ToArray();

    table.AddRow([itemNumberLabel, ..solutionsForItemNumber]);
}

AnsiConsole.Write(table);
