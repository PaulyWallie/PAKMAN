using UnityEngine;

/// <summary>
/// Responsible ONLY for updating the Animator state based on player physics.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateAnimation(bool isGrounded)
    {
        anim.SetFloat("moveSpeed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void TriggerHurt()
    {
        //anim.SetTrigger("Hurt");
    }
}
