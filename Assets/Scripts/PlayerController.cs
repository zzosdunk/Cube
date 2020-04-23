using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform groundPos;
    public Transform wallPos;

    public LayerMask groundLayer;
    private Rigidbody2D rb;

    public Vector2 wallHopDir;
    public Vector2 wallJumpDir;

    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isLookForward = true;
    private bool isSiliding;
    private bool canSimpleJump;
    private bool isAttemptingToJump;
    private bool canWallJump;
    private bool checkJumpMultiplayer;
    private bool canMove;
    private bool canRotate;
    private bool hasWallJumped;

    public float movementSpeed;
    public float jumpForce;
    public float amountOfJumps;
    public float groundPosRaius;
    public float wallPosDistance;
    public float slideSpeed;
    public float movementForceInAir;
    public float airDragMultiplayer;
    public float jumpHeightMultiplayer;
    public float wallHopForce;
    public float wallJumpForce;
    public float defaultJumpTime;
    public float defaultRotateTime;
    public float defaultWallJumpTime;

    private float moveDir;
    private float availableJumps;
    private float direction = 1;
    private float jumpTimer;
    private float rotateTimer;
    private float wallJumpTimer;
    private float lastWallJumprDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        availableJumps = amountOfJumps;
        wallHopDir.Normalize();
        wallJumpDir.Normalize();
    }

    void Update()
    {
        CheckInput();
        CheckSurroundings();
        PlayerDirection();
        IsCanJump();
        IsWallSliding();
        CheckJump();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void CheckInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (availableJumps > 0 && !isTouchingWall))
            {
                SimpleJump();
            }
            else
            {
                jumpTimer = defaultJumpTime;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && moveDir != direction)
            {
                canMove = false;
                canRotate = false;

                rotateTimer = defaultRotateTime;
            }
        }

        if (!canMove)
        {
            rotateTimer -= Time.deltaTime;

            if (rotateTimer <= 0)
            {
                canMove = true;
                canRotate = true;
            }
        }

        if (checkJumpMultiplayer && !Input.GetButton("Jump"))
        {
            checkJumpMultiplayer = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightMultiplayer);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (GameManager.Instance.CurrenGameState == GameManager.GameState.PlayState)
            {
                GameManager.Instance.ChangeGameState(GameManager.GameState.PauseState);
            }
            else
            {
                GameManager.Instance.ChangeGameState(GameManager.GameState.PlayState);
            }
        }
    }

    void IsCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            availableJumps = amountOfJumps;
        }
        if (isTouchingWall)
        {
            canWallJump = true;
        }
        if (availableJumps <= 0)
        {
            canSimpleJump = false;
        }
        else
        {
            canSimpleJump = true;
        }
    }

    void IsWallSliding()
    {
        if (isTouchingWall && moveDir == direction && rb.velocity.y < 0)
        {
            isSiliding = true;
        }
        else
        {
            isSiliding = false;
        }
    }

    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundPos.position, groundPosRaius, groundLayer);

        isTouchingWall = Physics2D.Raycast(wallPos.position, transform.right, wallPosDistance, groundLayer);
    }

    void PlayerDirection()
    {
        if (isLookForward && moveDir < 0)
        {
            RotatePlayer();
        }
        else if (!isLookForward && moveDir > 0)
        {
            RotatePlayer();
        }
    }

    void Movement()
    {
        if (!isGrounded && !isSiliding && moveDir == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplayer, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * moveDir, rb.velocity.y);
        }
        
        if (isSiliding)
        {
            if (rb.velocity.y < -slideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
            }
        }
    }

    void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!isGrounded && isTouchingWall && moveDir != 0 && moveDir != direction)
            {
                WallJump();
            }
            else if(isGrounded)
            {
                SimpleJump();
            }
        }
        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && moveDir == -lastWallJumprDir)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    void SimpleJump()
    {
        if (canSimpleJump && !isSiliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            availableJumps--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplayer = true;
        }
    }

    void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isSiliding = false;
            availableJumps = amountOfJumps;
            availableJumps--;
            Vector2 force = new Vector2(wallJumpForce * wallJumpDir.x * moveDir, wallJumpForce * wallJumpDir.y);
            rb.AddForce(force, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplayer = true;
            rotateTimer = 0;
            canMove = true;
            canRotate = true;
            hasWallJumped = true;
            wallJumpTimer = defaultWallJumpTime;
            lastWallJumprDir = -direction;
        }
    }

    void RotatePlayer()
    {
        if (!isSiliding && canRotate)
        {
            direction *= -1;
            isLookForward = !isLookForward;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundPos.position, groundPosRaius);

        Gizmos.DrawLine(wallPos.position, new Vector3(wallPos.position.x + wallPosDistance, wallPos.position.y, wallPos.position.z));
    }
}
