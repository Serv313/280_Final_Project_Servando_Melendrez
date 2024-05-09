using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: [Melendrez, Servando]
 * Last Updated: [05/08/2024]
 * [Handles Medigun mechanics, controls, Shooting, Ubercharge, and Line Renderer]
 */

public class Medigun : MonoBehaviour
{
    public Transform cameraHolder; 
    public Transform bulletSpawnPoint;
    public Transform muzzlePoint;

    public GameObject bulletPrefab;
    public LineRenderer healingBeam;  
    public AllyHealth targetHealth;

    public float healRate = 10f;
    public float uberChargeRate = 0.1f;
    public float uberChargeDuration = 8f;
    public float currentUberCharge = 0f;
    public float bulletSpeed = 50f;
    private bool isUberCharged = false;
    

    public Material uberMaterial; // Material used during UberCharge
    private Material originalMaterial; // To store the ally's original material

    private PlayerControls controls;
    private void Start()
    {
        if (targetHealth.GetComponent<Renderer>())
        {
            originalMaterial = targetHealth.GetComponent<Renderer>().material;
        }
    }

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Shoot.performed += _ => Shoot();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            // Call HealTarget to check if we should heal and accumulate UberCharge
            HealTarget();
            UpdateLineRenderer();
        }
        else
        {
            healingBeam.enabled = false;
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && currentUberCharge >= 100)
        {
            StartCoroutine(ActivateUberCharge());
        }
    }

    private void HealTarget()
    {
        //Raycast checks if you are looking at the ally

        RaycastHit hit;
        Vector3 rayOrigin = cameraHolder.position;
        Vector3 rayDirection = cameraHolder.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == targetHealth.gameObject)
            {
                if (!targetHealth.IsFullHealth())
                {
                    float healAmount = healRate * Time.deltaTime;
                    targetHealth.Heal(healAmount);
                }

                // Setting positions and enabling the beam only if the raycast hits the ally
                healingBeam.SetPositions(new Vector3[] { muzzlePoint.position, targetHealth.transform.position });
                healingBeam.enabled = true;

                // Accumulate UberCharge only when aiming at the ally and the beam is active
                currentUberCharge = Mathf.Min(currentUberCharge + uberChargeRate * Time.deltaTime, 100); // Cap at 100%
            }
            else
            {
                healingBeam.enabled = false;
            }
        }
        else
        {
            healingBeam.enabled = false;
        }
    }


    private void Shoot()
    {
        // Shooting mechanic is for depleting ally's health to test Medigun
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPoint.forward * bulletSpeed; // Shoots forward from the spawn point
        Destroy(bullet, 3f); // Destroys Bullet
    }

    private IEnumerator ActivateUberCharge()
    {
        targetHealth.isUberCharged = true;
        targetHealth.GetComponent<Renderer>().material = uberMaterial; // Change material to indicate UberCharge

        yield return new WaitForSeconds(uberChargeDuration);

        targetHealth.isUberCharged = false;
        targetHealth.GetComponent<Renderer>().material = originalMaterial; // Revert to original material
        currentUberCharge = 0;
    }
    private void UpdateLineRenderer()
    {
        if (healingBeam.enabled)
        {
            // Set the start position to the muzzle point's position
            healingBeam.SetPosition(0, muzzlePoint.position);
            // Set the end position to the target's position
            healingBeam.SetPosition(1, targetHealth.transform.position);
        }
    }
}