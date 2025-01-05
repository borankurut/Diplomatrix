using Unity.VisualScripting;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 50f;

    [SerializeField]
    private float rotationSpeed = 50f;

    // Movement bounds

    [SerializeField]
    private Vector3 minBounds = new Vector3(0f, 0f, 0f);   // Minimum (x, y, z)

    [SerializeField]
    private Vector3 maxBounds = new Vector3(100f, 20f, 100f); // Maximum (x, y, z)

    [SerializeField]
    GameObject playerSideShower;

    [SerializeField]
    GameObject enemySideShower;

    private bool canMove = false;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        ClampPosition();
        HandleGridShowers();
    }

    void HandleMovement()
    {
        if(!canMove)
            return;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        float moveY = 0;

        if (Input.GetKey(KeyCode.E)) moveY = 1;
        if (Input.GetKey(KeyCode.Q)) moveY = -1;

        Vector3 moveDirection = new Vector3(moveX, moveY, moveZ).normalized;
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.Self);
    }

    void HandleRotation(){
        if (Input.GetMouseButtonDown(1)){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canMove = true;
        }

        if (Input.GetMouseButton(1)){
            float mouseDeltaX = Input.GetAxis("Mouse X");
            float mouseDeltaY = Input.GetAxis("Mouse Y");

            float yaw = mouseDeltaX * rotationSpeed * Time.deltaTime;
            float pitch = -mouseDeltaY * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, yaw, Space.World);
            transform.Rotate(Vector3.right, pitch, Space.Self);
        }

        if (Input.GetMouseButtonUp(1)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canMove = false;
        }
    }


    void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.z, maxBounds.z);
        transform.position = clampedPosition;
    }

    void HandleGridShowers(){
        if(canMove){
            playerSideShower.GetComponent<SideScript>().deactivateAllActivePlanes();
            enemySideShower.GetComponent<SideScript>().deactivateAllActivePlanes();
            playerSideShower.SetActive(false);
            enemySideShower.SetActive(false);
        }
        else{
            playerSideShower.SetActive(true);
            enemySideShower.SetActive(true);
        }
    }
    void OnDestroy(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}