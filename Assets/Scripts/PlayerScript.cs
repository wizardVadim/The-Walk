using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private float HorizontalMove = 0f;
    private bool FacingRight = true;
    private Animator animator;

    [Header("Player movement settings")]
    [Range(0, 10f)] public float speed = 1f;
    [Range(0, 15f)] public float jumpForce = 8f;
    public Vector3 thisScale;

    [Header("Ground checker settings")]
    public bool isGrounded = false;
    [Range(-5f, 5f)] public float checkGroundOffsetY = -1.4f;
    [Range(0f, 5f)] public float checkGroundRadius = 1.1f;

    // Start is called before the first frame update
     void Start()
    {
        rb          = GetComponent<Rigidbody2D>();
        
        animator    = GetComponent<Animator>();   
    
        thisScale   = transform.localScale;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.H)
            && isGrounded
            )  
        {
            animator.SetTrigger("Smoke");
            HorizontalMove = 0;
            audioSource.PlayOneShot(audioSource.clip);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !IsAnimationPlaying("player_smoking"))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJump", true);

        }

        if (isGrounded && IsAnimationPlaying("JUMP"))
        {
            animator.SetBool("IsJump", false);
        }

        HorizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (HorizontalMove != 0 && !IsAnimationPlaying("player_smoking")) 
        {
            animator.SetFloat("Blend", speed * 5);
        } 
        else
        {
            animator.SetFloat("Blend", 0);
        }

        if (HorizontalMove < 0 && FacingRight)
        {
            Flip();
        }
        else if (HorizontalMove > 0 && !FacingRight) 
        {
            Flip();
        }

         if (HorizontalMove != 0 && !IsAnimationPlaying("player_smoking")) 
        {
            animator.SetFloat("Blend", speed * 5);
        } 
        else
        {
            animator.SetFloat("Blend", 0);
        }

        if (HorizontalMove < 0 && FacingRight)
        {
            Flip();
        }
        else if (HorizontalMove > 0 && !FacingRight) 
        {
            Flip();
        }
    }

    public void FixedUpdate() 
    {
        if (!IsAnimationPlaying("player_smoking")) 
        {
            Vector2 targetVelocity = new Vector2(HorizontalMove * 10f, rb.velocity.y);
            rb.velocity = targetVelocity;
        }

        CheckGround();
    }

    public void Flip() 
    {
        FacingRight = !FacingRight;

        thisScale.x *= -1;

        Vector3 position = transform.position;

        position.x -= (thisScale.x * 2);

        transform.position = position;
        transform.localScale = thisScale;    
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + checkGroundOffsetY), checkGroundRadius); 
        if (colliders.Length > 2) {
            isGrounded = true;
        }
        else 
        {
            isGrounded = false;
        }
    }

    public bool IsAnimationPlaying(string animationName) {        
    var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    if (animatorStateInfo.IsName(animationName))             
        return true;
    
    return false;
}

}
