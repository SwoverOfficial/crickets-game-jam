using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatueController : MonoBehaviour
{
    [SerializeField] private Animator statueAnim;
    
    [SerializeField] private GameObject itemSlot;

    public int piecesPlaced;
    public int totalPieces = 3;

    private void Start()
    {
        statueAnim.SetInteger("PiecesRestored", 0);
    }

    public void PlacePieceOnStatue()
    {
        if (itemSlot.transform.childCount == 0)
            return;

        GameObject itemHeld = itemSlot.transform.GetChild(0).gameObject;

        if (itemHeld.GetComponent<AdditionalTags>() == null)
            return;

        if (itemHeld.GetComponent<AdditionalTags>().tags.Contains("StatuePiece"))
        {
            itemHeld.GetComponent<PickUpController>().Drop();
            itemHeld.SetActive(false);

            piecesPlaced++;
            statueAnim.SetInteger("PiecesRestored", piecesPlaced);

            if (piecesPlaced == totalPieces)
                Debug.Log("Win");
        }

    }
}
