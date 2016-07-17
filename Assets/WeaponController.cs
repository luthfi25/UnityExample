using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public GameObject bullet;
	public float maxPower = 3.0f;
	public bool released = false;

	private SpringJoint2D spring;
	private Transform pivot;
	private Ray rayToTouch;
	private float maxPowerSqr;
	private bool touchedOn;
	private Vector2 prevVelocity;
	private GameObject projectile;
	// Use this for initialization

	void Awake () {
		projectile = Instantiate (bullet, new Vector3 (-5, -3, 0), Quaternion.identity) as GameObject;
		spring = GetComponent <SpringJoint2D> ();
		spring.connectedBody = projectile.GetComponent <Rigidbody2D> ();
		pivot = gameObject.transform;
		GetComponent<Rigidbody2D> ().isKinematic = true;
	}
	void Start () {
		rayToTouch = new Ray (pivot.position, Vector3.zero);
		maxPowerSqr = maxPower * maxPower;
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			Touch touchZero = Input.GetTouch (0);
			if (touchZero.phase == TouchPhase.Began) {
				spring.enabled = false;
				touchedOn = true;
			} else if (touchZero.phase == TouchPhase.Ended) {
				spring.enabled = true;
				GetComponent<Rigidbody2D> ().isKinematic = false;
				touchedOn = false;
			}
			if (touchedOn) {
				Dragging (touchZero);
			} 
		}
		if (spring != null) {
			if (!GetComponent<Rigidbody2D> ().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D> ().velocity.sqrMagnitude) {
				spring.connectedBody = null;
				GetComponent<Rigidbody2D> ().velocity = prevVelocity;
			}
			if (!touchedOn) {
				prevVelocity = GetComponent<Rigidbody2D> ().velocity;
			}
		}
	}

	void Dragging (Touch touch) {
		Vector3 touchWorldPoint = Camera.main.ScreenToWorldPoint(touch.position);
		Vector2 pivotToMouse = touchWorldPoint - pivot.position;

		if (pivotToMouse.sqrMagnitude > maxPowerSqr) {
			rayToTouch.direction = pivotToMouse;
			touchWorldPoint = rayToTouch.GetPoint(maxPower);
		}

		touchWorldPoint.z = 0f;
		transform.position = touchWorldPoint;

	}
	IEnumerator WaitNextBullet(){
		yield return new WaitForSeconds (3);
		bullet = Instantiate (bullet, new Vector3 (-5, -3, 0), Quaternion.identity) as GameObject;
		bullet.GetComponent<Rigidbody2D> ().isKinematic = true;

	}
}