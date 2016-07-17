using UnityEngine;
using System.Collections;

public class DestroyBoundary : MonoBehaviour {

	void OnCollisionEnterOnCollisionEnter(Collision col){
		if (col.gameObject.name == "Boundary")
			Destroy (gameObject);
	}
}
