using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Text healthText;
    public Slider healthSlider;
    public Image DamageImage;
    public Color damageColor;
    AudioSource playerAudio;
    public AudioClip deathClip;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool isDamaged;
    Animator anim;


    void Awake()
    {
        currentHealth = startingHealth;
        playerAudio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        healthText.text = currentHealth.ToString();
        healthSlider.maxValue = startingHealth;
        healthSlider.value = currentHealth;
    }
    void Update()
    {
        if(isDamaged)
            DamageImage.color = damageColor;    
        else
            DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, Time.deltaTime * 5);
        isDamaged = false;
    }
    public void TakeDamage(int amount)
    {//如果玩家已经死亡，则不执行
        if(isDead)
            return;
        isDamaged = true;
        //减少玩家生命值
        currentHealth -= amount;
        //确保血量不会低于0
        currentHealth = Mathf.Max(0, currentHealth);
        //更新血条  
        healthText.text = currentHealth.ToString();
        healthSlider.value = currentHealth;
        

        
        playerAudio.Play();
        //如果玩家生命值小于等于0，则死亡
        if(currentHealth <= 0)
        {
            Death();
        }
       
    }
    void Death()
    {
        isDead = true;
        playerAudio.clip = deathClip;
        playerAudio.Play();
        //禁用玩家移动和射击
        playerMovement.enabled = false;
        playerShooting.enabled = false;
        anim.SetTrigger("Die");

    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        
    }
}