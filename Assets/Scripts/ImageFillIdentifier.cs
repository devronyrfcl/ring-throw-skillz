using UnityEngine;
using UnityEngine.UI; // For using UI Image components

public class ImageFillIdentifier : MonoBehaviour
{
    public Image swipeFillImage; // Reference to the Image component

    void Start()
    {
        // Attempt to find the Image component in the scene if not assigned in inspector
        if (swipeFillImage == null)
        {
            swipeFillImage = FindObjectOfType<Image>();
        }
    }

    // Returns the found Image
    public Image GetFillImage()
    {
        return swipeFillImage;
    }
}
