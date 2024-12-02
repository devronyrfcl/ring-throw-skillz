using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management

public class GameStartManager : MonoBehaviour
{
    // This function will be called when the game starts
    void Start()
    {
        
    }

    // Method to switch to Scene 1
    public void LoadScene1()
    {
        // Make sure the scene at index 1 exists
        SceneManager.LoadScene(1);  // Load Scene 1 based on index or name
    }
}
