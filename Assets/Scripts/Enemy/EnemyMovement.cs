using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent nav;
    private Animator animator;
    EnemyHealth enemyHealth;//敌人生存状态
    PlayerHealth playerHealth;//玩家生存状态    

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        //如果敌人生存状态大于0且玩家生存状态大于0
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.SetDestination(player.transform.position);//设置目标位置
        }
        else
        {
            nav.enabled = false;
        }

    }

}
