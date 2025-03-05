using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPlayer : MonoBehaviour
{
    private CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    public void TeleportPlayerToArea(Transform spawnTransform)
    {
        cc.enabled = false; //Turning off character controller gets rid of glitch where player "rubberbands" back and doesn't teleport
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;
        cc.enabled = true;
    }

    public void TeleportPlayerToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
