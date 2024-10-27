using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/NewItem")]

public class ItemsSO : ScriptableObject
{
    public enum ItemType { coin, sword }
    public ItemType type;

    public bool isCollected;

    public void CollectItem()
    {
        if (!isCollected)
        {
            isCollected = true;
            Debug.Log($"{type} is collected.");
        }
    }
}
