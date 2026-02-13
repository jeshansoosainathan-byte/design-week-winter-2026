using Unity.Burst.Intrinsics;

using UnityEngine;
using UnityEngine.InputSystem;


public class ArmController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] float rotation;

    void Start()
    {
      


    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void look(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        float angle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
}
