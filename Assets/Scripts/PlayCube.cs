using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCube : MonoBehaviour {

	[SerializeField]
	private AudioClip bounceSound;
	[SerializeField]
	private AudioClip goalSound;
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource explosionSound;
    private AudioSource audioSource;

	[SerializeField] private GameObject manager;

    public bool canScore = true;
    public bool demo = false;
	public void Start() {
		audioSource = GetComponent<AudioSource>();
	}
	public void OnCollisionEnter(Collision col) {

		if (col.gameObject.tag == "Goal" && canScore) {
            if (demo)
            {
                transform.position = Vector3.zero;
                return;
            }
			audioSource.clip = goalSound;
            canScore = false;

            explosion.transform.position = transform.position;
            explosion.GetComponent<ParticleSystem>().Play();
            explosionSound.Play();

            gameObject.SetActive(false);
            if (manager != null) {
                if (transform.position.z > 0)
                {
                    manager.GetComponent<GameRoundManager>().ScoreIncrease(1);
                }
                else
                {
                    manager.GetComponent<GameRoundManager>().ScoreIncrease(0);
                }
            }
            
        } else {
			audioSource.clip = bounceSound;
		}

        //plays hit sound
        if (!audioSource.isPlaying) {
			audioSource.Play();
		}
	}

}
