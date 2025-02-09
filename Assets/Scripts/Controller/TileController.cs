using System;
using System.Threading.Tasks;
using TOWER.Components;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
public class TileController : MonoBehaviour
{
    private HealthComponent _healthComponent;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        _spriteRenderer.enabled =  false;
    }

    private void Awake()
    {
        _spriteRenderer.enabled =  false;
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.onDefeated.AddListener(OnDefeated);
        _healthComponent.onHealthChanged.AddListener(OnDamaged);
    }

    private void OnDamaged(int health)
    {
        if (health > 0)
        {
            OnDamagedAnimation();
        }
    }

    private async void OnDamagedAnimation()
    {
        _spriteRenderer.enabled = true;
        _spriteRenderer.color = Color.red;
        for (int i = 0; i < 10; i++)
        {
            _spriteRenderer.color = Color.Lerp(Color.red, Color.blue, 1/(i+1f));
            await Task.Delay(50);
        }
        _spriteRenderer.enabled = false;
    }
    private void OnDefeated()
    {
        var tilemap = transform.parent.GetComponent<Tilemap>();
        Vector3Int tilemapPosition = tilemap.WorldToCell(transform.position);
        GameManager.Instance.MapManager.RemoveTile(tilemap.name, tilemapPosition);
    }
}
}
