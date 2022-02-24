using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject UI;

    public GameObject Info;

    public SceneRunner runner;
    public Carro carro;

    private Vector3 originalCarPos;

    public Instructions instructions;
    void Start()
    {
        EnableUI();
        originalCarPos = carro.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.activeSelf && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            StartGame();
        }
    }

    public void EnableUI()
    {
        UI.SetActive(true);
    }
    public void StartGame()
    {
        UI.SetActive(false);
        runner.ObstaculosHabilitados = true;
        carro.morto = false;
        carro.Axis = 0;
        carro.Invoke("EnableTouchInput", 0.7f);
        instructions.ShowInstructions();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        carro.transform.position = originalCarPos;
        carro.transform.rotation = Quaternion.identity;
        carro.TouchInputEnabled = false;
        runner.ClearScene();
        runner.Parado = false;
        runner.Velocity = 1;
        UI.SetActive(true);

    }


    public void DoInfo()
    {
        if (Info.activeSelf)
        {
            Info.SetActive(false);
        }
        else
        {
            Info.SetActive(true);
        }
    }

    public void GoToInstagram()
    {
        Application.OpenURL("https://www.instagram.com/titoguidotti");
    }
}
