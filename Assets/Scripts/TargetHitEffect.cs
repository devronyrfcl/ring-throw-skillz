using UnityEngine;

public class TargetHitEffect : MonoBehaviour
{
    public ParticleSystem[] particleEffects; // Array to hold particle systems

    private void Start()
    {
        // Stop all particles on start
        foreach (ParticleSystem particle in particleEffects)
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }
    }

    // Public method to trigger all particles
    public void TargetHitEffectOn()
    {
        foreach (ParticleSystem particle in particleEffects)
        {
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }
    }
}
