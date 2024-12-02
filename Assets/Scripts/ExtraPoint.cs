using UnityEngine;

public class ExtraPoint : MonoBehaviour
{
    public GameObject tenXObject; // Reference to the 10X GameObject
    public float moveDistance = 0.5f; // Distance the object moves up and down
    public float moveDuration = 1f; // Duration for one complete up or down movement

    private void Start()
    {
        if (tenXObject != null)
        {
            StartTween();
        }
        else
        {
            Debug.LogWarning("10X Object is not assigned!");
        }
    }

    private void StartTween()
    {
        Vector3 originalPosition = tenXObject.transform.position;

        // Tween to move up
        LeanTween.moveY(tenXObject, originalPosition.y + moveDistance, moveDuration)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong(); // Loops back and forth between up and down
    }
}
