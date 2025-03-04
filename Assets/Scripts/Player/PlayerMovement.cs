using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed = 5f;//移动速度
    private Rigidbody rb;
    private Animator animator;
    void Awake()
    {
        //获取Rigidbody组件
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();//获取动画组件
    }
    private void FixedUpdate()
    {
        //获取输入
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Move(moveHorizontal,moveVertical);//移动
        Turning();//转向
        Animating(moveHorizontal,moveVertical);//动画
    }
    void Move(float moveHorizontal,float moveVertical)
    {
        // 创建一个基于相机方向的移动向量（不受角色旋转影响）
        Vector3 movement = Camera.main.transform.right * moveHorizontal + 
                          Camera.main.transform.forward * moveVertical;
        
        // 保持y轴不变，只在水平面上移动
        movement.y = 0f;
        movement = movement.normalized * Speed * Time.deltaTime;
        
        // 直接使用世界坐标系的移动向量，不再转换为本地坐标
        rb.MovePosition(rb.position + movement);
    }
    void Turning()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Floor");
        RaycastHit floorHit;
        bool hasHit = Physics.Raycast(cameraRay,out floorHit,Mathf.Infinity,layerMask);//射线检测
        if(hasHit)
        {
            Vector3 playerToMouse = floorHit.point - transform.position;//计算玩家到鼠标位置的向量
            playerToMouse.y = 0f;//防止玩家跳跃
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);//计算新的旋转
            rb.MoveRotation(newRotation);//更新玩家旋转
        }
    }
    void Animating(float moveHorizontal,float moveVertical)
    {
        bool walking = moveHorizontal != 0f || moveVertical != 0f;//判断是否在移动
        animator.SetBool("IsWalking",walking);//设置动画状态
    }
}

