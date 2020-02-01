using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUpandDown : MonoBehaviour
{
    public float maxYAcceleration = 10.0f;
    public float maxZAcceleration = -5f;
    // Start is called before the first frame update
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
            if (other.attachedRigidbody.transform.forward.z < 0 && other.attachedRigidbody.velocity.magnitude > 0
                && other.attachedRigidbody.velocity.z > -2) // If facing left and in movement and not too fast
            {
                other.attachedRigidbody.AddForce(0, maxYAcceleration, maxZAcceleration, ForceMode.Acceleration); // Accelerate the player
            }
        }
    }
}
