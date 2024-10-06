using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeTrigger : MonoBehaviour
{
    private Rigidbody[] allRigidbodies;  // Array for all Rigidbody components in the scene
    private bool earthquakeTriggered = false;

    void Start()
    {
        // Find all objects with Rigidbody components in the scene
        allRigidbodies = FindObjectsOfType<Rigidbody>();

        // Disable physics (set kinematic) for all objects at the start
        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.isKinematic = true;  // Prevent objects from being affected by physics initially
        }
    }

    void Update()
    {
        // Wait for the player to press 'E' to trigger the earthquake
        if (Input.GetKeyDown(KeyCode.E) && !earthquakeTriggered)
        {
            StartEarthquake();
        }
    }

    void StartEarthquake()
    {
        earthquakeTriggered = true;

        // Enable physics (set kinematic to false) for all objects when earthquake starts
        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.isKinematic = false;  // Enable physics for all objects
        }
    }
}
