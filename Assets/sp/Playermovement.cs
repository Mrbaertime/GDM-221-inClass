using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class Playermovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 moveInput;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayer;

    private bool jumpPressed;

    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");

    //private InputTime playerControls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
       //playerControls = new InputTime(); // InputAction 
    }
    //private void OnMove(InputValue value)
    //{
    //    moveInput = value.Get<Vector2>();
    //}

    //private void OnJump(InputValue value)
    //{
    //    if (value.isPressed)
    //        jumpPressed = true;
    //}

    //ปุ่ม
    //private void OnEnable()
    //{
    //    playerControls.Enable();
    //    playerControls.Player.Jump.performed += OnJumpPerformed;
    //}

    //private void OnDisable()
    //{
    //    playerControls.Disable();
    //    playerControls.Player.Jump.performed -= OnJumpPerformed;
    //}

    //private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    //{
    //    jumpPressed = true;
    //}

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Update()
    {
        //moveInput = playerControls.Player.Move.ReadValue<Vector2>();

        // Visual flip for side-scroller (use X axis)
        if (sr != null && Mathf.Abs(moveInput.x) > 0.01f)
            sr.flipX = moveInput.x < 0f;

        // Drive animation by movement magnitude (0..1)
        anim.SetFloat(SpeedHash, Mathf.Abs(moveInput.x));

        // Grounded check 
        bool grounded = IsGrounded();
        anim.SetBool(GroundedHash, grounded);
    }

    private void FixedUpdate()
    {
        // Physics move
        Vector2 v = rb.linearVelocity;
        v.x = moveInput.x * moveSpeed;
        rb.linearVelocity = v;

        if (jumpPressed)
        {
            jumpPressed = false;
            if (IsGrounded())
            {
                // Reset vertical velocity for consistent jump feel
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                // Trigger jump animation
                anim.SetTrigger(JumpHash);
            }
        }
    }

    public void SwipeMoveLeft() { moveInput = Vector2.left; }
    public void SwipeMoveRight() { moveInput = Vector2.right; }
    public void SwipeStop() { moveInput = Vector2.zero; }
    public void SwipeJump() { jumpPressed = true; }
}
