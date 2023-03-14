using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    [SerializeField] private float grabDistance, grabSpeed, grabDelta;
    [SerializeField] private Transform cam;
    private bool isGrabbing = false;
    private Rigidbody grabbedObject;
    private float curSpeed, curDistance;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, grabDistance))
            {
                if(hit.collider.TryGetComponent<Rigidbody>(out grabbedObject))
                {
                    grabbedObject.useGravity = false;
                    grabbedObject.drag = 10f;
                    isGrabbing = true;
                    curSpeed = grabSpeed / grabbedObject.mass;
                    curDistance = Vector3.Distance(grabbedObject.position, transform.position);
                }
            }
        }
        if (grabbedObject && (Input.GetMouseButtonUp(0) || 
            Vector3.Distance(grabbedObject.position, transform.position) > grabDistance))
        {
            grabbedObject.useGravity = true;
            grabbedObject.drag = 1f;
            isGrabbing = false;
        }
    }

    void FixedUpdate()
    {
        if (isGrabbing)
        {
            Vector3 targetPosition = transform.position + (cam.forward * curDistance);
                //Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
            Vector3 direction = targetPosition - grabbedObject.transform.position;
            if(Vector3.Distance(targetPosition, grabbedObject.transform.position) > grabDelta)
                grabbedObject.velocity = direction.normalized * curSpeed;
        }
    }
}