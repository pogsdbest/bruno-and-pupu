using UnityEngine;

public class DeviceAndCanvasChangeDetector : MonoBehaviour
{
    private RectTransform RectTransform;
    private Vector2 LastCanvasSize;
    private ScreenOrientation LastOrientation;
    public CardOrganizer CardOrganizer;

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        if (RectTransform != null)
        {
            LastCanvasSize = RectTransform.rect.size;
        }

        LastOrientation = Screen.orientation;
    }

    void Update()
    {
        // Detect Canvas Resize
        if (RectTransform != null)
        {
            Vector2 currentSize = RectTransform.rect.size;
            if (currentSize != LastCanvasSize)
            {
                LastCanvasSize = currentSize;
                CardOrganizer.ScaleBasedOnTargetArea();
            }
        }

        // Detect Orientation Change
        if (Screen.orientation != LastOrientation)
        {
            LastOrientation = Screen.orientation;
        }
    }
}

