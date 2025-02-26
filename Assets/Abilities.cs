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

    private bool isFlying = false;
    private bool isShielded = false;
    private Momo momo;

    private bool isAbilityActive = false; // Flag to track if an ability is currently active
    private int currentAbility = -1;

    private void Start(){
        momo = GetComponent<Momo>();
    }

    public void OnAirAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Air Ability Used");
            SpawnEffect(airEffectPrefab);
            // Here is where our ability logic goes (e.g. momo will float not be able to fall off platforms for a few seconds)
            isFlying = true;
            currentAbility = 0;
        }
    }

    public void OnWaterAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Water Ability Used");
            SpawnEffect(waterEffectPrefab);
             // Here is where our ability logic goes (e.g. momo regenerates 1 life)
             currentAbility = 1;
             GameManager.Instance.Heal();
        }
    }

    public void OnEarthAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Earth Ability Used");
            SpawnEffect(earthEffectPrefab);
            //  // Here is where ability logic goes (e.g. momo will get a shield for 3 seconds)
            isShielded = true;
            currentAbility = 2;
        }
    }

    public void OnFireAbility(InputAction.CallbackContext context)
    {
        if (context.performed && !isAbilityActive)
        {
            Debug.Log("Fire Ability Used");
            SpawnEffect(fireEffectPrefab);
             // Here is where ability logic goes (e.g. momo will shoot a fire explosion out, not sure what this does yet)
             currentAbility = 3;
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

    private void ResetAbility()
    {
        StopAbility();

        if(currentAbility == 0){ //ie) If we are resetting the air ability
            isFlying = false;
            //check if momo is touching the abyss and not on a platform (This is a case where momo used the air ability and didnt make it to a platform in time)
            Collider2D platform = Physics2D.OverlapBox(transform.position, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
            Collider2D abyss = Physics2D.OverlapBox(transform.position, Vector2.zero, 0f, LayerMask.GetMask("Abyss"));
            Collider2D ground = Physics2D.OverlapBox(transform.position, Vector2.zero, 0f, LayerMask.GetMask("Ground")); //non moving ground that momo is safe on
            if (abyss != null && platform == null && ground == null)
            {
                //Call our death function. Currently everything just does 1 damage for now
                momo.Death(transform.position, 1);
            }
            else if(platform !=null){
                //If momo lands on a platfrom at the end of the ability he should ride it
                transform.SetParent(platform.transform);
            }
        }

        if(currentAbility == 2){
            isShielded = false;
        }
        currentAbility = -1;
    }

    public void StopAbility(){
        Destroy(effect);
        isAbilityActive = false;
    }

    public bool GetIsFlying(){
        return isFlying;
    }
    public bool GetIsShielded(){
        return isShielded;
    }
}