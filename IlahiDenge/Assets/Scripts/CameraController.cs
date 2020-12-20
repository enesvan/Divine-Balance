using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Quaternion newRotation;
    public Vector3 newPosition;
    public Vector3 newZoom;

    public Vector3 draggingStartPosition;
    public Vector3 draggingCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        MouseClickHandler();
        KeyInputHandler();
    }

    void MouseClickHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
                draggingStartPosition = ray.GetPoint(entry);
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                draggingCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + draggingStartPosition - draggingCurrentPosition;
            }
        }

        if (Input.GetMouseButtonDown(1))
            rotateStartPosition = Input.mousePosition;
        if (Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 dif = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-dif.x / 5f));
        }
    }

    void KeyInputHandler()
    {
        if (Input.mouseScrollDelta.y != 0 && newZoom.y > 14f && newZoom.y<51f)
            newZoom += Input.mouseScrollDelta.y * zoomAmount;

        if (newZoom.y <= 14f && newZoom.z >= -14f)
        {
            newZoom.y = 15f;
            newZoom.z = -15f;
        }
        if (newZoom.y >= 51f && newZoom.z<=-51f)
        {
            newZoom.y = 50f;
            newZoom.z = -50f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
            movementSpeed = fastSpeed;
        else
            movementSpeed = normalSpeed;

        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
            newPosition += (transform.forward * movementSpeed);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            newPosition += (transform.forward * -movementSpeed);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            newPosition += (transform.right * movementSpeed);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            newPosition += (transform.right * -movementSpeed);

        if (newPosition.x <= -105f)
            newPosition.x = -103f;

        if (newPosition.x >= 87f)
            newPosition.x = 85f;

        if (newPosition.z >= 127f)
            newPosition.z = 125f;

        if (newPosition.z <= -127f)
            newPosition.z = -125f;

        if (Input.GetKey(KeyCode.Q))
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        if (Input.GetKey(KeyCode.E))
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);

        if (Input.GetKey(KeyCode.R))
            newZoom += zoomAmount;
        if (Input.GetKey(KeyCode.F))
            newZoom -= zoomAmount;


        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

    }
}
