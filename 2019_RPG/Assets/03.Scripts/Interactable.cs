using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    protected SphereCollider _collider;

	// Use this for initialization
	protected void Start () {
        _collider = GetComponent<SphereCollider>();
        if (_collider == null)
        {
            gameObject.AddComponent<SphereCollider>();
            _collider = GetComponent<SphereCollider>();

        }

        _collider.isTrigger = true;
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            InteractToPlayer(other);
        }
    }

    protected virtual void InteractToPlayer(Collider other)
    {
        Debug.Log("InteractToPlayer");
    }
}
