using System.Collections.Generic;
using game.GameEngine.GameObjects.Items;

namespace game.GameEngine.Components;

public class Inventory : Component
{
    public int Capacity { get; set; }
    public List<Item> Items { get; set; }

    public Inventory()
    {
        Items = new List<Item>();
    }

    public Inventory(List<Item> items)
    {
        Items = items;
    }
}