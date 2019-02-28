using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    Transform target_Tr;

    public Vector3 offset = new Vector3(-5,8,-5);
    Vector3 rotation = new Vector3(45, 45, 0);

    bool following = true;

    public float smoothSpeed = 0.125f;

	// Use this for initialization
	void Start () {
        target_Tr = PlayerController.instance.GetComponent<Transform>();

        transform.rotation = Quaternion.Euler(rotation);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (following)
        {
            Vector3 desiredPos = target_Tr.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
        }
	}
}
