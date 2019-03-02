using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();
    public int space = 20;

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            return false;
        }
        items.Add(item);

        Debug.Log(items[0]);
        //if(onItemChangedCallback != null)
        //    onItemChangedCallback.Invoke();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        //if (onItemChangedCallback != null)
        //    onItemChangedCallback.Invoke();
    }
}
