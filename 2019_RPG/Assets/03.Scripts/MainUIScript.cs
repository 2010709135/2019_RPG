using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIScript : MonoBehaviour {
    public static MainUIScript instance = null;
    PlayerController _PlayerController;

    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

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
