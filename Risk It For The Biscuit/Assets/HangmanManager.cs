using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HangmanManager : MonoBehaviour
{
    public List<char> Banco;
    public string TargetWord;
    public int Lives = 5;

    public char CurrentChar;

    [SerializeField] public TMP_Text CurrentCharText;
    [SerializeField] public GameObject WordArea;
    [SerializeField] public GameObject BancoArea;

    [SerializeField] public WordPad WordPadPrefab;
    public List<WordPad> Pads;

    private void Awake()
    {

        SetupRound("teste");

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
  
    public void SetupRound(string targetWord)
    {
        TargetWord = targetWord;
        foreach (var pad in Pads)
        {
            Destroy(pad);
        }

        Pads = new List<WordPad>();

        var i = 0;
        foreach (var letter in TargetWord)
        {
            WordPad newPad = Instantiate(WordPadPrefab);
            newPad.Cha = letter;
            Pads.Add(newPad);
            newPad.transform.SetParent(WordArea.transform);
            
        }
    }

    void CheckLetter(char cha)
    {
        if (TargetWord.Contains(cha))
        {
            AddToBanco(cha);

            foreach (var pad in Pads.FindAll(x => x.Cha == cha))
            {
                pad.SetFound();
            }

            CheckEnd();

        }
        else
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

        foreach (var pad in Pads)
        {
            if(pad.Found == false)
            {
                return;
            }
        }

        Debug.Log("win");
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
