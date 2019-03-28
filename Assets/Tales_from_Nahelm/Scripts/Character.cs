using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private string charName;            //Nom del personatge
    private string charClass;           //Classe del personatge
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
    private GameObject[] inventory;     //Motxilla del personatge

    //Funció inicialitzadora del personatge
    public void createCharacter(string tag, string cn, string cc, int pv, int str, int mag, int skl, int spd, int lck, int def, int res, int mov)
    {
        this.gameObject.tag = tag;  //Definim quin tipus de personatge és (Aliat, enemic o neutral)
        //definim la resta de caracteristiques del personatge
        charName = cn;
        charClass = cc;
        this.pv = pv;
        this.str = str;
        this.mag = mag;
        this.skl = skl;
        this.spd = spd;
        this.lck = lck;
        this.def = def;
        this.res = res;
        this.mov = mov;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Getters
    public int getDef()
    {
        return def;
    }

    public Weapon getEquipedWeapon()
    {
        return equipedWeapon;
    }

    //Funció per obtenir el resultat del dany realitzat a un enemic
    public double atackSelectedEnemy(Character enemy)
    {
        double damageYouCanDeal = 0;                                                    //Dany total que pot realitzar el personatge
        double damageToEnemy = 0;                                                       //Dany total que pot realitzar el personatge contra l'enemic seleccionat
        int triangleWeaponExtra = 0;                                                    //Bonificació de dany de l'arma respecte la de l'enemic
        int triangleWeaponHit = 0;                                                      //Bonificació d'encert al cop de l'arma respecte la de l'enemic
        int critical = 1;                                                               //Multiplicador aplicable depenent si es realitza atac crític o no
        double critprob = 0;  //Probabilitat de realitzar un atac crític
        double atkProb = 0;                                                             //Probabilitat de realitzar l'atac si no és critic
        bool isAHit = false;                                                            //Variable que indica si s'encerta el cop a realitzar
        int randV;                                                                      //Valor aleatori tret per a les comprovacions de les realitzacions dels diferent atacs

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

        //Es realitza un atac crític?
        critprob = ((skl * 0.5) + equipedWeapon.getCrit() - enemy.evadeCrit());
        randV = Random.Range(1, 100);
        if (randV <= critprob && randV != 0)
        {
            critical = 3;
            isAHit = true;
        }

        //Si l'atac és critic acerta sempre!
        //Si l'atac no és critic
        if (critical == 1)
        {
            //Es realitza l'atac normal?
            atkProb = (((skl * 3) + lck) / 2 + equipedWeapon.getHit() + triangleWeaponHit) - enemy.getEvasion();
            randV = Random.Range(1, 100);
            if (randV <= atkProb && randV != 0)
            {
                isAHit = true;
            }
        }

        if (isAHit)
        {
            if (equipedWeapon.getType().Equals("Sword") || equipedWeapon.getType().Equals("Axe") || equipedWeapon.getType().Equals("Lance") || equipedWeapon.getType().Equals("Bow"))
            {
                damageYouCanDeal = str + equipedWeapon.getAtk() + triangleWeaponExtra;
            }
            else
            {
                damageYouCanDeal = mag + equipedWeapon.getAtk() + triangleWeaponExtra;
            }

            damageToEnemy = (damageYouCanDeal - enemy.getDef()) * critical;
        }
        else
        {
            //Si ha fallat l'atac retornem un negatiu per a informar-ho
            damageToEnemy = -1;
        }
        return damageToEnemy;
    }

    //Funció que proporciona la evasió a atacs critics cap al personatge
    public double evadeCrit()
    {
        return lck;
    }

    //Funció per obtenir la evasió a un atac normal
    public double getEvasion()
    {
        double evasion = ((spd * 3) + lck) / 2;
        return evasion;
    }
}
