﻿using UnityEngine;
using System.Collections;

public class enemyScript : MonoBehaviour {
	public Transform leftBound;
	public Transform rightBound;
	//public Transform player;

	Vector3 dest;
	float dist;
	bool facingRight = true;
	bool isWalking = false;
	float lastFired;

	public GameObject bullet;
	public float bulletForce;
	public float bulletAngle;
	GameObject plr;
	Rigidbody2D rb;

	bool isDiem = false;
	float lastDiem;

	Animator anim;
	AudioSource au;

	public float health = 100.0f;

	// Use this for initialization
	void Start () {
		lastFired = Time.time;
		anim = GetComponent<Animator> ();
		au = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*cek hp 0 lalu set animasi mati*/

		if (!isWalking) {
			int arah = (int) Random.Range (1, 2.9f);
			float offset = Random.Range (0.0f, 2.5f);

			if (arah == 1) {
				dest = transform.position + new Vector3(offset,0);

				if (dest.x > rightBound.position.x)
					dest.x = rightBound.position.x;
			} else {
				dest = transform.position - new Vector3(offset,0);

				if(dest.x < leftBound.position.x)
					dest.x = leftBound.position.x;
			}
			Debug.Log (dest);

			face (arah);
			isWalking = true;
		} else if(isDiem){
			//set animasi nyerang
			if (Time.time - lastDiem > 0.25f) {
				isDiem = false;
				SpawnBullet ();
			}
		}
		else {
			//set animasi jalan
			if (transform.position.x != dest.x)
				transform.position = Vector3.MoveTowards (transform.position, dest, Time.deltaTime);
			else
				isWalking = false;
		}
			
	}

	void FixedUpdate(){
		if (Time.time - lastFired > 4) {
			isDiem = true;
			lastDiem = Time.time;
			lastFired = Time.time;
		}
	}

	void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void face(int arah){
		if (arah == 1) {
			if (!facingRight)
				flip ();
		} else {
			if (facingRight)
				flip ();
		}
	}

	void SpawnBullet(){
		plr = (GameObject) Instantiate(bullet,transform.position, Quaternion.identity);
		rb = plr.GetComponent<Rigidbody2D>();

		Vector3 dir = Quaternion.AngleAxis (bulletAngle, Vector3.forward) * Vector3.right;
		rb.AddForce (dir * bulletForce);

		plr.transform.rotation = Quaternion.Euler (0, 0, 180+bulletAngle);
		//matiin animasi nyerang
	}

	void OnTriggerEnter2D (Collider2D coll) {
		/*if gameobject == player bullet
			hp kurang
			bunyiin suara hit
		*/
	}
}