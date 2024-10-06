using System.Collections;
using UnityEngine;

public class EarthquakeSimulator : MonoBehaviour
{
    public GameObject[] ObjectsToShake;  // Array for multiple objects
    private bool shaking = false;

    public float initialIntensity = 0.2f; // Lower starting intensity of the shake
    public float maxIntensity = 1.0f;     // Maximum shake intensity (same as your original script)
    public float shakeDuration = 10f;     // Total duration of the shake
    public float rampUpTime = 3f;         // Time to reach max intensity
    public float rampDownTime = 3f;       // Time to decrease from max intensity
    private float currentShakeIntensity = 0f;

    void Start()
    {
        // Keep objects kinematic (not affected by physics) at the start
        for (int i = 0; i < ObjectsToShake.Length; i++)
        {
            Rigidbody rb = ObjectsToShake[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Ensure physics is disabled until the earthquake starts
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !shaking) // Press E to start the earthquake
        {
            StartCoroutine(StartEarthquake());
        }
    }

    IEnumerator StartEarthquake()
    {
        shaking = true;

        // Enable physics (disable kinematic) at the start of the earthquake
        for (int i = 0; i < ObjectsToShake.Length; i++)
        {
            Rigidbody rb = ObjectsToShake[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Physics will take effect
            }
        }

        // Ramp-up phase: gradually increase intensity (up to your original max)
        for (float t = 0; t < rampUpTime; t += Time.deltaTime)
        {
            currentShakeIntensity = Mathf.Lerp(initialIntensity, maxIntensity, t / rampUpTime);
            ApplyShakeToObjects(currentShakeIntensity);
            yield return null;
        }

        // Hold max intensity for the middle part of the earthquake
        for (float t = 0; t < shakeDuration - (rampUpTime + rampDownTime); t += Time.deltaTime)
        {
            currentShakeIntensity = maxIntensity;
            ApplyShakeToObjects(currentShakeIntensity);
            yield return null;
        }

        // Ramp-down phase: gradually decrease intensity back to lower values
        for (float t = 0; t < rampDownTime; t += Time.deltaTime)
        {
            currentShakeIntensity = Mathf.Lerp(maxIntensity, 0f, t / rampDownTime);
            ApplyShakeToObjects(currentShakeIntensity);
            yield return null;
        }

        shaking = false;
    }

    void ApplyShakeToObjects(float intensity)
    {
        // Loop through all objects in the array and trigger the shake
        for (int i = 0; i < ObjectsToShake.Length; i++)
        {
            GameObject obj = ObjectsToShake[i];
            if (obj != null)
            {
                // Start shaking each object with varying intensity
                shakeGameObject(obj, intensity);

                // Enable physics for falling objects if they have a Rigidbody
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false; // Ensure physics is disabled until the earthquake starts
                }
            }
            else
            {
                Debug.LogWarning("Object at index " + i + " is not assigned.");
            }
        }
    }

    void shakeGameObject(GameObject objectToShake, float intensity)
    {
        Transform objTransform = objectToShake.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        const float speed = 0.01f;  // Shake speed (position)
        const float angleRot = 0.05f;  // Shake intensity (rotation)

        Rigidbody rb = objectToShake.GetComponent<Rigidbody>();

        // If object does not have a Rigidbody, apply position shake
        if (rb == null)
        {
            objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * speed * intensity * 2f;
        }

        // Apply rotation shake regardless of Rigidbody
        objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot * intensity * 5f, angleRot * intensity * 5f), Vector3.one);
    }
}
