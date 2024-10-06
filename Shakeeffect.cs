using System.Collections;
using UnityEngine;

public class EarthquakeSimulator : MonoBehaviour
{
    public GameObject[] ObjectsToShake;  // Array for multiple objects
    private bool shaking = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !shaking)
        {
            // Loop through all objects in the array
            for (int i = 0; i < ObjectsToShake.Length; i++)
            {
                GameObject obj = ObjectsToShake[i];
                if (obj != null)
                {
                    // Trigger shake for each object
                    shakeGameObject(obj, 10f, 6f, false); // Shake duration: 10 seconds, decrease at 6 seconds
                }
                else
                {
                    // Log a warning if an object is not assigned
                    Debug.LogWarning("Object at index " + i + " is not assigned.");
                }
            }
        }
    }

    void shakeGameObject(GameObject objectToShake, float shakeDuration, float decreasePoint, bool objectIs2D = false)
    {
        shaking = true;
        StartCoroutine(shakeGameObjectCOR(objectToShake, shakeDuration, decreasePoint, objectIs2D));
    }

    IEnumerator shakeGameObjectCOR(GameObject objectToShake, float totalShakeDuration, float decreasePoint, bool objectIs2D = false)
    {
        if (decreasePoint >= totalShakeDuration)
        {
            Debug.LogError("decreasePoint must be less than totalShakeDuration...Exiting");
            yield break; // Exit if decreasePoint is invalid
        }

        // Get the object's transform
        Transform objTransform = objectToShake.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        float counter = 0f;
        const float speed = 0.5f;  // Shake speed (position)
        const float angleRot = 2f;  // Shake intensity (rotation)

        // Start shaking the object
        while (counter < totalShakeDuration)
        {
            counter += Time.deltaTime;
            float decreaseSpeed;

            // Determine shake intensity based on whether we passed the decrease point
            if (counter < decreasePoint)
            {
                decreaseSpeed = speed; // Full intensity before decrease point
            }
            else
            {
                // Gradually decrease shake intensity after the decrease point
                decreaseSpeed = Mathf.Lerp(speed, 0f, (counter - decreasePoint) / (totalShakeDuration - decreasePoint));
            }

            // Apply random position and rotation shakes
            objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed * 2f; // Stronger position shake
            objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot * 5f, angleRot * 5f), Vector3.one); // Stronger rotation shake

            yield return null; // Wait for the next frame
        }

        // Reset position and rotation after shaking
        objTransform.position = defaultPos;
        objTransform.rotation = defaultRot;
        shaking = false;  // Reset shaking flag
    }
}
