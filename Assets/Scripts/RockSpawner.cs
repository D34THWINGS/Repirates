using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject RockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Rock hit");
    }


    public void SpawnRock()
    {
        var rock = Instantiate(RockPrefab);
        rock.transform.position = new Vector3(transform.position.x + Random.Range(-8, 8), transform.position.y, transform.position.z);
        
    }
}
