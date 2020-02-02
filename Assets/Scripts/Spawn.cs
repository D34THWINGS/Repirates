using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawn : MonoBehaviour
{
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerJoined(PlayerInput input)
    {
        GameObject boat = GameObject.FindWithTag("Boat");
        input.transform.SetParent(boat.transform);
        // Put player at spawn point.
        var spawnPoint = GameObject.Find("SpawnPoint");
        input.transform.position = spawnPoint.transform.position;
        input.GetComponent<PlayerController>().PauseMenu = PauseMenu;
    }
}
