﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCube : MonoBehaviour {

	[SerializeField]
	private AudioClip bounceSound;
	[SerializeField]
	private AudioClip goalSound;
	private AudioSource audioSource;

	[SerializeField] private GameObject manager;
	public void Start() {
		audioSource = GetComponent<AudioSource>();
	}
	public void OnCollisionEnter(Collision col) {

		if (col.gameObject.tag == "Goal") {
			audioSource.clip = goalSound;
			Debug.Log("goal!");
			if (transform.position.z > 0) {
                 manager.GetComponent<GameRoundManager>().ScoreIncrease(1);
				}
			else {
                manager.GetComponent<GameRoundManager>().ScoreIncrease(0);
			}
		} else {
			audioSource.clip = bounceSound;
		}
		if (!audioSource.isPlaying) {
			audioSource.Play();
		}
	}

    
}
