using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public ScoreManager scoreManager;  // Reference to the ScoreManager script
    public CoinAnimation coinAnimation; // Reference to the CoinAnimation script
    public int scoreIncreaseAmount = 10;  // Score to add when hit by a "Ring"

    //public AudioSource audioSource;  // Reference to the AudioSource component
    //public AudioClip scoreSound;     // Sound to play when score increases

    public GameObject particlePrefab;  // Particle system prefab to spawn when score increases
    public TargetHitEffect targetHitEffect; // Reference to the TargetHitEffect script
    public string animationTriggerName = "DanceTrigger"; // The name of the animation trigger to play

    public GameObject threeXObject; // Reference to the 3X object (multiplier)
    public bool is3XActive = false;  // Flag to control if 3X multiplier is active

    public RandomTargetPositioner randomTargetPositioner; // Reference to the RandomTargetPositioner script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Ring"
        if (other.CompareTag("Ring"))
        {
            // Increase the score
            int finalScoreIncrease = scoreIncreaseAmount;

            // If the 3X multiplier is active, multiply the score increase by 3
            if (is3XActive)
            {
                finalScoreIncrease *= 5;
            }

            if (scoreManager != null)
            {
                scoreManager.IncreaseScore(finalScoreIncrease);
            }

            // Play coin animation
            if (coinAnimation != null)
            {
                coinAnimation.PlayCoinAnimation();
            }

            // Play score increase sound
            /*if (audioSource != null && scoreSound != null)
            {
                audioSource.PlayOneShot(scoreSound);  // Play the sound when the score increases
            }*/
            AudioManager.Instance.PlaySFX("ring_hit");

            // Spawn particle effect
            if (particlePrefab != null)
            {
                GameObject spawnedParticle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                Destroy(spawnedParticle, 2f);  // Destroy the spawned particle after 2 seconds
            }

            // Trigger the TargetHitEffect
            if (targetHitEffect != null)
            {
                targetHitEffect.TargetHitEffectOn();
            }

            // Call the function to move targets after 2.5 seconds
            Invoke("MoveTargets", 2.5f);
        }
    }

    // Function to call MoveTargetsToRandomPositions
    private void MoveTargets()
    {
        if (randomTargetPositioner != null)
        {
            randomTargetPositioner.MoveTargetsToRandomPositions(); // Call the random position function
        }
    }
}
