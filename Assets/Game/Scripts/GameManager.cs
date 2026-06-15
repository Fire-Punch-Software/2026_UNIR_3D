using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        bool noAlert = true;

        Collider[] enemies = Physics.OverlapSphere(player.transform.position, alertRadius, enemyLayer);

        foreach (Collider enemyCollider in enemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                noAlert = false;
                dynamicPlayer.SwitchParts(2);
                enemy.AlertEnemy();
            }
        }

        if (noAlert)
        {
            dynamicPlayer.SwitchParts(0);
        }
    }

    public void GameOver()
    {
        StartCoroutine(LoadGameOverWithDelay());
    }

    IEnumerator LoadGameOverWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }

    public void Goal()
    {
        StartCoroutine(LoadGoalWithDelay());
    }

    IEnumerator LoadGoalWithDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }

}
