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
    private Weapon equipedWeapon;       //Arma equipada
    private Object[] inventory;         //Motxilla del personatge
    private int actualPV;               //Valor actual dels punts de vida del personatge
    private char state;                 //Estat del personatge      A -> Actiu | M -> Mogut | D -> Mort

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
        this.state = 'A';
        inventory = new GameObject[5];
        //Tots els personatges començarán sense experiencia
        this.exp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Setters
    public void setWeapon(Weapon w)
    {
        equipedWeapon = w;
    }

    public void setState(char state)
    {
        this.state = state;
    }

    //Getters
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

    public char getState()
    {
        return state;
    }

    public int recieveDamage(int damageDealt)
    {
        pv -= damageDealt;
        if (pv < 0) pv = 0;
        return pv;
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

    /*
     * ToDo:
     *  -Funció per pujar de nivell
     */
}
