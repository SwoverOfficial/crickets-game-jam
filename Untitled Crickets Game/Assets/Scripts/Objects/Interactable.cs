using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private GameObject outline;
    [Range(1, 5)] public float outlineSize = 1.2f;
    public Color outlineColor;
    public string message;

    private SpriteRenderer interactableSr;
    private SpriteRenderer outlineSr;
    private SpriteMask outlineMask;

    public UnityEvent onInteraction;
    
    // Start is called before the first frame update
    void Start()
    {
        outline = transform.Find("Outline").gameObject;

        //GET SPRITE RENDERERS OF OUTLINE OBJECTS (TO CHANGE IN UPDATE)
        interactableSr = GetComponent<SpriteRenderer>();
        outlineSr = outline.GetComponent<SpriteRenderer>();
        outlineMask = transform.Find("OutlineSpriteMask").gameObject.GetComponent<SpriteMask>();

        //SET OUTLINE COLOR & SIZE
        outlineSr.material.SetColor("_OutlineColor", outlineColor);
        outline.transform.localScale = new Vector3(outlineSize, outlineSize, outline.transform.localScale.z);

        DisableOutline();
    }

    private void LateUpdate()
    {
        //SET SPRITE OF OUTLINE AND MASK TO THIS OBJECT'S SPRITE
        outlineSr.sprite = interactableSr.sprite;
        outlineMask.sprite = interactableSr.sprite;
    }

    public void Interact()
    {
        onInteraction.Invoke();
    }

    public void DisableOutline()
    {
        outline.SetActive(false);
    }

    public void EnableOutline()
    {
        outline.SetActive(true);
    }
}
