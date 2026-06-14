using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float alertRadius = 20f;
    public LayerMask enemyLayer;
    GameObject player;
    public DynamicPlayer dynamicPlayer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("GameManager: no se encontró ningún objeto con tag 'Player'");
        }
    }

    void Update()
    {
        AlertNearbyEnemies();
    }

    void AlertNearbyEnemies()
    {
        if (player == null) return;

        Collider[] enemies = Physics.OverlapSphere(player.transform.position, alertRadius, enemyLayer);

        foreach (Collider enemyCollider in enemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                dynamicPlayer.SwitchParts(2);
                enemy.AlertEnemy();
            }
        }
    }
}
