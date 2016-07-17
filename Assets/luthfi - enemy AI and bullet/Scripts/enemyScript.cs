using UnityEngine;
using System.Collections;

public class enemyScript : MonoBehaviour {
	public Transform leftBound;
	public Transform rightBound;
	public Transform player;

	Vector3 dest;
	bool facingRight = true;
	bool isWalking = false;
	float lastFired;

	public GameObject bullet;
	GameObject plr;
	Rigidbody2D rb;

	bool isDiem = false;
	float lastDiem;

	Animator anim;
	AudioSource au;

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
			float offset = Random.Range (0.0f, 5.0f);

			if (arah == 1) {
				dest = player.position + new Vector3(offset,0);

				if (dest.x > rightBound.position.x)
					dest.x = rightBound.position.x;
			} else {
				dest = player.position - new Vector3(offset,0);

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
			if (facingRight) {
				if (transform.position.x < dest.x)
					transform.Translate (Vector3.right * Time.deltaTime * 2);
				else
					isWalking = false;
			} else {
				if (transform.position.x > dest.x)
					transform.Translate (Vector3.left * Time.deltaTime * 2);
				else
					isWalking = false;
			}	
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
		Transform obj = new GameObject().transform;
		obj.position = new Vector3 (Random.Range(leftBound.position.x, rightBound.position.x),player.position.y);

		plr = (GameObject) Instantiate(bullet,transform.position, Quaternion.identity);
		rb = plr.GetComponent<Rigidbody2D>();	

		Vector3 dir = obj.position - transform.position;
		dir = obj.InverseTransformDirection(dir);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		plr.transform.rotation = Quaternion.Euler(0,0,angle+180);

		//First we get the direction we need to travel in
		Vector2 direction = (obj.position - transform.position).normalized;

		//Multiply it by the maximum speed we're trying to reach
		Vector2 desiredVelocity = direction * 100;

		//Apply the steering. The less the mass, the more effective the steering
		rb.AddForce(desiredVelocity);
		Destroy (obj.gameObject);

		//matiin animasi nyerang
	}

	void OnTriggerEnter2D (Collider2D coll) {
		/*if gameobject == player bullet
			hp kurang
			bunyiin suara hit
		*/
	}
}
