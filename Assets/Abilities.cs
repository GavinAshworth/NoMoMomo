using UnityEngine;
using UnityEngine.InputSystem;

// Class for our elemental abilities
public class Abilities : MonoBehaviour
{
    //our elemental animations prefabs
    [Header("Ability Prefabs")]
    [SerializeField] private GameObject airEffectPrefab;
    [SerializeField] private GameObject waterEffectPrefab;
    [SerializeField] private GameObject earthEffectPrefab;
    [SerializeField] private GameObject fireEffectPrefab;

    private GameObject effect;

    private bool isAbilityActive = false; // Flag to track if an ability is currently active


    public void OnAirAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Air Ability Used");
            SpawnEffect(airEffectPrefab);
            // Here is where our ability logic goes (e.g. momo will float not be able to fall off platforms for a few seconds)
        }
    }

    public void OnWaterAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Water Ability Used");
            SpawnEffect(waterEffectPrefab);
             // Here is where our ability logic goes (e.g. momo regenerates 1 life)
        }
    }

    public void OnEarthAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Earth Ability Used");
            SpawnEffect(earthEffectPrefab);
            //  // Here is where ability logic goes (e.g. momo will get a shield for 3 seconds)
        }
    }

    public void OnFireAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Fire Ability Used");
            SpawnEffect(fireEffectPrefab);
             // Here is where ability logic goes (e.g. momo will shoot a fire explosion out, not sure what this does yet)
        }
    }

    private void SpawnEffect(GameObject effectPrefab)
    {
        if (effectPrefab == null)
        {
            Debug.LogError("Effect prefab is not assigned!");
            return;
        }

        // Set the ability as active
        isAbilityActive = true;

        // Instantiate the effect prefab as a child of Momo so we can make it follow him
        effect = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform);

        // Get the Animator component of the effect
        Animator effectAnimator = effect.GetComponent<Animator>();
        if (effectAnimator != null)
        {
            // Play the animation
            effectAnimator.Play(0, 0, 0f);
        }

        // Destroy the effect after the animation finishes
        float animationLength = effectAnimator.GetCurrentAnimatorStateInfo(0).length;
        // Reset the ability flag after the animation finishes
        Invoke(nameof(ResetAbility), animationLength);
    }

    public void ResetAbility()
    {
        Destroy(effect);
        isAbilityActive = false;
    }
}