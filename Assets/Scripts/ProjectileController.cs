using TOWER.Components;
using UnityEngine;

namespace TOWER
{
    public class ProjectileController : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D physicComponent;
    public float velocity = 100f;
    private Transform _target;
    
    private int _damage = 1;
    private string _targetTag = "";

    private float _lifeRange = 1f;
    private Vector2 _spawnedPosition;
    private Vector2 _movementDirection;

    private void Awake()
    {
        _spawnedPosition = transform.position;
    }

    public void Initialize(Transform targetTransform, float lifeRange, int damage, string targetTag)
    {
        _target = targetTransform;
        _lifeRange = lifeRange;
        _damage = damage;
        _targetTag = targetTag;
        _movementDirection = (_target.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (_target)
        {
            _movementDirection = (_target.position - transform.position).normalized;
        }
        
        physicComponent.AddForce(_movementDirection * velocity, ForceMode2D.Force);
        
        _lifeRange -= Time.deltaTime;
        if (_lifeRange <= Vector2.Distance(transform.position, _spawnedPosition))
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(_targetTag))
        {
            col.gameObject.GetComponent<HealthComponent>()?.Damage(_damage);
            Destroy(gameObject);
        }
    }
}
}
