using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float m_speed = 1;

    public float m_life = 3;

    protected Transform m_transform;

    public Transform m_rocket;

    private float m_rocketRate = 0;

    //声音
    public AudioClip m_shootClip;

    //声音源
    protected AudioSource m_audio;

    //爆炸特效
    public Transform m_explosionFx;

    //目标位置
    protected Vector3 m_targerPos;

    //鼠标射线碰撞层
    public LayerMask m_inputMask;

    // Use this for initialization
    void Start () {
        m_transform = this.transform;

        //Api更新了
        m_audio = this.GetComponent<AudioSource>();

        m_targerPos = this.m_transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //纵向移动距离
        float move_v = 0;

        //水平移动距离
        float move_h = 0;

        //按上键
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move_v = move_v - m_speed * Time.deltaTime;
        }

        //按下键
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move_v = move_v + m_speed * Time.deltaTime;
        }

        //按左键
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move_h = move_h + m_speed * Time.deltaTime;
        }

        //按右键
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move_h = move_h - m_speed * Time.deltaTime;
        }

        //移动
        PlayerMove(move_h, move_v);

        MoveTo();



        m_rocketRate -= Time.deltaTime;
        if (m_rocketRate <= 0)
        {
            m_rocketRate = 0.25f;
            //按空格键或鼠标左键发射子弹
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                Instantiate(m_rocket, m_transform.position, m_transform.rotation);

                m_audio.PlayOneShot(m_shootClip);
            }
            
        }
    }

    private void PlayerMove(float move_h,float move_v) {
        this.m_transform.Translate(new Vector3(move_h, 0, move_v));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("PlayerRocket")!=0)
        {
            m_life -= 1;
            if(m_life<=0)
            {
                //爆炸特效
                Instantiate(m_explosionFx, m_transform.position, Quaternion.identity);

                Destroy(this.gameObject);
            }

        }
    }

    void MoveTo()
    {
        if(Input.GetMouseButton(0))
        {
            //获取鼠标屏幕位置
            Vector3 ms = Input.mousePosition;
            //将屏幕位置转为射线
            Ray ray = Camera.main.ScreenPointToRay(ms);
            //用来记录射线碰撞信息
            RaycastHit hitinfo;
            //产生射线
            bool iscast = Physics.Raycast(ray, out hitinfo, 1000, m_inputMask);

            if(iscast)
            {
                //如果射中目标，记录射线碰撞点
                m_targerPos = hitinfo.point;
            }
        }

        //使用Vector3提供的MoveTowards函数，获得朝目标移动的位置
        Vector3 pos = Vector3.MoveTowards(this.m_transform.position, m_targerPos, m_speed * Time.deltaTime);

        //更新当前位置
        this.m_transform.position = pos;
    }
}
