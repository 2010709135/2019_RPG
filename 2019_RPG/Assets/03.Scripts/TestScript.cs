using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
    int a = 0;
	// Use this for initialization
	void Start () {
       //StartCoroutine(PrintDebug());
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Update Start" + " frameCount : " + Time.frameCount);

        StartCoroutine(PrintDebug());

        Debug.Log("Update End" + " frameCount : " + Time.frameCount);

    }

    IEnumerator PrintDebug()
    {
        Debug.Log("test1-1-" + a + " frameCount : " + Time.frameCount);
        yield return null;
        a++;
        Debug.Log("test1-2-" + a + " frameCount : " + Time.frameCount);
    }
}
