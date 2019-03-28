using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Falta afegir el moviment de rotació de la camara per a les tecles "q" i "e"

    float mainSpeed = 50.0f;   //Velocitat de moviment de la camara
    float shiftAdd = 250.0f;    //Multiplicat per el temps que s'ha apretat el shift
    float maxShift = 1000.0f;
    private float totalRun = 1.0f;
    public GameObject target;   //Target de la camara sobre el qual rotará
    Vector3 point;

    // Start is called before the first frame update
    void Start()
    {
        point = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Codi per moure la camara
        Vector3 p = GetBaseInput();
        
        totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        p = p * mainSpeed;

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        transform.Translate(p);

        //Codi per rotar la camara en cas que es presionin les tecles 'Q' o 'E'
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * 10.0f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(point, new Vector3(0.0f, -1.0f, 0.0f), 20 * Time.deltaTime * 10.0f);
        }

    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }

        return p_Velocity;
    }
}
