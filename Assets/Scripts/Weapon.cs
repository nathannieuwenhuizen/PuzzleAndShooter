using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	[SerializeField]
	private GameObject projectile;
	
	[SerializeField]
	private int maxAmmo;
	private int ammo;

	[SerializeField]
	private float projectileBaseSpeed = 10f;
	[SerializeField]
	private float projectileExtraSpeed = 10f;

	[SerializeField]
	private AudioClip shootSound;
	private float ShootOffsetPos = 1.5f;
	private AudioSource audioSource;


	private Camera camera;
	private bool holdToShoot = false;
	private float normalCamerastate = 60f;
	private float maxCameraState = 50f;


	void Start () {
		ammo = maxAmmo;
		PoolManager.instance.CreatePool(projectile, 50);
		audioSource = GetComponent<AudioSource>();
		camera = GetComponent<Camera>();
	}
	
	void Update() {
		if (camera.isActiveAndEnabled)
		if (holdToShoot) {
			if (camera.fieldOfView > maxCameraState) {
				camera.fieldOfView -= .2f;
			} else {
				camera.fieldOfView = maxCameraState;
			}
		}
		else {
			if (camera.fieldOfView <  normalCamerastate) {
				camera.fieldOfView += 1f;
			} else {
				camera.fieldOfView = normalCamerastate;
			}

		}
	}
	public void Shoot() {
		holdToShoot = false;
		audioSource.clip = shootSound;
		audioSource.Play();

		GameObject bullet = PoolManager.instance.ReuseObject(projectile, transform.position + transform.forward * ShootOffsetPos, Quaternion.identity);
		bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;

		
		// Vector3 distance = bullet.transform.position;
		// float val = transform.rotation.x * 4;
		// distance.y += val;
		// distance.x -= val;
		// distance.z -= val;
		// bullet.transform.position = distance;

		float extraSpeed = (normalCamerastate - camera.fieldOfView) / (normalCamerastate - maxCameraState) * projectileExtraSpeed;
		// Debug.Log("camera = " + camera.fieldOfView);
		// Debug.Log("speed = " + projectileBaseSpeed);
		Debug.Log("extra speed = " + extraSpeed);
		
		bullet.GetComponent<Rigidbody>().velocity = (bullet.transform.position - transform.position) * (projectileBaseSpeed + extraSpeed);

	}

	public void HoldingToShoot () {
		holdToShoot = true;
	}
}
