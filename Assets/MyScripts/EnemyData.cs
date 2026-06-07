using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Monster Basic Info")]
    public string enemyName;
    public float maxHP;
    public float moveSpeed;
    public float knockbackPower; // 맞은 몬스터가 얼마나 넉백될지

    [Header("Monster Sprite")]
    public Sprite enemySprite;

    public RuntimeAnimatorController animatorController;
}