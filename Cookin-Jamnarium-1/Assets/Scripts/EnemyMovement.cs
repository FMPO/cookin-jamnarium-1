using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    MOVING, STUNNED
};

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Movement vars")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private EnemyState EnemyState = EnemyState.MOVING;

    [Header("Enemy Physics vars")]
    public Rigidbody2D rb;
    //public float damagedForce = 1f;

    [Header("Enemy Sprite vars")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite movingSprite;
    [SerializeField] private Sprite stunnedSprite;

    void Start()
    {
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
            //rotate sprite towards target
            Vector2 direction = targetTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            //apply a force in movementDirection equal to moveSpeed
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(transform.right.normalized * moveSpeed, ForceMode2D.Force);
        }
        //else if bug is stunned,...
        else if (EnemyState == EnemyState.STUNNED)
        {
            ////increment tempTimeToGetUp
            //tempTimeToGetUp += Time.fixedDeltaTime;

            ////if timeToGetUp has elapsed AND stunned bug has stopped moving,...
            //if (tempTimeToGetUp >= timeToGetUp && rb.linearVelocity.sqrMagnitude < 0.5f)
            //{
            //    EnterMovingState();
            //}
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
        //tempTimeToGetUp = 0;
    }

    public void EnterMovingState()
    {
        //change sprite to wandering
        //spriteRenderer.sprite = movingSprite;

        //enter the moving state
        EnemyState = EnemyState.MOVING;

        //prevent bug from spinning
        rb.freezeRotation = true;

        //zero out bug's velocities
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }
}
