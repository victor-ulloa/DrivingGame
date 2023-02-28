using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float rotationSpeed;
    [SerializeField] float brakeMultiplier;
    [SerializeField] bool isBraking;

    Rigidbody rb;
    PlayerControls input;

    float moveDirection = 0;
    float rotation = 0;

    float elapsedTime = 0;

    [SerializeField] bool canGoForward = true;
    [SerializeField] bool canGoBackwards = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new PlayerControls();
    }

    private void FixedUpdate()
    {
        if (moveDirection != 0)
        {
            elapsedTime += Time.fixedDeltaTime * moveDirection;
        }
        else
        {
            if (elapsedTime > 0)
            {
                elapsedTime = elapsedTime - Time.fixedDeltaTime * (isBraking ? brakeMultiplier : 1);
            }
            else if (elapsedTime < 0)
            {
                elapsedTime = elapsedTime + Time.fixedDeltaTime * (isBraking ? brakeMultiplier : 1);
            }
        }
        speed = speed > maxSpeed ? maxSpeed : acceleration * elapsedTime;
        if (!canGoForward && speed < 0) { speed = 0; } 
        if (!canGoBackwards && speed > 0) { speed = 0; } 
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);


        rb.AddRelativeTorque(new Vector3(0, rotationSpeed * rotation * Time.fixedDeltaTime, 0), ForceMode.VelocityChange);
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            canGoForward = speed > 0;
            canGoBackwards = speed < 0;
            speed = 0;
            elapsedTime = 0;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            canGoForward = true;
            canGoBackwards = true;
        }
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isBraking = true;
        }
        else if (context.canceled)
        {
            isBraking = false;
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
