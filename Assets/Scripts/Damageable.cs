using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: [Melendrez, Servando]
 * Last Updated: [05/08/2024]
 * [Handles Damage against Ally]
 */

public class Damageable : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        GetComponent<AllyHealth>().TakeDamage(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")) 
        {
            TakeDamage(10); 
            Destroy(other.gameObject); // Destroy the bullet on impact
        }
    }
}
