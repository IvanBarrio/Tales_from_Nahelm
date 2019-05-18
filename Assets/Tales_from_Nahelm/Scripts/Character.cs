using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private string charName;            //Nom del personatge
    private string charClass;           //Classe del personatge
    private int lvl;                    //Nivell actual del personatge
    private int exp;                    //Experiencia actual del personatge
    private int pv;                     //Punts de vitalitat del personatge
    private int str;                    //Força del personatge
    private int mag;                    //Magia del personatge
    private int skl;                    //Habilitat del personatge
    private int spd;                    //Velocitat del personatge
    private int lck;                    //Sort del personatge
    private int def;                    //Defensa del personatge
    private int res;                    //Resistencia del personatge
    private int mov;                    //Moviment del personatge
    private int maxPv;                  //Punts maxims de vitalitat del personatge
    private int maxStr;                 //Força maxima del personatge
    private int maxMag;                 //Magia maxima del personatge
    private int maxSkl;                 //Habilitat maxima del personatge
    private int maxSpd;                 //Velocitat maxima del personatge
    private int maxLck;                 //Sort maxima del personatge
    private int maxDef;                 //Defensa maxima del personatge
    private int maxRes;                 //Resistencia maxima del personatge
    private int pvGrowth;               //Creixement de punts de vitalitat del personatge
    private int strGrowth;              //Creixement de força del personatge
    private int magGrowth;              //Creixement de magia del personatge
    private int sklGrowth;              //Creixement d'habilitat del personatge
    private int spdGrowth;              //Creixement de velocitat del personatge
    private int lckGrowth;              //Creixement de sort del personatge
    private int defGrowth;              //Creixement de defensa del personatge
    private int resGrowth;              //Creixement de resistencia del personatge
    private Weapon equipedWeapon;       //Arma equipada
    private Item[] inventory;           //Motxilla del personatge
    private int actualPV;               //Valor actual dels punts de vida del personatge
    private bool canMove;
    private bool isDead;
    private bool hasActions;

    //Funció inicialitzadora del personatge
    public void createCharacter(string tag, string cn, string cc, int pv, int str, int mag, int skl, int spd, int lck, int def, int res, int mov, int lvl)
    {
        this.gameObject.tag = tag;  //Definim quin tipus de personatge és (Aliat, enemic o neutral)
        //definim la resta de caracteristiques del personatge
        charName = cn;
        charClass = cc;
        this.pv = pv;
        this.actualPV = pv;
        this.str = str;
        this.mag = mag;
        this.skl = skl;
        this.spd = spd;
        this.lck = lck;
        this.def = def;
        this.res = res;
        this.mov = mov;
        this.lvl = lvl;
        this.canMove = true;
        this.isDead = false;
        this.hasActions = true;
        inventory = new Item[5];
        //Tots els personatges començarán sense experiencia
        this.exp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Setters
 
    public void setStatsMaxs(int pvm, int strm, int magm, int sklm, int spdm, int lckm, int defm, int resm)
    {
        this.maxPv = pvm;
        this.maxStr = strm;
        this.maxMag = magm;
        this.maxSkl = sklm;
        this.maxSpd = spdm;
        this.maxLck = lckm;
        this.maxDef = defm;
        this.maxRes = resm;
    }

    public void setStatsGrowth(int pvg, int strg, int magg, int sklg, int spdg, int lckg, int defg, int resg)
    {
        this.pvGrowth = pvg;
        this.strGrowth = strg;
        this.magGrowth = magg;
        this.sklGrowth = sklg;
        this.spdGrowth = spdg;
        this.lckGrowth = lckg;
        this.defGrowth = defg;
        this.resGrowth = resg;
    }

    public void setWeapon(Weapon w)
    {
        equipedWeapon = w;
        inventory[0] = w;
    }

    public void SetHasActions(bool hasActions)
    {
        this.hasActions = hasActions;
    }

    public void setCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public void setIsDead(bool isDead)
    {
        this.isDead = isDead;
    }

    //Getters

    public string getCharName()
    {
        return this.charName;
    }

    public int getLvl()
    {
        return this.lvl;
    }

    public int getDef()
    {
        return def;
    }

    public int getSpd()
    {
        return spd;
    }

    public int getPV()
    {
        return pv;

    }

    public int getActualPV()
    {
        return actualPV;
    }

    public bool getCanMove()
    {
        return canMove;
    }

    public bool getisDead()
    {
        return isDead;
    }

    public bool gethasActions()
    {
        return hasActions;
    }

    public int recieveDamage(int damageDealt)
    {
        actualPV -= damageDealt;
        if (actualPV < 0) actualPV = 0;
        return actualPV;
    }

    public Weapon getEquipedWeapon()
    {
        return equipedWeapon;
    }

    //Funció per obtenir el resultat del dany realitzat a un enemic
    public int[] atackSelectedEnemy(Character enemy)
    {
        int damageYouCanDeal = 0;                                                    //Dany total que pot realitzar el personatge
        int damageToEnemy = 0;                                                       //Dany total que pot realitzar el personatge contra l'enemic seleccionat
        int triangleWeaponExtra = 0;                                                    //Bonificació de dany de l'arma respecte la de l'enemic
        int triangleWeaponHit = 0;                                                      //Bonificació d'encert al cop de l'arma respecte la de l'enemic
        double critprob = 0;                                                            //Probabilitat de realitzar un atac crític
        int atkProb = 0;                                                             //Probabilitat de realitzar l'atac si no és critic
        int[] dealtStats = new int[3];                                            //Valors que es retornen per a la realització de l'atac 
        /*
         * Pos 0 -> Percentatge d'encert
         * Pos 1 -> Dany realitzat en cas d'encert
         * Pos 2 -> Percentatge d'activació d'atac crític
         */

        //Comprovem les efectivitats de les armes avans de calcular res
        if (equipedWeapon.isEffectiveAgainst(enemy.getEquipedWeapon()))
        {
            triangleWeaponExtra = 1;
            triangleWeaponHit = 5;
        }
        else if (equipedWeapon.isIneffectiveAgainst(enemy.getEquipedWeapon()))
        {
            triangleWeaponExtra = -1;
            triangleWeaponHit = -5;
        }

        //Comprovem la provabilitat de critic
        critprob = ((skl * 0.5f) + equipedWeapon.getCrit()) - enemy.evadeCrit();
        dealtStats[2] = (int) critprob;

        //SCalculem la probabilitat d'encert
        atkProb = (((skl * 3) + lck) / 2 + equipedWeapon.getHit() + triangleWeaponHit) - enemy.getEvasion();
        dealtStats[0] = atkProb;

        if (equipedWeapon.getType().Equals("Sword") || equipedWeapon.getType().Equals("Axe") || equipedWeapon.getType().Equals("Lance") || equipedWeapon.getType().Equals("Bow"))
        {
            damageYouCanDeal = str + equipedWeapon.getAtk() + triangleWeaponExtra;
        }
        else
        {
            damageYouCanDeal = mag + equipedWeapon.getAtk() + triangleWeaponExtra;
        }

        damageToEnemy = (damageYouCanDeal - enemy.getDef());
        dealtStats[1] = damageToEnemy;

        return dealtStats;
    }

    //Funció que proporciona la evasió a atacs critics cap al personatge
    public int evadeCrit()
    {
        return lck;
    }

    //Funció per obtenir la evasió a un atac normal
    public int getEvasion()
    {
        int evasion = ((spd * 3) + lck) / 2;
        return evasion;
    }

    public void printStats()
    {
        Debug.Log("HP: " + pv);
    }

    //Retornem la quantitat de recuperació de vida que pot oferir una curació màgica.
    public int magicHealing()
    {
        return ((mag / 2) + 8);
    }

    //Retornem si la unitat pot realitzar accions curatives
    //Priestess o Sorcerer
    public bool canHeal()
    {
        bool healer = false;
        if (charClass == "Priestess" || charClass == "Sorcerer")
        {
            if (hasInInventory("Staff")) healer = true;
        }
        return healer;
    }

    /*
     * Funció que retorna si hi ha un objecte en l'inventori del personatge
     */
    public bool hasInInventory(string name)
    {
        bool hasIt = false;
        foreach (Item obj in inventory)
        {
            if (obj != null && obj.getName() == name)
            {
                hasIt = true;
            }
        }
        return hasIt;
    }

    /*
     * Funció per afegir un objecte a l'inventari
     */
    public void obtainObject(Item i)
    {
        int lastInvSp = 0;
        while (lastInvSp < 99)
        {
            if (inventory[lastInvSp] == null)
            {
                inventory[lastInvSp] = i;
                lastInvSp = 99;
            }else
                lastInvSp++;
        }
    }

    /*
     * Funció per pujar experiencia i de nivell en cas d'assolir 100 punts d'experiencia
     */
    public void lvlUp(int exp)
    {
        //Si no ens trobem a maxim nivell
        if (lvl < 20)
        {
            this.exp += exp;
            if (this.exp >= 100)
            {
                this.exp -= 100;
                //pujar el nivell ja que s'han superat els 100 punts d'experiencia
                int[] rands = new int[8];

                //Generar 8 aleatoris entre 1 i 100
                for (int i = 0; i < 8; i++)
                {
                    rands[i] = Random.Range(1, 100);
                }

                //Comprovem si el creixement ens permet pujar la estadistica per a cada valor generat si no es troba ja en el seu maxim valor
                if (rands[0] <= pvGrowth && pv < maxPv)
                {
                    pv += 1;
                }
                if (rands[1] <= strGrowth && str < maxStr)
                {
                    str += 1;
                }
                if (rands[2] <= magGrowth && mag < maxMag)
                {
                    mag += 1;
                }
                if (rands[3] <= sklGrowth && skl < maxSkl)
                {
                    skl += 1;
                }
                if (rands[4] <= spdGrowth && spd < maxSpd)
                {
                    spd += 1;
                }
                if (rands[5] <= lckGrowth && lck < maxLck)
                {
                    lck += 1;
                }
                if (rands[6] <= defGrowth && def < maxDef)
                {
                    def += 1;
                }
                if (rands[7] <= resGrowth && res < maxRes)
                {
                    res += 1;
                }
            }
        }
        Debug.Log(charName + " LVL: " + lvl + " Exp: " + exp);
    }
}
