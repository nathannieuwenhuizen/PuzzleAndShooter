using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerInput : MonoBehaviour {

	[SerializeField] private int controllerID;
	
	private FirstPersonController character;
	private Weapon weapon;
	// Use this for initialization
	void Start () {
		character = GetComponent<FirstPersonController>();
		weapon = GetComponentInChildren<Weapon>();

		CheckController();
	}
	
	// Update is called once per frame
	void Update () {
		InputJump();
		InputShoot();
		InputMovement();
	}

	private void InputMovement() {
		character.RotateView(CrossPlatformInputManager.GetAxis("Vertical_r" + controllerID), CrossPlatformInputManager.GetAxis("Horizontal_r" + controllerID));
		character.MoveSpeed = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal_l" + controllerID), CrossPlatformInputManager.GetAxis("Vertical_l" + controllerID));
	}
	private void InputShoot() {
		bool s_input = false;
		switch (controllerID)
		{
			case 0:
				s_input = Input.GetButtonDown("Fire0");
			break;
			case 1:
				s_input = Input.GetKeyDown(KeyCode.Joystick1Button7);
			break;
			case 2:
				s_input = Input.GetKeyDown(KeyCode.Joystick2Button7);
			break;
			default:
				// j_input = Input.GetButtonDown("Fire0");
			break;
		}

		if (s_input) {
			weapon.Shoot();
		}
	}

	private void InputJump() {
		if (!character.JumpInput)
            {
                bool j_input = false;
                switch (controllerID)
                {
                    case 0:
                        j_input = CrossPlatformInputManager.GetButtonDown("Jump0");
                    break;
                    case 1:
                        j_input = Input.GetKeyDown(KeyCode.Joystick1Button1);
                    break;
                    case 2:
                        j_input = Input.GetKeyDown(KeyCode.Joystick2Button1);
                    break;
                    default:
                        j_input = CrossPlatformInputManager.GetButtonDown("Jump0");
                    break;
                }
                character.JumpInput = j_input;

			}
	}
	public void CheckController()
        {
            string[] names = Input.GetJoystickNames();
            for (int x = 0; x < names.Length; x++)
            {

                if (names[x].Length == 19)
                {
                    Debug.Log("PS4 CONTROLLER IS CONNECTED");
                }
                if (names[x].Length == 33)
                {
                    //  print("XBOX ONE CONTROLLER IS CONNECTED");
                }
            }
        }

}
