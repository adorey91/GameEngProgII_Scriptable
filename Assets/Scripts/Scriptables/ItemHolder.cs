using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public ItemsSO chestItem;


    // Should also be apart of challenge
    private void Start()
    {
        chestItem.isCollected = false;
    }
}