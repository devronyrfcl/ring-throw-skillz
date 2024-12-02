using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.Events; // For UnityEvent
using TMPro; // For TextMeshPro
using System.Collections; // For IEnumerator

public class RingSpawner : MonoBehaviour
{
    public GameObject ringPrefab; // Assign your ring prefab here
    public Transform spawnPoint; // Where to spawn the ring
    public Transform ringHolder; // Parent object for holding the rings
    public float spawnDelay = 3f; // Delay before spawning a new ring (in seconds)
    public int maxRings = 6; // Max number of rings allowed
    public float ringSpacing = 0.5f; // Space between held rings (controlled via Inspector)
    public float transitionDuration = 1f; // Duration for moving the ring from holder to spawnPoint

    private int ringCount = 0; // To track the number of rings thrown
    private GameObject[] heldRings; // Array to store held rings

    public UnityEvent OnGameOver; // UnityEvent that will be triggered when the game is over

    public TextMeshProUGUI timerText; // Text for displaying the timer
    public TextMeshProUGUI earnedScoreText; // Text for displaying current earned score
    public TextMeshProUGUI timeLeftText; // Text for displaying the remaining time after rings finish
    public TextMeshProUGUI finalScoreText; // Text to display the final score
    public ScoreManager scoreManager; // Reference to the ScoreManager

    private float timer = 40.0f; // Initial timer value (40 seconds)
    private bool gameOverTriggered = false;

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
        InitializeHolder();
        SpawnNewRing(); // Ensure that a ring is spawned initially
        StartCoroutine(TimerCountdown()); // Start the timer countdown
    }

    private void InitializeHolder()
    {
        heldRings = new GameObject[maxRings];
        for (int i = 0; i < maxRings; i++)
        {
            Vector3 position = ringHolder.position + new Vector3(i * ringSpacing, 0, 0);
            heldRings[i] = Instantiate(ringPrefab, position, Quaternion.identity, ringHolder);

            Rigidbody rb = heldRings[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            heldRings[i].transform.rotation = ringHolder.rotation;
        }
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

        GameObject nextRing = heldRings[ringCount];
        nextRing.transform.SetParent(null);

        LeanTween.move(nextRing, spawnPoint.position, transitionDuration)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnStart(() =>
            {
                LeanTween.rotate(nextRing, Vector3.zero, transitionDuration).setEase(LeanTweenType.easeInOutSine);
            });

        Rigidbody rb = nextRing.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        nextRing.SetActive(true);
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
        int finalScore = Mathf.RoundToInt(scoreManager.score * timeLeft);

        // Display the final score
        finalScoreText.text = finalScore.ToString();

        yield return new WaitForSeconds(4.0f); // Wait for 3.5 seconds

        OnGameOver?.Invoke(); // Trigger game over
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
