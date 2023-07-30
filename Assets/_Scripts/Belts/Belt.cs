// Date : 2023-07-29
// Description : This is the practice project for CodingWithRus Unity3D
// https://www.youtube.com/watch?v=Zev8-i6uX_U

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Conveyor belt class manages the movement of box item on a conveyor belt. 
// it allows items to move from one belt to the next in a sequence by checking for available space on the next belt and using coroutines to animate the item's movement.
public class Belt : MonoBehaviour
{
    // define variables for Belt plate
    // beltID keeps track of the uniq ID of each belt
    // beltInSequence is a reference to the next belt in the sequence
    // beltItem is a reference to an item on the belt
    // isSpaceTaken is a boolean flag indicating the space on the belt is taken by an box
    // _beltManager is a reference to the BeltManager script
    private static int _beltID = 0;
    public Belt beltInSequence;
    public BeltItem beltItem;
    public bool isSpaceTaken;
    private BeltManager _beltManager;


    // function: initialize variable by finding the active instance of BeltManager
    // Set 'beltInSequence to 'null'
    // Call FindNextBelt
    // Sets the name of the game object to include the belt ID using string interpolation
    private void Start()
    {
        _beltManager = FindObjectOfType<BeltManager>();
        beltInSequence = null;
        beltInSequence = FindNextBelt();
        gameObject.name = $"Belt: {_beltID++}";
    }

    // function: checks if beltInSequence is null and tries to find the next belt using FindNextBelt()
    private void Update()
    {
        if (beltInSequence == null)
        {
            beltInSequence = FindNextBelt();
        }

        // if beltInSequence is not null and beltItem is not null on the current belt, it starts a StartCoroutine()
        if (beltInSequence != null && beltItem.item != null)
        {
            //  A coroutine is a special type of function in C# that enables you to pause the execution of a method
            //  and resume it later. This feature is particularly useful for performing actions over multiple frames,
            //  creating delays, or executing tasks that should not block the main thread.
            StartCoroutine(StartBeltMove());
        }
    }

    //function: returns a position just above the current belt's position with a small padding 0.3f in the vertical direction
    private Vector3 GetItemPosition()
    {
        var padding = 0.3f;
        var position = transform.position;
        return new Vector3(position.x, position.y + padding, position.z);
    }

    // function: Sets 'isSpaceTake" to true, indicating that the space on the belt is occupied by an item.
    // moves the item on the current belt towards the position of the next belt's space using MoveTowards
    private IEnumerator StartBeltMove()
    {
        isSpaceTaken = true;

        // checks if the current belt's space is not taken and if the next belt's space is not taken
        if (beltItem.item != null && beltInSequence != null && beltInSequence.isSpaceTaken == false)
        {
            Vector3 toPosition = beltInSequence.GetItemPosition();
            beltInSequence.isSpaceTaken = true;
            var step = _beltManager.speed * Time.deltaTime;

            // move the item on the current belt to towards the position of the next belt space (Vector3.MoveTowords)
            // while the itme is not yet in the 'toPosition' it keeps updating its position each frame
            while (beltItem.item.transform.position != toPosition)
            {
                beltItem.item.transform.position = Vector3.MoveTowards(beltItem.transform.position, toPosition, step);
                yield return null;
            }

            // after reaching the toPosition, it sets isSpaceTaken to false,
            // assigns the current belt's to the next belt's beltItem, and sets the current belt's 'beltItem' to null, moving the item to the next belt.
            isSpaceTaken = false;
            beltInSequence.beltItem = beltItem;
            beltItem = null;
        }
    }

    // function: defines the direction of the current belt
    // create Ray fromt the current belt's position in the direction of 'forward'
    // Performs a raycast with a maximum distance of 1f
    private Belt FindNextBelt()
    {
        Transform currentBeltTransform = transform;
        RaycastHit hit;

        var forward = transform.forward;

        // Ray.ray(vector Origin, vector Direction) - ray is a concept that represents a line segment with an origin point and a direction
        Ray ray = new Ray(currentBeltTransform.position, forward);

        // Performs a raycast with a maximum distance of 1f
        // if the ray his a collider that has a belt component attached, it returns the reference to that 'belt' component
        if (Physics.Raycast(ray, out hit, 1f))
        {
            Belt belt = hit.collider.GetComponent<Belt>();

            if (belt != null)
                return belt;
        }

        return null;
    }
    
}
