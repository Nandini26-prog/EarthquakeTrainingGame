using System.Collections;
using UnityEngine;

public class BooksShake : MonoBehaviour
{
    public Rigidbody[] books; // Array to hold the books on the rack
    public float earthquakeDuration = 5f; // Duration of the earthquake
    public float shakeIntensity = 0.2f; // How much force to apply to shake books
    public float forceMultiplier = 1f; // Additional force applied when shaking

    private bool isEarthquakeHappening = false;

    void Start()
    {
        // Automatically find all book Rigidbody components and assign them to the array
        FindAndAssignBooks();
    }

    void Update()
    {
        // Trigger earthquake when the "E" key is pressed
        if (Input.GetKeyDown(KeyCode.E) && !isEarthquakeHappening)
        {
            StartCoroutine(TriggerEarthquake());
        }
    }

    void FindAndAssignBooks()
    {
        // Find all objects in the scene with names starting with "book" and have a Rigidbody component
        GameObject[] allBooks = GameObject.FindObjectsOfType<GameObject>();
        var bookList = new System.Collections.Generic.List<Rigidbody>();

        foreach (GameObject obj in allBooks)
        {
            if (obj.name.StartsWith("book")) // Adjust this condition if needed
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    bookList.Add(rb);
                }
            }
        }

        books = bookList.ToArray(); // Assign the filtered Rigidbodies to the books array
    }

    IEnumerator TriggerEarthquake()
    {
        isEarthquakeHappening = true;
        float elapsed = 0f;

        while (elapsed < earthquakeDuration)
        {
            foreach (Rigidbody book in books)
            {
                if (book != null)
                {
                    // Apply random small forces to the books to simulate shaking
                    Vector3 randomForce = new Vector3(
                        Random.Range(-shakeIntensity, shakeIntensity),
                        0,
                        Random.Range(-shakeIntensity, shakeIntensity)
                    );
                    
                    Vector3 randomTorque = new Vector3(
                        Random.Range(-shakeIntensity, shakeIntensity),
                        Random.Range(-shakeIntensity, shakeIntensity),
                        Random.Range(-shakeIntensity, shakeIntensity)
                    );

                    // Add force and torque to each book
                    book.AddForce(randomForce * forceMultiplier, ForceMode.Impulse);
                    book.AddTorque(randomTorque * forceMultiplier, ForceMode.Impulse);
                }
            }

            // Wait for the next frame
            elapsed += Time.deltaTime;
            yield return null;
        }

        isEarthquakeHappening = false;
    }
}
