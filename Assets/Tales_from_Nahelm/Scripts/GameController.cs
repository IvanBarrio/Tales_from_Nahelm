using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int turn;                   //Comptador dels torns que s'han realitzat durant la partida (cada cop que passa el torn de l'enemic incrementa en 1
    private char actualTurn;            //Indicador de quin jugador té el torn actualment (P -> jugador | A -> inteligencia artificial)
    private int unitsToMove;            // Comptador de les unitats del jugador amb torn que falten per moure
    private char turnState;             //Variable que ens controlara que pot fer el jugador (I-> Estat base e inicial, C-> El jugador ha seleccionat un personatge, A -> El personatge pot atacar, G -> Game Over, T -> Mostrant el torn actual[no es poden realitzar accions durant aquest temps]) //S'ampliaran mes endevant
    private string selectedCharacter;   //Variable que ens indica quin personatge esta seleccionat en cas d'estar-ho, sino es trobara un valor vuit.
    Vector3 initialmovementPoint;
    Vector3 destination;
    bool flagIA;                        //Bolea que indicara si ha acabat el torn d'una unitat de la IA o encara no
    GameObject selAICharacter;
    GameObject enemyTarget;             //Unitat seleccionada per atacar
    bool isNotMoving;
    float timer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * Hay que generar los valores reales de las estadisticas de los personajes
         */

        GameObject.Find("UnitName").GetComponent<Text>().enabled = false;
        GameObject.Find("Live").GetComponent<Text>().enabled = false;
        GameObject.Find("ActualLive").GetComponent<Text>().enabled = false;
        GameObject.Find("TotalLive").GetComponent<Text>().enabled = false;
        GameObject.Find("GAMEOVER").GetComponent<Text>().enabled = false;
        GameObject.Find("BattleRes1").GetComponent<Text>().enabled = false;
        GameObject.Find("BattleRes2").GetComponent<Text>().enabled = false;
        GameObject.Find("BattleRes3").GetComponent<Text>().enabled = false;


        Debug.Log("Empezamos!");
        Debug.Log("Inicializamos aliados!");
        GameObject[] alies = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject unit in alies)
        {
            switch (unit.name)
            {
                case "Niva":
                    unit.GetComponent<Character>().createCharacter("Ally", "Niva", "Captain", 23, 13, 5, 9, 11, 8, 8, 5, 20, 1);//faltan los crecimientos y los totales
                    unit.GetComponent<Weapon>().setWeapon("Northern Axe", "Axe", "A", 10, 1, 65, 0);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                case "Aki":
                    unit.GetComponent<Character>().createCharacter("Ally", "Aki", "Nomad", 23, 8, 6, 10, 12, 7, 7, 7, 20, 1);//faltan los crecimientos y los totales
                    unit.GetComponent<Weapon>().setWeapon("Light Blade", "Sword", "A", 9, 1, 90, 30);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                case "Hilda":
                    unit.GetComponent<Character>().createCharacter("Ally", "Hilda", "Berserk", 31, 14, 4, 6, 13, 6, 7, 4, 30, 1);//faltan los crecimientos y los totales
                    unit.GetComponent<Weapon>().setWeapon("Siegfried's Lance", "Lance", "A", 11, 1, 80, 10);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
                case "Ann":
                    unit.GetComponent<Character>().createCharacter("Ally", "Ann", "Priestess", 17, 8, 10, 8, 9, 8, 8, 5, 20, 1);//faltan los crecimientos y los totales
                    unit.GetComponent<Weapon>().setWeapon("Aestus", "Sword", "A", 10, 1, 80, 0);
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
                    unit.GetComponent<Character>().createCharacter("Enemy", "Omak", "Warrior", 41, 18, 0, 13, 12, 11, 13, 1, 20, 1);
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
                GameObject.Find("TurnShow").GetComponent<Text>().text = "Turno " + turn + " del jugador";
                GameObject.Find("ActualTurn").GetComponent<Text>().text = "Player Turn";
                GameObject.Find("ActualTurn").GetComponent<Text>().color = Color.blue;
                Debug.Log("Turno " + turn + " del jugador");
                unitsToMove = GameObject.FindGameObjectsWithTag("Ally").Length;
                break;
            case 'A':
                GameObject.Find("TurnShow").GetComponent<Text>().text = "Turno " + turn + " de la IA";
                GameObject.Find("ActualTurn").GetComponent<Text>().text = "Enemy Turn";
                GameObject.Find("ActualTurn").GetComponent<Text>().color = Color.red;
                Debug.Log("Turno " + turn + " de la IA");
                unitsToMove = GameObject.FindGameObjectsWithTag("Enemy").Length;
                break;
        }

        GameObject.Find("ActualTurn").GetComponent<Text>().enabled = true;
        turnState = 'T';
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
         * Comprobar si hay algun personaje seleccionado para mostrar los datos en la UI
         */
        if (selectedCharacter != null && selectedCharacter != "")
        {
            GameObject.Find("UnitName").GetComponent<Text>().text = selectedCharacter;
            GameObject.Find("ActualLive").GetComponent<Text>().text = GameObject.Find(selectedCharacter).GetComponent<Character>().getActualPV().ToString();
            GameObject.Find("TotalLive").GetComponent<Text>().text = GameObject.Find(selectedCharacter).GetComponent<Character>().getPV().ToString();
            GameObject.Find("TotalLive").GetComponent<Text>().enabled = true;
            GameObject.Find("UnitName").GetComponent<Text>().enabled = true;
            GameObject.Find("Live").GetComponent<Text>().enabled = true;
            GameObject.Find("ActualLive").GetComponent<Text>().enabled = true;
            GameObject.Find("TotalLive").GetComponent<Text>().enabled = true;
        }
        else
        {
            GameObject.Find("UnitName").GetComponent<Text>().enabled = false;
            GameObject.Find("Live").GetComponent<Text>().enabled = false;
            GameObject.Find("ActualLive").GetComponent<Text>().enabled = false;
            GameObject.Find("TotalLive").GetComponent<Text>().enabled = false;
        }

        if (turnState == 'T')
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                GameObject.Find("ActualTurn").GetComponent<Text>().enabled = false;
                turnState = 'I';
            }
        }

        if (turnState == 'G')//En Game Over sortirem amb qualsevol input tant al teclat com al ratolí
        {
            if (Input.anyKey)  SceneManager.LoadScene(0);   //Retornem al menu inicial del joc
        }

        /*
         * Controlar las condiciones de juego
         */
        switch (actualTurn)
        {
            case 'P':   //Torn del jugador
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
                                if (hit.collider.tag == "Ally" && GameObject.Find(hit.collider.name).GetComponent<Character>().getState() == 'A')
                                {
                                    selectedCharacter = hit.collider.name;
                                    initialmovementPoint = GameObject.Find(selectedCharacter).transform.position;
                                    turnState = 'C';
                                    GameObject.Find("MovementArea").transform.position = new Vector3(initialmovementPoint.x, GameObject.Find("MovementArea").transform.position.y, initialmovementPoint.z);
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
                                        GameObject.Find(selectedCharacter).GetComponent<Character>().setState('D');
                                        GameObject.Destroy(GameObject.Find(selectedCharacter));
                                        break;
                                    case 2:
                                        Debug.Log(selectedCharacter + " a matat a " + en[0].name + " en combat.");
                                        en[0].GetComponent<Character>().setState('D');
                                        GameObject.Destroy(en[0]);
                                        break;
                                }
                                checkUnits();
                            }
                            else
                            {
                                Debug.Log("No hi ha enemics a prop.");
                            }
                            GameObject.Find("MovementArea").transform.position = new Vector3(371, GameObject.Find("MovementArea").transform.position.y, 88);
                            GameObject.Find(selectedCharacter).GetComponent<Character>().setState('M');
                            selectedCharacter = "";
                            disableUnit();
                        }
                        break;
                }
                break;
            case 'A':   //Torn de la IA
                if (selAICharacter == null)
                {
                    selAICharacter = findUnitWithClosestThreads();
                    flagIA = false;
                }
                else
                {
                    switch (turnState)
                    {
                        case 'I':
                            if (!flagIA)
                            {
                                flagIA = true;   //Bloquejem l'accés al bloc d'execució per a que no entri a cada frame quan s'executi l'Update.
                                GameObject closest = getClosestThread(selAICharacter.transform.position, selAICharacter.tag);
                                initialmovementPoint = selAICharacter.transform.position;
                                destination = closest.transform.position;
                                GameObject[] en = getEnemiesInRange(selAICharacter.transform.position, 4f, selAICharacter.tag); //El rang que es passa sera el rang que tingui l'arma d'atac
                                if (en[0] != null)
                                {
                                    int tots = 0;
                                    bool hiEs = false;
                                    do
                                    {
                                        if (en[tots] == closest)
                                        {
                                            hiEs = true;
                                        }
                                        tots++;
                                    } while (tots < en.Length);
                                    if (hiEs)   //De moment si no es troba anira a per el primer de la llista
                                    {
                                        Debug.Log("L'amenaça més propera es a rang d'atac!");
                                        enemyTarget = closest;
                                    }
                                    else
                                    {
                                        Debug.Log("Hi ha més enemics a rang que no són l'amenaça més propera");
                                        enemyTarget = en[0];
                                    }
                                    isNotMoving = true;
                                }
                                else
                                {
                                    enemyTarget = closest;
                                    Debug.Log("No hi ha enemics a rang d'atac");
                                    var heading = enemyTarget.transform.position - selAICharacter.transform.position;

                                    //De moment es mourá en la direcció a la unitat enemiga a la major distancia que pugui
                                    if (Vector3.Distance(destination, selAICharacter.transform.position) < 20)    //20 es el radi de la esfera i per tant el valor que haurem d'assignar al moviment del personatge
                                    {
                                        selAICharacter.GetComponent<NavMeshAgent>().SetDestination(destination);
                                        selAICharacter.GetComponent<NavMeshAgent>().stoppingDistance = 4;   //Fem que es quedi a una distancia prudencial de la unitat enemiga
                                        selAICharacter.GetComponent<NavMeshAgent>().isStopped = false;
                                        isNotMoving = true;
                                    }
                                    else
                                    {
                                        selAICharacter.GetComponent<NavMeshAgent>().SetDestination(destination);
                                        selAICharacter.GetComponent<NavMeshAgent>().stoppingDistance = (Vector3.Distance(destination, selAICharacter.transform.position) - 20);   //Fem que es quedi a una distancia prudencial de la unitat enemiga
                                        selAICharacter.GetComponent<NavMeshAgent>().isStopped = false;
                                        isNotMoving = false;
                                    }
                                }
                                turnState = 'A';
                            }
                            break;
                        case 'A':
                            if (flagIA)
                            {
                                float notMovingDist = 0;
                                if (isNotMoving)
                                    notMovingDist = Vector3.Distance(destination, initialmovementPoint);
                                else
                                    notMovingDist = (Vector3.Distance(destination, initialmovementPoint) - 20);
                                if (Vector3.Distance(destination, selAICharacter.transform.position) <= notMovingDist)
                                {
                                    flagIA = false;
                                    selAICharacter.GetComponent<NavMeshAgent>().isStopped = true;
                                    GameObject[] en = getEnemiesInRange(selAICharacter.transform.position, 4f, selAICharacter.tag); //El rang que es passa sera el rang que tingui l'arma d'atac
                                    if (en[0] == enemyTarget)
                                    {
                                        Debug.Log("Es pot atacar a un enemic!");
                                        //De moment com només tenim una unitat aliada i una unitat enemiga farem que realitzi l'atac directament.
                                        Debug.Log(selAICharacter.name + " es disposa a atacar a " + en[0].name);
                                        int dead = combatTurn(selAICharacter.GetComponent<Character>(), en[0].GetComponent<Character>());
                                        switch (dead)
                                        {
                                            case 1:
                                                Debug.Log(en[0].name + " a matat a " + selAICharacter.name + " en combat.");
                                                selAICharacter.GetComponent<Character>().setState('D');
                                                GameObject.Destroy(selAICharacter);
                                                break;
                                            case 2:
                                                Debug.Log(selAICharacter.name + " a matat a " + en[0].name + " en combat.");
                                                en[0].GetComponent<Character>().setState('D');
                                                GameObject.Destroy(en[0]);
                                                break;
                                        }
                                        checkUnits();
                                    }
                                    else
                                    {
                                        Debug.Log("No hi ha enemics a prop.");
                                    }
                                    selAICharacter.GetComponent<Character>().setState('M');
                                    selAICharacter = null;
                                    enemyTarget = null;
                                    disableUnit();
                                }
                            }
                            break;
                    }
                }
                break;
        }
    }

    /*
     * Funció que decrementa el numero d'unitats que falten per moure en el torn actiu, si arriba a 0 cambia el torn de forma automatica
     */
    void disableUnit()
    {
        checkUnits();
        unitsToMove = getActiveUnits();
        if (unitsToMove == 0 && turnState != 'G')
        {
            switch (actualTurn)
            {
                case 'P':
                    flagIA = true;
                    actualTurn = 'A';
                    Debug.Log("Turno " + turn + " de la IA");
                    GameObject.Find("TurnShow").GetComponent<Text>().text = "Turno " + turn + " de la IA";
                    GameObject.Find("ActualTurn").GetComponent<Text>().text = "Enemy Turn";
                    GameObject.Find("ActualTurn").GetComponent<Text>().color = Color.red;
                    unitsToMove = GameObject.FindGameObjectsWithTag("Enemy").Length;
                    activateUnits("Enemy");
                    break;
                case 'A':
                    turn++; //Cambiem de torn cada cop que acaba la IA el seu
                    actualTurn = 'P';
                    GameObject.Find("TurnShow").GetComponent<Text>().text = "Turno " + turn + " del jugador";
                    GameObject.Find("ActualTurn").GetComponent<Text>().text = "Player Turn";
                    GameObject.Find("ActualTurn").GetComponent<Text>().color = Color.blue;
                    Debug.Log("Turno " + turn + " del jugador");
                    unitsToMove = GameObject.FindGameObjectsWithTag("Ally").Length;
                    activateUnits("Ally");
                    break;
            }
            GameObject.Find("ActualTurn").GetComponent<Text>().enabled = true;
            timer = 5.0f;
            turnState = 'T';
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

        GameObject.Find("BattleRes1").GetComponent<Text>().text = "";
        GameObject.Find("BattleRes2").GetComponent<Text>().text = "";
        GameObject.Find("BattleRes3").GetComponent<Text>().text = "";

        GameObject.Find("BattleRes1").GetComponent<Text>().enabled = true;
        GameObject.Find("BattleRes2").GetComponent<Text>().enabled = true;
        GameObject.Find("BattleRes3").GetComponent<Text>().enabled = true;

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
        string resText;                                                                 //Text que es mostrará amb els resultats de la batalla

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
            resText = st.name + " realitza un atac crític a " + re.name + " que fa " + damageToEnemy + " punts de dany!";
            Debug.Log(st.name + " realitza un atac crític a " + re.name + " que fa " + damageToEnemy + " punts de dany!");
        }
        else if (isAHit)
        {
            resText = st.name + " realitza un atac a " + re.name + " que fa " + damageToEnemy + " punts de dany.";
            Debug.Log(st.name + " realitza un atac a " + re.name + " que fa " + damageToEnemy + " punts de dany.");
        }
        else
        {
            resText = "L'atac de " + st.name + " ha fallat.";
            Debug.Log("L'atac de " + st.name + " ha fallat.");
        }
        if (GameObject.Find("BattleRes1").GetComponent<Text>().text == "")
            GameObject.Find("BattleRes1").GetComponent<Text>().text = resText;
        else if (GameObject.Find("BattleRes2").GetComponent<Text>().text == "")
            GameObject.Find("BattleRes2").GetComponent<Text>().text = resText;
        else
            GameObject.Find("BattleRes3").GetComponent<Text>().text = resText;

        //Realitzar el dany a l'enemic
        reHP = re.recieveDamage(damageToEnemy);
        if (reHP == 0) isDead = true;
        return isDead;
    }

    /*
     * Funció que retorna la unitat (Activa) amb amenaces més properes per a realitzar el torn
     */
    public GameObject findUnitWithClosestThreads()
    {
        //Falta implementar el sistema de unidades activas e inactivas
        GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject un = null;
        int ct = 0;
        int ctant = -1;

        foreach (GameObject unit in units)
        {
            ct = getEnemiesInRange(unit.transform.position, 24, unit.tag).Length;   //Busquem el nombre d'enemic en rang d'atac màxim
            if (ct > ctant)
            {
                un = unit;
                ctant = ct;
            }
        }
        return un;
    }

    /*
     * Funció que retorna l'enemic més proper a la unitat (Més endevant aquesta amenaça no ha de ser la unitat més propera sino la que mes dany pugui causar)
     */
    public GameObject getClosestThread(Vector3 pos, string tag)
    {
        GameObject[] enemies = null;
        GameObject enInRange = null;
        int i = 0;
        float minDistance = -1;
        switch (tag)
        {
            case "Ally":
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                break;
            case "Enemy":
                enemies = GameObject.FindGameObjectsWithTag("Ally");
                break;
        }

        foreach (GameObject unit in enemies)
        {
            if (minDistance == -1) minDistance = Vector3.Distance(unit.transform.position, pos);
            if (Vector3.Distance(unit.transform.position, pos) <= minDistance)
            {
                enInRange = unit;
                i++;
            }
        }
        return enInRange;
    }

    /*
     * Funció que comprova el nombre d'unitats del jugador, en cas d'arribar a 0 trobem una death condition i acabem la partida
     */
    public void checkUnits()
    {
        GameObject[] alies = GameObject.FindGameObjectsWithTag("Ally");
        int alive = 0;

        foreach (GameObject unit in alies)
        {
            if (unit.GetComponent<Character>().getState() != 'D') alive++;
        }

        if (alive == 0)
        {
            GameObject.Find("GAMEOVER").GetComponent<Text>().enabled = true;
            turnState = 'G';
            Debug.Log("GAME OVER, ja no et queden unitats per continuar.");
        }
    }
    /*
     * Crear una función para la ejecucion de las curaciones entre dos personajes
     */

    /*
     * Funció per activar totes les unitats a l'inici del torn
     */
    public void activateUnits(string tag)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject unit in units)
        {
            unit.GetComponent<Character>().setState('A');
        }
    }

    /*
     * Funció per obtenir el nombre d'unitats activables en el torn actual
     */
    public int getActiveUnits()
    {
        int actUnits = 0;
        string tag = null;
        switch (actualTurn)
        {
            case 'P':
                tag = "Ally";
                break;
            case 'A':
                tag = "Enemy";
                break;
        }
        GameObject[] units = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<Character>().getState() == 'A')
                actUnits++;
        }
        return actUnits;
    }
}
