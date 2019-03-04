using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUISetting : MonoBehaviour
{
    private Toggle AItoggle;
    private Dropdown controllerdropdown;
    public void Start()
    {
        AItoggle = GetComponentInChildren<Toggle>();
        controllerdropdown = GetComponentInChildren<Dropdown>();
    }
    private bool AI
    {
        get
        {
            return AItoggle.isOn;
        }
    }
    private int CONTROLLER
    {
        get
        {
            return controllerdropdown.value;
        }
    }
    public void Update()
    {
        //Debug.Log(AI + " | " + CONTROLLER);
    }
    public MyCharacterInfo CHARINFO
    {
        get
        {
            MyCharacterInfo tempInfo = new MyCharacterInfo();
            tempInfo.goal = 1;
            tempInfo.controlledBy = CONTROLLER;
            tempInfo.AI = AI;
            return tempInfo;
        }
    }

}
