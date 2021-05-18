using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDatabase : MonoBehaviour {
    public List<Item> items = new List<Item>();

    void Start()
    {
        items.Add(new Item("Iron Sword", 1001, "This sword is normal style sword", 10, 1, 0, 0, Item.ItemType.Weapon));
    }

}
