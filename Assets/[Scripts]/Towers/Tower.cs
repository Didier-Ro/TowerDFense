using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [SerializeField] protected float attackCooldown = 1;
    protected float lastTimeAttack;

    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected float rotationSpeed = 10;
    private bool canRotate;

    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemyWithinRange();
            return;
        }

        if (CanAttack())
            Attack();
        

        if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange)
            currentEnemy = null;

        RotateTowardsEnemy();
    }

    protected virtual void Attack()
    {
        //Debug.Log("Attack perform at " + Time.time);
    }


    protected bool CanAttack()
    {
        if (Time.time > lastTimeAttack + attackCooldown)
        {
            lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }

    protected Transform FindRandomEnemyWithinRange()
    {
        List<Transform> possibleTarget = new List<Transform>();

        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            possibleTarget.Add(enemy.transform);
        }

        int randomIndex = Random.Range(0, possibleTarget.Count);

        if (possibleTarget.Count <= 0)
            return null;

        return possibleTarget[randomIndex];
    }

    public void EnableRotation(bool enable)
    {
        canRotate = enable;
    }
    protected virtual void RotateTowardsEnemy()
    {
        if (canRotate == false)
            return;

        if (currentEnemy == null)
            return;

        Vector3 directionToEnemy = currentEnemy.position - towerHead.position;

        if (directionToEnemy.magnitude == 0)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);

        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        towerHead.rotation = Quaternion.Euler(rotation);
    }

    protected Vector3 DirectionToEnemyFromStartPoint(Transform startPoint)
    {
        return (currentEnemy.position - startPoint.position).normalized;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }
}