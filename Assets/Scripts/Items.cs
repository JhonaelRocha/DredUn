using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory Items/Create New")]
public class Items : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
}
