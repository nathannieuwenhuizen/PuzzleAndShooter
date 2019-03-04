using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingManager : MonoBehaviour
{
    [SerializeField] private CharacterUISetting[] settings;
    void Start()
    {
        
    }

    public void ApplySetting()
    {

        GameSettings.matchCharacters = new List<MyCharacterInfo> { };
        foreach (CharacterUISetting setting in settings)
        {
            GameSettings.matchCharacters.Add(setting.CHARINFO);
        }
        Debug.Log(GameSettings.matchCharacters.Count);
        SceneManager.LoadScene("Game");
    }
}
