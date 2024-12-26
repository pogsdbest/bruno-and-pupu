using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOrganizer : MonoBehaviour
{

    private int _column;
    private int _row;

    public GameObject CardPrefab;

    public List<Card> cards = new List<Card>();

    public static event Action<SaveWrapper> OnSaveGame;

    private void Awake()
    {
        CardMatchGamePlayManager.OnSetupCards += SetupCards;
        CardMatchGamePlayManager.OnLoadCards += LoadCards;
        CardMatchGamePlayManager.OnCardMatched += CardMatched;
    }

    private void OnDestroy()
    {
        CardMatchGamePlayManager.OnSetupCards -= SetupCards;
        CardMatchGamePlayManager.OnLoadCards -= LoadCards;
        CardMatchGamePlayManager.OnCardMatched -= CardMatched;
    }

    private void LoadCards(SaveWrapper save)
    {
        this._column = save.Col;
        this._row = save.Row;

        foreach(var cardData in save.Cards)
        {
            var card = CreateCard(cardData.ID);
            if(cardData.IsFaceUp)
            {
                card.LoadedFaceUp = true;
            }
        }

        AddCardsToPanel();

        ScaleBasedOnTargetArea();
    }

    private void CardMatched()
    {
        SaveWrapper save = new()
        {
            Col = _column,
            Row = _row,
            Cards = GetData(cards)
        };
        OnSaveGame?.Invoke(save);
    }
    private List<CardData> GetData(List<Card> cards)
    {
        var cardData = new List<CardData>();
        foreach (var card in cards)
        {
            CardData data = new()
            {
                ID = card.ID,
                IsFaceUp = card.IsFaceUp
            };
            cardData.Add(data);
        }
        return cardData;
    }

    private void SetupCards(int column, int row)
    {
        this._column = column;
        this._row = row;

        Debug.Log("setting up cards");

        var totalCards = column * row;

        for(int i = 0; i < totalCards/2; i++)
        {
            var id = UnityEngine.Random.Range(0, GetComponent<SpriteLoader>().SpriteList.Count);

            //First Card
            CreateCard(id);
            //Pair
            CreateCard(id);
        }

        Shuffle(cards);
        AddCardsToPanel();

        ScaleBasedOnTargetArea();
    }

    private Card CreateCard(int id)
    {
        GameObject cardObject = Instantiate(CardPrefab, transform.position, CardPrefab.transform.rotation);
        Card card = cardObject.GetComponent<Card>();
        card.ID = id;
        card.FrontTexture = GetComponent<SpriteLoader>().GetSprite(id);
        cardObject.name = card.FrontTexture.name;

        cards.Add(card);
        return card;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void AddCardsToPanel()
    {
        foreach(var card in cards)
        {
            card.gameObject.transform.SetParent(gameObject.transform, false);
        }
    }

    public void ScaleBasedOnTargetArea()
    {
        var rectTransform = GetComponent<RectTransform>();
        var size = rectTransform.rect.size;

        GridLayoutGroup layout = GetComponent<GridLayoutGroup>();
        var renderWidth = size.x - (layout.padding.left + layout.padding.right) - (layout.spacing.x * (_column - 1));
        var renderHeight = size.y - (layout.padding.top + layout.padding.bottom) - (layout.spacing.y * (_row - 1));
        
        var cardWidth = renderWidth / _column;
        var cardHeight = renderHeight / _row;

        var cardSize = Mathf.Min(cardWidth, cardHeight);

        layout.constraintCount = _column;
        foreach ( var card in cards )
        {
            layout.cellSize = new Vector2(cardSize, cardSize);
        }
    }
}
