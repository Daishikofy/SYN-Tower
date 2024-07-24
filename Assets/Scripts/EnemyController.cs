using System;
using System.Collections.Generic;
using TOWER.Components;
using UnityEngine;

namespace TOWER
{
    [RequireComponent(typeof(Rigidbody2D),typeof(HealthComponent))]
    public class EnemyController : MonoBehaviour
    {
        public Rigidbody2D physicComponent;
        public HealthComponent healthComponent;

        [Header("Loot")] public int currencyDrop = 1;
        [Header("Attack")] public int damage = 1;

        public float attackRate = 2f;
        private float _attackTimer;
        public float velocity = 100f;
        public Transform target;

        private List<Vector2> _path;
        private int _currentPathStep;

        public void Initialize(Transform targetedTransform, List<Vector2> path)
        {
            target = targetedTransform;
            _path = path;
        }
        private void Awake()
        {
            healthComponent.onDefeated.AddListener(Death);
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
            target.gameObject.GetComponent<HealthComponent>()?.Damage(damage);
        }

        private void Death()
        {
            GameManager.Instance.OnEnemyDefeated(this);
            Destroy(gameObject);
        }
    }
}