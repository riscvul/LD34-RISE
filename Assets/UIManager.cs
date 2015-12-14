using UnityEngine;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour {

    CanvasGroup Title;
    CanvasGroup MainMenu;

    bool newGame = true;
    public static bool menuShown = true;
    public Teleprompter teleprompter;

	// Use this for initialization
	void Start () {
        Title = transform.FindChild("Title").GetComponent<CanvasGroup>();
        MainMenu = transform.FindChild("MainMenu").GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuShown) {
            MainMenu.blocksRaycasts = true;
            MainMenu.interactable = true;
            MainMenu.alpha = 1;

            Title.alpha = 1;
        }
	}

    public void StartResumeGame() {
        MainMenu.blocksRaycasts = false;
        MainMenu.interactable = false;
        MainMenu.alpha = 0;

        if(newGame) {
            Invoke("BeginningMessage", 2f);
            MainMenu.transform.FindChild("Panel/StartResumeButton/TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = "CONTINUE";
            LeanTween.value(gameObject, CameraZoomCallback, 3, 8, 2);
            newGame = false;
        }

        LeanTween.value(gameObject, TitleAlphaCallback, 1, 0, 2);
        menuShown = false;
    }

    void BeginningMessage() {
        teleprompter.PrepMessage("The world ended 4 weeks ago... \n It doesn't matter how.");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void TitleAlphaCallback(float value) {
        Title.alpha = value;
    }

    public void CameraZoomCallback(float value) {
        Camera.main.orthographicSize = value;
    }
}
