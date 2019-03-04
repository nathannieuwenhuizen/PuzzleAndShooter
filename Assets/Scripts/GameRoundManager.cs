using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class GameRoundManager : MonoBehaviour {

	[SerializeField] Text[] scores;
	public List<int> score_values;//had to be private, but collision bug of playcube happened.
	[SerializeField] private Transform[] playerTransforms;
	[SerializeField] private Transform cube;
	[SerializeField] private int endScore = 5;

    private
	// Use this for initialization
	void Start () {
		for (int i = 0 ; i < scores.Length; i++) {
			score_values.Add(0);
			scores[i].text = score_values[i] + "";
		}
        SetupCharacters();
	}


    private void SetupCharacters()
    {

        for (int i = 0; i < GameSettings.matchCharacters.Count; i++)
        {
            Debug.Log("does it work?" + i);
            playerTransforms[i].GetComponent<FirstPersonController>().m_Camera.enabled = true;

            if (sharedScreen())
            {
                playerTransforms[i].GetComponent<FirstPersonController>().m_Camera.rect = new Rect((i == 0 ? .5f : 0), 0, .5f, 1);
            } else
            {
                playerTransforms[i].GetComponent<FirstPersonController>().m_Camera.rect = new Rect(0, 0, 1, 1);
                if (GameSettings.matchCharacters[i].AI)
                {
                    playerTransforms[i].GetComponent<FirstPersonController>().m_Camera.enabled = false;
                }
            }

            playerTransforms[i].GetComponent<AIController>().enabled = GameSettings.matchCharacters[i].AI;
            playerTransforms[i].GetComponent<PlayerInput>().enabled = !GameSettings.matchCharacters[i].AI;

            playerTransforms[i].GetComponent<PlayerInput>().controllerID = GameSettings.matchCharacters[i].controlledBy;

        }
    }
    private bool sharedScreen()
    {
        if (GameSettings.matchCharacters[0].AI == GameSettings.matchCharacters[1].AI)
        {
            return true;
        }
        return false;
    }

    public void ScoreIncrease(int index) {

		score_values[index] += 1;
		scores[index].text = score_values[index] + "";	
		if (score_values[index] >= endScore) {
			Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene("Menu");
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
