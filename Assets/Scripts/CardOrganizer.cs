using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOrganizer : MonoBehaviour
{

    private int _column;
    private int _row;

    public GameObject CardPrefab;

    public List<Card> cards = new List<Card>();

    private void Awake()
    {
        CardMatchGamePlayManager.OnSetupCards += SetupCards;
    }

    private void OnDestroy()
    {
        CardMatchGamePlayManager.OnSetupCards -= SetupCards;
    }

    private void SetupCards(int column, int row)
    {
        this._column = column;
        this._row = row;

        Debug.Log("setting up cards");

        var totalCards = column * row;
        var spriteLoader = GetComponent<SpriteLoader>();

        for(int i = 0; i < totalCards/2; i++)
        {
            var randomSprite = spriteLoader.SpriteList[ Random.Range(0, spriteLoader.SpriteList.Count)];

            //First Card
            CreateCard(randomSprite);
            //Pair
            CreateCard(randomSprite);
        }

        Shuffle(cards);
        AddCardsToPanel();

        ScaleBasedOnTargetArea();
    }

    private void CreateCard(Sprite sprite)
    {
        GameObject cardObject = Instantiate(CardPrefab, transform.position, CardPrefab.transform.rotation);
        Card card = cardObject.GetComponent<Card>();
        card.FrontTexture = sprite;
        cardObject.name = sprite.name;

        cards.Add(card);
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
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
