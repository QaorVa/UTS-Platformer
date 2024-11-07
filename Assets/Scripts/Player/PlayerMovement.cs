using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEditor.ShaderGraph.Internal;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float runSpeed = 18f;
    private float activeSpeed;
    [SerializeField] private float jumpForce = 16f;
    private bool isDoubleJumping;
    [SerializeField] private float maxFallSpeed = -70f;


    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpCounter;
    [SerializeField] private float wallJumpDuration = 0.4f;
    [SerializeField] private float wallJumpTime = 0.2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8f, 16f);
    

    private bool isFacingRight = true;

    [SerializeField] private GameObject target;
    [SerializeField] private float lookAheadModifier = 3f;

    [HideInInspector] public Animator anim;

    [SerializeField] private CinemachineVirtualCamera vcam;

    [SerializeField] private AudioClip jumpSound;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        activeSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {

        if(IsGrounded() && horizontal == 0)
        {
            anim.Play("Player_Idle");
        }
        if(IsGrounded() && (horizontal > 0 || horizontal < 0))
        {
            anim.Play("Player_Run");
        }

        if(!IsGrounded() && !isWallSliding && rb.velocity.y < 0)
        {
            anim.Play("Player_Fall");
        }

        if(!IsGrounded() && rb.velocity.y > 0 && !isWallSliding)
        {
            anim.Play("Player_Jump");
        }

        if (!IsGrounded() && isWallSliding)
        {
            anim.Play("Player_WallHang");
        }

        if(horizontal > 0 || horizontal < 0)
        {
            target.transform.position = new Vector2(transform.position.x + horizontal * lookAheadModifier + (rb.velocity.x / 4f), transform.position.y + (rb.velocity.y / 1.7f));
        } else
        {
            target.transform.position = new Vector2(transform.localPosition.x, transform.position.y + (rb.velocity.y / 1.7f));
        }

        if(rb.velocity.y <= maxFallSpeed + 1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadIgnoreY = false;
            
        } else
        {
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadIgnoreY = true;

        }


        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            isWallSliding = false;
        } else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (!isWallJumping)
        {
            if (!isFacingRight && horizontal > 0f)
            {
                Flip();
            }
            else if (isFacingRight && horizontal < 0f)
            {
                Flip();
            }
        }

        WallSlide();

        if (wallJumpCounter > 0)
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if(!IsGrounded())
        {
            activeSpeed = speed;
        }
    }

    private void FixedUpdate()
    {
        if(!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * activeSpeed, rb.velocity.y);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            PlayAudio(jumpSound);
            anim.Play("Player_Jump");
            isDoubleJumping = false;
        } else if(context.performed && !isDoubleJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            PlayAudio(jumpSound);
            isDoubleJumping = true;
        }

        if (context.canceled && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    public void Run(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            activeSpeed = runSpeed;
        }

        if (context.canceled)
        {
            activeSpeed = speed;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        if(horizontal > 0f)
        {
            horizontal = 1f;
        } else if (horizontal < 0f)
        {
            horizontal = -1f;
        }

    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (isWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        } else
        {
            isWallSliding = false;
        }
    }

    public void WallJump(InputAction.CallbackContext context)
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }

        if(context.performed && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            PlayAudio(jumpSound);
            rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y);
            wallJumpCounter = 0f;

            if(transform.localScale.x != wallJumpDirection)
            {
                Flip();
            }

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
