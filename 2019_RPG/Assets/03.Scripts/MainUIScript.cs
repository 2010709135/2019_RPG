using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIScript : MonoBehaviour {
    PlayerController _PlayerController;

	// Use this for initialization
	void Start () {
        _PlayerController = PlayerController.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DashBtnPressed()
    {
        _PlayerController.SetDashTrue();
    }
}
