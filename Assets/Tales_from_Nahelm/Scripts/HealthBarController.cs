using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public void reduceHP(float max, float life)
    {
        transform.localScale = new Vector3((life/max)*0.3f, 0.4f, 0.4f);
    }
}
