using TMPro;
using UnityEngine;

public class WordPad : MonoBehaviour
{
    public char Cha;
    public TMP_Text TextBox;
    public bool Found = false;

    public WordPad(char cha)
    {
        Cha = cha;
    }

    private void Awake()
    {
        TextBox.text = "";
    }

    public void SetFound()
    {
        TextBox.text = Cha.ToString();
        Found = true;
    }
  
    void Update()
    {
        
    }
}
