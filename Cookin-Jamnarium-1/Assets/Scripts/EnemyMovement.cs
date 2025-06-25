using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
enum EnemyMovementType
{
    WALK_ON_GROUND, //WALK_ON_GROUND_AND_WALLS, FLY
};

enum EnemyMovementDirection
{
    LEFT, RIGHT
};

public enum EnemyState
{
    MOVING, STUNNED
};

public class EnemyMovement : MonoBehaviour
{
    private float timeToGetUp = 3f;
    private float tempTimeToGetUp = 0f;
    private float bugGravity;

    [Header("Important Bug Movement vars")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private EnemyMovementDirection startingDirection = EnemyMovementDirection.LEFT;

    [Header("Bug Movement vars")]
    public EnemyState EnemyState = EnemyState.MOVING;
    [SerializeField] private float wallDetectionRange = 1f;
    private Vector2 wallDetectionDirection = Vector2.left;
    [SerializeField] private EnemyMovementType bugMovementType = EnemyMovementType.WALK_ON_GROUND;
    private Vector2 movementDirection = Vector2.left;
    [SerializeField] private Transform leftLedgeTransform;
    [SerializeField] private Transform rightLedgeTransform;
    [SerializeField] private float ledgeDetectionRange = 1f;


    [Header("Bug Physics vars")]
    public float damagedForce = 1f;
    public Rigidbody2D rb;

    [Header("Bug Sprite vars")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    //[SerializeField] private Sprite movingSprite;
    //[SerializeField] private Sprite stunnedSprite;

    void Start()
    {
        bugGravity = rb.gravityScale;

        //flip sprite and movementDirection depending on startingDirection
        if (startingDirection == EnemyMovementDirection.LEFT)
        {
            spriteRenderer.flipX = true;
            movementDirection = Vector2.left;
            wallDetectionDirection = Vector2.left;
        }
        else if (startingDirection == EnemyMovementDirection.RIGHT)
        {
            spriteRenderer.flipX = false;
            movementDirection = Vector2.right;
            wallDetectionDirection = Vector2.right;
        }

        //enter the moving state
        EnterMovingState();
    }

    //UPDATE FUNCTION FOR DEBUGGING PURPOSES
    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.M))
    //    {
    //        EnterStunnedState();
    //    }
    //}

    void FixedUpdate()
    {
        //if the bug should be moving,...
        if (EnemyState == EnemyState.MOVING)
        {
            //***** Wall detection *****
            //cast a ray in wallDetectionDirection direction
            RaycastHit2D[] wallHits = Physics2D.RaycastAll(transform.position, wallDetectionDirection.normalized, wallDetectionRange, LayerMask.GetMask("Solids") | LayerMask.GetMask("Bug"));

            //iterate through each thing the raycast hit,...
            foreach (RaycastHit2D hit in wallHits)
            {
                //if the afformentioned ray hits something and that something is NOT this bug,...
                if (hit && hit.transform.gameObject != gameObject)
                {
                    //flip moving direction
                    FlipMovingDirection();

                    //do not detect any other hits in the array
                    break;
                }
            }

            //***** Ledge detection *****
            //cast a ray downwards from leftLedgeTransform
            RaycastHit2D leftLedgeHit = Physics2D.Raycast(leftLedgeTransform.position, Vector2.down, ledgeDetectionRange, LayerMask.GetMask("Ground"));

            //cast a ray downwards from rightLedgeTransform
            RaycastHit2D rightLedgeHit = Physics2D.Raycast(rightLedgeTransform.position, Vector2.down, ledgeDetectionRange, LayerMask.GetMask("Ground"));

            //based on leftLedgeHit, rightLedgeHit, and movementDirection, flip or don't flip
            bool leftDetected = leftLedgeHit;
            bool rightDetected = rightLedgeHit;
            Debug.Log(leftDetected + " " +rightDetected);
            switch ((leftDetected, rightDetected))
            {
                //if both detections have solids below them,...
                case (true, true):
                    //do nothing
                    break;
                //if only the left detection has a solid below it,...
                case (true, false):
                    if (movementDirection == Vector2.right)
                    {
                        //flip moving direction
                        FlipMovingDirection();
                    }
                    break;
                //if only the right detection has a solid below it,...
                case (false, true):
                    if (movementDirection == Vector2.left)
                    {
                        //flip moving direction
                        FlipMovingDirection();
                    }
                    break;
                //if neither detections have solids below them,...
                case (false, false):
                    //do nothing
                    break;
            }

            //apply a force in movementDirection equal to moveSpeed
            rb.AddForce(movementDirection.normalized * moveSpeed, ForceMode2D.Force);
        }
        //else if bug is stunned,...
        else if (EnemyState == EnemyState.STUNNED)
        {
            //increment tempTimeToGetUp
            tempTimeToGetUp += Time.fixedDeltaTime;

            //if timeToGetUp has elapsed AND stunned bug has stopped moving,...
            if (tempTimeToGetUp >= timeToGetUp && rb.linearVelocity.sqrMagnitude < 0.5f)
            {
                EnterMovingState();
            }
        }
    }

    public void EnterStunnedState()
    {
        //change sprite to stunned
        //spriteRenderer.sprite = stunnedSprite;

        //enter the stunned state
        EnemyState = EnemyState.STUNNED;

        //allow bug to spin
        rb.freezeRotation = false;

        //begin to countdown til getting back up
        tempTimeToGetUp = 0;
    }

    public void EnterMovingState()
    {
        //change sprite to wandering
        //spriteRenderer.sprite = movingSprite;

        //enter the moving state
        EnemyState = EnemyState.MOVING;

        //reset rotation
        transform.rotation = Quaternion.identity;

        //prevent bug from spinning
        rb.freezeRotation = true;

        //zero out bug's velocities
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }

    private void FlipMovingDirection()
    {
        //flip sprite and movementDirection depending on the currect direction
        if (movementDirection == Vector2.right)
        {
            spriteRenderer.flipX = true;
            movementDirection = Vector2.left;
            wallDetectionDirection = Vector2.left;
        }
        else if (movementDirection == Vector2.left)
        {
            spriteRenderer.flipX = false;
            movementDirection = Vector2.right;
            wallDetectionDirection = Vector2.right;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, wallDetectionDirection.normalized * wallDetectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(leftLedgeTransform.position, Vector2.down * ledgeDetectionRange);
        Gizmos.DrawRay(leftLedgeTransform.position, Vector2.down * ledgeDetectionRange);
    }

    //A get variable that returns the gravityScale of this bug's rigidBody
    public float BugGravity
    {
        get
        {
            return bugGravity;
        }
    }
}
