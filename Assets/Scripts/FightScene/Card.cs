////////////////////////////////////////////////
//
// Card
//
// 카드가 가지는 스크립트
// 19. 06. 18
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 지휘관의 특성에 따라서 사용 가능한 스킬 카드
/// </summary>
public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 현재 부모, 즉 카드패가 있는 손을 저장한다. >> 카드를 사용하지 않을 경우 대비
    /// </summary>
    public Transform ParentToRetrunTo = null;
    /// <summary>
    /// 손에서 몇 번째 자리에 있었는지 저장함 >> 카드를 사용하지 않을 경우 대비
    /// </summary>
    public int IndexToReturnTo = 0;
    
    /// <summary>
    /// 카드를 확대 할 때 위치를 옮김 >> 다시 원위치 했을 때를 대비
    /// </summary>
    private Vector3 PosToReturnTo;

    
    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그가 시작되면 카드를 사용하지 않았을 때를 대비 돌아갈 원래 위치를 저장한다
        ParentToRetrunTo = transform.parent;
        IndexToReturnTo = transform.GetSiblingIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그가 진행되면 카드가 손패에서 벗어났는지 아닌지 판단 시작.
        // 손패에서 벗어난다면 카드 크기를 축소하여 드래그를 따라다님
        if (Hand.instance.IsPointerOut)
        {
            GetComponent<Image>().raycastTarget = true;
            transform.position = eventData.position;
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            transform.SetParent(ParentToRetrunTo.parent);
        }
        // 아직 손패 영역 안에 있다면, 카드의 이미지의 Raycast를 풀어서
        // 카드 뒷면에 Raycast를 쏠 수 있도록 함 >> 손패를 벗어났는지 확인
        else
        {
            GetComponent<Image>().raycastTarget = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 카드를 선택하기 위해 손패를 터치하는 순간 포인터가 손패에 있으므로 갱신
        Hand.instance.IsPointerOut = false;

        // 터치를 처음 한 순간 처음 위치를 저장
        // 카드를 잘 볼 수 있게 확대시킨 후 위치를 조정
        if (!eventData.dragging)
        {
            PosToReturnTo = transform.localPosition;

            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                160,
                transform.localPosition.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 사용하지 않으면 카드가 돌아옴
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.localPosition = PosToReturnTo;
        GetComponent<Image>().raycastTarget = true;

        // 카드가 손패 밖에서 떨어졌을때
        if (Hand.instance.IsPointerOut)
        {
            // 카드를 사용하지 않으면 원래 자리로 돌아옴
            //transform.SetParent(ParentToRetrunTo);
            //transform.SetSiblingIndex(IndexToReturnTo);

            Effect.instance.ActivateCard(int.Parse(name));

            Destroy(gameObject);
        }
    }
}
