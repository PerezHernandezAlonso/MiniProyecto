using System;
using System.Collections;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    private bool canHit = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisionando");
        if(collision.transform.TryGetComponent(out PlayerHealth playerHealth) && canHit)
        {
            playerHealth.TakeDamage(10);
            canHit = false;
            ReactivateHit();
           
        }
    }

    IEnumerator ReactivateHit()
    {
        yield return new WaitForSeconds(2f);
        canHit = true;
    }
}
