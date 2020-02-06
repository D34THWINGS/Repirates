using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;
    public float JumpForce = 10.0f;
    public int SmashNumber = 20;
    public GameObject PauseMenu;
    private Transform playerRig;
    private Vector2 direction = new Vector2();
    private Animator animator;
    private Rigidbody rigidBody;
    private GameProgress GameManager;
    private List<Collider> collisions = new List<Collider>();
    private bool hasPlank;
    private bool isGrounded;
    private bool wasGrounded;

    private GameObject holeRepairing;
    private bool smashStarted;
    private float currentSpeed;
    private int keySmashed = 0;

    public Image SmashBar;

    // Start is called before the first frame update
    void Start()
    {
        playerRig = transform.GetChild(0);
        animator = playerRig.gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameProgress>();
        SmashBar.transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetBool("Grounded", isGrounded);   

        MovePlayer();

        if (!wasGrounded && isGrounded)
        {
            animator.ResetTrigger("Jump");
            animator.SetTrigger("Land");
        }

        wasGrounded = isGrounded;
    }

    void OnTogglePauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }

    public void OnMove(InputValue inputValue)
    {
        if (PauseMenu.activeInHierarchy) return;
        direction = inputValue.Get<Vector2>();
    }

    public void OnJump()
    {
        if (PauseMenu.activeInHierarchy) return;
        if (isGrounded)
        {
            animator.SetTrigger("Jump");
            rigidBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
    public void OnInteract()
    {
        if (PauseMenu.activeInHierarchy) {
            PauseMenu.SetActive(false);
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3, LayerMask.GetMask(new string[] { "Interactible" }));
        if (hitColliders.Length > 0)
        {
            if (hitColliders[0].gameObject.tag == "Planks")
            {
                Debug.Log("You got planks"); // Show plank above player
                hasPlank = true;
            }
            else if (hitColliders[0].gameObject.tag == "Holes" && hitColliders[0].gameObject.activeSelf)
            {
                    
                if (!smashStarted)
                {
                    if (hasPlank)
                    {
                        SmashBar.transform.parent.gameObject.SetActive(true);
                        SmashBar.fillAmount = 0;
                        Debug.Log("Repairing Hole, please SMASH Y"); // Show smash animation above player 
                        holeRepairing = hitColliders[0].gameObject;
                        smashStarted = true;
                        hasPlank = false;
                        currentSpeed = Speed;
                        Speed = 0;
                        animator.SetBool("isSmashing", true);
                    }
                    else
                    {
                        Debug.Log("You need plank !"); // Show red plank above player
                    }
                }
            }

        }
    }
    public void OnSmash()
    {
        if (PauseMenu.activeInHierarchy) return;
        if (smashStarted)
        {
            keySmashed++;
            keySmashed = Mathf.Clamp(keySmashed, 0, SmashNumber);
            float amount = (float)keySmashed / SmashNumber;
            SmashBar.fillAmount = amount;
            if (keySmashed == SmashNumber)  
            {
                Debug.Log("Hole repaired"); // Fix hole in GameManager and remove plank from player ui
                animator.SetBool("isSmashing", false);
                GameManager.repairHole();
                keySmashed = 0;
                smashStarted = false;
                Speed = currentSpeed;
                holeRepairing.SetActive(false);
                SmashBar.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void MovePlayer()
    {
        if (PauseMenu.activeInHierarchy) return;
        var camera = Camera.main.transform;
        var cameraDirection = camera.forward * direction.y + camera.right * direction.x;
        var directionLength = direction.magnitude;
        cameraDirection.y = 0;
        var normalizedDirection = cameraDirection.normalized * directionLength;
        var moveVector = new Vector3(normalizedDirection.x, 0, normalizedDirection.z) * Speed * Time.deltaTime;

        transform.position += moveVector;
        animator.SetFloat("MoveSpeed", direction.magnitude);


        if (direction != Vector2.zero)
            playerRig.rotation = Quaternion.LookRotation(moveVector);
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
