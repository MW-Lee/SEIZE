using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public List<Item> inventory = new List<Item>();
    // 인벤토리를 리스트로 만듭니다.
    private itemDatabase db;
    // 아이템 데이터베이스는 db로 축약해서 사용합니다.

	// Use this for initialization
	void Start () {
        db = GameObject.FindGameObjectWithTag("Item Database").GetComponent<itemDatabase>();
        // 디비 변수에 "Item Database" 태그를 가진 오브젝트를 연결합니다.
        // 그리고 그 중 가져오는 컴포넌트는 "itemDatabse"라는 속성입니다.
        inventory.Add(db.items[0]);
        // 저는 Red Spear 하나를 만들었었으므로, 한개만 추가해보도록 합니다.
        // 만약 여러개의 아이템을 설정하였다면 반복문 등으로 추가해줍니다.

        /* for(int i=0; i<n; i++) {
         *      inventory.Add(db.items[i]);
         * }
         * 식으로 응용 가능하겠죠?
         */
	}

    void OnGUI()
    {
        for(int i=0; i<inventory.Count; i++)
            // 반복문을 인벤토리에 추가되어 있는 아이템의 갯수만큼 반복합니다.
        {
            GUI.Label(new Rect(10, i * 20, 200, 50), inventory[i].itemName);
            // 화면에 출력해줍니다. 가로, 세로, 가로 끝, 세로 끝의 속성(rect)와
            // 아이템의 이름을 가진 inventory[i]의 세부 속성을 설정해줍니다.
        }
    }
}
