using TMPro;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    public TMP_Text Matches;
    public TMP_Text Turns;
    public TMP_Text Score;
    public TMP_Text Combo;

    public void Awake()
    {
        CardMatchGamePlayManager.OnUpdateInfoDisplay += UpdateInfoDisplay;
    }

    public void OnDestroy()
    {
        CardMatchGamePlayManager.OnUpdateInfoDisplay -= UpdateInfoDisplay;
    }

    private void UpdateInfoDisplay(int matches, int turns, int score, int combo)
    {
        Matches.text = "Matches: "+matches;
        Turns.text = "Turns: "+turns;
        Score.text = "Score: "+score;
        Combo.text = "Combo: "+combo;
    }
}
