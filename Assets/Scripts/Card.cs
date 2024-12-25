using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Card : MonoBehaviour
{
    public int id;
    public Sprite BackTexture;
    public Sprite FrontTexture;
    public bool IsFlipping;
    public float Speed;

    private Image Image;
    public bool IsFaceUp;

    private void Awake()
    {
        Image = GetComponent<Image>();
        IsFaceUp = false;
    }

    public void Flip(bool faceup)
    {
        IsFlipping = true;
    }

    private void FixedUpdate()
    {
        if (IsFlipping)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion wantedRotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Speed);

            if (currentRotation.eulerAngles.y < 270 && currentRotation.eulerAngles.y > 180
                || currentRotation.eulerAngles.y > 90 && currentRotation.eulerAngles.y < 180)
            {
                Image.sprite = IsFaceUp ? BackTexture : FrontTexture;
            }
            if (currentRotation == wantedRotation)
            {
                IsFaceUp = IsFaceUp ? false : true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                IsFlipping = false;
            }
        }
    }
}
