using UnityEngine;
using UnityEngineInternal;

public class SpriteDirectionalController : MonoBehaviour
{
    [Range(0f, 360f)][SerializeField] private float frontFacingAngle = 0f;
    private float backAngle = 65f;
    private float sideAngle = 155f;
    private Transform mainTransform;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        mainTransform = gameObject.transform.parent.gameObject.transform;
        
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);

        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);

        //CALCULATE ANGLE BASED ON ROTATION OF CHARACTER
        float signedAngleWithRotation = signedAngle + frontFacingAngle;
        if (signedAngleWithRotation > 180f)
            signedAngleWithRotation -= 360f;

        Vector2 animationDirection = new Vector2(0f, -1f); //Initializes animation angle and defaults it to front animation

        float angle = Mathf.Abs(signedAngleWithRotation);

        //DETERMINING ANIMATION TO PLAY
        if (angle < backAngle)
            animationDirection = new Vector2(0f, -1f); //BACK ANIMATION
        else if (angle < sideAngle)
        {
            if (signedAngleWithRotation < 0)
                animationDirection = new Vector2(-1f, 0f); //SIDE ANIMATION (LEFT)
            else
                animationDirection = new Vector2(1f, 0f); //SIDE ANIMATION (RIGHT)
        }
        else
            animationDirection = new Vector2(0f, 1f); //FRONT ANIMATION

        //SETTING ANIMATION
        animator.SetFloat("moveX", animationDirection.x);
        animator.SetFloat("moveY", animationDirection.y);
    }
}
