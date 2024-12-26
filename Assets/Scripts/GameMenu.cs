using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public TMP_Text MessageText;
    public TMP_InputField ColumnField;
    public TMP_InputField RowField;

    public void StartButtonClick()
    {
        var column = int.Parse(ColumnField.text);
    }
}
