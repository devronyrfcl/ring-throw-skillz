using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public Image backgroundImage; // The rotating background
    public RectTransform upperFrame; // UI Object for the upper frame
    public RectTransform frame; // UI Object for the main frame
    public TextMeshProUGUI timeSavedValue; // Time saved value text
    public TextMeshProUGUI timeSavedTitle; // Time saved title text
    public TextMeshProUGUI earnedScoreValue; // Earned score value text
    public TextMeshProUGUI earnedScoreTitle; // Earned score title text
    public RectTransform trophyTextFrame; // UI Object for trophy text frame
    public RectTransform tapButton; // UI Object for tap button

    public float backgroundRotateSpeed = 10f; // Speed for rotating background

    private void FixedUpdate()
    {
        RotateBackground();
    }

    private void RotateBackground()
    {
        // Continuously rotate the background in the Z axis
        backgroundImage.rectTransform.Rotate(0, 0, backgroundRotateSpeed * Time.fixedDeltaTime);
    }

    public void AnimateUI()
    {
        // Animate Upper Frame
        upperFrame.localScale = Vector3.zero; // Default size
        LeanTween.scale(upperFrame, Vector3.one, 0.8f)
            .setEase(LeanTweenType.easeOutBounce);

        // Animate Main Frame
        frame.sizeDelta = new Vector2(frame.sizeDelta.x, 0); // Default height
        frame.anchoredPosition = new Vector2(frame.anchoredPosition.x, -10); // Default Y position
        LeanTween.value(frame.gameObject, 0, 730, 0.8f)
            .setEase(LeanTweenType.easeOutBounce)
            .setOnUpdate((float value) => frame.sizeDelta = new Vector2(frame.sizeDelta.x, value));

        LeanTween.moveY(frame, -369.0624f, 0.8f)
            .setEase(LeanTweenType.easeOutBounce)
            .setDelay(0.2f);

        // Animate Texts
        float delay = 0.4f;
        AnimateText(timeSavedValue, delay);
        AnimateText(timeSavedTitle, delay + 0.2f);
        AnimateText(earnedScoreValue, delay + 0.4f);
        AnimateText(earnedScoreTitle, delay + 0.6f);

        // Animate Trophy Text Frame
        trophyTextFrame.anchoredPosition = new Vector2(1000, trophyTextFrame.anchoredPosition.y); // Default X position
        LeanTween.moveX(trophyTextFrame, 35, 0.8f)
            .setEase(LeanTweenType.easeOutElastic)
            .setDelay(1.2f);

        // Animate Tap Button
        tapButton.localScale = Vector3.zero; // Default size
        tapButton.anchoredPosition = new Vector2(tapButton.anchoredPosition.x, -1500); // Default Y position
        LeanTween.moveY(tapButton, -777, 0.8f)
            .setEase(LeanTweenType.easeOutElastic)
            .setDelay(1.4f)
            .setOnComplete(() =>
            {
                LeanTween.scale(tapButton, Vector3.one, 0.6f)
                    .setEase(LeanTweenType.easeOutBounce);
            });
    }

    private void AnimateText(TextMeshProUGUI text, float delay)
    {
        text.rectTransform.anchoredPosition = new Vector2(-1000, text.rectTransform.anchoredPosition.y); // Default X position
        LeanTween.moveX(text.rectTransform, 0, 0.8f)
            .setEase(LeanTweenType.easeOutElastic)
            .setDelay(delay);
    }
}
