using UnityEngine;

public class NeonAnimation : MonoBehaviour
{
    public GameObject[] objects;  // Array of objects to animate
    public Material[] materials; // Array of materials to assign
    public float animationSpeed = 0.5f; // Time between material changes
    public GameObject ferrisWheel; // Ferris wheel object to rotate
    public float rotationSpeed = 50f; // Speed at which the Ferris wheel rotates (in degrees per second)

    private float timer = 0f; // Timer to control material animation speed

    void Update()
    {
        if (objects.Length == 0 || materials.Length == 0 || ferrisWheel == null)
            return;

        // Rotate the Ferris wheel around the Z-axis
        ferrisWheel.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Handle material animation
        timer += Time.deltaTime;

        // When timer exceeds animationSpeed, assign a random material to a random object
        if (timer >= animationSpeed)
        {
            timer = 0f; // Reset the timer

            // Select a random object
            GameObject randomObject = objects[Random.Range(0, objects.Length)];
            Renderer renderer = randomObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                // Assign a random material to the selected object
                Material randomMaterial = materials[Random.Range(0, materials.Length)];
                renderer.material = randomMaterial;
            }
        }
    }
}
