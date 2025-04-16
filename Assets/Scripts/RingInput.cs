using UnityEngine;
using UnityEngine.UI;

public class RingInput : MonoBehaviour
{
    public delegate void InputDetectedHandler(Vector2 startPos, Vector2 endPos, float swipeTime);
    public static event InputDetectedHandler OnInputDetected;

    private float startTime, endTime;
    private Vector2 startPos, endPos;

    public Image swipeFillImage; // Reference to the UI Image for fill amount
    private bool isSwiping = false; // Track if the player is actively swiping

    public float MinSwipeDist = 100f; // Minimum swipe distance for a valid throw
    public float MaxSwipeDist = 1000f; // Maximum swipe distance for fill normalization

    private void Start()
    {
        if (swipeFillImage != null)
        {
            swipeFillImage.fillAmount = 0; // Initialize fill amount
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTime = Time.time;
            startPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButton(0) && isSwiping)
        {
            // Update the fill amount based on the swipe distance
            Vector2 currentPos = Input.mousePosition;
            float swipeDistance = (currentPos - startPos).magnitude;

            if (swipeFillImage != null)
            {
                // Normalize the swipe distance to a value between 0 and 1
                swipeFillImage.fillAmount = Mathf.Clamp(swipeDistance / MaxSwipeDist, 0, 1);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTime = Time.time;
            endPos = Input.mousePosition;
            isSwiping = false;

            float swipeDistance = (endPos - startPos).magnitude;
            float swipeTime = endTime - startTime;

            if (swipeDistance > MinSwipeDist)
            {
                // Trigger the input event only if swipe distance is valid
                OnInputDetected?.Invoke(startPos, endPos, swipeTime);
            }

            // Reset the fill amount after swipe ends
            if (swipeFillImage != null)
            {
                swipeFillImage.fillAmount = 0;
            }
        }
    }
}
