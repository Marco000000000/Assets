using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform target;  // L'oggetto da seguire (es: il personaggio)
    public Vector3 offset;     // Offset per regolare la distanza della camera dall'oggetto

    void Update()
    {
        // Aggiorna la posizione della telecamera in base alla posizione dell'oggetto
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}

