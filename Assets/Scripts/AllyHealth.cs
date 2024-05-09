using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: [Melendrez, Servando]
 * Last Updated: [05/08/2024]
 * [Handles Health mechanics and variables for the Ally]
 */

public class AllyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isUberCharged = false; // UberCharge status

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    public void TakeDamage(float amount)
    {
        if (!isUberCharged) // Only take damage if not UberCharged
        {
            currentHealth = Mathf.Max(currentHealth - amount, 0);
        }
    }
    public bool IsFullHealth()
    {
        return currentHealth == maxHealth;
    }
}
