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

    public void Move(InputAction.CallbackContext context)
    {

        horizontal = context.ReadValue<Vector2>().x;

    }

    private void FixedUpdate()
    {

        rigidbody2D.linearVelocity = new Vector2(horizontal * speed, rigidbody2D.linearVelocity.y);


    }






    }
