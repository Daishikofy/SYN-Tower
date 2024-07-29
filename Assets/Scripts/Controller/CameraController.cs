using TOWER;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 offset;

    private void Start()
    {
        if (playerTransform == null)
        {
            var player = FindFirstObjectByType<PlayerController>();
            playerTransform = player.transform;

            offset = transform.position - playerTransform.position;
        }
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }
}
