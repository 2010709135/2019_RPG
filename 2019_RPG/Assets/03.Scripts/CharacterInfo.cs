using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour {
    #region Singleton
    public static CharacterInfo instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int _health { get; private set; }
    public int _stamina { get; private set; }
    public Inventory _inventory {get; private set;}

	// Use this for initialization
	void Start () {
        _inventory = new Inventory();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
