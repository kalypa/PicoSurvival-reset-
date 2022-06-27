using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
    private bool isPause;
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private GameObject status;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            pauseUI.SetActive(isPause);
            status.SetActive(!isPause);
            if (isPause == true)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    public void OnClickExitButton()
    {
        Application.Quit();
    }
    public void OnClickContinueButton()
    {
        isPause = !isPause;
        pauseUI.SetActive(isPause);
        Time.timeScale = 1f;
    }
    public void OnClickTutorialButton()
    {
        tutorialUI.SetActive(true);
        pauseUI.SetActive(false);
        status.SetActive(false);
        Time.timeScale = 0f;
    }
    public void OnClickExitTutorialButton()
    {
        status.SetActive(true);
        tutorialUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
