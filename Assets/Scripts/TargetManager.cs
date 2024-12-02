using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public ScoreManager scoreManager;  // Reference to the ScoreManager script
    public CoinAnimation coinAnimation; // Reference to the CoinAnimation script
    public int scoreIncreaseAmount = 10;  // Score to add when hit by a "Ring"

    public AudioSource audioSource;  // Reference to the AudioSource component
    public AudioClip scoreSound;     // Sound to play when score increases

    public GameObject particlePrefab;  // Particle system prefab to spawn when score increases
    public TargetHitEffect targetHitEffect; // Reference to the TargetHitEffect script
    public Background_Character_Anim backgroundCharacterAnim; // Reference to the Background_Character_Anim script
    public Background_Character_Anim backgroundCharacterAnim2; // Reference to the Background_Character_Anim script
    public string animationTriggerName = "DanceTrigger"; // The name of the animation trigger to play

    public GameObject threeXObject; // Reference to the 3X object (multiplier)
    public bool is3XActive = false;  // Flag to control if 3X multiplier is active

    private bool isTriggered = false;  // Flag to ensure the target is triggered only once

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Ring" and the target is not already triggered
        if (!isTriggered && other.CompareTag("Ring"))
        {
            isTriggered = true;  // Mark as triggered

            // Increase the score
            int finalScoreIncrease = scoreIncreaseAmount;

            // If the 3X multiplier is active, multiply the score increase by 3
            if (is3XActive)
            {
                finalScoreIncrease *= 3;
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
            if (audioSource != null && scoreSound != null)
            {
                audioSource.PlayOneShot(scoreSound);  // Play the sound when the score increases
            }

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

            // Trigger the background character animation
            if (backgroundCharacterAnim != null)
            {
                backgroundCharacterAnim.PlayAnimation(animationTriggerName);
            }
            // Trigger the background character animation
            if (backgroundCharacterAnim2 != null)
            {
                backgroundCharacterAnim2.PlayAnimation(animationTriggerName);
            }
        }
    }
}
