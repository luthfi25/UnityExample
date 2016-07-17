using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float maxPower = 3.0f;
	private SpringJoint2D spring;
	private Transform pivot;
	private Ray rayToTouch;
	private float maxPowerSqr;
	private bool touchedOn;
	private Vector2 prevVelocity;
	// Use this for initialization

	void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		pivot = spring.connectedBody.transform;
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
				Destroy (spring);
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
}
