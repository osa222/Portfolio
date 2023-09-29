using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public Battle.Enemy[] spawnEnemies;
    public int count;
    public float spawnIntarval = 1f;
}