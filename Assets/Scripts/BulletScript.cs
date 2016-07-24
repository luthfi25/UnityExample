using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	Animator anim;
	AudioSource au;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		au = GetComponent<AudioSource> ();

		//bunyiin bullet (kalo perlu)
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.gameObject.tag == "Player Bullet") {
			Destroy (coll.gameObject);
			Destroy (gameObject);
		}
	}
}
