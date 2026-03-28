using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float velocidad = 2f;
    public Vector2 direccion;
    // Start is called before the first frame update 
    void Start()
    {

    }

    // Update is called once per frame 
    void Update()
    {
        //normalizamos el vector para que no de un movimiento sin modificar, de esta manera 
        //con deltaTime evitamos que el movimiento sea brusco o lento. 

        Vector2 movimiento = direccion.normalized * velocidad * Time.deltaTime;

        transform.position=new Vector2(transform.position.x + movimiento.x, transform.position.y + movimiento.y);
        transform.Translate(movimiento);
    }
}