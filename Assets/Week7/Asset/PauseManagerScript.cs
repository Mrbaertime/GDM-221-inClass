using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Canvas))]
public class PauseManagerScript : MonoBehaviour
{
    private InputTime playerControls;

    Canvas canvas;
	private bool m_player;

    private void Awake()
    {
        playerControls = new InputTime(); // InputAction 
    }
    private void OnEnable()
    {
        playerControls.Enable();
		playerControls.menu.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.menu.Pause.performed -= OnPause;
    }

    void Start()
	{
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
    }


    void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Escape))
		//{
		//	canvas.enabled = !canvas.enabled;
		//	Pause();
		//}
		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	QuitGame();
		//}
	}

	private void QuitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
	
	public void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {

		Time.timeScale = Time.timeScale == 0 ? 1 : 0;

		canvas.enabled = !canvas.enabled;
		GameManager.I?.SetPause(m_player);
    }
}
