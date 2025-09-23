using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���
/// ��Ҳٿص�С����
/// </summary>
public class Amount : PhysicistWood<Amount>
{
    private float PikeStore= 5f; // �ƶ��ٶ�
    
    public Sprite[] Squat;  //��ҵ�Ƥ������Ҫ��Ƥ��ҳ���Ƥ������˳��һ������Ϊ�ǰ����±����л���

    public void Lade()
    {
        transform.position = new Vector3(0, Camera.main.orthographicSize / 2, 0); //��ʼ��λ��
        transform.rotation = Quaternion.identity; //��ʼ������
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; //��ʼ���ٶ�
        GetComponent<Rigidbody2D>().angularVelocity = 0; //��ʼ�����ٶ�

        GetRare(PlayerPrefs.GetInt("Plummet9999_PlayerSkin", 0)); //��ʼ��Ƥ��
    }

    void Update()
    {
        if (AeroTrickle.Religion.OfEntity)
        {
            //��Ϸ����
            if (Input.GetMouseButton(0) || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)))
            {
                Vector2 pointerPosition = Input.GetMouseButton(0) ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;

                if (!OfCushionEdgeUINeptune(pointerPosition))  //�������λ�ò���UI
                {
                    //����ƶ�
                    if (pointerPosition.x > Screen.width / 2)
                    {
                        transform.position += Vector3.right * PikeStore * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += Vector3.left * PikeStore * Time.deltaTime;
                    }
                }
            }

            //�����ҷ��������Ļ����Ϸ����
            if (transform.position.y < -Camera.main.orthographicSize - 0.3f
                || transform.position.y > Camera.main.orthographicSize + 0.1f
                || transform.position.x < -Camera.main.orthographicSize * ((float)Screen.width / Screen.height) - 0.1f
                || transform.position.x > Camera.main.orthographicSize * ((float)Screen.width / Screen.height) + 0.1f)
            {
                AeroTrickle.Religion.HubAero();
            }
        }
    }

    /// <summary>
    /// ����ƽ̨ʱ�ӷ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            A_AudioManager.Instance.PlaySound("Down");
            BirchTrickle.Religion.WanBirch(1);

            // ������ʱ�����������ƶ�
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    //�������߼��Ľ��������
    PointerEventData FaultDale;
    List<RaycastResult> Macaque;
    /// <summary>
    /// ���ָ��λ�����Ƿ����UIԪ��
    /// </summary>
    /// <param name="pointerPosition"></param>
    /// <returns></returns>
    private bool OfCushionEdgeUINeptune(Vector2 pointerPosition)
    {
        // �������߼�����������
        FaultDale = new PointerEventData(EventSystem.current);
        FaultDale.position = pointerPosition;

        // �������߼����
        Macaque = new List<RaycastResult>();
        EventSystem.current.RaycastAll(FaultDale, Macaque);

        // �����
        // ������κν�������ҵ�һ�������߻��еĶ�����UI����GraphicRaycaster������������Ϊ�����UI��
        if (Macaque.Count > 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// �л�Ƥ��
    /// </summary>
    /// <param name="index">Ƥ���±�</param>
    public void GetRare(int index)
    {
        GetComponent<SpriteRenderer>().sprite = Squat[index];
    }
}


/// <summary>
/// �����λ��
/// ����������ת����
/// </summary>
public enum EClickPos
{
    None,
    Left,
    Right
}
