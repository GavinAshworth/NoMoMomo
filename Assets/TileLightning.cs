using UnityEngine;

public class TileLightning : MonoBehaviour
{
    private Collider2D lightningCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightningCollider = gameObject.GetComponent<Collider2D>();
        lightningCollider.enabled = false;
    }

    private void Damage(){
        Debug.Log("My name jeff");
        lightningCollider.enabled = true;
    }
    private void Reset(){
        Debug.Log("My name jeff2");
        lightningCollider.enabled = false;
    }
}
