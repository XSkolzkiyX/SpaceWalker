using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float sensetivity, health, speed, jumpForce;
    [SerializeField] private Transform camPosition;
    Camera cam;
    private PhotonView photonView;
    float minSpeed, maxSpeed;
    bool canJump = true;
    Rigidbody rb;

    void Start()
    {
        //cam = Instantiate(cam.gameObject, new Vector3(0, 0.8f, 0), Quaternion.identity, transform).GetComponent<Camera>();
        photonView = GetComponent<PhotonView>();
        if(photonView.AmOwner) cam = camPosition.gameObject.AddComponent<Camera>();
        maxSpeed = speed;
        minSpeed = speed / 10;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }
    [PunRPC]
    void Update()
    {
        if (!photonView.AmOwner)
        {
            return;
        }
        
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX != 0f || moveY != 0f)
        {
            rb.AddForce(transform.forward * moveY * speed);
            rb.AddForce(transform.right * moveX * speed);
        }

        float camX = Input.GetAxis("Mouse X") * sensetivity;
        float camY = Input.GetAxis("Mouse Y") * sensetivity;
        transform.Rotate(0, camX, 0);
        cam.transform.Rotate(camY, 0, 0);
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        //if (Input.GetMouseButtonDown(0)) Shoot();
    }

    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rb.drag = 1f;
            speed = minSpeed;
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.ClearDeveloperConsole();
        Debug.Log("You Lost");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground" && rb)
        {
            rb.drag = 10f;
            speed = maxSpeed;
            canJump = true;
        }
    }
}