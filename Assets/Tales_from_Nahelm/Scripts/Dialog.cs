using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
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
        GameObject.Find("Portrait").GetComponent<RawImage>().texture = blankP;
        GameObject.Find("Background").GetComponent<RawImage>().texture = map;
        GameObject.Find("MusicControl").GetComponent<MusicControl>().playMusic(0);
        StartCoroutine(Type());

    }

    void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }


    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence() {

        continueButton.SetActive(false);

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
            else if(index == 1)
                GameObject.Find("Background").GetComponent<RawImage>().texture = battle;
            else if(index >= 9 && index <= 17)
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

    public void Skip()
    {
        textDisplay.text = "";
        textDisplay.enabled = false;
        continueButton.SetActive(false);
        GameObject.Find("DialogPanel").GetComponent<Image>().enabled = false;
        GameObject.Find("GameController").GetComponent<GameController>().setTurnState('T');
        GameObject.Find("Portrait").GetComponent<RawImage>().enabled = false;
        GameObject.Find("Background").GetComponent<RawImage>().enabled = false;
    }

}
