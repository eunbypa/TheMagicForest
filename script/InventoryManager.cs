using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : InventoryManager
 * Description : 인벤토리 내 아이템 삽입 삭제 등 인벤토리를 관리하는 클래스입니다. 유니티의 생명 주기 함수들을 사용하기 위해 MonoBehaviour 클래스를 상속받았습니다.
 */
public class InventoryManager : MonoBehaviour
{
    int maxSize = 28; // 인벤토리 최대 칸 수
    bool isFull = false; // 가방이 빈 공간 없이 다 찼는지 여부

    List<Item> invenItemList = new List<Item>(); // 인벤토리 각 칸에 위치한 아이템 나타냄
    List<int> invenItemQuantityList = new List<int>(); // 인벤토리 각 칸의 아이템 수량을 나타냄

    //향후 데이터 저장&로드 기능 구현 시 저장된 데이터에서 인벤토리 정보를 가져오는 작업 구현 예정
    void Start()
    {
        
    }

    /* Property */
    public int MaxSize
    {
        get
        {
            return maxSize;
        }
    }
    /* Property */
    public bool IsFull
    {
        get
        {
            return isFull;
        }
    }
    /* Property */
    public List<Item> InvenItemList
    {
        get
        {
            return invenItemList;
        }
    }
    /* Property */
    public List<int> InvenItemQuantityList
    {
        get
        {
            return invenItemQuantityList;
        }
    }

    /* Method : ItemInsert
     * Description : 인벤토리에 보유중이지 않은 아이템을 획득한 경우 인벤토리 아이템 리스트에 해당 아이템을 삽입해주는 동작을 수행하는 메서드입니다.
     * 만약 삽입하고 나서 인벤토리가 꽉 찬 상태가 되면 IsFull 값을 true로 바꾸도록 구현했습니다.
     * Parameter : Item item - 아이템, int num - 아이템 수량
     * Return Value : void
     */
    public void ItemInsert(Item item, int num)
    {
        invenItemList.Add(item);
        invenItemQuantityList.Add(num);
        if (invenItemList.Count == maxSize) isFull = true;
    }

    /* Method : ItemDelete
     * Description : 인벤토리에 보유 중인 아이템의 수량이 0이 되었을 때 해당 아이템을 인벤토리에서 삭제하는 동작을 수행하는 메서드입니다.
     * 만약 삭제하기 전 인벤토리가 꽉 찬 상태였으면 삭제 후 빈 공간이 있다는 뜻으로 IsFull 값을 false로 바꾸도록 구현했습니다.
     * Parameter : int idx - 인벤토리 내 아이템 위치
     * Return Value : void
     */
    public void ItemDelete(int idx)
    {
        invenItemList.RemoveAt(idx);
        invenItemQuantityList.RemoveAt(idx);
        if (isFull) isFull = false;
    }

    /* Method : ItemQuantityIncrease
     * Description : 인벤토리 내 해당 아이템 보유 수량을 증가시키는 메서드입니다.
     * Parameter : int idx - 인벤토리 내 아이템 위치, int num - 아이템 수량
     * Return Value : void
     */
    public void ItemQuantityIncrease(int idx, int num)
    {
        invenItemQuantityList[idx] += num;
    }

    /* Method : ItemQuantityDecrease
     * Description : 인벤토리 내 해당 아이템 보유 수량을 감소시키는 메서드입니다.
     * Parameter : int idx - 인벤토리 내 아이템 위치, int num - 아이템 수량
     * Return Value : void
     */
    public void ItemQuantityDecrease(int idx, int num)
    {
        if (invenItemQuantityList[idx] - num <= 0)
        {
            ItemDelete(idx);
            return;
        }
        invenItemQuantityList[idx] -= num;
    }

    /* Method : FindItem
     * Description : 인벤토리 내 보유중인 아이템들 중 해당 아이디를 가지는 아이템 탐색 동작을 수행하는 메서드입니다. 만약 보유중일 경우 해당 아이템의 위치 값을 반환하고 보유중인 아이템이 아니라면 -1을 반환합니다.  
     * Parameter : int itemId - 아이템 아이디
     * Return Value : void
     */
    public int FindItem(int itemId)
    {
        for(int i = 0; i < invenItemList.Count; i++)
        {
            if(invenItemList[i].ItemId == itemId)
            {
                return i;
            }
        }
        return -1;
    }
}
