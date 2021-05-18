using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public List<Item> inventory = new List<Item>();
    // 인벤토리를 리스트로 만듭니다.
    private itemDatabase db;
    // 아이템 데이터베이스는 db로 축약해서 사용합니다.

    public int slotX, slotY;    // 인벤토리 가로 세로 속성 설정 위한 변수
    public List<Item> slots = new List<Item>(); // 인벤토리 속성 변수

    private bool showInventory = false;
    // I버튼을 누르면 활성화/비활성화 되는 부울 변수
    public GUISkin skin;

	// Use this for initialization
	void Start () {
        for(int i=0; i<slotX*slotY; i++)
        {
            slots.Add(new Item());
            // 아이템 슬롯칸에 빈 오브젝트 추가하기
        }
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

    void Update()
    {
        if(Input.GetButtonDown("Inventory"))
            // 만약 Inventory(I)버튼이 눌리면 아래 내용을 실행합니다.
        {
            showInventory = !showInventory;
            // showInventory 앞에 느낌표는 낫(Not)연산자이며, 참>거짓, 거짓>참으로 바꿔주는 연산자입니다.
            // 누를때마다 참>거짓>참>거짓으로 바뀌겠죠
        }
    }
    void OnGUI()
    {
        GUI.skin = skin;
        // Skin 을 skin 답게 ' ㅅ'
        if(showInventory)
        {
            DrawInventory();
        }
    }

    void DrawInventory()
    {
        for(int i=0; i<slotX; i++)
        {
            for(int j=0; j<slotY; j++)
            {
                GUI.Box(new Rect(i * 52+100, j * 52+30, 50, 50), "", skin.GetStyle("slot background"));
                // 각 박스의 생성 위치를 설정해주는 곳입니다. skin.GetStyle은 이전에 만들었던 skin을 불러오는 것임
            }
        }
    }
}
