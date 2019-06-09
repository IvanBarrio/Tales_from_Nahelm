using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UnitMenuController : MonoBehaviour
{
    // Botons per al menu d'accions
    public GameObject cmdMove;
    public GameObject cmdAttack;
    public GameObject cmdInv;
    public GameObject cmdTrade;
    public GameObject cmdWait;
    public GameObject cmdHeal;
    public GameObject cmdTake;

    //Botons per al menu de selecció d'arma i moviment per el menu d'objectes
    public GameObject cmdObj1;
    public GameObject cmdObj2;
    public GameObject cmdObj3;
    public GameObject cmdObj4;
    public GameObject cmdObj5;

    public GameObject cmdUseEquip;
    public GameObject cmdDrop;

    public bool selectedItem;
    public int sItem;

    //Botons per al menu d'opcions
    public GameObject cmdHTP;
    public GameObject cmdExit;
    public GameObject cmdEndTurn;
    public GameObject cmdMusic;
    public GameObject cmdSFX;

    public AudioSource source;
    public AudioClip book;

    public Texture check;
    public Texture notCheck;


    // Start is called before the first frame update
    void Start()
    {
        displayUnitMenu(false);
        displayWeaponMenu(false, 'I');
        displayItemUsage(false, 0);
        displayPauseMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Funció que mostra o fa desapareixer el menu d'unitat
     */
    public void displayUnitMenu(bool show)
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
        string selChar = gc.getSelectedCharacter();

        source.PlayOneShot(book);

        GameObject.Find("UnitActionsPanel").GetComponent<RawImage>().enabled = show;
        cmdMove.SetActive(show);
        cmdAttack.SetActive(show);
        cmdHeal.SetActive(show);
        cmdInv.SetActive(show);
        cmdWait.SetActive(show);
        cmdTake.SetActive(show);


        if (show)
        {
            //Si el menu esta activo mostrar las acciones que puede hacer el personaje
            if (!GameObject.Find(selChar).GetComponent<Character>().getCanMove())
               cmdMove.SetActive(false);

            GameObject[] en = gc.getEnemiesInRange(GameObject.Find(selChar).transform.position, 4f, GameObject.Find(selChar).tag); //El rang que es passa sera el rang que tingui l'arma d'atac
            if (en[0] == null || en.Length == 0)
               cmdAttack.SetActive(false);

            GameObject[] al = gc.getAliesInRange(GameObject.Find(selChar).transform.position, 4f, GameObject.Find(selChar).tag); //El rang que es passa sera el rang que tingui l'arma d'atac
            if (al[0] != null)
            {
                //Comprobar si la unidat pot curar a algú si el te a rang
                if (al[0] == GameObject.Find(selChar) || !GameObject.Find(selChar).GetComponent<Character>().canHeal())
                    cmdHeal.SetActive(false);
            }
            
            /*
             * Si el general enemic segueix en peu no es pot conquerir el castell. Quan el general enemic mori la opció de conquerir serà habilitada.
             */
            if (GameObject.Find("Omak") != null)
                cmdTake.SetActive(false);
        }
    }

    //Controles del menu de seleccion de armas
    public void displayWeaponMenu(bool show, char ent)
    {

        //source = GameObject.Find("SFXSource").GetComponent<AudioSource>();
        source.PlayOneShot(book);

        selectedItem = false;
        sItem = 9;
        
        //ent -> A (desde la seecció d'atac) -> I (desde la selecció d'inventari
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
        string selChar = gc.getSelectedCharacter();

        GameObject.Find("InventoryPanel").GetComponent<RawImage>().enabled = show;
        cmdObj1.SetActive(show);
        cmdObj2.SetActive(show);
        cmdObj3.SetActive(show);
        cmdObj4.SetActive(show);
        cmdObj5.SetActive(show);

        if (show)
        {
            if (ent == 'I')
            {
                gc.setTurnState('W');
                gc.setMenuState(6);
            }

            //Només mostrarem el nombre d'objectes que tingui el personatge seleccionat a l'inventari
            Item[] inventory = GameObject.Find(selChar).GetComponent<Character>().getInventory();
            if (inventory[0] != null)
            {
                if (ent == 'A' && inventory[0].getiType() == "Weapon")
                    cmdObj1.GetComponent<TextMeshProUGUI>().text = inventory[0].getName();
                else if (ent == 'I' || ent == 'X')
                    cmdObj1.GetComponent<TextMeshProUGUI>().text = inventory[0].getName();
                else
                    cmdObj1.SetActive(false);
            }
            else
                cmdObj1.SetActive(false);

            if (inventory[1] != null)
            {
                if (ent == 'A' && inventory[1].getiType() == "Weapon")
                    cmdObj2.GetComponent<TextMeshProUGUI>().text = inventory[1].getName();
                else if (ent == 'I' || ent == 'X')
                    cmdObj2.GetComponent<TextMeshProUGUI>().text = inventory[1].getName();
                else
                    cmdObj2.SetActive(false);
            }
            else
                cmdObj2.SetActive(false);

            if (inventory[2] != null)
                if (ent == 'A' && inventory[2].getiType() == "Weapon")
                    cmdObj3.GetComponent<TextMeshProUGUI>().text = inventory[2].getName();
                else if (ent == 'I' || ent == 'X')
                    cmdObj3.GetComponent<TextMeshProUGUI>().text = inventory[2].getName();
                else
                    cmdObj3.SetActive(false);
            else
                cmdObj3.SetActive(false);

            if (inventory[3] != null)
                if (ent == 'A' && inventory[3].getiType() == "Weapon")
                    cmdObj4.GetComponent<TextMeshProUGUI>().text = inventory[3].getName();
                else if (ent == 'I' || ent == 'X')
                    cmdObj4.GetComponent<TextMeshProUGUI>().text = inventory[3].getName();
                else
                    cmdObj4.SetActive(false);
            else
                cmdObj4.SetActive(false);

            if (inventory[4] != null)
                if (ent == 'A' && inventory[4].getiType() == "Weapon")
                    cmdObj5.GetComponent<TextMeshProUGUI>().text = inventory[4].getName();
                else if (ent == 'I' || ent == 'X')
                    cmdObj5.GetComponent<TextMeshProUGUI>().text = inventory[4].getName();
                else
                    cmdObj5.SetActive(false);
            else
                cmdObj5.SetActive(false);
        }
    }

    public void displayItemUsage(bool show, int nmbr)
    {

        //source = GameObject.Find("SFXSource").GetComponent<AudioSource>();
        source.PlayOneShot(book);

        if (!selectedItem) //Si no s'està seleccionan l'arma per atacar
        {
            GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
            string selChar = gc.getSelectedCharacter();

            GameObject.Find("UsagePanel").GetComponent<RawImage>().enabled = show;
            GameObject.Find("DropPanel").GetComponent<RawImage>().enabled = show;
            cmdUseEquip.SetActive(show);
            cmdDrop.SetActive(show);

            if(show) gc.setMenuState(7);

            //Dependiendo de si es un arma o un objeto saldra una opcion u otra en cmdUseEquip
            if (show && GameObject.Find(selChar).GetComponent<Character>().getInventory()[nmbr].getiType() == "Weapon")
            {
                if (GameObject.Find(selChar).GetComponent<Character>().getInventory()[nmbr] != GameObject.Find(selChar).GetComponent<Character>().getSelWeapon())
                    cmdUseEquip.GetComponent<TextMeshProUGUI>().text = "EQUIP";
                else
                    cmdUseEquip.SetActive(false);
            }else
                cmdUseEquip.GetComponent<TextMeshProUGUI>().text = "USE";

        }
    }

    public void displayPauseMenu(bool show)
    {
        //source = GameObject.Find("SFXSource").GetComponent<AudioSource>();
        source.PlayOneShot(book);
        GameObject.Find("PausePanel").GetComponent<RawImage>().enabled = show;
        GameObject.Find("PauseLabel").GetComponent<Text>().enabled = show;
        
        cmdExit.SetActive(show);
        cmdEndTurn.SetActive(show);

        cmdMusic.SetActive(show);
        cmdSFX.SetActive(show);
        GameObject.Find("MusicLabel").GetComponent<Text>().enabled = show;
        if (GameObject.Find("GameController").GetComponent<GameController>().music)
            cmdMusic.GetComponent<RawImage>().texture = check;
        else
            cmdMusic.GetComponent<RawImage>().texture = notCheck;
        GameObject.Find("SFXLabel").GetComponent<Text>().enabled = show;
        if (GameObject.Find("GameController").GetComponent<GameController>().sfx)
            cmdSFX.GetComponent<RawImage>().texture = check;
        else
            cmdSFX.GetComponent<RawImage>().texture = notCheck;
}

    /* 
     * Funció que inicia la fase de moviment d'una unitat seleccionada
     */
    public void initMovement()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        displayUnitMenu(false);
        gc.iniMovement();
    }

    /*
     * Funció que deshabilita el personatje actiu del jugador
     */
    public void endUnitTurn()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        displayUnitMenu(false);
        gc.deactivateUnit();
    }

    /*
     * Funció que inicialitza la selecció d'enemic a atacar
     */
    public void selectEnemyToAttack()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        displayUnitMenu(false);
        displayWeaponMenu(true, 'A');
        gc.selectWeaponToAttack();
    }

    /*
     * Funció que inicialitza la conquesta del castell i la victoria del jugador.
     */
    public void conquerCastle()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        displayUnitMenu(false);
        gc.endGameVictory();
    }

    /*
     * Funció que inicialitza una curació a una unitat aliada
     */
    public void heal()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        displayUnitMenu(false);
        gc.healAlly();
    }

    /*
     * Funció que mostra l'apartat de l'inventari
     */
    public void showInventory()
    {
        displayUnitMenu(false);
        displayWeaponMenu(true, 'I');
    }

    public void selectedInvItem(int itemNmbr)
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        sItem = itemNmbr;

        if (gc.getTurnState() == 'S')
            selectedItem = true;
        else
        {
            displayItemUsage(true, itemNmbr);
        }
    }

    public void dropItem()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();

        gc.dropItem(sItem);
    }

    public void useItem()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
        if (cmdUseEquip.GetComponent<TextMeshProUGUI>().text == "EQUIP")
        {
            GameObject.Find(gc.getSelectedCharacter()).GetComponent<Character>().setEquipedWeapon(GameObject.Find(gc.getSelectedCharacter()).GetComponent<Character>().getWeaponInInventory(sItem));
            displayItemUsage(true, sItem);
        }
        else
        {
            gc.useItem(sItem);
            displayWeaponMenu(false, 'X');
        }
    }

    public void endT()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
        displayPauseMenu(false);
        gc.endTurn();
    }

    public void exitGame()
    {
        // ToDo -> Mostrar mensaje de que se perdera todo lo hecho durante la partida al salir.
        SceneManager.LoadScene(0);
    }

    public void controlMusic()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().music)
        {
            //Deshabilitar
            GameObject.Find("GameController").GetComponent<GameController>().music = false;
            cmdMusic.GetComponent<RawImage>().texture = notCheck;
            GameObject.Find("MusicControl").GetComponent<MusicControl>().mSource.volume = 0;
        }
        else
        {
            //Habilitar
            GameObject.Find("GameController").GetComponent<GameController>().music = true;
            cmdMusic.GetComponent<RawImage>().texture = check;
            GameObject.Find("MusicControl").GetComponent<MusicControl>().mSource.volume = 1;
        }
    }

    public void controlSFX()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().sfx)
        {
            //Deshabilitar
            GameObject.Find("GameController").GetComponent<GameController>().sfx = false;
            cmdSFX.GetComponent<RawImage>().texture = notCheck;
            source.volume = 0;
        }
        else
        {
            //Habilitar
            GameObject.Find("GameController").GetComponent<GameController>().sfx = true;
            cmdSFX.GetComponent<RawImage>().texture = check;
            source.volume = 1;
        }
    }

}
