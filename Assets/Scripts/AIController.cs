using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
public class AIController : MonoBehaviour {

	[SerializeField] private Transform goalPos;
	private bool GoalZHiger = false;
	[SerializeField] private Transform target;

	private FirstPersonController character;
	private Weapon weapon;

	private Vector2 speed;
	private Vector2 rotation;

	private bool shooting = false;

	private Vector3 dist;
	[SerializeField] private float aimSpeed = 10f;
	[SerializeField] private float moveSpeed = 10f;
	[SerializeField] private Vector3 detectionDistances = new Vector3(3f, 2f, 2f);
	[SerializeField] private Vector2 detectionAngle = new Vector2(1f, 1f);
	[SerializeField] private float wantedAimDistance = 5f;
	// Use this for initialization
	void Start () {
		character = GetComponent<FirstPersonController>();
		weapon = GetComponentInChildren<Weapon>();

		speed = new Vector2(0f,0f);
		rotation = new Vector2(0f,0f);
		GoalZHiger = transform.position.z < goalPos.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		InputJump();
		InputShoot();
		InputMovement();
	}

	private void InputMovement() {
		dist = target.position - weapon.gameObject.transform.localPosition - transform.position;
		
		//Aim rotation to target -----------------------------------------------------------------------
		float angle = Mathf.Rad2Deg * Mathf.Atan2(dist.x, dist.z);
		float cRotX =  transform.localRotation.eulerAngles.y;
		if (!GoalZHiger) {
			if (cRotX > 180) {
				cRotX = -(360 - cRotX);
			}
		} else {
			// Debug.Log("angle | " + angle + " ||| crotxxx | " + cRotX);
			cRotX = (360 + cRotX)%360;
			angle = (360 + angle)%360;
		}

		// Debug.Log("angle | " + angle);
		// Debug.Log("current angle | " + cRotX);
	
		//rotation on Y-as
		if (Mathf.Abs(cRotX - angle) > detectionAngle.x) {
			rotation.y = Mathf.Clamp((angle - cRotX) / 180f * aimSpeed, -1f, 1f);
		} else {
			rotation.y = 0;
		}


		//rotation on Z-as
		float squaredDist = Mathf.Sqrt( Mathf.Pow(dist.x, 2) + Mathf.Pow(dist.z, 2));
	    angle = Mathf.Rad2Deg * Mathf.Atan2(dist.y, squaredDist);

		float cRotY =  weapon.gameObject.transform.eulerAngles.x;
		if (cRotY > 180) {
			cRotY = 360 - cRotY;
		} else {
			cRotY *= -1;
		}

		if (Mathf.Abs(cRotY - angle) > detectionAngle.y) {
			rotation.x = Mathf.Clamp((cRotY- angle) / 180f * aimSpeed, -1f, 1f);
		} else {
			rotation.x = 0;
		}

		character.RotateView(rotation.y, rotation.x);
		// end aim ---------------------------------------------------------------------------------

		
		//move -------------------------------------------------------------------------------------
		float distx = target.position.x - transform.position.x;
		if (Mathf.Abs(distx) > detectionDistances.x) {
			speed.x = Mathf.Clamp(distx  / 1000f * moveSpeed, -1f, 1f)  * (GoalZHiger ? -1 : 1);
		} else {
			speed.x = 0;
		}
		float disty = target.position.z - transform.position.z + (GoalZHiger ? wantedAimDistance : - wantedAimDistance);
		if (Mathf.Abs(disty) > detectionDistances.z) {
			speed.y = Mathf.Clamp(disty  / 1000f * moveSpeed, -1f, 1f) * (GoalZHiger ? -1 : 1);
		} else {
			speed.y = 0;
		}
		Debug.Log(speed);
		character.MoveSpeed = speed;
		//move -------------------------------------------------------------------------------------

	}

	
	private void InputShoot() {
		if (inPositionToShoot() && !shooting) {
			shooting = true;
			StartCoroutine(Shooting());
		}
	}
	IEnumerator Shooting() {
		weapon.Shoot();
		yield return new WaitForSeconds(Random.value * 1f);
		if (inPositionToShoot()) {
			StartCoroutine(Shooting());
		} else {
			shooting = false;
		}
	}
	private bool inPositionToShoot() {
		if (dist.z * (GoalZHiger ? -1 : 1) > 0) {
			if (Mathf.Abs(rotation.x) < .1f &&  Mathf.Abs(rotation.y) < .1f) {
				return true;
			} else if (speed.y * (GoalZHiger ? -1 : 1) < 0) {
				return true;
			}
		}
		
		return false;
	}
	private void InputJump() {
		if (Mathf.Abs(dist.y) > detectionDistances.y) {
			character.JumpInput = true;
		} else {
			character.JumpInput = false;
		}
	}
}
