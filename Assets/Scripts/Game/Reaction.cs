using UnityEngine;

/// <summary>
/// ƽ̨��
/// </summary>
public class Reaction : MonoBehaviour
{
    private bool ByBank= false;    //ƽ̨�Ƿ��ƶ�
[UnityEngine.Serialization.FormerlySerializedAs("defaultSprite")]
    public Sprite ArabianBluish;   //Ĭ��ƽ̨ͼƬ
[UnityEngine.Serialization.FormerlySerializedAs("collideSprite")]    public Sprite ScatterBluish;   //��ײƽ̨ͼƬ

    public void Lade()
    {
        GetComponent<SpriteRenderer>().sprite = ArabianBluish;
        ByBank = true;
    }

    private void OnDisable()
    {
        ByBank = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = ScatterBluish;
        }
    }

    void FixedUpdate()
    {
        // ƽ̨�ƶ�
        if (ByBank)
        {
            transform.position += Vector3.up * Time.fixedDeltaTime * InducibleTrickle.Religion.BankStore;
        }

        //���ƽ̨������Ļ��Χ�������
        if (transform.position.y > InducibleTrickle.Religion.UncrumpleStarch && gameObject.activeSelf)
        {
            PoolMgr.Religion.CardNss(gameObject);
        }
    }
}
