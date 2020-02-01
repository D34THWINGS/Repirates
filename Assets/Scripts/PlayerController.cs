using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movement;
    public float Speed = 0.5f;

    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    public void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<Vector2>();
    }

    public void MovePlayer()
    {
        transform.Translate(new Vector3(movement.x, 0, movement.y) * Speed);
    }
}
