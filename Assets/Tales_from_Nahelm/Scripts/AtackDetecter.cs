using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackDetecter : MonoBehaviour
{
    string character;
    string cTag;
    bool hasEnemies;
    string[] enemies;

    private void OnCollisionEnter(Collision collision)
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().getTurnState() == 'A')
        {
            hasEnemies = false;
            character = GameObject.Find("GameController").GetComponent<GameController>().getSelectedCharacter();
            cTag = GameObject.Find(character).tag;
            int i = 0;
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.Log(contact.thisCollider.name);
                switch (cTag)
                {
                    case "Ally":
                        if (contact.thisCollider.tag == "Enemy")
                        {
                            hasEnemies = true;
                            enemies[i] = contact.thisCollider.name;
                            i++;
                        }
                        break;
                    case "Enemy":
                        if (contact.thisCollider.tag == "Ally")
                        {
                            hasEnemies = true;
                            enemies[i] = contact.thisCollider.name;
                            i++;
                        }
                        break;
                }
            }
        }
    }

    public bool areEnemiesInRange()
    {
        return hasEnemies;
    }

    public string[] enemiesInRange()
    {
        return enemies;
    }

}
