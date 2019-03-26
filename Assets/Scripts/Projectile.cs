using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject {

    [SerializeField] private GameObject explosionObject;
    private void Start()
    {
    }

    public override void OnObjectReuse()
    {
    }

    public void OnCollisionEnter(Collision col) {
        GameObject explosion = PoolManager.instance.ReuseObject(explosionObject, transform.position, Quaternion.identity);

        this.Destroy();
	}
}
