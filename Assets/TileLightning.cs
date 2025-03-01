using UnityEngine;

public class TileLightning : MonoBehaviour
{
    private Collider2D lightningCollider;
    private Azula azula;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightningCollider = gameObject.GetComponent<Collider2D>();
        lightningCollider.enabled = false;
        azula = GameObject.FindWithTag("Azula").GetComponent<Azula>();
    }

    private void Damage(){
        lightningCollider.enabled = true;
    }
    private void Reset(){
        lightningCollider.enabled = false;
    }

    private void ResetAzulaTiles(){
        //This is because there is a frame perfect bug that prevents the lightning tiles from being deactivated and I dont want to find another solution
        azula.resetLightningTiles();
    }
}
