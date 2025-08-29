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
    const int MAX_LIVES = 5;
    public int curLives = MAX_LIVES;
    public int maxGameplayLives = MAX_LIVES;

    int points;

    bool alreadyWon;

    public List<char> Banco;
    public int Difficulty = 0;

    public char CurrentChar;

    [SerializeField] public TMP_Text CurrentCharText;
    [SerializeField] public GameObject BancoArea;
    [SerializeField] public GameObject WordsArea;

    [SerializeField] public GameObject WordAreaPrefab;
    [SerializeField] public WordPad WordPadPrefab;

    [SerializeField] TextMeshProUGUI pontuationText;

    SelectedPowerUpsManager choosedPowerUps;



    public List<WordArea> Words;

    PowerUpScreen powerUpScreen;

    void Start()
    {
        pontuationText.text = "0/100";
        choosedPowerUps = FindFirstObjectByType<SelectedPowerUpsManager>();
        powerUpScreen = FindFirstObjectByType<PowerUpScreen>();
        //TIRAR TODOS SETCARDFUNC DAQUI
        powerUpScreen.SetCardFunc(choosedPowerUps.AddPowerUp);
        powerUpScreen.SetCardFunc(choosedPowerUps.AddPowerUp);
        
    }

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
                if (cha == (char)ConsoleKey.Enter && char.IsLetter(CurrentChar) && !powerUpScreen.isChoosing)
                {
                    CheckLetter(CurrentChar);
                    CurrentChar = ' ';
                    CurrentCharText.text = "";
                }
                else if (cha == (char)ConsoleKey.Delete || cha == (char)ConsoleKey.Backspace)
                {
                    CurrentChar = ' ';
                    CurrentCharText.text = "";
                }

            }
        };
    }

    public void SetupRound(List<string> targetWords)
    {
        
        curLives = maxGameplayLives;

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
        //choosedPowerUps.DuringRound(GetContext());
        if (!found)
        {
            AddToBanco(cha);
            curLives -= 1;

            if (curLives <= 0)
            {
                choosedPowerUps.EndRound(GetContext());
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


        choosedPowerUps.EndRound(GetContext());
        powerUpScreen.Activate();
        Debug.Log("win");
        Difficulty += 1;
        curLives = maxGameplayLives;
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


    List<string> GetCurrentWords()
    {
        List<string> currentWords = new List<string>();
        foreach (var word in Words)
        {
            currentWords.Add(word.Word);
        }
        return currentWords;
    }

    PowerUpContext GetContext()
    {
        return new PowerUpContext
        {
            CurChar = CurrentChar,
            Points = points,
            PointsToSumBeforePowerUps = 0,
            Lives = curLives,
            Words = GetCurrentWords(),
            Won = alreadyWon,
            Lose = curLives <= 0
        };
    }

}
