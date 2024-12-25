using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Card : MonoBehaviour
{
    public int ID;
    public Sprite BackTexture;
    public Sprite FrontTexture;
    public bool IsFlipping;
    public float Speed;

    private Image Image;
    private bool Rotated;
    public bool IsFaceUp;
    public bool LoadedFaceUp;

    public static event Action<Card> OnCardSelected;

    private void Awake()
    {
        Image = GetComponent<Image>();
        IsFaceUp = false;
        Rotated = false;
    }

    public void Start()
    {
        if ( LoadedFaceUp)
        {
            IsFaceUp = true;
            Image.sprite = FrontTexture;
        }
    }

    public void Flip()
    {
        if (IsFlipping || IsFaceUp)
        {
            return;
        }
        IsFlipping = true;
        OnCardSelected?.Invoke(this);
    }

    public void FaceDown()
    {
        IsFlipping = true;
    }

    private void FixedUpdate()
    {
        if (IsFlipping)
        {
            if(!Rotated)
            {
                DoRotation(Quaternion.Euler(0, 90, 0));
                if (transform.rotation.eulerAngles.y >= 90)
                {
                    Rotated = true;
                    Image.sprite = IsFaceUp ? BackTexture : FrontTexture;
                }
            }
            else
            {
                DoRotation(Quaternion.Euler(0, 0, 0));
                if (transform.rotation.eulerAngles.y <= 0)
                {
                    Rotated = false;
                    IsFlipping=false;
                    IsFaceUp = IsFaceUp ? false : true;
                }
            }
            
        }
    }

    private void DoRotation(Quaternion wantedRotation)
    {
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Speed);
    }
}
