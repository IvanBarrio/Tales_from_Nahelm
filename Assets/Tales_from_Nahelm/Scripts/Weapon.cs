using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{

    private string weaponName;  //Nom de l'arma
    private string type;        //Tipus d'arma (Espasa, Llança, Destral, etc)
    private string skill;       //Habilitat que ha de tenir l'usuari per emprar l'arma
    private int atk;            //Potencia d'atac de l'arma
    private int range;          //Rang d'atac de l'arma
    private int hit;            //Probabilitat d'impacte de l'arma
    private int crit;           //Probabilitat d'atac critic de l'arma

    //Inicialitzem les variables de l'arma quan la creem
    public void setWeapon(string n, string t, string s, int a, int r, int h, int c)
    {
        weaponName = n;
        iName = n;
        type = t;
        skill = s;
        atk = a;
        range = r;
        hit = h;
        crit = c;
    }

    public string getWeaponName()
    {
        return weaponName;
    }

    public string getType()
    {
        return type;
    }

    public string getSkill()
    {
        return skill;
    }

    public int getAtk()
    {
        return atk;

    }

    public int getRange()
    {
        return range;
    }

    public int getHit()
    {
        return hit;
    }

    public int getCrit()
    {
        return crit;
    }

    //Funció que determina si una arma és més forta que una altra proporcionada (Si és així proporciona un multiplicador x3 a la potencia de l'arma)
    public bool isEffectiveAgainst(Weapon enemyWeapon)
    {
        if (type == "Sword" && enemyWeapon.getType() == "Axe") return true;
        if (type == "Axe" && enemyWeapon.getType() == "Lance") return true;
        if (type == "Lance" && enemyWeapon.getType() == "Sword") return true;
        if (type == "AnimaMagic" && enemyWeapon.getType() == "LightMagic") return true;
        if (type == "LightMagic" && enemyWeapon.getType() == "DarkMagic") return true;
        if (type == "DarkMagic" && enemyWeapon.getType() == "AnimaMagic") return true;
        return false;
    }

    //Funció que determina si una arma proporcionada és més forta que aquesta (Retorna si o no)
    public bool isIneffectiveAgainst(Weapon enemyWeapon)
    {
        if (type == "Axe" && enemyWeapon.getType() == "Sword") return true;
        if (type == "Lance" && enemyWeapon.getType() == "Axe") return true;
        if (type == "Sword" && enemyWeapon.getType() == "Lance") return true;
        if (type == "LightMagic" && enemyWeapon.getType() == "AnimaMagic") return true;
        if (type == "DarkMagic" && enemyWeapon.getType() == "LightMagic") return true;
        if (type == "AnimaMagic" && enemyWeapon.getType() == "DarkMagic") return true;
        return false;
    }
}
