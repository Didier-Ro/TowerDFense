using Unity.Mathematics;
using UnityEngine;

public class TowerCrossbow : Tower
{
    private CrossbowVisuals visuals;

    [Header("Crossbow details")]
    [SerializeField] private Transform gunPoint;

    protected override void Awake()
    {
        visuals = GetComponent<CrossbowVisuals>();
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = DirectionToEnemyFromStartPoint(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hit, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;

            visuals.PlayAttackFX(gunPoint.position, hit.point);
            visuals.PlayReloadVFX(attackCooldown);
        }
    }
}
