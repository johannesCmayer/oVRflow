using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactivateMeshRenderer : MonoBehaviour {
    
	void Start ()
    {
		GetComponent<MeshRenderer> ().enabled = false;
	}
}
