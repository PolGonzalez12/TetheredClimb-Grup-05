using UnityEngine;
using System.Collections;

public class Player2Controller : MonoBehaviour
{
    [Header("Movimiento y Salto")]
    public float speed = 5f;
    public float jumpForce = 20f;
    public float maxJumpVelocity = 20f;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Sistema de Cuerda")]
    public DistanceJoint2D ropeJoint;
    public Rigidbody2D connectedBody;
    public float tensionThreshold = 0.05f;

    [Header("Paredes y Wall Jump")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public LayerMask wallLayer;
    public float wallStickTime = 0.2f;
    public float wallJumpForceX = 10f;
    public float wallJumpForceY = 15f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private Collider2D playerCollider;
    private Collider2D connectedCollider;

    private bool isGrounded;
    private float wallStickCounter = 0f;
    private float originalRopeDistance;
    private bool isWallSliding = false;
    private int wallDirection = 0;
    private bool wasGrounded = false;

    void Start()
    {
        originalRopeDistance = ropeJoint.distance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        if (connectedBody != null)
            connectedCollider = connectedBody.GetComponent<Collider2D>();
    }

    void Update()
    {
        transform.localScale = Vector3.one;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && isGrounded)
            animator.SetBool("Jump", false);

        float moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;

        bool isTense = Vector2.Distance(transform.position, connectedBody.position) > ropeJoint.distance - tensionThreshold;
        Vector2 toOther = (connectedBody.position - rb.position).normalized;
        float directionToOther = Mathf.Sign(toOther.x);

        if (isTense && Mathf.Sign(moveInput) == -directionToOther)
            moveInput *= 0.5f;

        isWallSliding = false;

        bool touchingLeft = Physics2D.Raycast(wallCheckLeft.position, Vector2.left, 0.1f, wallLayer);
        bool touchingRight = Physics2D.Raycast(wallCheckRight.position, Vector2.right, 0.1f, wallLayer);

        if ((touchingLeft || touchingRight) && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            wallDirection = touchingLeft ? -1 : 1;
            wallStickCounter += Time.deltaTime;

            if (wallStickCounter < wallStickTime)
            {
                rb.velocity = new Vector2(0f, Mathf.Max(rb.velocity.y, -0.5f));
                rb.gravityScale = 0f;
                animator.SetBool("OnWall", true);
            }
        }
        else
        {
            wallStickCounter = 0f;
            rb.gravityScale = 1f;
            animator.SetBool("OnWall", false);
        }

        if (isWallSliding && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(-wallDirection * wallJumpForceX, wallJumpForceY);
            rb.gravityScale = 1f;
            animator.SetTrigger("Jump");
        }

        if (!isWallSliding || wallStickCounter == 0f)
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (rb.velocity.y > maxJumpVelocity)
            rb.velocity = new Vector2(rb.velocity.x, maxJumpVelocity);

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (isWallSliding)
        {
            sr.flipX = wallDirection == -1;
        }
        else if (moveInput != 0)
        {
            sr.flipX = moveInput < 0;
        }
        else
        {
            Vector2 toRope = connectedBody.position - (Vector2)transform.position;
            sr.flipX = toRope.x < 0;
        }


        if (Input.GetMouseButtonDown(0)) animator.SetTrigger("Attack1");
        if (Input.GetMouseButtonDown(1)) animator.SetTrigger("Attack2");

        bool otherIsBelow = connectedBody.position.y < transform.position.y - 0.5f;

        if (Input.GetKey(KeyCode.P) && (isGrounded || otherIsBelow))
        {
            float minDistance = 1.5f;
            float retractSpeed = 3f;

            if (ropeJoint.distance > minDistance)
                ropeJoint.distance -= retractSpeed * Time.deltaTime;

            if (connectedCollider != null)
                Physics2D.IgnoreCollision(playerCollider, connectedCollider, true);

            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            Vector2 dirToAnchor = ((Vector2)transform.position - (Vector2)connectedBody.position).normalized;
            float pullForce = 20f;
            connectedBody.velocity = new Vector2(connectedBody.velocity.x, 0f);
            connectedBody.AddForce(dirToAnchor * pullForce, ForceMode2D.Force);

            animator.SetTrigger("Pull");
        }
        else
        {
            if (connectedCollider != null)
                Physics2D.IgnoreCollision(playerCollider, connectedCollider, false);
            ropeJoint.distance = originalRopeDistance;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.UpArrow))
            StartCoroutine(DisablePlatformCollision());

        wasGrounded = isGrounded;
    }

    IEnumerator DisablePlatformCollision()
    {
        Collider2D platform = GetPlatformBelow();
        if (platform != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, true);
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreCollision(playerCollider, platform, false);
        }
    }

    Collider2D GetPlatformBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Platform"));
        return hit.collider;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheckLeft != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheckLeft.position, wallCheckLeft.position + Vector3.left * 0.1f);
        }

        if (wallCheckRight != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(wallCheckRight.position, wallCheckRight.position + Vector3.right * 0.1f);
        }
    }
}
