using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public struct WordArea
{
    public GameObject Area;
    public List<WordPad> Pads;
    public string Word;
}
public class HangmanManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    float pointsToWin = 100;


    bool alreadyWon;

    public List<char> Banco;
    public int Difficulty = 1;

    public char CurrentChar;

    [SerializeField] public TMP_Text CurrentCharText;
    [SerializeField] public GameObject BancoArea;
    [SerializeField] public GameObject WordsArea;

    [SerializeField] public GameObject WordAreaPrefab;
    [SerializeField] public WordPad WordPadPrefab;

    [SerializeField] public List<Sprite> letterSprites;

    [SerializeField] public PlayerManager player;

    [SerializeField] public UnityEngine.UI.Button SendLetter;
    [SerializeField] public UnityEngine.UI.Button AddWord;

    SelectedPowerUpsManager choosedPowerUps;

    public List<WordArea> Words;

    PowerUpScreen powerUpScreen;

    public List<string> PastWords;
    public List<string> WordList;


    void Start()
    {
        choosedPowerUps = FindFirstObjectByType<SelectedPowerUpsManager>();
        powerUpScreen = FindFirstObjectByType<PowerUpScreen>();
        GameStateManager.instance.SetState(GameState.Playing);
    }

    string GetWord()
    {
        var nl = WordList.FindAll(x => !PastWords.Contains(x));
        var word = nl[UnityEngine.Random.Range(0, nl.Count - 1)];
        PastWords.Add(word);
        Debug.Log(word);
        return word;
    }


    private void Awake()
    {

        SendLetter.onClick.AddListener(SendLetterFunc);
        AddWord.onClick.AddListener(AddWordFunc);


        ResetGame();

        Keyboard.current.onTextInput += cha =>
        {
            if (GameStateManager.instance.GetState() != GameState.Playing) return;
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
                    SendLetterFunc();
                }
                else if (cha == (char)ConsoleKey.Delete)
                {
                    CurrentChar = ' ';
                    CurrentCharText.text = " ";
                }

            }
        };
    }

    private void ResetGame()
    {
        PastWords = new List<string>();
        WordList = new List<string>();

        var dataset = Resources.Load("words");
        WordList = dataset.ToString().Split(", ").ToList().FindAll(x => x.Length <= 9);

        Words = new List<WordArea>();

        Difficulty = 1;
        List<string> words = new List<string>() { GetWord() };
        SetupRound(words);

    }

    public void SetupRound(List<string> targetWords)
    {
        player.restore();
        scoreText.text = 0 + " / " + pointsToWin;
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

            GameObject WordArea = Instantiate(WordAreaPrefab, Vector3.zero, Quaternion.identity);
            //WordArea.transform.position = new Vector3(WordArea.transform.position.x, WordArea.transform.position.y, 1);
            //WordArea.transform.localPosition = new Vector3(WordArea.transform.localPosition.x, WordArea.transform.localPosition.y, 1);
            WordArea.transform.localScale = WordsArea.transform.localScale;
            WordArea.transform.SetParent(WordsArea.transform, false);
            word.Area = WordArea;

            List<WordPad> padList = new List<WordPad>();

            foreach (var letter in targetWord)
            {
                WordPad newPad = Instantiate(WordPadPrefab);
                newPad.Cha = letter;
                newPad.charSprite = letterSprites.Find(x => x.name == letter.ToString().ToUpper());
                padList.Add(newPad);
                newPad.transform.SetParent(WordArea.transform, false);

            }

            word.Pads = padList;

            Words.Add(word);
        }

    }

    void CheckLetter(char cha)
    {
        bool found = false;
        bool newWord = false;
        foreach (var word in Words)
        {
            if (word.Word.Contains(cha))
            {
                AddToBanco(cha);

                foreach (var pad in word.Pads.FindAll(x => x.Cha == cha))
                {
                    pad.SetFound();
                }
                if (player.points >= pointsToWin)
                {
                    CheckEnd();
                }
                else
                {
                    newWord = true;
                    
                }

                found = true;
            }
        }
        if (newWord) GetNewWord();
        //choosedPowerUps.DuringRound(GetContext());
        if (!found)
        {
            AddToBanco(cha);
            player.removeHealth();

            if (player.curLives <= 0)
            {
                choosedPowerUps.EndRound(GetContext());
                Debug.Log("morto");
                ResetGame();
            }

        }
    }
    void GetNewWord()
    {
        if (CheckIfAllFound())
        {
            AddWordFunc();
            ResetBanco();
        }
    }
    bool CheckIfAllFound()
    {
        foreach (var word in Words)
        {
            foreach (var pad in word.Pads)
            {
                if (pad.Found == false)
                {
                    return false;
                }
            }
        }
        return true;
    }
    void CheckEnd()
    {
        if (!CheckIfAllFound()) return;
        choosedPowerUps.EndRound(GetContext());
        powerUpScreen.Activate();
        Debug.Log("winRound");
        if (Difficulty == 10)
        {
            Debug.Log("win total");
        }
        else
        {
            Difficulty += 1;
            player.restore();
            List<string> words = new List<string>();
            int wordCount = (int)Math.Floor((double)Difficulty / 2);
            if (wordCount < 1) wordCount = 1;
            for (int i = 0; i < (int)Math.Floor((double)Difficulty / 2); i++)
            {
                words.Add(GetWord());
            }

            SetupRound(words);
        }
    }

    void AddToBanco(char cha)
    {
        if (!Banco.Contains(cha))
        {
            
            Banco.Add(cha);
            WordPad newPad = Instantiate(WordPadPrefab);
            newPad.Cha = cha;
            newPad.charSprite = letterSprites.Find(x => x.name == cha.ToString().ToUpper());
            newPad.SetFound();
            newPad.transform.SetParent(BancoArea.transform, false);
        }

    }
    void ResetBanco() {
        foreach (Transform child in BancoArea.transform)
        {
            Destroy(child.gameObject);
        }
        Banco = new List<char>();
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
            Points = player.points,
            Lives = player.curLives,
            Words = GetCurrentWords(),
            Won = alreadyWon,
        };
    }

    public void SendLetterFunc()
    {
        if (GameStateManager.instance.GetState() == GameState.Playing && CurrentChar != ' ')
        {
            CheckLetter(CurrentChar);
            CurrentChar = ' ';
            CurrentCharText.text = " ";
        }
        
    }
    public void AddWordFunc()
    {
        if(Words.Count < 5)
        {
            WordArea word = new WordArea();
            word.Word = GetWord();

            GameObject WordArea = Instantiate(WordAreaPrefab, Vector3.zero, Quaternion.identity);
            WordArea.transform.localScale = WordsArea.transform.localScale;
            WordArea.transform.SetParent(WordsArea.transform, false);
            word.Area = WordArea;

            List<WordPad> padList = new List<WordPad>();

            foreach (var letter in word.Word)
            {
                WordPad newPad = Instantiate(WordPadPrefab);
                newPad.Cha = letter;
                newPad.charSprite = letterSprites.Find(x => x.name == letter.ToString().ToUpper());
                padList.Add(newPad);
                newPad.transform.SetParent(WordArea.transform, false);

            }

            word.Pads = padList;

            Words.Add(word);
        }
    }
}
