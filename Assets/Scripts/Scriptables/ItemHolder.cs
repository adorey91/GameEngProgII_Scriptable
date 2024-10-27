using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public ItemsSO chestItem;

    private void Start()
    {
        chestItem.isCollected = false;
    }
}
