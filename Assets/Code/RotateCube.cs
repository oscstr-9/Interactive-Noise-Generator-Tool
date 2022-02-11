using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    Rigidbody selectedRigidbody;
    Camera targetCamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    float selectionDistance;

    Vector3 prevMousePos = Vector3.zero;
    Vector3 MousePosDelta = Vector3.zero;
    public float rotSpeed = 1f;

    void Start()
    {
        targetCamera = GetComponent<Camera>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //Check if we are hovering over Rigidbody, if so, select it
            selectedRigidbody = GetRigidbodyFromMouseClick();
        }
        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            //Release selected Rigidbody if there any
            selectedRigidbody = null;
        }
    }

    void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            MousePosDelta = Input.mousePosition - prevMousePos;
            selectedRigidbody.transform.Rotate(Camera.main.transform.up, Vector3.Dot(-MousePosDelta / rotSpeed, Camera.main.transform.right), Space.World);
            selectedRigidbody.transform.Rotate(Camera.main.transform.right, Vector3.Dot(MousePosDelta / rotSpeed, Camera.main.transform.up), Space.World);
        }
        prevMousePos = Input.mousePosition;
    }

    Rigidbody GetRigidbodyFromMouseClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
                originalRigidbodyPos = hitInfo.collider.transform.position;
                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }

}
