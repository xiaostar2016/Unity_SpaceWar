using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("MyGame/Enemy")]
public class Enemy : MonoBehaviour {
    public float m_speed = 1;

    protected float m_rotSpeed = 30;

    protected Transform m_transform;

    public float m_life = 10;

    //声音
    public AudioClip m_shootClip;

    //声音源
    protected AudioSource m_audio;

    //爆炸特效
    public Transform m_explosionFx;

    public int m_point = 10;

    // Use this for initialization
    protected void Start () {
        m_transform = this.transform;

        //Api更新了
        m_audio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected void Update () {
        UpdateMove();
	}

    protected virtual void UpdateMove()
    {
        float rx = Mathf.Sin(Time.time) * Time.deltaTime;

        m_transform.Translate(new Vector3(rx, 0, -m_speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("PlayerRocket") == 0)
        {
            Rocket rocket = other.GetComponent<Rocket>();
            if (rocket != null)
            {
                m_life -= rocket.m_power;

                if (m_life <= 0)
                {
                    //爆炸特效
                    Instantiate(m_explosionFx, m_transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
            }
        }
        else if (other.tag.CompareTo("Player") == 0)
        {
            m_life = 0;

            //爆炸特效
            Instantiate(m_explosionFx, m_transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (other.tag.CompareTo("bound") == 0)
        {
            m_life = 0;
            Destroy(this.gameObject);
        }

        if(m_life <=0)
        {
            GameManager.Instance.AddScore(m_point);

            Instantiate(m_explosionFx, m_transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.1f);
        }
    }
}
