using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public RectTransform coinPrefab;  // Prefab of the coin UI sprite
    public Transform startPosition;   // Starting position of the animation
    public Transform targetPosition;  // Target position of the animation
    public float animationDuration = 1f;  // Duration of the animation
    public AnimationCurve animationCurve; // Curve for a smooth, custom motion
    public int coinsToIncrease = 1;   // Number of coins to increase per animation

    private int totalCoins = 0;       // Tracks total coins collected

    // Public method to play the coin animation
    public void PlayCoinAnimation()
    {
        // Instantiate a new coin UI element at the starting position
        RectTransform animatedCoin = Instantiate(coinPrefab, startPosition.position, Quaternion.identity, startPosition.parent);

        // Ensure the starting position matches the provided startPosition
        animatedCoin.position = startPosition.position;

        // Animate the coin to the target position with the provided animation curve
        LeanTween.move(animatedCoin.gameObject, targetPosition.position, animationDuration)
            .setEase(animationCurve)  // Apply the custom curve
            .setOnComplete(() =>
            {
                // Destroy the animated coin once it reaches the target
                Destroy(animatedCoin.gameObject);

                // Increase the coin count
                IncreaseCoins(coinsToIncrease);
            });
    }

    // Method to increase and log total coins
    private void IncreaseCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log("Total Coins: " + totalCoins);
        // Update your coin display UI here if needed
    }
}
