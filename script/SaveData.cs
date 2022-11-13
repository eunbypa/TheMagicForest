using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class : SaveData
 * Description : 게임 저장 데이터를 담당하는 클래스입니다. 저장해야 할 데이터를 모아두었습니다.
 */
[Serializable]
public class SaveData
{
    public Vector3 playerPos; // 플레이어 위치
    public int curMapNum; // 플레이어가 위치한 맵 번호
    public int level; // 플레이어 레벨
    public string name; // 플레이어 이름
    public int gold = -1; // 보유 골드량(-1 이면 저장된 값이 없다는 의미)
    public int curHp = -1; // 현재 체력(-1 이면 저장된 값이 없다는 의미)
    public int maxHp = -1; // 최대 체력(-1 이면 저장된 값이 없다는 의미)
    public int curMp = -1; // 현재 마력(-1 이면 저장된 값이 없다는 의미)
    public int maxMp = -1; // 최대 마력(-1 이면 저장된 값이 없다는 의미)
    public int curExp = -1; // 현재 경험치(-1 이면 저장된 값이 없다는 의미)
    public int maxExp = -1; // 최대 경험치(-1 이면 저장된 값이 없다는 의미)
    public string magicStone; // 현재 착용중인 마법석
    public int[] shortCutPotions = new int[2] { -1, -1 }; // 포션 단축키에 등록된 포션 아이템 id
    public int[] shortCutPotionsQuantity; // 포션 단축키에 등록된 포션 남은 개수
    public List<int> invenItemIdList; // 인벤토리 각 칸에 위치한 아이템의 아이디를 나타냄
    public List<int> invenItemQuantityList; // 인벤토리 각 칸의 아이템 수량을 나타냄
    public int curQuestNum; // 현재 수행 퀘스트 번호
    public int curQuestNpcId; // 현재 수행 퀘스트 담당 npc id
    public bool accept; // 수락 여부
    public bool success; // 성공 여부
    public List<int> curQuestReqNum; // 현재 진행중인 퀘스트 성공에 필요한 조건 수행 상태 목록
  
}
