using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;
using TMPro;
using System.Collections.Generic;

public class TitleMenuScript : MonoBehaviour
{
    public GameObject titleButtons;

    public GameObject backPanel;
    public GameObject optionsList;
    public GameObject levelSelectList;

    void Start(){
        titleButtons.SetActive(true);
        optionsList.SetActive(false);
        levelSelectList.SetActive(false);
        backPanel.SetActive(false);
    }

    public void NewGame(){
        SceneManager.LoadScene("Level1");
    }

    public void LevelSelect()
    {
        titleButtons.SetActive(false);
        levelSelectList.SetActive(true);
        backPanel.SetActive(true);
    }

    public void Options(){
        titleButtons.SetActive(false);
        optionsList.SetActive(true);
        backPanel.SetActive(true);
    }

    public void loadLevel(string s){
        SceneManager.LoadScene(s);
    }

    public void BackButton(){
        titleButtons.SetActive(true);
        levelSelectList.SetActive(false);
        optionsList.SetActive(false);
        backPanel.SetActive(false);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
