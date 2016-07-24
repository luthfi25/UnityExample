using UnityEngine;
using System.Collections;

public class BoundScript : MonoBehaviour {

	void OnTriggerExit2D (Collider2D coll) {
		Destroy(coll.gameObject);
	}
}
