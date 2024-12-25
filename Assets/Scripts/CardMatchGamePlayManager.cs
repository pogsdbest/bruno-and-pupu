using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMatchGamePlayManager : MonoBehaviour
{
    public int column = 2;
    public int row = 2;

    private CardPair _cardPair;
    public List<CardPair> pairs = new List<CardPair>();
    public static event Action<int,int> OnSetupCards;

    private void Awake()
    {
        _cardPair = null;
        Card.OnCardSelected += CardSelected;
    }

    private void OnDestroy()
    {
        Card.OnCardSelected -= CardSelected;
    }

    public void Start()
    {
        SetupCards();
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
        }
        else
        {
            pair.card1.FaceDown();
            pair.card2.FaceDown();
        }
        pairs.Remove(pair);
    }
}
