using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUpandDown : MonoBehaviour
{
    public float maxYAcceleration = 10.0f;
    public float maxZAcceleration = -10f;
    // Start is called before the first frame updat
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            other.attachedRigidbody.useGravity = false;
            other.attachedRigidbody.velocity = new Vector3(0, 0, 0);
            Transform PlayerMesh = other.attachedRigidbody.transform.GetChild(0);
            if (PlayerMesh.forward.z < 0 && other.attachedRigidbody.velocity.magnitude > 0) // If facing left and in movement and not too fast
            {
                other.attachedRigidbody.AddForce(0, maxYAcceleration, maxZAcceleration, ForceMode.Acceleration); // Accelerate the player
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.attachedRigidbody.velocity = new Vector3(0, -10, 0);
            other.attachedRigidbody.useGravity = true;
        }
    }
}
