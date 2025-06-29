using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Move,
        Jump,
        Code
    }

    //Variables
    public Animator animator;
    public PlayerState state = PlayerState.Idle;
    public InputSystem_Actions inputActions;
    public GameObject player;

    public int health = 1; //if we're doing 1-hit we dont really need this,
    public bool hasDied = false; //only this

    public bool facingRight = true;

    private BoxCollider2D bCollider;
    private Rigidbody2D rBody;
    [SerializeField]
    private float speed;
    private Vector2 moveInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = PlayerState.Idle;
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        bCollider = GetComponent<BoxCollider2D>();
        rBody = GetComponent<Rigidbody2D>();
        if (rBody is null)
        {
            Debug.Log("Rigid Body is null");
        }

    }

    private void FixedUpdate()
    {

        if (bCollider.IsTouchingLayers(6))
        {
            moveInput.y = 0f;
        }

        switch (state)
        {
           case PlayerState.Idle:
                if (inputActions.Player.Move.IsPressed())
                {
                    state = PlayerState.Move;
                    break;
                }
                if (inputActions.Player.CodeState.IsPressed())
                {
                    state = PlayerState.Code;
                    break;
                }
                if (inputActions.Player.Jump.IsPressed())
                {
                    state = PlayerState.Jump;
                }

                //disable coding actions
                if (inputActions.Player.Coding.enabled == true)
                {
                    inputActions.Player.Coding.Disable();
                }

                break;

            case PlayerState.Move:
                if(!inputActions.Player.Jump.IsPressed())
                {
                    state = PlayerState.Idle;
                    break;
                }
                if (inputActions.Player.Jump.IsPressed())
                {
                    state = PlayerState.Jump;
                    break;
                }
                if (inputActions.Player.CodeState.IsPressed())
                {
                    state = PlayerState.Code;
                    break;
                }

                //disable coding actions
                if (inputActions.Player.Coding.enabled == true)
                {
                    inputActions.Player.Coding.Disable();
                }    

                moveInput = inputActions.Player.Move.ReadValue<Vector2>();
                moveInput.y = 0f;
                rBody.transform.position += new Vector3(moveInput.x * speed, moveInput.y, 0);

                break;

            case PlayerState.Code:
                while (inputActions.Player.CodeState.IsPressed())
                {
                    inputActions.Player.Coding.Enable();
                    break;
                }

                break;
                
        }
    }

    //private void OnEnable()
    //{
    //    inputActions.Player.Enable();
    //}

    //private void OnDisable()
    //{
    //    inputActions.Player.Disable();
    //}
}
