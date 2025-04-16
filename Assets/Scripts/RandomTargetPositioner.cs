using UnityEngine;
using DG.Tweening;

public class RandomTargetPositioner : MonoBehaviour
{
    public GameObject[] TargetObjects;          // Array to hold the target objects
    public Transform[] TargetObjectsPosition;  // Array to hold the target positions

    public float transitionDuration = 1f;      // Duration of the position change animation

    // Function to randomly position TargetObjects to TargetObjectsPosition
    public void MoveTargetsToRandomPositions()
    {
        if (TargetObjects.Length != 6 || TargetObjectsPosition.Length != 6)
        {
            Debug.LogError("Please ensure there are exactly 6 TargetObjects and 6 TargetObjectsPosition.");
            return;
        }

        // Create an array of available positions
        Transform[] availablePositions = (Transform[])TargetObjectsPosition.Clone();

        // Shuffle the available positions using Skillz RNG for fairness
        ShuffleArrayUsingSkillzRNG(availablePositions);

        // Assign positions randomly to each TargetObject
        for (int i = 0; i < TargetObjects.Length; i++)
        {
            GameObject targetObject = TargetObjects[i];
            Transform targetPosition = availablePositions[i];

            // Move the object smoothly to its new position using DoTween
            targetObject.transform.DOMove(targetPosition.position, transitionDuration).SetEase(Ease.InOutQuad);
        }
    }

    // Helper function to shuffle an array using Skillz RNG
    private void ShuffleArrayUsingSkillzRNG(Transform[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = SkillzCrossPlatform.Random.Range(0, array.Length); // Use Skillz RNG for random index
            // Swap the elements
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
