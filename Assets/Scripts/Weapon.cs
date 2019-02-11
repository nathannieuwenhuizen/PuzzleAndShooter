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
	private float projectileSpeed = 100f;

	[SerializeField]
	private AudioClip shootSound;
	private float ShootOffsetPos = 1.5f;
	private AudioSource audioSource;


	void Start () {
		ammo = maxAmmo;
		PoolManager.instance.CreatePool(projectile, 50);
		audioSource = GetComponent<AudioSource>();
	}
	
	public void Shoot() {

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
