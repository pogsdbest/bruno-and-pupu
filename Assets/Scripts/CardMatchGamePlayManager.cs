using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchGamePlayManager : MonoBehaviour
{
    public int column = 2;
    public int row = 2;

    public bool HasSave = false;

    private CardPair _cardPair;
    public List<CardPair> pairs = new List<CardPair>();

    public static event Action<int,int> OnSetupCards;
    public static event Action<SaveWrapper> OnLoadCards;
    public static event Action OnCardMatched;

    private void Awake()
    {
        _cardPair = null;
        HasSave =  GetComponent<SaveLoadManager>().CheckSaveFile();
        Card.OnCardSelected += CardSelected;
    }

    private void OnDestroy()
    {
        Card.OnCardSelected -= CardSelected;
    }

    public void Start()
    {
        if (HasSave)
        {
            GetComponent<SaveLoadManager>().LoadSaveFile( (saveFile) =>
            {
                OnLoadCards(saveFile);
            });
        }
        else
        {
            SetupCards();
        }
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
            //score

            OnCardMatched?.Invoke();
        }
        else
        {
            pair.card1.FaceDown();
            pair.card2.FaceDown();
        }
        pairs.Remove(pair);
    }
}
