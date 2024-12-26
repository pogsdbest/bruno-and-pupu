using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchGamePlayManager : MonoBehaviour
{
    public int column = 2;
    public int row = 2;

    public int ScoreReward;

    public int Matches;
    public int Turns;
    public int Score;
    public int Combo;

    private CardPair _cardPair;
    public List<CardPair> pairs = new List<CardPair>();

    public static event Action<int,int> OnSetupCards;
    public static event Action<SaveWrapper> OnLoadCards;
    public static event Action OnCardMatched;
    public static event Action<int,int,int,int> OnUpdateInfoDisplay;

    private void Awake()
    {
        _cardPair = null;
        Card.OnCardSelected += CardSelected;
        CardOrganizer.OnSaveGame += SaveGame;
    }

    private void OnDestroy()
    {
        Card.OnCardSelected -= CardSelected;
        CardOrganizer.OnSaveGame -= SaveGame;
    }

    public void Start()
    {
        if (GetComponent<SaveLoadManager>().CheckSaveFile())
        {
            GetComponent<SaveLoadManager>().LoadSaveFile( (saveFile) =>
            {
                Matches = saveFile.Matches;
                Turns = saveFile.Turns;
                Score = saveFile.Score;
                Combo = saveFile.Combo;
                OnUpdateInfoDisplay?.Invoke(Matches, Turns, Score, Combo);
                OnLoadCards(saveFile);
            });
        }
        else
        {
            SetupCards();
        }
    }

    public void SaveGame(SaveWrapper save)
    {
        save.Matches = Matches;
        save.Turns = Turns;
        save.Score = Score;
        save.Combo = Combo;
        StartCoroutine(GetComponent<SaveLoadManager>().SaveGameAsync(save));
    }

    public void SetupCards()
    {
        OnSetupCards?.Invoke(column, row);
    }

    private void CardSelected(Card card)
    {
        if (_cardPair == null)
        {
            _cardPair = new CardPair();
            _cardPair.card1 = card;
        }
        else
        {
            if(_cardPair.card1 == null)
            {
                _cardPair.card1 = card;
                return;
            }
            else
            {
                _cardPair.card2 = card;
                pairs.Add(_cardPair);
                StartCoroutine(CheckCardPair(_cardPair));
                _cardPair = new CardPair();
                
            }
        }
    }

    private IEnumerator CheckCardPair(CardPair pair)
    {
        while( (pair.card1.IsFlipping) || 
            (pair.card2.IsFlipping) )
        {
            yield return null;
        }
        if(pair.card1.name.Equals(pair.card2.name))
        {
            Matches += 1;
            Turns += 1;
            Score += ScoreReward + (ScoreReward * Combo);
            Combo += 1;

            OnUpdateInfoDisplay?.Invoke(Matches, Turns, Score, Combo);
            OnCardMatched?.Invoke();
        }
        else
        {
            Turns += 1;
            Combo = 0;

            pair.card1.FaceDown();
            pair.card2.FaceDown();

            OnUpdateInfoDisplay?.Invoke(Matches, Turns, Score, Combo);
        }
        pairs.Remove(pair);
    }
}
