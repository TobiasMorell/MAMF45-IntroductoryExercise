using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown)
	    {
            Destroy(this.gameObject);
	    }
	}
}
