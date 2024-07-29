using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TOWER
{
    public class PlayerController : MonoBehaviour
    {
        //Movement variables
        [Header("Movement Variables")] public Rigidbody2D physicComponent;
        public float velocity = 10f;

        private Vector2 _movementDirection = Vector2.zero;

        [Header("Interaction Variables")] [SerializeField]
        private float maxInteractionDistance = 2;

        [Header("Tower Variables")] public TowerController towerPrefab;
        public List<TowerController> towers;
        private List<int> _availableTowers;
        private List<int> _placedTowers;

        [Header("Attack Variables")] [SerializeField]
        private ShieldPowerController shieldPower;


        // Start is called before the first frame update
        void Start()
        {
            _placedTowers = new List<int>();
            _availableTowers = new List<int>();
            for (int i = 0; i < towers.Count; i++)
            {
                _availableTowers.Add(i);
            }

            TOW_UIManager.Instance.UpdateTowerCount(towers.Count);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (shieldPower.IsActivated)
            {
                shieldPower.UpdateRotation(_movementDirection);
            }
            else
            {
                physicComponent.AddForce(_movementDirection * velocity, ForceMode2D.Force);
            }
        }

        public void Move(InputAction.CallbackContext context) //Input system
        {
            Vector2 input = context.ReadValue<Vector2>();
            _movementDirection = input;
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                shieldPower.Prepare(quaternion.LookRotation(Vector3.forward, (Vector3) _movementDirection));
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                shieldPower.Place();
            }
        }

        public void Interact(InputAction.CallbackContext context) //Input system
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (shieldPower.IsActivated)
                {
                    shieldPower.Break();
                    return;
                }
                
                Vector2 currentPosition = transform.position;

                //Interact
                InteractableComponent[] interactableComponents =
                    FindObjectsByType<InteractableComponent>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                if (interactableComponents.Length > 0)
                {
                    int closestComponentId = 0;
                    float minDistance = Vector2.Distance(currentPosition,
                        interactableComponents[closestComponentId].transform.position);
                    for (int i = 1; i < interactableComponents.Length; i++)
                    {
                        float distance = Vector2.Distance(currentPosition,
                            interactableComponents[i].transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestComponentId = i;
                        }
                    }

                    if (minDistance <= maxInteractionDistance)
                    {
                        interactableComponents[closestComponentId].Interact();
                        return;
                    }
                }


                //Place / Remove tower
                for (int i = 0; i < _placedTowers.Count; i++)
                {
                    int towerIndex = _placedTowers[i];
                    TowerController tower = towers[_placedTowers[i]];

                    if (Vector2.Distance(tower.transform.position, currentPosition) <= maxInteractionDistance)
                    {
                        tower.DisableTower();
                        _availableTowers.Add(towerIndex);

                        _placedTowers.RemoveAt(i);
                        return;
                    }
                }

                if (_availableTowers.Count > 0)
                {
                    int towerIndex = _availableTowers[0];
                    TowerController tower = towers[_availableTowers[0]];
                    _placedTowers.Add(towerIndex);
                    tower.EnableTower(currentPosition);

                    _availableTowers.RemoveAt(0);
                }
            }
        }

        public void BuyTower() //Input system + Interactable
        {
            if (GameManager.Instance.CanBuyTower())
            {
                TowerController newTower = Instantiate(towerPrefab);
                newTower.DisableTower();
                towers.Add(newTower);
                _availableTowers.Add(towers.Count - 1);
                TOW_UIManager.Instance.UpdateTowerCount(towers.Count);
            }
        }
    }
}