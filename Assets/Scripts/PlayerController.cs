using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;

    Rigidbody rb;
    PlayerControls input;

    float moveDirection = 0;
    float rotation = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new PlayerControls();
    }

    private void Start()
    {

    }

    private void FixedUpdate() {
        Vector3 forwardMove = transform.forward * speed * moveDirection * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);

        rb.AddRelativeTorque(new Vector3(0,rotationSpeed * rotation * Time.fixedDeltaTime,0), ForceMode.VelocityChange);
    }

    private void OnEnable()
    {
        input.Player.SetCallbacks(this);
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.SetCallbacks(null);
        input.Player.Disable();
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("START");
        }
        else if (context.canceled)
        {
            Debug.Log(message: "CANCEL");
        }
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<float>();
    }

    public void OnTurn(InputAction.CallbackContext context)
    {
        rotation = context.ReadValue<float>();
    }
}
