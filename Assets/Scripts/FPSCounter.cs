using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // TextMeshProUGUI to display FPS
    private float deltaTime = 0.0f;

    private void Start()
    {
        LockFPS(60); // Lock the frame rate to 60 FPS
    }

    private void Update()
    {
        // Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Update FPS display
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }

    /// <summary>
    /// Locks the game's frame rate to the specified value.
    /// </summary>
    /// <param name="targetFPS">The target FPS to lock the game to.</param>
    public void LockFPS(int targetFPS)
    {
        Application.targetFrameRate = targetFPS;
    }
}
