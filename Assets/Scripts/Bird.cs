using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    private const float JUMP_VELOCITY = 5f;

    private Rigidbody2D birdRigidbody2D;
    private InputSystemAction playerInputActions;
    private Animator animator;


    private void Awake()
    {
        birdRigidbody2D = GetComponent<Rigidbody2D>();

        playerInputActions = new();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    //Jumps when the player appropriate key is pressed
    private void Jump(InputAction.CallbackContext obj) 
    {
        if (obj.performed)
        {
            birdRigidbody2D.linearVelocity = Vector2.up * JUMP_VELOCITY;
        }

    }

    //Turns off gravity, flight animations and player input
    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerInputActions.Player.Disable();

        animator = GetComponentInChildren<Animator>();
        animator.enabled = false;

        birdRigidbody2D.simulated = false;
    }

}
