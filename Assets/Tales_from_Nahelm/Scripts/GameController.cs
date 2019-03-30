using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int turn;           //Comptador dels torns que s'han realitzat durant la partida (cada cop que passa el torn de l'enemic incrementa en 1
    private char actualTurn;    //Indicador de quin jugador té el torn actualment (P -> jugador | A -> inteligencia artificial)
    private int unitsToMove;    // Comptador de les unitats del jugador amb torn que falten per moure
    
    // Start is called before the first frame update
    void Start()
    {
        /*
         * Hay que generar los valores reales de las estadisticas de los personajes
         */

        Debug.Log("Empezamos!");
        Debug.Log("Inicializamos aliados!");
        GameObject[] alies = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject unit in alies)
        {
            switch (unit.name)
            {
                case "Niva":
                    unit.GetComponent<Character>().createCharacter("Ally", "Niva", "Fighter", 20, 8, 0, 5, 5, 0, 4, 0, 30, 1);
                    break;
                case "Aki":
                    unit.GetComponent<Character>().createCharacter("Ally", "Aki", "Assassin", 21, 8, 0, 13, 12, 0, 5, 1, 30, 1);
                    break;
                case "Hilda":
                    unit.GetComponent<Character>().createCharacter("Ally", "Hilda", "Berserk", 22, 8, 4, 7, 9, 0, 7, 10, 40, 1);
                    break;
                case "Ann":
                    unit.GetComponent<Character>().createCharacter("Ally", "Ann", "Tactician", 16, 4, 3, 5, 5, 0, 5, 3, 30, 1);
                    break;
            }
        }

        Debug.Log("Inicializamos enemigos!");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject unit in enemies)
        {
            switch (unit.name)
            {
                case "Omak":
                    unit.GetComponent<Character>().createCharacter("Enemy", "Omak", "Warrior", 20, 8, 0, 5, 5, 0, 4, 0, 30, 1);
                    break;
                case "Ardsede":
                    unit.GetComponent<Character>().createCharacter("Enemy", "Ardsede", "Sorcerer", 21, 8, 0, 13, 12, 0, 5, 1, 30, 1);
                    break;
                default:
                    //Hacer un generador aleatorio de diferentes tipos de unidades para los soldados / Que lo mire dependiendo de las armas que lleve
                    unit.GetComponent<Character>().createCharacter("Enemy", "Therthas Soldier", "Soldier", 22, 8, 4, 7, 9, 0, 7, 10, 40, 1);
                    break;
            }
        }

        Debug.Log("Inicializamos Turnos!");
        turn = 1;
        actualTurn = 'P';
        switch (actualTurn)
        {
            case 'P':
                Debug.Log("Turno " + turn + " del jugador");
                unitsToMove = GameObject.FindGameObjectsWithTag("Ally").Length;
                break;
            case 'A':
                Debug.Log("Turno " + turn + " de la IA");
                unitsToMove = GameObject.FindGameObjectsWithTag("Enemy").Length;
                break;
        }

        /*Todo
          * Inicializar un par de enemigos y un pj de jugador
          * Inicializar armas para cada uno de los personajes creados
          * Empezar a controlar los turnos empezando por el primer turno jugador
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Controlar las condiciones de juego
         */

        //Si es fa click sobre un objecte
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Si pulsem sobre una unitat del jugador en el torn del jugador passem a comprobar si es el torn de la IA i al reves
                if (hit.collider.tag == "Ally" && actualTurn == 'P')
                {
                    disableUnit();
                }
                if (hit.collider.tag == "Enemy" && actualTurn == 'A')
                {
                    disableUnit();
                }
            }
        }
    }

    /*
     * Funció que decrementa el numero d'unitats que falten per moure en el torn actiu, si arriba a 0 cambia el torn de forma automatica
     */
    void disableUnit()
    {
        if (Input.GetMouseButton(0))
        {
            unitsToMove--;
            if (unitsToMove == 0)
            {
                switch (actualTurn)
                {
                    case 'P':
                        actualTurn = 'A';
                        Debug.Log("Turno " + turn + " de la IA");
                        unitsToMove = GameObject.FindGameObjectsWithTag("Enemy").Length;
                        break;
                    case 'A':
                        actualTurn = 'P';
                        Debug.Log("Turno " + turn + " del jugador");
                        unitsToMove = GameObject.FindGameObjectsWithTag("Ally").Length;
                        break;
                }
            }
        }
    }

    /*
     * Crear una función para la generación de unidades enemigas como refuerzos
     */

    /*
     * Crear una función para la ejecucion de los combates entre 2 personajes
     */

    /*
     * Crear una función para la ejecucion de las curaciones entre dos personajes
     */
}
