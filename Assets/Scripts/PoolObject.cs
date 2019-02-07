using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {

	public virtual void OnObjectReuse() {

	}
	public virtual void Destory(){
		gameObject.SetActive(false);
	}
}
