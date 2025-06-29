using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Movement Details")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8f;
    private float xInput;
    private bool facingRight = true;
    private bool canMove = true;
    private bool canJump = true;

    [Header("Collision Details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        HandleCollision();

        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void EnableJumpAndMovement(bool enable)
    {
        canJump = enable;
        canMove = enable;
    }

    private void HandleAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);

        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryToAttack();
        }
    }

    private void TryToAttack()
    {
        if (isGrounded)
        {
            anim.SetTrigger("attack");
        }
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void HandleMovement()
    {
        if (canMove)
        {
            rb.velocity = new(xInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new(0, rb.velocity.y);
        }
    }


    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void HandleFlip()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
