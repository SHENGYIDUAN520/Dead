using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;//每次射击伤害
    public float timeBetweenBullets = 0.15f;//射击间隔
    public float range = 100f;//射击范围


    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");//获取射击层
        gunParticles = GetComponent<ParticleSystem> ();//获取粒子系统
        gunLine = GetComponent <LineRenderer> ();//获取线渲染器
        gunAudio = GetComponent<AudioSource> ();//获取音频源
        gunLight = GetComponent<Light> ();//获取灯光
    }


    void Update ()
    {
        timer += Time.deltaTime;//计时器

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)//如果按下鼠标左键并且计时器大于射击间隔并且时间缩放不为0
        {
            Shoot ();//射击
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();//禁用效果
        }
    }


    public void DisableEffects ()//禁用效果
    {
        gunLine.enabled = false;//禁用线渲染器
        gunLight.enabled = false;//禁用灯光
    }


    void Shoot ()//射击
    {
        timer = 0f;//计时器

        gunAudio.Play ();//播放音频

        gunLight.enabled = true;//启用灯光

        gunParticles.Stop ();//停止粒子系统
        gunParticles.Play ();//播放粒子系统

        gunLine.enabled = true;//启用线渲染器
        gunLine.SetPosition (0, transform.position);//设置位置

        // 创建从摄像机到鼠标位置的射线
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit cameraHit;
        
        // 设置射击射线原点为枪的位置
        shootRay.origin = transform.position;
        
        // 如果摄像机射线击中了某物体
        if(Physics.Raycast(cameraRay, out cameraHit))
        {
            // 设置射击方向为从枪到鼠标点击位置的方向
            shootRay.direction = (cameraHit.point - transform.position).normalized;
            
            // 进行射击检测
            if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();//获取敌人健康脚本
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage (damagePerShot, shootHit.point);//伤害敌人
                }
                gunLine.SetPosition (1, shootHit.point);//设置位置为击中点
            }
            else
            {
                // 如果没有击中可射击物体，则射线终点为鼠标点击位置
                gunLine.SetPosition (1, cameraHit.point);
            }
        }
        else
        {
            // 如果鼠标没有点击到任何物体，则使用原来的前向射击
            shootRay.direction = transform.forward;
            
            if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage (damagePerShot, shootHit.point);
                }
                gunLine.SetPosition (1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
            }
        }
    }
}
