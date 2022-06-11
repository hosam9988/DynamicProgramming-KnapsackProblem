using System;
using System.Collections.Generic;
using System.Linq;

namespace Knapsack_Problem
{
    class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
    }
    class SelectedItems
    {
        public int TotalPrice { get; set; }
        public List<Item> Items { get; set; }
    }
    class Program
    {
        private static int GetMaxWeight(List<Item> items)
        {
            int max = 0;

            foreach (var item in items)
            {
                if (item.Weight > max)
                    max = item.Weight;
            }
            return max;
        }
        private static SelectedItems GetMaxValue(SelectedItems previuosTotalPrice, Item currentItemPrice)
        {
            if (previuosTotalPrice.TotalPrice > currentItemPrice.Price)
            {
                return previuosTotalPrice;
            }
            else
            {
                SelectedItems selectedItems = new SelectedItems();
                selectedItems.TotalPrice = currentItemPrice.Price;
                selectedItems.Items = new List<Item>();
                selectedItems.Items.Add(currentItemPrice);
                return selectedItems;
            }
        }
        private static string GetItemsNames(List<Item> items)
        {
            string s = "";
            if (items.Count != 0)
            {
                Item lastItem = items.Last();

                s = "(";

                foreach (var item in items)
                {
                    s += item.Name.Substring(0,1);
                    if (!item.Equals(lastItem))
                        s += ",";
                    if (item.Equals(lastItem))
                        s += ")";
                }
            }
            return s;
        }
        static void Main(string[] args)
        {
            List<Item> items = new List<Item>();
            items.Add(new Item() { Name = "Stereo", Price = 3000, Weight = 4 });
            items.Add(new Item() { Name = "Laptop", Price = 2000, Weight = 3 });
            items.Add(new Item() { Name = "Guitar", Price = 1500, Weight = 1 });

            SelectedItems[,] grid = new SelectedItems[items.Count, GetMaxWeight(items)];

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (i == 0)
                    {
                        if (items[i].Weight <= j + 1)
                        {
                            grid[i, j] = new SelectedItems();
                            grid[i, j].TotalPrice = items[i].Price;
                            grid[i, j].Items = new List<Item>();
                            grid[i, j].Items.Add(items[i]);
                        }
                        else
                        {
                            grid[i, j] = new SelectedItems();
                            grid[i, j].Items = new List<Item>();
                            grid[i, j].TotalPrice = 0;
                        }

                        continue;
                    }

                    SelectedItems previousMaxValue = grid[i - 1, j];

                    if ((j + 1) - items[i].Weight == 0)
                    {
                        grid[i, j] = GetMaxValue(previousMaxValue, items[i]);
                    }

                    if ((j + 1) - items[i].Weight > 0)
                    {
                        SelectedItems remainingSpaceMaxPreviousValue = grid[i - 1, j - items[i].Weight];

                        SelectedItems newSelectedValue = new SelectedItems();
                        //value of current item + remaining space's max previous value
                        newSelectedValue.Items = new List<Item>(remainingSpaceMaxPreviousValue.Items);
                        newSelectedValue.Items.Add(items[i]);
                        newSelectedValue.TotalPrice = items[i].Price + remainingSpaceMaxPreviousValue.TotalPrice;


                        grid[i, j] = previousMaxValue.TotalPrice > newSelectedValue.TotalPrice ? previousMaxValue : newSelectedValue;
                    }

                    if ((j + 1) - items[i].Weight < 0)
                    {
                        grid[i, j] = previousMaxValue;
                    }

                }
            }

            Console.Write("Size \t \t");
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                Console.Write($"\t{i + 1 }");
            }
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                Console.Write(items[i].Name + " (Weight :" + items[i].Weight + ")" + "\t");

                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write($"{grid[i, j].TotalPrice}{GetItemsNames(grid[i, j].Items)}\t");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine($"So Max Value I can steel in my Knapsack is { grid[items.Count - 1, GetMaxWeight(items) - 1].TotalPrice } :D ");

        }
    }
}
