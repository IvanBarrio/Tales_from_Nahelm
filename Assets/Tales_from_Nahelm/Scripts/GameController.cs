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
    private char turnState;             //Variable que ens controlara que pot fer el jugador (I-> Estat base e inicial, C-> El jugador ha seleccionat un personatge, A -> El personatge ha acabat de moure, G -> Game Over, T -> Mostrant el torn actual[no es poden realitzar accions durant aquest temps], D-> Dialeg inicial del joc on es realitza un petit tutorial, W->En espera de la selecció d'acció sobre una unitat, B->Estat de batalla, S->Selecció d'arma per realitzar la batalla, P->Enemic confrimat i inici del combat, H->Curació, U-> Menu de Pausa) //S'ampliaran mes endevant
    //I-C-A-G-T-D-W-B-S-P-H-U
    private int menuState;              //Variable que controla en quin moment del menu ens trobem per poder tirar enrere accions
    private string selectedCharacter;   //Variable que ens indica quin personatge esta seleccionat en cas d'estar-ho, sino es trobara un valor vuit.
    Vector3 initialmovementPoint;
    Vector3 destination;
    bool flagIA;                        //Bolea que indicara si ha acabat el torn d'una unitat de la IA o encara no
    GameObject selAICharacter;
    GameObject enemyTarget;             //Unitat seleccionada per atacar
    bool isNotMoving;
    float timer = 5.0f;
    bool goBack;
    char nextTurn;
    bool playerUnitMissed;
    string whoIsAttacking;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go;              //Variable que emprarem per generar els diferents objectes de les unitats

        GameObject.Find("UnitName").GetComponent<Text>().enabled = false;
        GameObject.Find("Live").GetComponent<Text>().enabled = false;
        GameObject.Find("ActualLive").GetComponent<Text>().enabled = false;
        GameObject.Find("TotalLive").GetComponent<Text>().enabled = false;
        GameObject.Find("GAMEOVER").GetComponent<Text>().enabled = false;
        GameObject.Find("TurnShow").GetComponent<Text>().enabled = false;
        GameObject.Find("UnitInfo").GetComponent<Image>().enabled = false;

        int[] randomStats = new int[10];
        string[] randomWeapon = new string[7];

        Debug.Log("Empezamos!");
        Debug.Log("Inicializamos aliados!");
        GameObject[] alies = GameObject.FindGameObjectsWithTag("Ally");
        Item staff = new Item();
        staff.setName("Staff");

        foreach (GameObject unit in alies)
        {
            switch (unit.name)
            {
                case "Niva":
                    unit.GetComponent<Character>().createCharacter("Ally", "Niva", "Captain", 23, 13, 5, 9, 11, 8, 8, 5, 20, 5);//faltan los crecimientos y los totales
                    unit.GetComponent<Character>().setStatsMaxs(60, 29, 17, 28, 27, 30, 27, 24);
                    unit.GetComponent<Character>().setStatsGrowth(80, 75, 25, 60, 50, 55, 45, 20);
                    unit.GetComponent<Weapon>().setWeapon("Northern Axe", "Axe", "A", 10, 1, 65, 0);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    go = new GameObject("NivaVulnerary");
                    go.AddComponent<Item>().setName("Vulnerary");
                    go.transform.SetParent(unit.transform);
                    unit.GetComponent<Character>().obtainObject(go.GetComponent<Item>());
                    break;
                case "Aki":
                    unit.GetComponent<Character>().createCharacter("Ally", "Aki", "Nomad", 23, 8, 6, 10, 12, 7, 7, 7, 20, 5);//faltan los crecimientos y los totales
                    unit.GetComponent<Character>().setStatsMaxs(60, 24, 19, 29, 31, 32, 22, 23);
                    unit.GetComponent<Character>().setStatsGrowth(80, 40, 25, 65, 70, 60, 35, 20);
                    unit.GetComponent<Weapon>().setWeapon("Light Blade", "Sword", "A", 9, 1, 90, 30);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    go = new GameObject("AkiVulnerary");
                    go.AddComponent<Item>().setName("Vulnerary");
                    go.transform.SetParent(unit.transform);
                    unit.GetComponent<Character>().obtainObject(go.GetComponent<Item>());
                    break;
                case "Hilda":
                    unit.GetComponent<Character>().createCharacter("Ally", "Hilda", "Berserk", 31, 14, 4, 6, 13, 6, 7, 4, 30, 2);//faltan los crecimientos y los totales
                    unit.GetComponent<Character>().setStatsMaxs(80, 54, 27, 37, 43, 45, 36, 29);
                    unit.GetComponent<Character>().setStatsGrowth(90, 80, 25, 55, 50, 55, 40, 20);
                    unit.GetComponent<Weapon>().setWeapon("Siegfried's Lance", "Lance", "A", 11, 1, 80, 10);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    go = new GameObject("HildaVulnerary");
                    go.AddComponent<Item>().setName("Vulnerary");
                    go.transform.SetParent(unit.transform);
                    unit.GetComponent<Character>().obtainObject(go.GetComponent<Item>());
                    break;
                case "Ann":
                    unit.GetComponent<Character>().createCharacter("Ally", "Ann", "Priestess", 17, 8, 10, 8, 9, 8, 8, 5, 20, 5);//faltan los crecimientos y los totales
                    unit.GetComponent<Character>().setStatsMaxs(60, 24, 28, 25, 27, 29, 24, 26);
                    unit.GetComponent<Character>().setStatsGrowth(60, 55, 65, 50, 55, 55, 35, 30);
                    unit.GetComponent<Weapon>().setWeapon("Aestus", "Sword", "A", 10, 1, 80, 0);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    go = new GameObject("AnnStaff");
                    go.AddComponent<Item>().setName("Staff");
                    go.transform.SetParent(unit.transform);
                    unit.GetComponent<Character>().obtainObject(go.GetComponent<Item>());

                    go = new GameObject("AnnVulnerary");
                    go.AddComponent<Item>().setName("Vulnerary");
                    go.transform.SetParent(unit.transform);
                    unit.GetComponent<Character>().obtainObject(go.GetComponent<Item>());

                    go = new GameObject("Elfire");
                    go.AddComponent<Weapon>().setWeapon("Elfire", "Magic", "E", 2, 2, 90, 0);
                    go.transform.SetParent(unit.transform);
                    unit.GetComponent<Character>().obtainObject(go.GetComponent<Weapon>());
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
                    unit.GetComponent<Character>().createCharacter("Enemy", "Ardsede", "Sorcerer", 24, 4, 8, 4, 6, 3, 8, 7, 20, 6);
                    unit.GetComponent<Weapon>().setWeapon("Shadows", "BlackMagic", "C", 4, 2, 60, 50);
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    go = new GameObject("ArdsedeStaff");
                    go.AddComponent<Item>().setName("Staff");
                    go.transform.SetParent(unit.transform);
                    unit.GetComponent<Character>().obtainObject(go.GetComponent<Item>());
                    break;
                default:
                    //Hacer un generador aleatorio de diferentes tipos de unidades para los soldados / Que lo mire dependiendo de las armas que lleve
                    randomStats = getRandomSoldierStats();
                    unit.GetComponent<Character>().createCharacter("Enemy", "Terthas Soldier", "Soldier", randomStats[0], randomStats[1], randomStats[2], randomStats[3], randomStats[4], randomStats[5], randomStats[6], randomStats[7], randomStats[8], randomStats[9]);
                    randomWeapon = getRandomWeapon();
                    unit.GetComponent<Weapon>().setWeapon(randomWeapon[0], randomWeapon[1], randomWeapon[2], int.Parse(randomWeapon[3]), int.Parse(randomWeapon[4]), int.Parse(randomWeapon[5]), int.Parse(randomWeapon[6]));
                    unit.GetComponent<Character>().setWeapon(unit.GetComponent<Weapon>());
                    break;
            }
        }

        //Inicialitzem els torns
        turn = 1;
        actualTurn = 'P';
        turnState = 'D';
        selectedCharacter = "";
        menuState = 0;
        goBack = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Comprobar si hay algun personaje seleccionado para mostrar los datos en la UI
         */
        if (selectedCharacter != null && selectedCharacter != "")
        {
            GameObject.Find("UnitName").GetComponent<Text>().text = GameObject.Find(selectedCharacter).GetComponent<Character>().getCharName(); ;
            GameObject.Find("ActualLive").GetComponent<Text>().text = GameObject.Find(selectedCharacter).GetComponent<Character>().getActualPV().ToString();
            GameObject.Find("TotalLive").GetComponent<Text>().text = GameObject.Find(selectedCharacter).GetComponent<Character>().getPV().ToString();
            GameObject.Find("UnitInfo").GetComponent<Image>().enabled = true;
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
            GameObject.Find("UnitInfo").GetComponent<Image>().enabled = false;
        }

        if (turnState == 'T')
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                if (goBack)
                {
                    GameObject.Find("ActualTurn").GetComponent<Text>().enabled = false;
                    turnState = 'I';
                }
                else
                {
                    goBack = true;
                    turnState = nextTurn;
                }
            }
        }

        if (turnState == 'G')//En Game Over sortirem amb qualsevol input tant al teclat com al ratolí
        {
            if (Input.anyKey) SceneManager.LoadScene(0);   //Retornem al menu inicial del joc
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
                                if (hit.collider.tag == "Ally")
                                {
                                    selectedCharacter = hit.collider.name;
                                    //Si la unitat té accions pendents de realitzar
                                    if (GameObject.Find(selectedCharacter).GetComponent<Character>().gethasActions())
                                    {
                                        //Poner el menu visible y un turnState donde no se contemple nada hasta que el jugador seleccione una opción.
                                        turnState = 'W';
                                        menuState = 1;
                                        GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayUnitMenu(true);
                                    }
                                }
                                else if (hit.collider.tag == "Enemy")
                                {
                                    //Si seleccionem una unitat enemiga podem veure les seves estadístiques
                                    selectedCharacter = hit.collider.name;
                                }
                            }
                        }
                        //Si pulsem ESC sense tenir res activat mostrem el menu d'opcions
                        if (Input.GetKey(KeyCode.Escape) && goBack)
                        {
                            goBack = false;
                            turnState = 'T';
                            timer = 0.5f;
                            nextTurn = 'U';
                            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayPauseMenu(true);
                        }
                        break;
                    //Personatge seleccionat i disponible per a moure
                    case 'C':
                        if (Input.GetKey(KeyCode.Escape) && menuState == 2 && goBack)
                        {
                            goBack = false;
                            //Se cancela la accion de movimiento y volvemos al menu anterior
                            turnState = 'T';
                            timer = 0.5f;
                            nextTurn = 'W';
                            menuState = 1;
                            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayUnitMenu(true);
                            GameObject.Find("MovementArea").transform.position = new Vector3(371, GameObject.Find("MovementArea").transform.position.y, 88);

                        }
                        else if (Input.GetMouseButtonDown(0))
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
                        if (GameObject.Find(selectedCharacter).GetComponent<NavMeshAgent>().remainingDistance < 0.1f) {
                            GameObject.Find(selectedCharacter).GetComponent<NavMeshAgent>().isStopped = true;

                            GameObject.Find(selectedCharacter).GetComponent<Character>().setCanMove(false);

                            //Mostraremos otra vez el menú de unidad!!!
                            turnState = 'W';
                            menuState = 3;
                            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayUnitMenu(true);

                            GameObject.Find("AtackArea").transform.position = new Vector3(GameObject.Find(selectedCharacter).transform.position.x, GameObject.Find("AtackArea").transform.position.y, GameObject.Find(selectedCharacter).transform.position.z);

                            GameObject.Find("MovementArea").transform.position = new Vector3(371, GameObject.Find("MovementArea").transform.position.y, 88);
                        }
                        break;
                    //Personatge en espera de selecció d'un enemic al que atacar
                    case 'B':
                        if (Input.GetKey(KeyCode.Escape) && menuState == 5 && goBack)
                        {
                            //Si ens trobem en la seleccio d'enemic tornem a la seleccio d'arma
                            goBack = false;
                            nextTurn = 'S';
                            turnState = 'T';
                            timer = 0.5f;
                            menuState = 4;
                            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayWeaponMenu(true, 'A');
                        }
                        else if (Input.GetMouseButtonDown(0))
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit))
                            {
                                //Si pulsem sobre un enemic
                                if (hit.collider.tag == "Enemy")
                                {
                                    if (enemyTarget == GameObject.Find(hit.collider.name))
                                    {
                                        //Confirmem que se selecciona aquest enemic per atacar i comencem l'atac
                                        turnState = 'P';
                                    }
                                    else
                                    {
                                        GameObject[] en = getEnemiesInRange(GameObject.Find(selectedCharacter).transform.position, 4f, GameObject.Find(selectedCharacter).tag); //El rang que es passa sera el rang que tingui l'arma d'atac
                                        bool isInRange = false;
                                        foreach (GameObject e in en)
                                        {
                                            if (e != null && e.name == hit.collider.name)
                                            {
                                                isInRange = true;
                                            }
                                        }
                                        //Si l'enemic es toba a rang d'atac de la unitat
                                        if (isInRange)
                                        {
                                            enemyTarget = GameObject.Find(hit.collider.name);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    //Personatge en espera de selecció d'arma amb la que atacar
                    case 'S':
                        if (Input.GetKey(KeyCode.Escape) && menuState == 4 && goBack)
                        {
                            //Si ens trobem en la selecció d'arma tornem a la selecció d'acció
                            goBack = false;
                            turnState = 'T';
                            timer = 0.5f;
                            nextTurn = 'W';
                            if (GameObject.Find(selectedCharacter).GetComponent<Character>().getCanMove() == true)
                                menuState = 1;
                            else
                                menuState = 3;
                            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayWeaponMenu(false, 'A');
                            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayUnitMenu(true);
                            GameObject.Find("AtackArea").transform.position = new Vector3(GameObject.Find(selectedCharacter).transform.position.x, GameObject.Find("AtackArea").transform.position.y, GameObject.Find(selectedCharacter).transform.position.z);
                            GameObject.Find("MovementArea").transform.position = new Vector3(371, GameObject.Find("MovementArea").transform.position.y, 88);
                        }
                        else
                        {
                            /*
                             * Esperar hasta seleccionar el arma
                             */
                            if (GameObject.Find("UnitActions").GetComponent<UnitMenuController>().selectedItem)
                            {
                                if (GameObject.Find(selectedCharacter).GetComponent<Character>().getWeaponInInventory(GameObject.Find("UnitActions").GetComponent<UnitMenuController>().sItem) != GameObject.Find(selectedCharacter).GetComponent<Character>().getEquipedWeapon())
                                {
                                    //Si l'arma seleccionada no coincideix amb la equipada cambiem la equipació del personatge i continuem
                                    GameObject.Find(selectedCharacter).GetComponent<Character>().setEquipedWeapon(GameObject.Find(selectedCharacter).GetComponent<Character>().getWeaponInInventory(GameObject.Find("UnitActions").GetComponent<UnitMenuController>().sItem));
                                }    
                                GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayWeaponMenu(false, 'A');
                                turnState = 'B';
                                menuState = 5;
                            }
                        }
                        break;
                    //Comença el combat contra l'enemic seleccionat
                    case 'P':
                        turnState = 'I';
                        int dead = combatTurn(GameObject.Find(selectedCharacter).GetComponent<Character>(), enemyTarget.GetComponent<Character>());
                        switch (dead)
                        {
                            case 1:
                                Debug.Log(enemyTarget.name + " a matat a " + selectedCharacter + " en combat.");
                                GameObject.Find(selectedCharacter).GetComponent<Character>().setIsDead(true);
                                GameObject.Destroy(GameObject.Find(selectedCharacter));
                                break;
                            case 2:
                                Debug.Log(selectedCharacter + " a matat a " + enemyTarget.name + " en combat.");
                                enemyTarget.GetComponent<Character>().setIsDead(true);
                                GameObject.Destroy(enemyTarget);
                                break;
                        }
                        checkUnits();
                        GameObject.Find(selectedCharacter).GetComponent<Character>().SetHasActions(false);
                        selectedCharacter = "";
                        enemyTarget = null;
                        disableUnit();
                        break;
                    //Curem a un aliat amb màgia
                    case 'H':
                        if (Input.GetKey(KeyCode.Escape) && menuState == 6 && goBack)
                        {
                            //Si ens trobem en la seleccio d'enemic tornem a la seleccio d'arma
                            goBack = false;
                            nextTurn = 'W';
                            turnState = 'T';
                            if (GameObject.Find(selectedCharacter).GetComponent<Character>().getCanMove() == true)
                                menuState = 1;
                            else
                                menuState = 3;
                            timer = 0.5f;
                        }
                        else if (Input.GetMouseButtonDown(0))
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit))
                            {
                                //Si pulsem sobre un enemic
                                if (hit.collider.tag == "Ally")
                                {
                                    if (selectedCharacter != hit.collider.name)
                                    {
                                        turnState = 'I';
                                        GameObject[] al = getAliesInRange(GameObject.Find(selectedCharacter).transform.position, 4f, GameObject.Find(selectedCharacter).tag); //El rang que es passa sera el rang que tingui l'arma d'atac
                                        bool isInRange = false;
                                        foreach (GameObject a in al)
                                        {
                                            if (a != null && a.name == hit.collider.name)
                                            {
                                                isInRange = true;
                                            }
                                        }
                                        //Si l'aliat seleccionat es troba a rang i necessita curacions
                                        if (isInRange && GameObject.Find(hit.collider.name).GetComponent<Character>().isInjured())
                                        {
                                            GameObject.Find(hit.collider.name).GetComponent<Character>().heal(GameObject.Find(selectedCharacter).GetComponent<Character>().magicHealing());
                                            checkUnits();
                                            GameObject.Find(selectedCharacter).GetComponent<Character>().SetHasActions(false);
                                            selectedCharacter = "";
                                            enemyTarget = null;
                                            disableUnit();
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    //Esperem a alguna acció del jugador
                    case 'W':
                        if (Input.GetKey(KeyCode.Escape) && goBack)
                        {
                            //Pasar al menu anterior
                            switch (menuState)
                            {
                                case 1:
                                    //S'ha seleccionat una unitat -> tirem enrere i no la seleccionem
                                    goBack = false;
                                    selectedCharacter = "";
                                    GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayUnitMenu(false);
                                    turnState = 'T';
                                    timer = 0.5f;
                                    nextTurn = 'I';
                                    menuState = 0;
                                    break;
                                case 3:
                                    //La unitat s'ha mogut, per tant la tornem a posicionar a la seva posició inicial i tornem a entrar en l'estat de moviment
                                    goBack = false;
                                    GameObject.Find(selectedCharacter).transform.position = initialmovementPoint;
                                    GameObject.Find(selectedCharacter).GetComponent<Character>().setCanMove(true);
                                    GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayUnitMenu(false);
                                    menuState = 2;
                                    turnState = 'T';
                                    timer = 0.5f;
                                    nextTurn = 'C';
                                    GameObject.Find("MovementArea").transform.position = new Vector3(initialmovementPoint.x, GameObject.Find("MovementArea").transform.position.y, initialmovementPoint.z);
                                    break;
                                case 6:
                                    //L'inventari està en pantalla i es vol tornar a la selecció d'accions
                                    goBack = false;
                                    turnState = 'T';
                                    timer = 0.5f;
                                    nextTurn = 'W';
                                    if (GameObject.Find(selectedCharacter).GetComponent<Character>().getCanMove() == true)
                                        menuState = 1;  //Si la unitat no s'ha mogut
                                    else
                                        menuState = 3;  //Si la unitat s'ha mogut
                                    GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayWeaponMenu(false, 'I');
                                    GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayUnitMenu(true);
                                    break;

                                case 7:
                                    //La pantalla d'us d'objectes està activa i volem tornar a la selecció d'objectes
                                    goBack = false;
                                    turnState = 'T';
                                    timer = 0.5f;
                                    nextTurn = 'W';
                                    menuState = 6;
                                    GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayWeaponMenu(true, 'X');
                                    GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayItemUsage(false, 0);
                                    break;

                            }
                        }
                        break;
                    case 'U':
                        //estat del Menu de pausa
                        if (Input.GetKey(KeyCode.Escape) && goBack)
                        {
                            //tornem a l'estat I si pulsem ESC en el menu de pausa
                            goBack = false;
                            turnState = 'T';
                            timer = 0.5f;
                            nextTurn = 'I';
                            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayPauseMenu(false);
                        }
                        break;
                }
                break;
            case 'A':   //Torn de la IA
                if (selAICharacter == null)
                {
                    selAICharacter = findUnitWithClosestThreads();
                    if (selAICharacter == null)
                        disableUnit();
                    else
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
                                GameObject closest = getClosestThread(selAICharacter.transform.position, selAICharacter.tag, selAICharacter);
                                initialmovementPoint = selAICharacter.transform.position;
                                destination = closest.transform.position;
                                GameObject[] en = getEnemiesInRange(selAICharacter.transform.position, 4f, selAICharacter.tag);
                                bool rangedEn = false;
                                foreach (GameObject e in en)
                                {
                                    if (e != null) rangedEn = true;
                                }
                                if (rangedEn)
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
                                        destination = enemyTarget.transform.position;
                                    }
                                    isNotMoving = true;
                                }
                                else
                                {
                                    enemyTarget = closest;

                                    if (Vector3.Distance(destination, selAICharacter.transform.position) < 20)    //20 es el radi de la esfera i per tant el valor que haurem d'assignar al moviment del personatge
                                    {
                                        selAICharacter.GetComponent<NavMeshAgent>().SetDestination(destination);
                                        selAICharacter.GetComponent<NavMeshAgent>().stoppingDistance = 4;   //Fem que es quedi a una distancia prudencial de la unitat enemiga
                                        selAICharacter.GetComponent<NavMeshAgent>().isStopped = false;
                                        isNotMoving = false;
                                    }
                                    else if (Vector3.Distance(destination, selAICharacter.transform.position) > 50 || selAICharacter.name == "Omak")// Si la unitat es troba a molta distancia de les unitats aliades o es el cap no es mouran
                                    {
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

                                if (Vector3.Distance(destination, selAICharacter.transform.position) <= selAICharacter.GetComponent<NavMeshAgent>().stoppingDistance || isNotMoving)
                                {
                                    flagIA = false;
                                    selAICharacter.GetComponent<NavMeshAgent>().isStopped = true;
                                    GameObject[] en = getEnemiesInRange(selAICharacter.transform.position, 4f, selAICharacter.tag); 

                                    bool rangedEn = false;
                                    foreach (GameObject e in en)
                                    {
                                        if (e != null) rangedEn = true;
                                    }
                                    if (rangedEn)
                                    {
                                        bool hiEs = false;
                                        int i = 0;
                                        do
                                        {
                                            if (en[i] == enemyTarget)
                                                hiEs = true;
                                            else
                                                i++;
                                        } while (hiEs == false && i < en.Length);

                                        if (hiEs == false && i == en.Length) i = 0;  //Si l'enemic seleccionat no es troba dins de la llista atacara al primer que trobi a rang

                                        Debug.Log("Es pot atacar a un enemic!");
                                        Debug.Log(selAICharacter.name + " es disposa a atacar a " + en[i].name);
                                        int dead = combatTurn(selAICharacter.GetComponent<Character>(), en[i].GetComponent<Character>());
                                        switch (dead)
                                        {
                                            case 1:
                                                Debug.Log(en[i].name + " a matat a " + selAICharacter.name + " en combat.");
                                                selAICharacter.GetComponent<Character>().setIsDead(true);
                                                GameObject.Destroy(selAICharacter);
                                                break;
                                            case 2:
                                                Debug.Log(selAICharacter.name + " a matat a " + en[i].name + " en combat.");
                                                en[0].GetComponent<Character>().setIsDead(true);
                                                GameObject.Destroy(en[i]);
                                                break;
                                        }
                                        checkUnits();
                                    }
                                    else
                                    {
                                        Debug.Log("No hi ha enemics a prop.");
                                    }
                                    enemyTarget = null;
                                    isNotMoving = false;
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
        //Desactivem el personatge del jugador
        if (selectedCharacter != null && selectedCharacter != "")
        {
            GameObject.Find(selectedCharacter).GetComponent<Character>().SetHasActions(false);
            selectedCharacter = "";
        }
        //Desactivem el personatge de la IA que estigui seleccionat
        if (selAICharacter != null)
        {
            selAICharacter.GetComponent<Character>().SetHasActions(false);
            selAICharacter = null;
        }
        unitsToMove = getActiveUnits();
        if (actualTurn == 'A' && turnState != 'G') turnState = 'I';
        if (unitsToMove == 0 && turnState != 'G')
        {
            switch (actualTurn)
            {
                case 'P':
                    activateUnits("Enemy");
                    flagIA = true;
                    actualTurn = 'A';
                    displayTurn();
                    unitsToMove = GameObject.FindGameObjectsWithTag("Enemy").Length;
                    break;
                case 'A':
                    turn++; //Cambiem de torn cada cop que acaba la IA el seu
                    actualTurn = 'P';
                    displayTurn();
                    unitsToMove = GameObject.FindGameObjectsWithTag("Ally").Length;
                    activateUnits("Ally");
                    break;
            }
            timer = 5.0f;
            turnState = 'T';
        }
    }

    public char getTurnState()
    {
        return turnState;
    }

    public void setTurnState(char turn)
    {
        this.turnState = turn;
        if (turnState == 'T')
        {
            displayTurn();
        }
    }

    public string getSelectedCharacter()
    {
        return selectedCharacter;
    }

    //Funció que retornará els enemics del personatge que estiguin a rang d'atac
    public GameObject[] getEnemiesInRange(Vector3 pos, float range, string tag)
    {
        string enemies = "";
        switch (tag)
        {
            case "Ally":
                enemies = "Enemy";
                break;
            case "Enemy":
                enemies = "Ally";
                break;
        }
        return getUnitsInRange(pos, range, enemies);
    }

    //Funció que retornará els aliats del personatge que estiguin a rang
    public GameObject[] getAliesInRange(Vector3 pos, float range, string tag)
    {
        string allies = "";
        switch (tag)
        {
            case "Ally":
                allies = "Ally";
                break;
            case "Enemy":
                allies = "Enemy";
                break;
        }
        return getUnitsInRange(pos, range, allies);
    }

    //Funció que retorna les unitats a rang de la seleccionada
    public GameObject[] getUnitsInRange(Vector3 pos, float range, string tag)
    {
        GameObject[] enemies = null;
        int i = 0;
        int enNmbr = 0;

        enemies = GameObject.FindGameObjectsWithTag(tag);


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
     * Crear una función para la ejecucion de los combates entre 2 personajes
     */
    public int combatTurn(Character starter, Character recieber)
    {
        // Mes endevant podrem trobar més opcions depenent de les armes que portin equipades de moment no es contempla

        //ToDo-> contar con las animaciones de batalla

        int atkmode = 0;    //Variable que indicara qui realitzara el doble atac en cas de poder-se realitzar (1-> l'atacant, 2-> l'atacat, 0-> Cap d'ells)
        bool recIsDead = false;
        playerUnitMissed = false;

        if (starter.getSpd() >= (recieber.getSpd() + 5))
        {
            atkmode = 1;
        }
        else if (recieber.getSpd() >= (starter.getSpd() + 5))
        {
            atkmode = 2;
        }

        playerUnitMissed = true;

        //Ataca l'atacant
        if (isAnAlly(starter.getCharName()))
            whoIsAttacking = "Ally";
        else
            whoIsAttacking = "Enemy";
        recIsDead = realizeAtack(starter, recieber);
        if (recIsDead)
        {
            if (whoIsAttacking == "Ally")
            {
                starter.lvlUp(experienceGainedInBattle(starter, recieber, recIsDead, playerUnitMissed));
            }
            return 2;
        }
        //Ataca l'atacat
        if (isAnAlly(recieber.getCharName()))
            whoIsAttacking = "Ally";
        else
            whoIsAttacking = "Enemy";
        recIsDead = realizeAtack(recieber, starter);
        if (recIsDead)
        {
            if (whoIsAttacking == "Ally")
            {
                recieber.lvlUp(experienceGainedInBattle(recieber, starter, recIsDead, playerUnitMissed));
            }
            return 1;
        }

        switch (atkmode)
        {
            case 1:
                if (isAnAlly(starter.getCharName()))
                    whoIsAttacking = "Ally";
                else
                    whoIsAttacking = "Enemy";
                recIsDead = realizeAtack(starter, recieber);
                if (recIsDead)
                {
                    if (whoIsAttacking == "Ally")
                    {
                        starter.lvlUp(experienceGainedInBattle(starter, recieber, recIsDead, playerUnitMissed));
                    }
                    return 2;
                }
                break;
            case 2:
                if (isAnAlly(recieber.getCharName()))
                    whoIsAttacking = "Ally";
                else
                    whoIsAttacking = "Enemy";
                recIsDead = realizeAtack(recieber, starter);
                if (recIsDead)
                {
                    if (whoIsAttacking == "Ally")
                    {
                        recieber.lvlUp(experienceGainedInBattle(recieber, starter, recIsDead, playerUnitMissed));
                    }
                    return 1;
                }
                break;
        }
        if (isAnAlly(starter.getCharName()))
            whoIsAttacking = "Ally";
        else
            whoIsAttacking = "Enemy";
        if (whoIsAttacking == "Ally")
        {
            starter.lvlUp(experienceGainedInBattle(starter, recieber, recIsDead, playerUnitMissed));
        }
        else
        {
            recieber.lvlUp(experienceGainedInBattle(recieber, starter, recIsDead, playerUnitMissed));
        }

        whoIsAttacking = "";

        return 0;
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
        if (!isAHit && whoIsAttacking == "Ally" && playerUnitMissed)
        {
            playerUnitMissed = true;
        }
        else
        {
            playerUnitMissed = false;
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
        //De moment passarem qualsevol unitat que estigui activa
        GameObject[] units = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject un = null;
        int i = 0;

        do
        {
            if (units[i].GetComponent<Character>().gethasActions())
                un = units[i];
            else
                i++;
        } while (un == null);
        return un;
    }

    /*
     * Funció que retorna l'enemic més proper a la unitat (Més endevant aquesta amenaça no ha de ser la unitat més propera sino la que mes dany pugui causar)
     */
    public GameObject getClosestThread(Vector3 pos, string tag, GameObject centerUn)
    {
        GameObject[] enemies = null;
        GameObject enInRange = null;
        GameObject[] closestThread = new GameObject[20];
        string weaponWeakness = "";
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
                closestThread[i] = unit;
                i++;
            }
        }
        switch (centerUn.GetComponent<Character>().getEquipedWeapon().getType())
        {
            case "Sword":
                weaponWeakness = "Axe";
                break;
            case "Lance":
                weaponWeakness = "Sword";
                break;
            case "Axe":
                weaponWeakness = "Lance";
                break;
        }
        minDistance = -1;
        foreach (GameObject un in closestThread)
        {
            if (un != null)
            {
                if (minDistance == -1)
                {
                    enInRange = un;
                    minDistance = Vector3.Distance(un.transform.position, pos);
                }
                else
                {
                    if (un.GetComponent<Character>().getEquipedWeapon().getType() == weaponWeakness)
                    {
                        enInRange = un;
                    }
                    else
                    {
                        if (Vector3.Distance(un.transform.position, pos) <= minDistance)
                        {
                            enInRange = un;
                            i++;
                        }
                    }
                }
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
            if (!unit.GetComponent<Character>().getisDead()) alive++;
        }

        if (alive == 0)
        {
            GameObject.Find("MusicControl").GetComponent<MusicControl>().playMusic(5);
            GameObject.Find("GAMEOVER").GetComponent<Text>().enabled = true;
            turnState = 'G';
            Debug.Log("GAME OVER, ja no et queden unitats per continuar.");
        }
    }

    /*
     * Funció que inicialitza la curació a una unitat amb màgia
     */
    public void healAlly()
    {
        menuState = 6;
        turnState = 'H';
    }

    /*
     * Funció per activar totes les unitats a l'inici del torn
     */
    public void activateUnits(string tag)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject unit in units)
        {
            unit.GetComponent<Character>().SetHasActions(true);
            unit.GetComponent<Character>().setCanMove(true);
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
            if (unit.GetComponent<Character>().gethasActions())
                actUnits++;
        }
        return actUnits;
    }

    public void displayTurn()
    {
        GameObject.Find("TurnShow").GetComponent<Text>().enabled = true;
        switch (actualTurn)
        {
            case 'P':
                GameObject.Find("MusicControl").GetComponent<MusicControl>().playMusic(1);
                GameObject.Find("TurnShow").GetComponent<Text>().text = "Turno " + turn + " del jugador";
                GameObject.Find("ActualTurn").GetComponent<Text>().text = "Player Turn";
                GameObject.Find("ActualTurn").GetComponent<Text>().color = Color.blue;
                Debug.Log("Turno " + turn + " del jugador");
                unitsToMove = GameObject.FindGameObjectsWithTag("Ally").Length;
                break;
            case 'A':
                GameObject.Find("MusicControl").GetComponent<MusicControl>().playMusic(2);
                GameObject.Find("TurnShow").GetComponent<Text>().text = "Turno " + turn + " de la IA";
                GameObject.Find("ActualTurn").GetComponent<Text>().text = "Enemy Turn";
                GameObject.Find("ActualTurn").GetComponent<Text>().color = Color.red;
                Debug.Log("Turno " + turn + " de la IA");
                unitsToMove = GameObject.FindGameObjectsWithTag("Enemy").Length;
                break;
        }

        GameObject.Find("ActualTurn").GetComponent<Text>().enabled = true;
    }

    /*
     * Funció que inicia el moviment de la unitat
     */
    public void iniMovement()
    {
        initialmovementPoint = GameObject.Find(selectedCharacter).transform.position;
        turnState = 'C';
        menuState = 2;
        GameObject.Find("MovementArea").transform.position = new Vector3(initialmovementPoint.x, GameObject.Find("MovementArea").transform.position.y, initialmovementPoint.z);
    }

    /*
     * Funció que desactiva la unitat activa seleccionada per el jugador.
     */
    public void deactivateUnit()
    {
        turnState = 'I';
        disableUnit();
    }

    /*
     * Funció que habilita la selecció d'un enemic a atacar per una unitat del jugador
     */
    public void selectAttackTarget()
    {
        turnState = 'B';
        menuState = 5;
    }

    /*
     * Funció que mostra l'elecció de l'arma a emprar per atacar a un enemic
     */
    public void selectWeaponToAttack()
    {
        turnState = 'S';
        menuState = 4;
        goBack = true;
    }

    /*
     * Funció que ens inicia l'estat de victoria
     */
    public void endGameVictory()
    {
        GameObject.Find("MusicControl").GetComponent<MusicControl>().playMusic(4);
        GameObject.Find("GAMEOVER").GetComponent<Text>().text = "VICTORY!";
        GameObject.Find("GAMEOVER").GetComponent<Text>().color = Color.blue;
        turnState = 'G';
    }

    /*
     * Funció que calcula la quantitat d'experiencia guanyada en una batalla
     */
    public int experienceGainedInBattle(Character unit, Character enemy, bool kill, bool missed)
    {
        int ld = 0;     //Variable que ens informarà de la diferencia de nivells entre la unitat i l'enemic
        int exp = 0;    //Variable que ens indicarà quanta experiencia guaña en total.
        int bonus = 0;

        if (missed)
        {
            //Si la unitat falla tots els atacs no aconsegueix experiencia
            exp = 0;
        }
        else
        {
            //Si no ha fallat el cop calculem la diferencia de nivells
            if (enemy.name == "Omak" || enemy.name == "Ardsede")
            {
                //Si l'enemic es un enemic especial apliquem bonificadors als resultats
                ld = (enemy.getLvl() + 20) - unit.getLvl();
                bonus = 20;
            }
            else
            {
                ld = enemy.getLvl() - unit.getLvl();
            }
            if (kill)
            {
                //La unitat mata a l'enemic
                if (ld >= 0)
                {
                    exp = 20 + (ld * 3) + bonus;
                }
                else if (ld == -1)
                {
                    exp = 20 + bonus;
                }
                else if (ld <= -2)
                {
                    exp = Mathf.Max((26 + (ld) * 3) + bonus, 7);
                }
            }
            else
            {
                //La unitat no ha matat a l'enemic
                if (ld >= 0)
                {
                    exp = (31 + ld) / 3;
                }
                else if (ld == -1)
                {
                    exp = 10;
                }
                else if (ld <= -2)
                {
                    exp = Mathf.Max((33 + ld) / 3, 1);
                }
            }
        }

        return exp;
    }

    /*
     * Funció que ens indica si una unitat concreta es aliada o enemiga
     */
    public bool isAnAlly(string name)
    {
        GameObject ally = GameObject.Find(name);

        if (name == "Terthas Soldier") return false;

        if (ally.tag == "Ally")
            return true;
        else
            return false;
    }

    /*
     * Funció que genera les estadístiques aleatories dels soldats enemics
     */
    public int[] getRandomSoldierStats()
    {
        int[] stats = new int[10];
        //Generem el nivell dels soldats (entre 1 i 5)
        int lvl = Random.Range(1, 5);
        int growth;
        //inicialitzem els valors base de la classe, els quals modificarem depenent del nivell generat automaticament.
        int pv = 16;
        int str = 3;
        int mag = 0;
        int skl = 4;
        int spd = 6;
        int lck = 0;
        int def = 6;
        int res = 6;
        int mov = 20;

        if (lvl != 1)
        {
            for (int i = 1; i < lvl; i++)
            {
                growth = Random.Range(1, 100);
                if (growth <= 40) pv++;

                growth = Random.Range(1, 100);
                if (growth <= 10) str++;

                growth = Random.Range(1, 100);
                if (growth <= 0) mag++;

                growth = Random.Range(1, 100);
                if (growth <= 10) skl++;

                growth = Random.Range(1, 100);
                if (growth <= 10) spd++;

                growth = Random.Range(1, 100);
                if (growth <= 0) lck++;

                growth = Random.Range(1, 100);
                if (growth <= 5) def++;

                growth = Random.Range(1, 100);
                if (growth <= 5) res++;
            }
        }

        stats[0] = pv;
        stats[1] = str;
        stats[2] = mag;
        stats[3] = skl;
        stats[4] = spd;
        stats[5] = lck;
        stats[6] = def;
        stats[7] = res;
        stats[8] = mov;
        stats[9] = lvl;

        return stats;
    }

    public string[] getRandomWeapon()
    {
        string[] weapon = new string[7];

        switch (Random.Range(1, 3))
        {
            case 1:
                //Generem una espasa
                weapon[0] = "Steel Sword";
                weapon[1] = "Sword";
                weapon[2] = "C";
                weapon[3] = "8";
                weapon[4] = "1";
                weapon[5] = "90";
                weapon[6] = "0";
                break;
            case 2:
                //Generem una llança
                weapon[0] = "Steel Lance";
                weapon[1] = "Lance";
                weapon[2] = "C";
                weapon[3] = "9";
                weapon[4] = "1";
                weapon[5] = "80";
                weapon[6] = "0";
                break;
            case 3:
                //Generem una destral
                weapon[0] = "Steel Axe";
                weapon[1] = "Axe";
                weapon[2] = "C";
                weapon[3] = "11";
                weapon[4] = "1";
                weapon[5] = "70";
                weapon[6] = "0";
                break;
        }

        return weapon;
    }

    public void setMenuState(int state)
    {
        this.menuState = state;
    }

    public void dropItem(int i)
    {
        GameObject.Find(selectedCharacter).GetComponent<Character>().dropItem(i);
        GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayItemUsage(false, 0);
        deactivateUnit();
    }

    public void useItem(int i)
    {
        if (GameObject.Find(selectedCharacter).GetComponent<Character>().useItem(i))
        {
            GameObject.Find("UnitActions").GetComponent<UnitMenuController>().displayItemUsage(false, 0);
            deactivateUnit();
        }
    }

    public void endTurn()
    {
        flagIA = true;
        actualTurn = 'A';
        displayTurn();
        unitsToMove = GameObject.FindGameObjectsWithTag("Enemy").Length;
        timer = 5.0f;
        turnState = 'T';
    }
}
