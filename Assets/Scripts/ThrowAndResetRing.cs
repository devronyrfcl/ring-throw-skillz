using UnityEngine;

public class ThrowAndResetRing : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 angle;
    private float RingSpeed = 0;
    public float MaxRingSpeed = 300;

    public GameObject RingMiddlePart; // Reference to the middle part of the ring
    public float rotationSpeed = 100f; // Speed of rotation for the middle part

    public delegate void RingThrownHandler();
    public static event RingThrownHandler OnRingThrown; // Notify spawner


    private CameraManager cameraManager; // Reference to CameraManager

    private bool thrown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetRing(); // Ensure the ring is reset when the game starts

        // Try to find the CameraManager component
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnEnable()
    {
        RingInput.OnInputDetected += HandleInputData;
    }

    private void OnDisable()
    {
        RingInput.OnInputDetected -= HandleInputData;
    }

    private void Update()
    {
        // Rotate the RingMiddlePart around its local Z-axis
        if (RingMiddlePart != null)
        {
            RingMiddlePart.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void HandleInputData(Vector2 startPos, Vector2 endPos, float swipeTime)
    {
        if (thrown) return;

        float swipeDistance = (endPos - startPos).magnitude;
        if (swipeDistance > 30f)
        {
            // Calculate speed and angle, then throw the ring
            CalculateSpeed(swipeDistance);
            CalculateAngle(endPos);
            ThrowRing();
        }
        else
        {
            ResetRing(); // Invalid swipe, reset the ring
        }
    }

    private void ThrowRing()
    {
        // Apply the force to the ring
        rb.useGravity = true;
        rb.AddForce(new Vector3(
            angle.x * RingSpeed,
            angle.y * RingSpeed / 2,
            angle.z * RingSpeed));

        thrown = true;

        // Notify the spawner to spawn a new ring
        OnRingThrown?.Invoke();

        // Trigger camera movement when the ring is thrown
        if (cameraManager != null)
        {
            cameraManager.TriggerCameraMovement();
        }

        AudioManager.Instance.PlaySFX("throw");

        Destroy(gameObject, 2.5f); // Destroy the ring after 2.5 seconds
    }

    private void ResetRing()
    {
        // Reset the ring to its initial state
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        thrown = false;
    }

    private void CalculateAngle(Vector2 endPos)
    {
        // Calculate the angle based on the swipe
        angle = Camera.main.ScreenToWorldPoint(new Vector3(
            endPos.x,
            endPos.y + 50f,
            Camera.main.nearClipPlane + 5));
    }

    private void CalculateSpeed(float swipeDistance)
    {
        // Calculate speed based on swipe distance directly
        RingSpeed = swipeDistance * 0.27f; // You can adjust the multiplier to control the speed

        // Limit the speed to a maximum value
        if (RingSpeed > MaxRingSpeed)
            RingSpeed = MaxRingSpeed;
    }
}
