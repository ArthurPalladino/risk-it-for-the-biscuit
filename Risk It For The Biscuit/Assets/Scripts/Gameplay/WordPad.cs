using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordPad : MonoBehaviour
{
    public char Cha;
    public bool Found = false;
    public Image Letter;
    public Sprite charSprite;


    private void Awake()
    {
        Letter.sprite = null;
        Letter.color = new Color(0, 0, 0, 0);
    }

    public void SetFound()
    {
        Letter.color = new Color(1, 1, 1, 1);
        Letter.sprite = charSprite;
        Found = true;
    }

    void Update()
    {

    }
}
