using UnityEngine;

public class ConnectWalls : MonoBehaviour
{
    public GameObject wall1; // The first wall
    public GameObject wall2; // The wall connected to wall1
    public GameObject wall3; // The wall connected to wall2
    public GameObject wall4; // The wall connected to wall3

    void Start()
    {
        // Ensure all walls have Rigidbody components
        Rigidbody rb1 = wall1.GetComponent<Rigidbody>();
        Rigidbody rb2 = wall2.GetComponent<Rigidbody>();
        Rigidbody rb3 = wall3.GetComponent<Rigidbody>();
        Rigidbody rb4 = wall4.GetComponent<Rigidbody>();

        if (rb1 == null || rb2 == null || rb3 == null || rb4 == null)
        {
            Debug.LogError("All walls must have Rigidbody components.");
            return;
        }

        // Connect wall1 to wall2
        AddFixedJoint(wall1, rb2);

        // Connect wall2 to wall3
        AddFixedJoint(wall2, rb3);

        // Connect wall3 to wall4
        AddFixedJoint(wall3, rb4);

        // Connect wall4 to wall1 (forming a loop)
        AddFixedJoint(wall4, rb1);
    }

    void AddFixedJoint(GameObject wall, Rigidbody connectedBody)
    {
        FixedJoint fixedJoint = wall.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = connectedBody;

        // Optional: Configure break force and break torque if needed
        fixedJoint.breakForce = Mathf.Infinity; // Set to appropriate value
        fixedJoint.breakTorque = Mathf.Infinity; // Set to appropriate value
    }
}
