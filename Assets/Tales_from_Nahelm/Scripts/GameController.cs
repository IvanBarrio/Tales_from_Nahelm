using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    private int turn;                   //Comptador dels torns que s'han realitzat durant la partida (cada cop que passa el torn de l'enemic incrementa en 1
    private char actualTurn;            //Indicador de quin jugador té el torn actualment (P -> jugador | A -> inteligencia artificial)
    private int unitsToMove;            // Comptador de les unitats del jugador amb torn que falten per moure
    private char turnState;             //Variable que ens controlara que pot fer el jugador (I-> Estat base e inicial, C-> El jugador ha seleccionat un personatge, A -> El personatge pot atacar) //S'ampliaran mes endevant
    private string selectedCharacter;   //Variable que ens indica quin personatge esta seleccionat en cas d'estar-ho, sino es trobara un valor vuit.
    Vector3 initialmovementPoint;
    Vector3 destination;

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
                    unit.GetComponent<Character>().createCharacter("Ally", "Niva", "Fighter", 20, 8, 0, 5, 5, 0, 4, 0, 20, 1);//todo
                    unit.GetComponent<Weapon>().setWeapon("Armads", "Axe", "A", 17, 1, 80, 10);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                case "Aki":
                    unit.GetComponent<Character>().createCharacter("Ally", "Aki", "Assassin", 21, 8, 0, 13, 12, 0, 5, 1, 20, 1);//todo
                    unit.GetComponent<Weapon>().setWeapon("Armads", "Axe", "A", 17, 1, 80, 10);//todo
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                case "Hilda":
                    unit.GetComponent<Character>().createCharacter("Ally", "Hilda", "Berserk", 22, 8, 4, 7, 9, 0, 7, 10, 30, 1);//todo
                    unit.GetComponent<Weapon>().setWeapon("Armads", "Axe", "A", 17, 1, 80, 10);//todo
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                case "Ann":
                    unit.GetComponent<Character>().createCharacter("Ally", "Ann", "Tactician", 16, 4, 3, 5, 5, 0, 5, 3, 20, 1);//todo
                    unit.GetComponent<Weapon>().setWeapon("Armads", "Axe", "A", 17, 1, 80, 10);//todo
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
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
                    unit.GetComponent<Character>().createCharacter("Enemy", "Omak", "Warrior", 20, 8, 0, 5, 5, 0, 4, 0, 20, 1);//todo
                    unit.GetComponent<Weapon>().setWeapon("Silver Lance", "Lance", "B", 13, 1, 75, 0);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                case "Ardsede":
                    unit.GetComponent<Character>().createCharacter("Enemy", "Ardsede", "Sorcerer", 21, 8, 0, 13, 12, 0, 5, 1, 20, 1);//todo
                    unit.GetComponent<Weapon>().setWeapon("Armads", "Axe", "A", 17, 1, 80, 10);//todo
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                default:
                    //Hacer un generador aleatorio de diferentes tipos de unidades para los soldados / Que lo mire dependiendo de las armas que lleve
                    unit.GetComponent<Character>().createCharacter("Enemy", "Therthas Soldier", "Soldier", 22, 8, 4, 7, 9, 0, 7, 10, 20, 1);//todo
                    unit.GetComponent<Weapon>().setWeapon("Armads", "Axe", "A", 17, 1, 80, 10);//todo
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
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

        turnState = 'I';
        selectedCharacter = "";

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

        switch (turnState)
        {
            //Estat inicial/Cerca de personatge per moure/Observacio per el mapa
            case 'I':
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
                            selectedCharacter = hit.collider.name;
                            initialmovementPoint = GameObject.Find(selectedCharacter).transform.position;
                            turnState = 'C';
                            GameObject.Find("MovementArea").transform.position = new Vector3(initialmovementPoint.x, GameObject.Find("MovementArea").transform.position.y, initialmovementPoint.z);
                            //disableUnit();
                        }
                        if (hit.collider.tag == "Enemy" && actualTurn == 'A')
                        {
                            disableUnit();
                        }
                    }
                }
                break;
            //Personatge seleccionat i disponible per a moure
            case 'C':
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        destination = new Vector3(hit.point.x, 0, hit.point.z);
                        if (Vector3.Distance(destination, initialmovementPoint) < 20)    //20 es el radi de la esfera i per tant el valor que haurem d'assignar al moviment del personatge
                        {
                            GameObject.Find(selectedCharacter).GetComponent<NavMeshAgent>().SetDestination(destination);
                            GameObject.Find(selectedCharacter).GetComponent<NavMeshAgent>().isStopped = false;
                            turnState = 'A';
                        }
                    }
                }
                break;
            //Personatge mogut i amb possibilitat d'atacar
            case 'A':
                if (GameObject.Find(selectedCharacter).GetComponent<NavMeshAgent>().remainingDistance < 0.1f){
                    GameObject.Find(selectedCharacter).GetComponent<NavMeshAgent>().isStopped = true;
                    GameObject.Find("AtackArea").transform.position = new Vector3(GameObject.Find(selectedCharacter).transform.position.x, GameObject.Find("AtackArea").transform.position.y, GameObject.Find(selectedCharacter).transform.position.z);
                    GameObject[] en = getEnemiesInRange(GameObject.Find(selectedCharacter).transform.position, 4f, GameObject.Find(selectedCharacter).tag); //El rang que es passa sera el rang que tingui l'arma d'atac
                    if (en[0] != null)
                    {
                        Debug.Log("Es pot atacar a un enemic!");
                        //De moment com només tenim una unitat aliada i una unitat enemiga farem que realitzi l'atac directament.
                        Debug.Log(selectedCharacter + " es disposa a atacar a " + en[0].name);
                        int dead = combatTurn(GameObject.Find(selectedCharacter).GetComponent<Character>(), en[0].GetComponent<Character>());
                        switch (dead)
                        {
                            case 1:
                                Debug.Log(en[0].name + " a matat a " + selectedCharacter + " en combat.");
                                break;
                            case 2:
                                Debug.Log(selectedCharacter + " a matat a " + en[0].name + " en combat.");
                                break;
                        }
                        turnState = 'I';
                        disableUnit();
                    }
                    else
                    {
                        Debug.Log("No hi ha enemics a prop.");
                    }
                    turnState = 'I';
                    GameObject.Find("MovementArea").transform.position = new Vector3(371, GameObject.Find("MovementArea").transform.position.y, 88);
                    //GameObject.Find("AtackArea").transform.position = new Vector3(371, GameObject.Find("AtackArea").transform.position.y, 88);
                }
                
                break;
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

    public char getTurnState()
    {
        return turnState;
    }

    public string getSelectedCharacter()
    {
        return selectedCharacter;
    }

    //Funció que retornará els enemics del personatge que estiguin a rang d'atac
    public GameObject[] getEnemiesInRange(Vector3 pos, float range, string tag)
    {
        GameObject[] enemies = null;
        int i = 0;
        int enNmbr = 0;
        switch (tag)
        {
            case "Ally":
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case "Enemy":
                enemies = GameObject.FindGameObjectsWithTag("Ally");
                break;
        }

        enNmbr = enemies.Length;
        GameObject[] enInRange = new GameObject[enNmbr];

        foreach (GameObject unit in enemies)
        {
            if (Vector3.Distance(unit.transform.position, pos) < range)
            {
                enInRange[i] = unit;
                i++;
            }
        }
        return enInRange;
    }

    /*
     * Crear una función para la generación de unidades enemigas como refuerzos
     */

    /*
     * Crear una función para la ejecucion de los combates entre 2 personajes
     */
    public int combatTurn(Character starter, Character recieber)
    {
        // Mes endevant podrem trobar més opcions depenent de les armes que portin equipades de moment no es contempla
        int atkmode = 0;    //Variable que indicara qui realitzara el doble atac en cas de poder-se realitzar (1-> l'atacant, 2-> l'atacat, 0-> Cap d'ells)
        bool recIsDead = false;

        if (starter.getSpd() >= (recieber.getSpd() + 5))
        {
            atkmode = 1;
        }
        else if (recieber.getSpd() >= (starter.getSpd() + 5))
        {
            atkmode = 2;
        }

        //Ataca l'atacant
        recIsDead = realizeAtack(starter, recieber);
        if (recIsDead) return 2;
        //Ataca l'atacat
        recIsDead = realizeAtack(recieber, starter);
        if (recIsDead) return 1;

        switch (atkmode)
        {
            case 1:
                recIsDead = realizeAtack(starter, recieber);
                if (recIsDead) return 2;
                break;
            case 2:
                recIsDead = realizeAtack(recieber, starter);
                if (recIsDead) return 1;
                break;
        }

        return 0;
        //Decidida la sequencia de ataques hay que realizar los calculos del ataque
            //Sera critico?
                //Si -> daño x3
                //No -> Fallara el ataque?
                    //No -> Se calcula el daño
                    //Si -> no se realizan mas calculos
                    //Muere el enemigo al recibir el ataque?
                        //No -> Inicia su ataque (Repetir anteriores)
                        //Si -> Se acaba el combate
    }

    public bool realizeAtack(Character st, Character re)
    {
        bool isDead = false;
        int[] atckStats = st.atackSelectedEnemy(re);
        int randV;                                                                      //Valor aleatori tret per a les comprovacions de les realitzacions dels diferent atacs
        int critical = 1;                                                               //Multiplicador aplicable depenent si es realitza atac crític o no
        bool isAHit = false;                                                            //Variable que indica si s'encerta el cop a realitzar
        int damageToEnemy = 0;
        int reHP;

        randV = Random.Range(1, 100);
        if (randV <= atckStats[2] && randV != 0)
        {
            critical = 3;
            isAHit = true;  //Un atac critic sempre encerta
        }

        if (!isAHit)
        {
            //Es realitza l'atac normal?
            
            randV = Random.Range(1, 100);
            if (randV <= atckStats[0] && randV != 0)
            {
                isAHit = true;
            }
        }

        if (isAHit)
        {
            damageToEnemy = atckStats[1] * critical;
        }

        if (critical == 3)
        {
            Debug.Log(st.name + " realitza un atac crític a " + re.name + " que fa " + damageToEnemy + " punts de dany!");
        }
        else if (isAHit)
        {
            Debug.Log(st.name + " realitza un atac a " + re.name + " que fa " + damageToEnemy + " punts de dany.");
        }
        else
        {
            Debug.Log("L'atac de " + st.name + " ha fallat.");
        }

        //Realitzar el dany a l'enemic
        reHP = re.recieveDamage(damageToEnemy);
        if (reHP == 0) isDead = true;
        return isDead;
    }

    /*
     * Crear una función para la ejecucion de las curaciones entre dos personajes
     */
}
