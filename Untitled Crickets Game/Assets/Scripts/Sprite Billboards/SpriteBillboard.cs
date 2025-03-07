using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = true;
    // Update is called once per frame
    void LateUpdate()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        else
            transform.rotation = Camera.main.transform.rotation;
    }
}
