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

    public TMP_Dropdown displaySelection;
    private List<string> displayNames;

    void Start()
    {
        titleButtons.SetActive(true);
        optionsList.SetActive(false);
        levelSelectList.SetActive(false);
        backPanel.SetActive(false);

        displaySelection.ClearOptions();

        /*
        displayNames = new List<string>();

        if (Display.displays.Length > 1)
        {
            for(int x = 1; x < Display.displays.Length; x++)
            {
                Display.displays[x].Activate();
                displayNames.Add("Monitor: " + x);
            }
        }

        displaySelection.AddOptions(displayNames);
        */

    }



    public void NewGame(){
        SceneManager.LoadScene("");
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

    public void SetDisplay(int displayIndex)
    {
        if(displayIndex < Display.displays.Length)
        {
            
        }
    }




}
