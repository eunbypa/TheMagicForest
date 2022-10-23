using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : Portal
 * Description : �� �̵��� �ʿ��� ��Ż�� ����ϴ� Ŭ�����Դϴ�. ����Ƽ�� ���� �ֱ� �Լ���� �浹 ó�� �Լ��� ����ϱ� ���� MonoBehaviour Ŭ������ ��ӹ޽��ϴ�. 
 */
public class Portal : MonoBehaviour
{
    // [SerializeField] �� ����Ƽ Inspector�� �ش� �������� ǥ�õǵ��� �ϱ� ���� ����߽��ϴ�.
    [SerializeField] private GameObject player; // �÷��̾�
    [SerializeField] private GameObject destination; // ������ ��Ż
    [SerializeField] private GameObject curMap; // ���� ��
    [SerializeField] private GameObject nextMap; // ���� ��
    [SerializeField] private int departureMapNum; // ����� �� ��ȣ
    [SerializeField] private int destinationMapNum; // ������ �� ��ȣ

    bool teleport = false; // �÷��̾ ��Ż�� ������ �־ �ڷ���Ʈ�� ������ �������� ����


    void Start()
    {

    }

    void Update()
    {
        if (teleport) // �ڷ���Ʈ ������ ������ ��
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // �÷��̾ ���� ����Ű�� ���� ��� 
            {
                GameManager.instance.WaitForTeleportReady(); // �ڷ���Ʈ �Ϸ���� �ɸ��� �ð���ŭ ����ϱ� ���� gm�� WaitForTeleportReady �޼��� ȣ��
            }
            if (GameManager.instance.TeleportReady) TeleportEvent(); // �ڷ���Ʈ �غ� �������� Teleport ����
        }
    }

    /* Method : TeleportEvent
     * Description : �ڷ���Ʈ �� ������ �����ϴ� �޼����Դϴ�. gm�� TeleportMap �Լ��� ������ �� ��ȣ�� �Ű������� �����ؼ� �̵� ���� �� ��ȣ�� �� �� �ְ� �߰� �÷��̾��� ��ġ�� ������ ��Ż ��ġ�� �ٲٵ���
     * �����߽��ϴ�. �׸��� ���� ���� ��Ȱ��ȭ�ϰ� ������ ���� Ȱ��ȭ�մϴ�.  
     * Return Value : void
     */
    public void TeleportEvent()
    {
        GameManager.instance.TeleportMap(destinationMapNum);
        player.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
        curMap.SetActive(false);
        nextMap.SetActive(true);
        destination.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player") // �÷��̾ ��Ż�� ������ ����
        {
            teleport = true;
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "Player") // �÷��̾ ��Ż�� �������� ���� ����
        {
            teleport = false;
        }
    }
}
