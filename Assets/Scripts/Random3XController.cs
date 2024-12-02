using UnityEngine;
using System.Collections;

public class Random3XController : MonoBehaviour
{
    public TargetManager[] targetManagers;  // Array to hold references to multiple TargetManager scripts
    public float minActivationTime = 2f;   // Minimum time between activations
    public float maxActivationTime = 5f;   // Maximum time between activations
    public float minDeactivationTime = 3f; // Minimum time for deactivation
    public float maxDeactivationTime = 6f; // Maximum time for deactivation

    private Coroutine activationCoroutine; // To keep track of the coroutine

    private void Start()
    {
        // Start the coroutine to control the 3X object activation and deactivation
        activationCoroutine = StartCoroutine(ControlRandom3XActivation());
    }

    private IEnumerator ControlRandom3XActivation()
    {
        while (true)
        {
            // Pick a random TargetManager from the array to control its 3X object
            TargetManager selectedTargetManager = targetManagers[Random.Range(0, targetManagers.Length)];

            // Randomly activate the 3X object
            selectedTargetManager.is3XActive = true;
            selectedTargetManager.threeXObject.SetActive(true);

            // Wait for a random time before deactivating it
            yield return new WaitForSeconds(Random.Range(minActivationTime, maxActivationTime));

            // Deactivate the 3X object
            selectedTargetManager.is3XActive = false;
            selectedTargetManager.threeXObject.SetActive(false);

            // Wait for a random time before activating it again
            yield return new WaitForSeconds(Random.Range(minDeactivationTime, maxDeactivationTime));
        }
    }

    public void StopControlRandom3XActivation()
    {
        if (activationCoroutine != null)
        {
            StopCoroutine(activationCoroutine); // Stop the coroutine
            activationCoroutine = null; // Clear the reference

            // Ensure all 3X objects are deactivated
            foreach (var targetManager in targetManagers)
            {
                targetManager.is3XActive = false;
                targetManager.threeXObject.SetActive(false);
            }
        }
    }
}
