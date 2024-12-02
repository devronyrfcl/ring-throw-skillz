using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform Point1; // Start position of the camera
    public Transform Point2; // End position of the camera
    public float transitionDuration = 1f; // Duration of the camera movement
    public float delayBeforeReturn = 3f; // Delay before returning to Point1

    public Camera mainCamera; // Reference to the main camera
    public float maxFOV = 80f; // Field of view at Point2
    public float minFOV = 60f; // Field of view at Point1

    private bool isMoving = false;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically use the main camera if not assigned
        }
    }

    // Call this function to start the camera movement process
    public void TriggerCameraMovement()
    {
        if (!isMoving)
        {
            StartCoroutine(CameraMovementSequence());
        }
    }

    private System.Collections.IEnumerator CameraMovementSequence()
    {
        isMoving = true;

        // Move camera to Point2 and increase FOV to 80
        LeanTween.move(mainCamera.gameObject, Point2.position, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.value(gameObject, minFOV, maxFOV, transitionDuration).setOnUpdate((float val) => {
            mainCamera.fieldOfView = val;
        }).setEase(LeanTweenType.easeInOutQuad);

        // Wait for the camera to reach Point2
        yield return new WaitForSeconds(transitionDuration);

        // Wait for the specified delay before returning to Point1
        yield return new WaitForSeconds(delayBeforeReturn);

        // Move camera back to Point1 and decrease FOV to 60
        LeanTween.move(mainCamera.gameObject, Point1.position, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.value(gameObject, maxFOV, minFOV, transitionDuration).setOnUpdate((float val) => {
            mainCamera.fieldOfView = val;
        }).setEase(LeanTweenType.easeInOutQuad);

        // Wait for the camera to return to Point1
        yield return new WaitForSeconds(transitionDuration);

        isMoving = false;
    }
}
