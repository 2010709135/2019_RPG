using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour {
    private PlayerController _playerController;

	// Use this for initialization
	void Start () {
        _playerController = PlayerController.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerDashFalse()
    {
        _playerController.SetDashFalse();
    }
}
