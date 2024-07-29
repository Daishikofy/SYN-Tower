using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Searcher;
using UnityEngine;

public class ShieldPowerController : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    public bool IsActivated => gameObject.activeSelf;
    private bool _isPreparing => !collider.enabled;

    public void Prepare(Quaternion rotation)
    {
        gameObject.SetActive(true);
        collider.enabled = false;
    }

    public void UpdateRotation(Vector2 lookAt)
    {
        if (_isPreparing && lookAt != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector3) lookAt);
        }
    }

    public void Place()
    {
        collider.enabled = true;
        Debug.Log("Placed shield at rotation : " + transform.rotation.normalized.eulerAngles);
    }

    public void Break()
    {
        Debug.Log("Shield is broken");
        gameObject.SetActive(false);
    }
}
