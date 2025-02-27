using UnityEngine;

public class ParticlesCrystal : MonoBehaviour
{
    [SerializeField] private Transform boss; // Azula
    private ParticleSystem healingParticles;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    private void Start()
    {
        //This is code to make the particles move towards azula
        healingParticles = GetComponent<ParticleSystem>();
        velocityModule = healingParticles.velocityOverLifetime;
        velocityModule.enabled = true;
    }

    private void Update()
    {
        if (boss != null)
        {
            Vector3 directionToBoss = (boss.position - transform.position).normalized;
            velocityModule.x = directionToBoss.x;
            velocityModule.y = directionToBoss.y;
            velocityModule.z = directionToBoss.z;
        }
    }
}
