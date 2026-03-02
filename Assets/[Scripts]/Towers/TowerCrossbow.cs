using Unity.Mathematics;
using UnityEngine;

public class TowerCrossbow : Tower
{
    private CrossbowVisuals visuals;

    [Header("Crossbow details")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private int damage;


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

            Enemy enemyTarget = null;
            IDamagable damagable = hit.transform.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                enemyTarget = currentEnemy;
            }

            visuals.PlayAttackFX(gunPoint.position, hit.point, enemyTarget);
            visuals.PlayReloadVFX(attackCooldown);
        }
    }
}
