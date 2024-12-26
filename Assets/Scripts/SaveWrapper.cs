using System.Collections.Generic;

[System.Serializable]
public class SaveWrapper
{
    public int Col;
    public int Row;
    public int Matches;
    public int Turns;
    public int Score;
    public int Combo;
    public List<CardData> Cards;
}
