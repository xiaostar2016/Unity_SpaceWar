using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/Rocket")]
public class Rocket : MonoBehaviour {

    //子弹飞行速度
    public float m_speed = 1;

    //生存时间
    public float m_liveTime = 1;

    //威力
    public float m_power = 1.0f;

    protected Transform m_Transform;

	// Use this for initialization
	void Start () {
        m_Transform = this.transform;
        Destroy(this.gameObject, m_liveTime);

    }
	
	// Update is called once per frame
	void Update () {
        m_Transform.Translate(new Vector3(0, 0, -m_speed * Time.deltaTime));
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Enemy") == 0 || other.tag.CompareTo("bound") == 0)
        {
            Destroy(this.gameObject);
        }

        
    }
}
