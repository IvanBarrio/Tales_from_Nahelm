using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI defeatTextDisplay;
    public TextMeshProUGUI victoryTextDisplay;
    public TextMeshProUGUI tutorialTextDisplay;
    public string[] sentences;
    public string[] defeatSentences;
    public string[] victorySentences;
    public string[] tutorialSentences;
    private int index;
    private int deathIndex;
    private int victoryIndex;
    private int tutorialIndex;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject acceptButton;
    public GameObject negateButton;
    public GameObject skipButton;

    //Portraits para el cambio de personaje en los dialogos
    public Texture blankP;
    public Texture nivaP;
    public Texture akiP;
    public Texture hildaP;
    public Texture annP;
    public Texture baagulP;
    public Texture baagulMP;
    public Texture omakP;
    public Texture ardsedeP;
    public Texture soldierP;

    //Imatges per el fons dels dialegs inicial i final
    public Texture map;
    public Texture battle;
    public Texture post;
    public Texture postInterior;


    void Start()
    {
        continueButton.SetActive(false);
        acceptButton.SetActive(false);
        negateButton.SetActive(false);
        defeatTextDisplay.enabled = false;
        tutorialTextDisplay.enabled = false;
        victoryTextDisplay.enabled = false;
        GameObject.Find("Portrait").GetComponent<RawImage>().texture = blankP;
        GameObject.Find("Background").GetComponent<RawImage>().texture = map;
        GameObject.Find("MusicControl").GetComponent<MusicControl>().playMusic(0);
        StartCoroutine(Type());

    }

    void Update()
    {
        if (textDisplay.enabled)
        {
            if (textDisplay.text == sentences[index])
            {
                continueButton.SetActive(true);
            }
        }
        else if (tutorialTextDisplay.enabled)
        {
            if (tutorialTextDisplay.text == tutorialSentences[index])
            {
                if (index == 2) //En aquesta posició tenim la pregunta si es vol veure el tutorial, en aquest cas mostrar les opcions si o no i actuar en conseqüencia
                {
                    acceptButton.SetActive(true);
                    negateButton.SetActive(true);
                }
                else
                    continueButton.SetActive(true);
            }
        }
        else if (defeatTextDisplay.enabled)
        {
            if (defeatTextDisplay.text == defeatSentences[index])
            {
                continueButton.SetActive(true);
            }
        }
        else if (victoryTextDisplay.enabled)
        {
            if (victoryTextDisplay.text == victorySentences[index])
            {
                continueButton.SetActive(true);
            }
        }
    }


    IEnumerator Type()
    {
        if (textDisplay.enabled == true)
        {
            foreach (char letter in sentences[index].ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else if (tutorialTextDisplay.enabled == true)
        {
            foreach (char letter in tutorialSentences[index].ToCharArray())
            {
                tutorialTextDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else if (defeatTextDisplay.enabled == true)
        {
            foreach (char letter in defeatSentences[index].ToCharArray())
            {
                defeatTextDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else if (victoryTextDisplay.enabled == true)
        {
            foreach (char letter in victorySentences[index].ToCharArray())
            {
                victoryTextDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }

    public void NextSentence()
    {

        continueButton.SetActive(false);
        if (textDisplay.enabled == true)
        {
            if (index < sentences.Length - 1)
            {
                index++;
                switch (index)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 5:
                    case 7:
                    case 9:
                    case 18:
                    case 28:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = blankP;
                        break;
                    case 3:
                    case 11:
                    case 13:
                    case 15:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = baagulP;
                        break;
                    case 4:
                    case 10:
                    case 12:
                    case 14:
                    case 16:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = omakP;
                        break;
                    case 6:
                    case 19:
                    case 21:
                    case 26:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = annP;
                        break;
                    case 8:
                    case 20:
                    case 25:
                    case 27:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = nivaP;
                        break;
                    case 17:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = ardsedeP;
                        break;
                    case 22:
                    case 24:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = akiP;
                        break;
                    case 23:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = hildaP;
                        break;
                }
                if (index == 0 || index == 2)
                    GameObject.Find("Background").GetComponent<RawImage>().texture = map;
                else if (index == 1)
                    GameObject.Find("Background").GetComponent<RawImage>().texture = battle;
                else if (index >= 9 && index <= 17)
                    GameObject.Find("Background").GetComponent<RawImage>().texture = postInterior;
                else
                    GameObject.Find("Background").GetComponent<RawImage>().texture = post;
                textDisplay.text = "";
                StartCoroutine(Type());
            }
            else
            {
                Skip();
            }
        }
        else if (defeatTextDisplay.enabled == true) {
            defeatTextDisplay.text = "";
            defeatTextDisplay.enabled = false;
            continueButton.SetActive(false);
            GameObject.Find("DialogPanel").GetComponent<Image>().enabled = false;
            GameObject.Find("Portrait").GetComponent<RawImage>().enabled = false;
            GameObject.Find("GameController").GetComponent<GameController>().setTurnState('I');
        } else if (victoryTextDisplay.enabled == true) {
            if (index < victorySentences.Length - 1)
            {
                index++;
                switch (index)
                {
                    case 0:
                    case 8:
                    case 11:
                    case 16:
                    case 32:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = blankP;
                        break;
                    case 21:
                    case 23:
                    case 29:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = akiP;
                        break;
                    case 1:
                    case 4:
                    case 6:
                    case 10:
                    case 12:
                    case 15:
                    case 24:
                    case 31:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = annP;
                        break;
                    case 2:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = baagulMP;
                        break;
                    case 3:
                    case 14:
                    case 18:
                    case 20:
                    case 22:
                    case 25:
                    case 28:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = nivaP;
                        break;
                    case 5:
                    case 9:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = ardsedeP;
                        break;
                    case 7:
                    case 30:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = hildaP;
                        break;
                    case 13:
                    case 17:
                    case 19:
                    case 26:
                    case 27:
                        GameObject.Find("Portrait").GetComponent<RawImage>().texture = baagulP;
                        break;
                }
                if (index < 16)
                    GameObject.Find("Background").GetComponent<RawImage>().texture = postInterior;
                else
                    GameObject.Find("Background").GetComponent<RawImage>().texture = post;
                victoryTextDisplay.text = "";
                StartCoroutine(Type());
            }
            else
            {
                victoryTextDisplay.text = "";
                victoryTextDisplay.enabled = false;
                continueButton.SetActive(false);
                GameObject.Find("DialogPanel").GetComponent<Image>().enabled = false;
                GameObject.Find("Portrait").GetComponent<RawImage>().enabled = false;
                GameObject.Find("Background").GetComponent<RawImage>().enabled = false;
                SceneManager.LoadScene(0);
            }
        } else if (tutorialTextDisplay.enabled == true) {
            if (index < tutorialSentences.Length - 1)
            {
                index++;
                tutorialTextDisplay.text = "";
                StartCoroutine(Type());
            }
            else
            {
                tutorialTextDisplay.text = "";
                tutorialTextDisplay.enabled = false;
                continueButton.SetActive(false);
                GameObject.Find("DialogPanel").GetComponent<Image>().enabled = false;
                GameObject.Find("GameController").GetComponent<GameController>().setTurnState('I');
                GameObject.Find("Portrait").GetComponent<RawImage>().enabled = false;
                GameObject.Find("Background").GetComponent<RawImage>().enabled = false;
            }
        }
    }

    public void Skip()
    {
        textDisplay.text = "";
        textDisplay.enabled = false;
        continueButton.SetActive(false);
        skipButton.SetActive(false);
        GameObject.Find("DialogPanel").GetComponent<Image>().enabled = false;
        GameObject.Find("GameController").GetComponent<GameController>().setTurnState('T');
        GameObject.Find("Portrait").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Background").GetComponent<RawImage>().enabled = false;
    }

    public void iniTutorial()
    {
        index = 0;
        GameObject.Find("DialogPanel").GetComponent<Image>().enabled = true;
        GameObject.Find("Portrait").GetComponent<RawImage>().texture = nivaP;
        GameObject.Find("Portrait").GetComponent<RawImage>().enabled = true;
        tutorialTextDisplay.text = "";
        tutorialTextDisplay.enabled = true;
        StartCoroutine(Type());
    }

    public void iniVictory()
    {
        index = 0;
        GameObject.Find("DialogPanel").GetComponent<Image>().enabled = true;
        GameObject.Find("Portrait").GetComponent<RawImage>().texture = blankP;
        GameObject.Find("Portrait").GetComponent<RawImage>().enabled = true;
        GameObject.Find("Background").GetComponent<RawImage>().texture = postInterior;
        GameObject.Find("Background").GetComponent<RawImage>().enabled = true;
        victoryTextDisplay.text = "";
        victoryTextDisplay.enabled = true;
        StartCoroutine(Type());
    }

    public void iniDeath(Character ch)
    {
        switch (ch.getCharName())
        {
            case "Niva":
                index = 0;
                GameObject.Find("Portrait").GetComponent<RawImage>().texture = nivaP;
                break;
            case "Aki":
                index = 1;
                GameObject.Find("Portrait").GetComponent<RawImage>().texture = akiP;
                break;
            case "Hilda":
                index = 2;
                GameObject.Find("Portrait").GetComponent<RawImage>().texture = hildaP;
                break;
            case "Ann":
                index = 3;
                GameObject.Find("Portrait").GetComponent<RawImage>().texture = annP;
                break;
            case "Ardsede":
                index = 5;
                GameObject.Find("Portrait").GetComponent<RawImage>().texture = ardsedeP;
                break;
            case "Omak":
                index = 4;
                GameObject.Find("Portrait").GetComponent<RawImage>().texture = omakP;
                break;
            default:
                index = 99;
                break;
        }
        if (index < 99)
        {
            GameObject.Find("DialogPanel").GetComponent<Image>().enabled = true;
            GameObject.Find("Portrait").GetComponent<RawImage>().enabled = true;
            defeatTextDisplay.text = "";
            defeatTextDisplay.enabled = true;
            StartCoroutine(Type());
        }
    }

    public void showTutorial(bool show)
    {
        if (show)
        {
            index = 6;
        }
        acceptButton.SetActive(false);
        negateButton.SetActive(false);
        NextSentence();
    }

}
