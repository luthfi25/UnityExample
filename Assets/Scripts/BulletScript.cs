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

	void OnTriggerEnter2D (Collider2D coll) {
		/*if gameobject == player
			nyalain animasi hit
			destroy gameobject
		*/
	}
}
