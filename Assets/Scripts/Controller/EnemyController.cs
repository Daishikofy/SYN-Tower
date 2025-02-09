using System.Collections.Generic;
using TOWER.Components;
using UnityEngine;

namespace TOWER
{
    [RequireComponent(typeof(Rigidbody2D),typeof(HealthComponent))]
    public class EnemyController : MonoBehaviour
    {
        public Animator animator;
        public Rigidbody2D physicComponent;
        public HealthComponent healthComponent;

        [Header("Loot")] public int currencyDrop = 1;
        [Header("Attack")] public int damage = 1;

        public float attackRate = 2f;
        private float _attackTimer;
        public float velocity = 100f;
        private Transform _target;
        private HealthComponent _targetedHealthComponent;

        private List<Vector2> _path;
        private int _currentPathStep;

        public void Initialize(Transform targetedTransform, List<Vector2> path)
        {
            _target = targetedTransform;
            _targetedHealthComponent = _target.gameObject.GetComponent<HealthComponent>();
            _path = path;
        }
        private void Awake()
        {
            healthComponent.onDefeated.AddListener(Death);
            healthComponent.onHealthChanged.AddListener(OnDamaged);
            _attackTimer = attackRate;
        }

        private void FixedUpdate()
        {
            if (_currentPathStep < _path.Count)
            {
                Vector2 movementDirection = _path[_currentPathStep] - (Vector2)transform.position;
                if (movementDirection.magnitude > 0.2f)
                {
                    physicComponent.AddForce(movementDirection.normalized * velocity, ForceMode2D.Force);
                }
                else
                {
                    _currentPathStep++;
                }
            }
            else
            {
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= attackRate)
                {
                    Attack();
                }
            }
        }

        private void Attack()
        {
            _attackTimer = 0f;
            _targetedHealthComponent?.Damage(damage);
        }

        private void OnDamaged(int value)
        {
            animator.SetTrigger("IsAttacked");
        }

        private void Death()
        {
            GameManager.Instance.OnEnemyDefeated(this);
            Destroy(gameObject);
        }
        
        public void UpdatePathFinding()
        {
            _currentPathStep = 1;
            Pathfinder pathfinder = GameManager.Instance.pathfinderManager;
            _path = pathfinder.ShortestPath(transform.position,
                _target.position,
                pathfinder.PATHOFFSET);
        }
        
        private void OnDrawGizmos()
        {
            if (_path == null)
                return;
            for (int i = 1; i < _path.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(_path[i-1], _path[i]);
                Gizmos.DrawSphere(_path[i], 0.1f);
            }
        }
    }
}