using UnityEngine;

public class MineDoor : MonoBehaviour
{
    public Transform transformToTransport;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = transformToTransport.position;
        }
    }
}
