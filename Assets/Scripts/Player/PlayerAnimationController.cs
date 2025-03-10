using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoveDirection(float moveX, float moveY, float moveSpeed)
    {
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    public float GetMoveSpeed()
    {
        return animator.GetFloat("MoveSpeed");
    }

    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }


    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }


}
