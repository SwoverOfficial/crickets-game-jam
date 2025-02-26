using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    private Interactable currentInteractable;

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //Creates line directly outwards from center of camera

        //INTERACTIONS
        if (Physics.Raycast(ray, out hit, playerReach)) //If colliders of objects in reach of player
        {
            if (hit.collider.tag == "Interactable") //If object is interactable
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (currentInteractable && newInteractable != currentInteractable) //If there is a currentInteractable and it is not the newInteractable
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else //If interactable component is not enabled on object
                {
                    DisableCurrentInteractable();
                }
            } 
            else //If object not interactable
            {
                DisableCurrentInteractable();
            }
        }
        else //If nothing is in reach
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();

        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
