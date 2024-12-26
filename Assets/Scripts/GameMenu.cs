using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public TMP_Text MessageText;
    public TMP_InputField ColumnField;
    public TMP_InputField RowField;
    public Button LoadButton;

    public static event Action<int,int> OnStartGame;
    public static event Action OnLoadGame;

    public void Awake()
    {
        gameObject.SetActive(false);
        LoadButton.interactable = false;
        CardMatchGamePlayManager.OnShowMenu += ShowMenu;
    }

    public void OnDestroy()
    {
        CardMatchGamePlayManager.OnShowMenu -= ShowMenu;
    }

    private void ShowMenu(string message, bool hasSaveFile)
    {
        MessageText.text = message;
        ColumnField.text = "4";
        RowField.text = "4";
        gameObject.SetActive(true);

        LoadButton.interactable = hasSaveFile;
    }

    public void StartButtonClick()
    {
        var column = int.Parse(ColumnField.text);
        var row = int.Parse(RowField.text);
        //just to make sure card count are multiples of 2
        if((column * row) % 2 == 0)
        {
            OnStartGame?.Invoke(column, row);
            gameObject.SetActive(false);
        }
        else
        {
            MessageText.text = column + " x " + row + " is not multiples of 2.";
        }
    }

    public void LoadButtonClick()
    {
        OnLoadGame?.Invoke();
        gameObject.SetActive(false);
    }
}
