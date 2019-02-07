using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCube : MonoBehaviour {

	[SerializeField]
	private AudioClip bounceSound;
	[SerializeField]
	private AudioClip goalSound;
	private AudioSource audioSource;
	public void Start() {
		audioSource = GetComponent<AudioSource>();
	}
	public void OnCollisionEnter(Collision col) {

		if (col.gameObject.tag == "goal") {
			audioSource.clip = goalSound;
			//GOAL!
		} else {
			audioSource.clip = bounceSound;
		}
		if (!audioSource.isPlaying) {
			audioSource.Play();
		}
	}
	
}
