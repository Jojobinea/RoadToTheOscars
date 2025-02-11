using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public int health;
    public int speed;
    public Sprite sprite;
}
