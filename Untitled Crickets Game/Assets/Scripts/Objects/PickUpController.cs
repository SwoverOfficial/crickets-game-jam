using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private SpriteBillboard spriteBillboardScript;
    
    public static bool slotFull;
    private bool pickedUp;

    public GameObject playerItemSlot;
    public GameObject parentObject;
    public GameObject spriteChildObject;

    private Vector3 originalChildPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slotFull && pickedUp && Input.GetKeyDown(KeyCode.Q))
            Drop();
    }

    public void PickUp()
    {
        if (slotFull)
            return;

        originalChildPosition = spriteChildObject.transform.localPosition;
        spriteChildObject.transform.localPosition = Vector3.zero;

        slotFull = true;
        pickedUp = true;

        spriteBillboardScript.enabled = false;

        transform.SetParent(playerItemSlot.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        spriteChildObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void Drop()
    {
        slotFull = false;
        pickedUp = false;

        transform.SetParent(parentObject.transform);

        transform.localRotation = Quaternion.Euler(Vector3.zero);
        spriteChildObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        spriteChildObject.transform.localPosition = originalChildPosition;

        spriteBillboardScript.enabled = true;
    }
}
