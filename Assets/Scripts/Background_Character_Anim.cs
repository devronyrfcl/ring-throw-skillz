using UnityEngine;

public class Background_Character_Anim : MonoBehaviour
{
    public Animator characterAnimator; // Reference to the Animator component
    public string animationTriggerName = "DanceTrigger"; // Name of the animation trigger
    public string animationStateName = "Dance"; // Name of the animation state

    private bool isAnimationPlaying = false;

    void Update()
    {
        if (characterAnimator != null && isAnimationPlaying)
        {
            // Check if the animation is still playing
            AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(animationStateName) && stateInfo.normalizedTime >= 1f)
            {
                // Animation completed
                characterAnimator.ResetTrigger(animationTriggerName);
                isAnimationPlaying = false;
            }
        }
    }

    // Public method to trigger a specific animation
    public void PlayAnimation(string animationTrigger)
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger(animationTrigger);
            isAnimationPlaying = true;
        }
        else
        {
            Debug.LogWarning("Animator is not assigned.");
        }
    }
}
