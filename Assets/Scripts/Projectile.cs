﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject {

	public override void OnObjectReuse() {
		Debug.Log("Reuse");
	}
}
