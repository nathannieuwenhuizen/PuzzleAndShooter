using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameRoundManager : MonoBehaviour {

	[SerializeField] Text[] scores;
	public List<int> score_values;//had to be private, but collision bug of playcube happened.
	[SerializeField] private Transform[] playerTransforms;
	[SerializeField] private Transform cube;
	[SerializeField] private int endScore = 5;
	// Use this for initialization
	void Start () {
		for (int i = 0 ; i < scores.Length; i++) {
			score_values.Add(0);
			scores[i].text = score_values[i] + "";
		}
	}
	public void ScoreIncrease(int index) {

		score_values[index] += 1;
		scores[index].text = score_values[index] + "";	
		if (score_values[index] >= endScore) {
			Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
			return;
		}
		newRound();
	}
	public void newRound() {

		cube.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, - 1f + Random.value * 2f, 0);
		cube.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);
		// playerTransforms[0].position = new Vector3(0,0, -9);
		// playerTransforms[1].position = new Vector3(0,0, 9);
		cube.position = Vector3.zero;


	}
	
}
