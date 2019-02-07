using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	[SerializeField]
	private GameObject projectile;
	
	[SerializeField]
	private int maxAmmo;
	private int ammo;

	// Use this for initialization
	void Start () {
		ammo = maxAmmo;
		PoolManager.instance.CreatePool(projectile, 5);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("l")) {
			Shoot();
		}
	}
	void Shoot() {
		GameObject bullet = PoolManager.instance.ReuseObject(projectile, transform.position + transform.forward * 4, Quaternion.identity);
		bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;

		
		// Vector3 distance = bullet.transform.position;
		// float val = transform.rotation.x * 4;
		// distance.y += val;
		// distance.x -= val;
		// distance.z -= val;
		// bullet.transform.position = distance;
		bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.position - transform.position) * 100);

	}
}
