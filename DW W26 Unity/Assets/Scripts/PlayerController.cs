using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Player Component References")]
    [SerializeField] Rigidbody2D rigidbody2D;


    [Header("Player Settings")]
 
    [SerializeField] float speed;
    [SerializeField] float jumpPower;

    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;


    private float horizontal;
    private float up;
    public void Move(InputAction.CallbackContext context)
    {

        horizontal = context.ReadValue<Vector2>().x;

    }

    public void jump(InputAction.CallbackContext context)
    {

        if (context.started && isGrounded())
        {
             
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocityX, jumpPower);

        }


    }

    private void Update()
    {

        rigidbody2D.linearVelocity = new Vector2(horizontal * speed, rigidbody2D.linearVelocity.y);

       
    }

    private bool isGrounded()
    {

        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

    }
 



    }
