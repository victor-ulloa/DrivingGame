using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;

    void Update()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = targetPosition;
    }
}
