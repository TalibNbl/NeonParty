using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private LayerMask groundLayer;

    private PlayerInput playerInput;
    private Camera mainCam;
    private Rigidbody playerRB;
    private Vector2 moveInput;
    private Vector2 mousePos;
    

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRB = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        
    }

    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    public void OnLook(InputValue input)
    {
        mousePos = input.Get<Vector2>();
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 velocity = direction * movementSpeed;

        playerRB.linearVelocity = new Vector3(velocity.x, playerRB.linearVelocity.y, velocity.z);
    }

    private void RotatePlayer()
    {
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0;

            if(direction.sqrMagnitude < 0.001f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.Slerp(playerRB.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            playerRB.MoveRotation(newRotation);
        }
        
    }
}
