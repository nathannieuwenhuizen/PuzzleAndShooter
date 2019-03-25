using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
public class AIController : MonoBehaviour {

	[SerializeField] private Transform goalPos;
	[SerializeField] private Transform target;

	[SerializeField] private Vector3 positionFromTarget;

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

    public bool paused = false;

    // Use this for initialization
    void Start () {
		character = GetComponent<FirstPersonController>();
		weapon = GetComponentInChildren<Weapon>();

		speed = new Vector2(0f,0f);
		rotation = new Vector2(0f,0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (paused) { return; }

        InputJump();
		InputShoot();
		Aim();
		InputMovement(); 
	}

	private void InputMovement() {
		//move -------------------------------------------------------------------------------------
		Vector3 positionIWant = target.position;
		positionIWant.x += positionFromTarget.x;
		positionIWant.z += positionFromTarget.z;

		Vector3 myRelativeDistance = transform.InverseTransformDirection(positionIWant - transform.position );

		if (Mathf.Abs(myRelativeDistance.x) > detectionDistances.x) {
			speed.x = Mathf.Clamp(myRelativeDistance.x  / 1000f * moveSpeed, -1f, 1f);
		} else { 
			speed.x = 0;
		}
		
		if (Mathf.Abs(myRelativeDistance.z) > detectionDistances.z) {
			speed.y = Mathf.Clamp(myRelativeDistance.z  / 1000f * moveSpeed, -1f, 1f);
		} else {
			speed.y = 0;
		}

		character.MoveSpeed = speed;
		//move -------------------------------------------------------------------------------------

	}

	private void Aim() {
		dist = target.position - weapon.gameObject.transform.localPosition - transform.position;
		//Aim rotation to target -----------------------------------------------------------------------
		float angle = Mathf.Rad2Deg * Mathf.Atan2(dist.x, dist.z);
		float cRotX =  transform.localRotation.eulerAngles.y;
		if (goalPos.position.z < transform.position.z) {
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
		if (inPositionToShoot() && !paused) {
			StartCoroutine(Shooting());
		} else {
			shooting = false;
		}
	}
	private bool inPositionToShoot() {
		//if other side of goal

		float myDistToGoal = Vector3.Distance(goalPos.position, transform.position);
		float targetDistToGoal = Vector3.Distance(goalPos.position, target.position);
		if (myDistToGoal < targetDistToGoal) {
			//if aim is correctly.
			if (Mathf.Abs(rotation.x) < .1f &&  Mathf.Abs(rotation.y) < .1f) {
				return true;
			}
			//else check if player goes back even though its aim isn't correct 
			else if (speed.y < 0) {
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
