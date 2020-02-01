using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float JumpForce = 10.0f;

    private Vector2 direction = new Vector2();
    private Animator animator;
    private Rigidbody rigidBody;
    private PlayerInput playerInput;
    private List<Collider> collisions = new List<Collider>();
    private bool isGrounded;
    private bool wasGrounded;

    // Start is called before the first frame update
    void Start()
    {
            animator = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetBool("Grounded", isGrounded);

        MovePlayer();

        if (!wasGrounded && isGrounded)
        {
            animator.SetTrigger("Land");
        }

        if (!isGrounded && wasGrounded)
        {
            animator.SetTrigger("Jump");
        }

        wasGrounded = isGrounded;
    }

    public void OnMove(InputValue inputValue)
    {
        direction = inputValue.Get<Vector2>();
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            rigidBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    public void MovePlayer()
    {
        if (direction != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        transform.position += new Vector3(direction.x, 0, direction.y) * Speed * Time.deltaTime;
        animator.SetFloat("MoveSpeed", direction.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!collisions.Contains(collision.collider))
            {
                collisions.Add(collision.collider);
            }
        }
        else
        {
            if (collisions.Contains(collision.collider))
            {
                collisions.Remove(collision.collider);
            }
            if (collisions.Count == 0) { isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }
        if (collisions.Count == 0) { isGrounded = false; }
    }
}
