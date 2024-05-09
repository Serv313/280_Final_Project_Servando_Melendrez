using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
 * Author: [Melendrez, Servando]
 * Last Updated: [05/08/2024]
 * [Handles UI Components, Health and Ubercharge Displays]
 */

public class HealthDisplay : MonoBehaviour
{
    public AllyHealth allyHealth; 
    public Medigun medigun; 

    private TextMeshProUGUI healthText;

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        if (allyHealth != null && medigun != null)
        {
            healthText.text = $"Health: {allyHealth.currentHealth}\nUberCharge: {medigun.currentUberCharge.ToString("F2")}%"; // Update health and UberCharge display
        }
    }
}
