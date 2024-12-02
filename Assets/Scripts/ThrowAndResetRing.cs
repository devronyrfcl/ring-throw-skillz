using UnityEngine;

public class ThrowAndResetRing : MonoBehaviour
{
    private Rigidbody rb;

    float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos;
    private Vector2 endPos;

    public float MinSwipeDist = 30f;
    private float RingVelocity = 0;
    private float RingSpeed = 0;
    public float MaxRingSpeed = 300;

    private Vector3 angle;

    private bool holding, thrown;

    public GameObject RingMiddlePart; // Reference to the middle part of the ring
    public float rotationSpeed = 100f; // Speed of rotation for the middle part

    public delegate void RingThrownHandler();
    public static event RingThrownHandler OnRingThrown; // Notify spawner

    public AudioSource audioSource;  // Reference to the AudioSource component
    public AudioClip ThrowSound;

    private CameraManager cameraManager; // Reference to CameraManager

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetRing(); // Ensure the ring is reset when the game starts

        // Try to find the CameraManager component
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        // Rotate the RingMiddlePart around its local Z-axis
        if (RingMiddlePart != null)
        {
            RingMiddlePart.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
        }

        if (holding)
            PickupRing();

        if (thrown)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f) && hit.transform == transform)
            {
                startTime = Time.time;
                startPos = Input.mousePosition;
                holding = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTime = Time.time;
            endPos = Input.mousePosition;
            swipeDistance = (endPos - startPos).magnitude;
            swipeTime = endTime - startTime;

            if (swipeTime < 0.5f && swipeDistance > MinSwipeDist)
            {
                ThrowRing();
            }
            else
            {
                ResetRing();
            }
        }
    }

    private void PickupRing()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane * 5f;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePos);
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, 80f * Time.deltaTime);
    }

    private void ThrowRing()
    {
        CalculateSpeed();
        CalculateAngle();

        rb.useGravity = true;
        rb.AddForce(new Vector3(
            angle.x * RingSpeed,
            angle.y * RingSpeed / 2,
            angle.z * RingSpeed));

        holding = false;
        thrown = true;

        // Notify the spawner to spawn a new ring
        OnRingThrown?.Invoke();

        // Trigger camera movement when the ring is thrown
        if (cameraManager != null)
        {
            cameraManager.TriggerCameraMovement(); // Call the camera movement
        }
        if (audioSource != null && ThrowSound != null)
        {
            audioSource.PlayOneShot(ThrowSound);  // Play the sound when the score increases
        }
    }

    private void ResetRing()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        holding = false;
        thrown = false;

        // Optionally reset position or hide this ring
        // gameObject.SetActive(false);
    }

    private void CalculateAngle()
    {
        angle = Camera.main.ScreenToWorldPoint(new Vector3(
            endPos.x,
            endPos.y + 50f,
            Camera.main.nearClipPlane + 5));
    }

    private void CalculateSpeed()
    {
        if (swipeTime > 0)
            RingVelocity = swipeDistance / swipeTime;

        RingSpeed = RingVelocity * 40;

        if (RingSpeed > MaxRingSpeed)
            RingSpeed = MaxRingSpeed;

        swipeTime = 0;
    }
}
