using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
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
    [SerializeField] public float pointsToWin = 0;

    public char lastChar;
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


    public List<WordArea> Words;

    PowerUpScreen powerUpScreen;

    public List<string> PastWords;
    public List<string> WordList;

    [SerializeField] AudioClip lifeLostSound;

    [SerializeField] AudioClip GameMusic;

    [SerializeField] AudioClip LostGameSound;

    [SerializeField] FinalScreen finalScreen;
    void Start()
    {
        powerUpScreen = FindFirstObjectByType<PowerUpScreen>();
        var pu= powerUpScreen.SelectedPowerUp;
        foreach (var card in powerUpScreen.cards)
        {
            card.onSelect += (pu) =>
            {
                foreach (var p in SelectedPowerUpsManager.Instance.GetSelectedPowerUps()) {
                    if (p.powerUpType == PowerUpType.GameplayLife)
                    {
                        p.Apply(GetContext(0));
                    }
                }

                Difficulty += 1;
                pointsToWin = Difficulty * 30;
                player.restoreRound();
                List<string> words = new List<string>();
                int wordCount = (int)Math.Floor((double)Difficulty / 2);
                if (wordCount < 1) wordCount = 1;
                for (int i = 0; i < (int)Math.Floor((double)Difficulty / 2); i++)
                {
                    words.Add(GetWord());
                }
                CleanWordsArea();
                SetupRound(words);
                powerUpScreen.Close();
                GameStateManager.instance.SetState(GameState.Playing);
            };
        }
        GameStateManager.instance.SetState(GameState.Playing);
        AudioManager.Instance.Play(new Audio { Clip = GameMusic, Loop = true });
        finalScreen.SetAction(ResetGame);
        ResetGame();
    }

    string GetWord()
    {
        var nl = WordList.FindAll(x => !PastWords.Contains(x));

        var powerUps = SelectedPowerUpsManager.Instance.GetSelectedPowerUps();
        var pu = powerUps.FirstOrDefault(p => !p.alreadyActivate && p.powerUpType == PowerUpType.WordBuyer);
        if (pu != null)
        {
            if(pu.name == "Now I Kinda Like These Letters")
            {
                if(UnityEngine.Random.Range(0, 10) <= 2) //20%
                {
                    var nnl = nl.FindAll(x => x.Contains("x") || x.Contains("y") || x.Contains("w"));
                    if (nl.Count > 0) nl = nnl;
                }
            }
        }

        var word = nl[UnityEngine.Random.Range(0, nl.Count - 1)];
        PastWords.Add(word);
        Debug.Log(word);
        return word;
    }

    private void Awake()
    {
        pointsToWin = Difficulty * 30;
        SendLetter.onClick.AddListener(SendLetterFunc);
        AddWord.onClick.AddListener(() =>
        {
            if (Banco.Count==0) {

                AddWordFunc();
            }
     
        });

    
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
        powerUpScreen.ResetPowerUps();
        AddWord.interactable = true;
        PastWords = new List<string>();
        WordList = new List<string>();

        var dataset = Resources.Load("words");
        WordList = dataset.ToString().Split(", ").ToList().FindAll(x => x.Length <= 9);
        CleanWordsArea();
        Words = new List<WordArea>();

        Difficulty = 1;
        pointsToWin = Difficulty * 30;
        List<string> words = new List<string>() { GetWord() };
        player.RestoreGameplay();
        SetupRound(words);

    }

    void CleanWordsArea()
    {
        foreach (Transform child in BancoArea.transform)
        {
            Destroy(child.gameObject);
        }
        if (Words == null) return;
        foreach (var word in Words)
        {
            foreach (var pad in word.Pads)
            {
                Destroy(pad);

            }

            Destroy(word.Area);
        }
    }
    public void SetupRound(List<string> targetWords)
    {
        scoreText.text = 0 + " / " + pointsToWin;

        Banco = new List<char>();

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
        AddWord.interactable = false;
        cha =char.ToLower(cha);
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
                
                CountPoints(word.Pads.Count(x => x.Cha == cha));
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
        if (!found)
        {
            AddToBanco(cha);
            if (player.curLives - 1 <= 0)
            {
                var powerUps = SelectedPowerUpsManager.Instance.GetSelectedPowerUps();
                var pu = powerUps.FirstOrDefault(p => !p.alreadyActivate && p.powerUpType == PowerUpType.LastChance);
                Debug.Log(pu);
                if (pu == null)
                {
                    SelectedPowerUpsManager.Instance.ClearPowerUps();
                    player.RestoreGameplay();
                    AudioSource.PlayClipAtPoint(LostGameSound, Camera.main.transform.position, 0.3f);
                    finalScreen.ActivateFinalScreen("You Lose!");
                }
                else
                {
                    pu.alreadyActivate = true;
                    pu.Apply(GetContext(0));

                }
                return;

            }
            else
            {
                AudioSource.PlayClipAtPoint(lifeLostSound, Camera.main.transform.position, 0.3f);
            }
            player.removeHealth();

        }
    }


    void CountPoints(int rightLetters)
    {
        var pus = SelectedPowerUpsManager.Instance.GetSelectedPowerUps();
        var context = GetContext(rightLetters);
        player.actualMuitpl = player.baseMultipl;
        //Debug.Log("MULTIPLICADOR ANTES DA MUDANCA: " + player.actualMuitpl);
        foreach (var pu in pus)
        {
            if (pu.powerUpType == PowerUpType.Points)
            {
                pu.Apply(context);
            }
        }
        //Debug.Log("MULTIPLICADOR DEPOIS DA MUDANCA: " + player.actualMuitpl);
        //Debug.Log("PONTOS ANTES DA MUDANCA: " + player.points);
        player.points += rightLetters * player.actualMuitpl;
        //Debug.Log("PONTOS DEPOIS DA MUDANCA: " + player.points);
        scoreText.text = player.points + " / " + pointsToWin;
        player.actualMuitpl = player.baseMultipl;
    }
    void GetNewWord()
    {
        if (CheckIfAllFound())
        {
            if (Words.Count <= 5)
            {
                AddWordFunc();
                ResetBanco();
            }
            else
            {
                SelectedPowerUpsManager.Instance.ClearPowerUps();
                player.RestoreGameplay();
                AudioSource.PlayClipAtPoint(LostGameSound, Camera.main.transform.position, 0.3f);
                finalScreen.ActivateFinalScreen("You Lose! Maximum Words Reached!");
            }
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
        
        player.points = 0;
        if (Difficulty == 10)
        {
            finalScreen.ActivateFinalScreen("You Win!");
        }
        else
        {
            powerUpScreen.Activate();
            // Difficulty += 1;
            // player.restoreRound();
            // List<string> words = new List<string>();
            // int wordCount = (int)Math.Floor((double)Difficulty / 2);
            // if (wordCount < 1) wordCount = 1;
            // for (int i = 0; i < (int)Math.Floor((double)Difficulty / 2); i++)
            // {
            //     words.Add(GetWord());
            // }
            // CleanWordsArea();
            // SetupRound(words);
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

    PowerUpContext GetContext(int rightLetters)
    {
        return new PowerUpContext
        {
            RightLetters = rightLetters,
            player = player,
            CurChar = CurrentChar,
            Points = player.points,
            LastChar = lastChar,
            Words = GetCurrentWords(),
            Won = alreadyWon,
        };
    }

    public void SendLetterFunc()
    {
        if (GameStateManager.instance.GetState() == GameState.Playing && CurrentChar != ' ')
        {
            CheckLetter(CurrentChar);
            lastChar = CurrentChar;   
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
