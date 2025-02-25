using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1000f;

    public Transform playerBody;

    private float xRotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Keeps cursor from leaving center
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; //Rotating around x axis for up & down motion
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Stops over-rotation on up and down movement

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Up and down mouse & camera movement
        playerBody.Rotate(Vector3.up * mouseX); //Side to side mouse & camera movement
    }
}
