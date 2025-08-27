using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public struct WordArea
{
    public GameObject Area;
    public List<WordPad> Pads;
    public string Word;
}
public class HangmanManager : MonoBehaviour
{
    public List<char> Banco;
    public int Lives = 5;
    public int Difficulty = 0;

    public char CurrentChar;

    [SerializeField] public TMP_Text CurrentCharText;
    [SerializeField] public GameObject BancoArea;
    [SerializeField] public GameObject WordsArea;

    [SerializeField] public GameObject WordAreaPrefab;
    [SerializeField] public WordPad WordPadPrefab;

    public List<WordArea> Words;


    private void Awake()
    {
        Words = new List<WordArea>();
        Difficulty = 0;
        List<string> words = new List<string>() { "teste" };
        SetupRound(words);

        Keyboard.current.onTextInput += cha =>
        {

            if (char.IsLetter(cha))
            {
                if (!Banco.Contains(cha))
                {
                    CurrentChar = cha;
                    CurrentCharText.text = cha.ToString();
                }
            }
            else
            {
                if (cha == (char)ConsoleKey.Enter && char.IsLetter(CurrentChar))
                {
                    CheckLetter(CurrentChar);
                    CurrentChar = ' ';
                    CurrentCharText.text = "";
                }
                else if (cha == (char)ConsoleKey.Delete)
                {
                    CurrentChar = ' ';
                    CurrentCharText.text = "";
                }

            }
        };
    }
  
    public void SetupRound(List<string> targetWords)
    {
        Lives = 5;

        foreach (Transform child in BancoArea.transform)
        {
            Destroy(child.gameObject);
        }
        Banco = new List<char>();

        foreach (var word in Words)
        {
            foreach (var pad in word.Pads)
            {
                Destroy(pad);

            }

            Destroy(word.Area);
        }

        Words = new List<WordArea>();

        foreach (var targetWord in targetWords)
        {
            WordArea word = new WordArea();
            word.Word = targetWord;

            GameObject WordArea = Instantiate(WordAreaPrefab);
            WordArea.transform.SetParent(WordsArea.transform);
            word.Area = WordArea;

            List<WordPad> padList = new List<WordPad>();

            foreach (var letter in targetWord)
            {
                WordPad newPad = Instantiate(WordPadPrefab);
                newPad.Cha = letter;
                padList.Add(newPad);
                newPad.transform.SetParent(WordArea.transform);

            }

            word.Pads = padList;

            Words.Add(word);
        }
        
    }

    void CheckLetter(char cha)
    {
        bool found = false;

        foreach (var word in Words)
        {
            if (word.Word.Contains(cha))
            {
                AddToBanco(cha);

                foreach (var pad in word.Pads.FindAll(x => x.Cha == cha))
                {
                    pad.SetFound();
                }

                CheckEnd();
                found = true;
            }
        }
       
        if(!found)
        {
            AddToBanco(cha);
            Lives = Lives - 1;

            if(Lives <= 0)
            {
                Debug.Log("morto");
            }
           
        }
    }

    void CheckEnd()
    {

        foreach (var word in Words)
        {
            foreach (var pad in word.Pads)
            {
                if (pad.Found == false)
                {
                    return;
                }
            }
        }


        Debug.Log("win");
        Difficulty += 1;
        List<string> words = new List<string>() { "teste", "morte" };

        SetupRound(words);
    }


    void AddToBanco(char cha)
    {
        if (!Banco.Contains(cha))
        {
            Banco.Add(cha);
            WordPad newPad = Instantiate(WordPadPrefab);
            newPad.Cha = cha;
            newPad.SetFound();
            newPad.transform.SetParent(BancoArea.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
