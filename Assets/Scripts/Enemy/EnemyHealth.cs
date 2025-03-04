using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;//每个敌人的得分
    
    public AudioClip deathClip;
    
    Animator anim;//动画
    AudioSource enemyAudio;//音频
    ParticleSystem hitParticles;//粒子系统
    CapsuleCollider capsuleCollider;//胶囊碰撞器
    bool isDead;//是否死亡
    bool isSinking;//是否下沉
    

    void Awake()
    {
        anim = GetComponent<Animator>();//获取动画
        enemyAudio = GetComponent<AudioSource>();//获取音频
        hitParticles = GetComponentInChildren<ParticleSystem>();//获取子物体粒子系统
        capsuleCollider = GetComponent<CapsuleCollider>();//获取胶囊碰撞器
        currentHealth = startingHealth;//初始化当前血量
    }

    void Start()
    {
        isDead = false;
        isSinking = false;
    }
    void Update()
    {
        if(isSinking)//如果下沉
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }

    }
    public void TakeDamage(int amount,Vector3 hitPoint)
    {
        if(isDead)
            return;
        enemyAudio.Play();
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Death();
        }

    }
    void Death()
    {
        isDead = true;
        //得分
        ScoreManager.score += scoreValue;
        capsuleCollider.isTrigger = true;
        anim.SetTrigger("Death");//触发死亡动画
        enemyAudio.clip = deathClip;
        enemyAudio.Play();

    }
    public void StartSinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 2f);
    }
}


