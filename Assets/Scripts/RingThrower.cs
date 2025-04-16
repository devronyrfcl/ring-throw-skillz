using UnityEngine;

public class RingThrower : MonoBehaviour
{
    public float swipePower = 10f;        // The maximum force applied to the ring
    public float swipeSensitivity = 0.1f; // Sensitivity of the swipe for detecting the power
    private Vector2 swipeStartPos;        // Start position of the swipe
    private Vector2 swipeEndPos;          // End position of the swipe
    private bool isSwiping = false;       // Flag to check if swipe is in progress
    private Rigidbody ringRigidbody;      // Reference to the Rigidbody component
    private bool swipeEnded = false;      // Flag to check if swipe has ended

    void Start()
    {
        ringRigidbody = GetComponent<Rigidbody>();
        ringRigidbody.isKinematic = true; // Start with kinematic mode
    }

    void Update()
    {
        HandleSwipeInput();

        if (swipeEnded && ringRigidbody.isKinematic)
        {
            // Disable kinematic and apply force
            ringRigidbody.isKinematic = false;
            Vector3 throwDirection = new Vector3(swipeEndPos.x - swipeStartPos.x, swipeEndPos.y - swipeStartPos.y, 0);
            ringRigidbody.AddForce(throwDirection * swipePower, ForceMode.Impulse);
            swipeEnded = false; // Reset for next swipe
        }
    }

    void HandleSwipeInput()
    {
        // Check if there is at least one touch on the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            // Start swipe detection
            if (touch.phase == TouchPhase.Began)
            {
                swipeStartPos = touch.position;
                isSwiping = true;
            }

            // Track swipe position
            if (isSwiping && touch.phase == TouchPhase.Moved)
            {
                swipeEndPos = touch.position;
            }

            // End swipe detection and apply force
            if (isSwiping && touch.phase == TouchPhase.Ended)
            {
                swipeEndPos = touch.position;
                isSwiping = false;
                swipeEnded = true;
            }
        }
    }
}
