using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.Events; // For UnityEvent
using TMPro; // For TextMeshPro
using UnityEngine.UI; // For Slider
using System.Collections; // For IEnumerator
using SkillzSDK; // For Skillz API

public class RingSpawner : MonoBehaviour
{
    public GameObject ringPrefab; // Assign your ring prefab here
    public Transform spawnPoint; // Where to spawn the ring
    public float spawnDelay = 3f; // Delay before spawning a new ring (in seconds)
    public int maxRings = 6; // Max number of rings allowed
    public float transitionDuration = 1f; // Duration for moving the ring from holder to spawnPoint
    
    private int ringCount = 0; // To track the number of rings thrown

    public UnityEvent OnGameOver; // UnityEvent that will be triggered when the game is over

    public TextMeshProUGUI timerText; // Text for displaying the timer
    public TextMeshProUGUI earnedScoreText; // Text for displaying current earned score
    public TextMeshProUGUI timeLeftText; // Text for displaying the remaining time after rings finish
    public TextMeshProUGUI finalScoreText; // Text to display the final score
    public ScoreManager scoreManager; // Reference to the ScoreManager

    private float timer = 40.0f; // Initial timer value (40 seconds)
    private bool gameOverTriggered = false;
    private int finalScore; // Store the final score

    private void OnEnable()
    {
        ThrowAndResetRing.OnRingThrown += StartSpawnTimer;
    }

    private void OnDisable()
    {
        ThrowAndResetRing.OnRingThrown -= StartSpawnTimer;
    }

    private void Start()
    {
        SpawnNewRing(); // Ensure that a ring is spawned initially
        StartCoroutine(TimerCountdown()); // Start the timer countdown
    }

    private void StartSpawnTimer()
    {
        if (ringCount < maxRings)
        {
            Invoke(nameof(SpawnNewRing), spawnDelay);
        }
        else
        {
            StartCoroutine(DelayedGameOver());
        }
    }

    private void SpawnNewRing()
    {
        if (ringCount >= maxRings) return;

        // Spawn a new ring at the spawn point
        GameObject newRing = Instantiate(ringPrefab, spawnPoint.position, Quaternion.identity);
        
        // Optional: If you want the ring to be thrown with an initial force or any other specific behavior, you can set that here
        Rigidbody rb = newRing.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Move the ring to the spawn point with a transition animation
        LeanTween.move(newRing, spawnPoint.position, transitionDuration)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnStart(() =>
            {
                LeanTween.rotate(newRing, Vector3.zero, transitionDuration).setEase(LeanTweenType.easeInOutSine);
            });

        ringCount++;
    }

    private IEnumerator TimerCountdown()
    {
        while (timer > 0 && ringCount < maxRings)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Round(timer).ToString() + "s";
            earnedScoreText.text = "" + scoreManager.score.ToString();
            yield return null;
        }

        if (!gameOverTriggered)
        {
            StartCoroutine(DelayedGameOver());
        }
    }

    private IEnumerator DelayedGameOver()
    {
        gameOverTriggered = true;
        float timeLeft = Mathf.Round(timer);

        // Display the remaining time
        timeLeftText.text = "" + timeLeft.ToString() + "s";

        // Calculate the final score
        finalScore = Mathf.RoundToInt(scoreManager.score * timeLeft);

        // Display the final score
        finalScoreText.text = finalScore.ToString();

        yield return new WaitForSeconds(4.0f); // Wait for 4 seconds

        //SubmitFinalScoreToSkillz(); // Submit the final score to Skillz

        OnGameOver?.Invoke(); // Trigger game over
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Public method to get the final score
    public int GetFinalScore()
    {
        return finalScore; // Return the final score
    }

    // Public method to submit the final score to Skillz
    public void SubmitFinalScoreToSkillz()
    {
        Debug.Log("Submitting score to Skillz: " + finalScore);
        SubmitScore(finalScore); // Submit the score
    }

    // Private method to submit the score to Skillz
    private void SubmitScore(int score)
    {
        SkillzCrossPlatform.SubmitScore(score, OnScoreSubmittedSuccessfully, OnScoreSubmissionFailed);
    }

    // Callback when the score is successfully submitted
    private void OnScoreSubmittedSuccessfully()
    {
        Debug.Log("Score submission successful!");
        SkillzCrossPlatform.ReturnToSkillz(); // Return to Skillz after score submission
    }

    // Callback when the score submission fails
    private void OnScoreSubmissionFailed(string reason)
    {
        Debug.LogWarning("Score submission failed: " + reason);
        SkillzCrossPlatform.DisplayTournamentResultsWithScore(finalScore); // Display results with the score
    }
}
