using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMenuController : MonoBehaviour
{
    // Botons per al menu d'accions
    public GameObject cmdMove;
    public GameObject cmdAttack;
    public GameObject cmdInv;
    public GameObject cmdTrade;
    public GameObject cmdWait;
    public GameObject cmdHeal;

    //Botons per al menu de selecció d'arma

    // Start is called before the first frame update
    void Start()
    {
        displayUnitMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayUnitMenu(bool show)
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
        string selChar = gc.getSelectedCharacter();

        GameObject.Find("UnitActionsPanel").GetComponent<Image>().enabled = show;
        cmdMove.SetActive(show);
        cmdAttack.SetActive(show);
        cmdHeal.SetActive(show);
        cmdInv.SetActive(show);
        cmdTrade.SetActive(show);
        cmdWait.SetActive(show);

        if (show)
        {
            //Si el menu esta activo mostrar las acciones que puede hacer el personaje
            if (!GameObject.Find(selChar).GetComponent<Character>().getCanMove())
               cmdMove.SetActive(false);

            //Comprobar si la unidad puede curar a algien
            if (!GameObject.Find(selChar).GetComponent<Character>().canHeal())
                cmdHeal.SetActive(false);

            GameObject[] en = gc.getEnemiesInRange(GameObject.Find(selChar).transform.position, 4f, GameObject.Find(selChar).tag); //El rang que es passa sera el rang que tingui l'arma d'atac
            if (en[0] == null)
               cmdAttack.SetActive(false);

            GameObject[] al = gc.getAliesInRange(GameObject.Find(selChar).transform.position, 4f, GameObject.Find(selChar).tag); //El rang que es passa sera el rang que tingui l'arma d'atac
            if (al[0] == null)
               cmdTrade.SetActive(false);
        }
    }

    //Controles del menu de seleccion de armas
    public void displayWeaponMenu(bool show)
    {
        //ToDo
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
        gc.selectWeaponToAttack();
    }
}
