using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
public class Weapon : MonoBehaviour {

	[SerializeField]
	private GameObject projectile;
	
	[SerializeField]
	private int maxAmmo;
	private int ammo;

	[SerializeField]
	private float projectileSpeed = 100f;

	[SerializeField]
	private AudioClip shootSound;
	private float ShootOffsetPos = 1.5f;
	[SerializeField] private UnityStandardAssets.Characters.FirstPerson.FirstPersonController character;
	private AudioSource audioSource;


	void Start () {
		ammo = maxAmmo;
		PoolManager.instance.CreatePool(projectile, 50);
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		bool j_input = false;
		switch (character.controllerID)
		{
			case 0:
				j_input = Input.GetButtonDown("Fire0");
			break;
			case 1:
				j_input = Input.GetKeyDown(KeyCode.Joystick1Button7);
			break;
			case 2:
				j_input = Input.GetKeyDown(KeyCode.Joystick2Button7);
			break;
			default:
				j_input = Input.GetButtonDown("Fire0");
			break;
		}

		if (j_input) {
			Shoot();
		}
	}
	void Shoot() {

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
		bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.position - transform.position) * projectileSpeed);

	}
}
}