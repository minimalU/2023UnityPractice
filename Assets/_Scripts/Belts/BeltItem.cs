using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BeltItem Class is a component that is attached to GameObjects representing Box on a conveyor belt
public class BeltItem : MonoBehaviour
{
    // public variable GameObject will hold a reference to the Box(BeltItem) component.
    // assign an item GameObject to this variable in the Unity Inspector
    public GameObject item;

    // Awake method is a callback that called when the GameObject this script is attached to is initialized. It is executed before the 'Start' method
    private void Awake()
    {
        // the script sets the item variable to the reference of the gameObject. it sets 'item' to represent the GameObject itself
        item = gameObject;
    }

}
