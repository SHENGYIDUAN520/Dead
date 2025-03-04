using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI; // 添加NavMesh支持

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;//玩家血量
    public GameObject[] enemies;//敌人数组，包含多种敌人
    public float spawnTime = 3f;//生成时间
    public Transform[] spawnPoints;//生成点
    
    [Range(0, 100)]
    public int bossSpawnChance = 10; //Boss生成概率，默认10%
    
    public float minDistanceFromPlayer = 5f; //生成点与玩家的最小距离

    private Transform playerTransform; //玩家Transform引用
    private List<int> validSpawnPointIndices = new List<int>(); //有效生成点索引列表

    void Start()
    {
        // 获取玩家Transform
        if (playerHealth != null)
        {
            playerTransform = playerHealth.transform;
        }
        else
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
        
        // 检查生成点设置
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("没有设置生成点！请在Inspector中设置生成点。");
        }
        
        InvokeRepeating("Spawn", spawnTime, spawnTime);//每隔spawnTime秒生成一个敌人
    }
    
    void Spawn()
    {
        if(playerHealth.currentHealth <= 0f)//如果玩家死亡
        {
            return;
        }
        
        // 使用Transform数组作为生成点
        if (spawnPoints.Length == 0)
        {
            return;
        }
        
        // 清空有效生成点列表
        validSpawnPointIndices.Clear();
        
        // 检查每个生成点是否有效
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null) continue; // 跳过空的生成点
            
            Transform spawnPoint = spawnPoints[i];
            
            // 检查与玩家的距离
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnPoint.position, playerTransform.position);
                if (distanceToPlayer < minDistanceFromPlayer)
                {
                    continue; // 太靠近玩家，跳过
                }
            }
            
            // 通过所有检查，添加到有效列表
            validSpawnPointIndices.Add(i);
        }
        
        // 随机选择一个有效生成点
        int randomIndex = Random.Range(0, validSpawnPointIndices.Count);
        int spawnPointIndex = validSpawnPointIndices[randomIndex];
        
        // 随机选择敌人类型
        GameObject enemyToSpawn;
        
        // 根据概率决定是否生成Boss
        if (Random.Range(0, 100) < bossSpawnChance && enemies.Length > 2)
        {
            // 生成Boss (假设Boss是数组中的第三个元素)
            enemyToSpawn = enemies[2];
        }
        else
        {
            // 生成普通敌人 (随机选择前两个)
            int enemyIndex = Random.Range(0, Mathf.Min(2, enemies.Length));
            enemyToSpawn = enemies[enemyIndex];
        }
        
        // 在选定的生成点实例化选定的敌人
        Instantiate(enemyToSpawn, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
    
    // 在编辑器中可视化生成点和玩家安全区域（仅在编辑器中可见）
    void OnDrawGizmosSelected()
    {
        // 绘制Transform生成点
        if (spawnPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.5f);
                }
            }
        }
        
        // 如果有玩家，显示玩家周围的安全区域
        if (playerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, minDistanceFromPlayer);
        }
    }
}
