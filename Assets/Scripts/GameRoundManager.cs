using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class GameRoundManager : MonoBehaviour {

    [SerializeField] private bool demo = false;

	public List<int> score_values;//had to be private, but collision bug of playcube happened.
	[SerializeField] private Transform[] playerTransforms;

	[SerializeField] private Transform cube;
	[SerializeField] private int endScore = 5;

    [Header("UI")]
    [SerializeField] Text[] scores;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text countDownText;

    [Header("Pause screen")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private FadeScreen transitionScreen;

    [Header("win screen")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject winCamera;
    private bool inWinScreen = false;

    // Use this for initialization
    void Start () {
        playerScoreText.enabled = false;

        for (int i = 0 ; i < scores.Length; i++) {
			score_values.Add(0);
			scores[i].text = score_values[i] + "";
		}
        SetupCharacters();

        Pause(false); //sadly needs to happen
        newRound();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(GameSettings.paused ? false : true);
        }
    }

    public void Pause(bool pause)
    {
        if (inWinScreen) return;

        GameSettings.paused = pause;
        Debug.Log("pause = " + pause);

        Time.timeScale = GameSettings.paused ? 0 : 1f;
        pauseScreen.SetActive(GameSettings.paused);
        DisableCharacters(GameSettings.paused);
        if (!pause)
        {
            Debug.Log("locked!");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
    private void SetupCharacters()
    {

        for (int i = 0; i < playerTransforms.Length; i++)
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
    private void DisableCharacters(bool val, bool cameraEnable = false)
    {
        foreach (Transform player in playerTransforms)
        {
            player.GetComponent<AIController>().paused = val;
            player.GetComponent<PlayerInput>().paused = val;
            if (cameraEnable)
            {
                player.GetComponent<PlayerInput>().cameraEnable = true;
            } else
            {
                player.GetComponent<PlayerInput>().cameraEnable = !val;
            }
            player.GetComponent<FirstPersonController>().enabled = !val;

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

        StartCoroutine(ScoreIncreasing(index));
	}
    public IEnumerator ScoreIncreasing(int index)
    {
        score_values[index] += 1;
        scores[index].text = score_values[index] + "";
        playerScoreText.enabled = true;
        playerScoreText.text = "player " + (index + 1) + " scored!";
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(.05f);
        while (Time.timeScale < 1)
        {
            Time.timeScale += 0.1f;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);
        transitionScreen.FadeTo(1, .5f);
        yield return new WaitForSeconds(.55f);
        playerScoreText.enabled = false;
        Time.timeScale = 1f;
        if (score_values[index] == endScore)
        {
            ActivateWinScreen(index);
            //GoBackToMenu();
        } else
        {
            newRound();
        }

    }
    public void CountDown()
    {
        transitionScreen.FadeTo(0, .2f);
        GetComponent<AudioSource>().Play();

        DisableCharacters(true, true);
        countDownText.gameObject.SetActive(true);
        StartCoroutine( CountDowning(3));
        Outline outline = countDownText.gameObject.GetComponent<Outline>();
        StartCoroutine(CountDownAnimation(outline));
    }
    private IEnumerator CountDowning(int sec)
    {
        Outline outline = countDownText.gameObject.GetComponent<Outline>();
        outline.effectDistance = new Vector2(4.5f, 0f);

        countDownText.text = sec.ToString();
        yield return new WaitForSeconds(1f);
        sec--;
        if (sec > 0)
        {
            StartCoroutine(CountDowning(sec));
        }
        else
        {
            countDownText.gameObject.SetActive(false);
            DisableCharacters(false);
        }
    }
    private IEnumerator CountDownAnimation(Outline outline)
    {
        while (outline.gameObject.active)
        {
            outline.effectDistance = new Vector2(outline.effectDistance.x + 0.5f, outline.effectDistance.y);
            yield return new WaitForFixedUpdate();
        }
    }

    private void ActivateWinScreen(int index)
    {
        inWinScreen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        transitionScreen.FadeTo(0, .2f);

        DisableCharacters(true);
        resetPositions();

        winScreen.SetActive(true);
        winCamera.SetActive(true);
        winCamera.GetComponent<FollowTarget>().target = playerTransforms[index == 1 ? 0 : 1];
        for (int i = 0; i < GameSettings.matchCharacters.Count; i++)
        {
            playerTransforms[i].GetComponent<FirstPersonController>().m_Camera.enabled = false;
        }
    }

    public void GoBackToMenu()
    {
        Time.timeScale =  1f;
        SceneManager.LoadScene("Menu");
    }
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    public void newRound() {


        resetPositions();
        CountDown();

    }
    private void resetPositions()
    {
        cube.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, -1f + Random.value * 2f, 0);
        cube.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);
        cube.GetComponent<PlayCube>().canScore = true;
        cube.gameObject.SetActive(true);
        playerTransforms[0].position = new Vector3(0, 0, -9);
        playerTransforms[1].position = new Vector3(0, 0, 9);
        cube.position = Vector3.zero;

        playerTransforms[0].LookAt(cube.transform);
        playerTransforms[1].LookAt(cube.transform);
    }

}
