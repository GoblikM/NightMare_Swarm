using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Skeleton,
    Bat,
    Other
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{

    [SerializeField]
    private EnemyType enemyType;
    public EnemyType EnemyType { get => enemyType; private set => enemyType = value; }
    // Base stats for the enemy
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }
    [SerializeField]
    private float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    private float attackRange;
    public float AttackRange { get => attackRange; private set => attackRange = value; }

}
