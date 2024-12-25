using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOrganizer : MonoBehaviour
{

    public int column = 2;
    public int row = 2;

    public List<Image> cards = new List<Image>();

    public void ScaleBasedOnTargetArea()
    {
        var rectTransform = GetComponent<RectTransform>();
        var size = rectTransform.rect.size;

        GridLayoutGroup layout = GetComponent<GridLayoutGroup>();
        var renderWidth = size.x - (layout.padding.left + layout.padding.right) - (layout.spacing.x * (column - 1));
        var renderHeight = size.y - (layout.padding.top + layout.padding.bottom) - (layout.spacing.y * (row - 1));
        
        var cardWidth = renderWidth / column;
        var cardHeight = renderHeight / row;

        var cardSize = Mathf.Min(cardWidth, cardHeight);

        layout.constraintCount = column;
        foreach ( var card in cards )
        {
            layout.cellSize = new Vector2(cardSize, cardSize);
        }
    }
}
