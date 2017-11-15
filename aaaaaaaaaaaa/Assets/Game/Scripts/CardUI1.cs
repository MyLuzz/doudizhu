using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 卡牌UI类
/// </summary>
public class CardUI1 : ReuseableObject, IPointerClickHandler
{
    /// <summary>
    /// 用来显示的图片
    /// </summary>
    private Image image;
    private Card card;
    private bool isSelected;
	private Vector3 VecUp;
	private Vector3 VecDown;
    /// <summary>
    /// 卡牌的信息
    /// </summary>
    public Card Card
    {
        get { return card; }
        set
        {
            card = value;
            SetImage();
        }
    }

    /// <summary>
    /// 显示图片
    /// </summary>
    private void SetImage()
    {
        if (card.BelongTo == CharacterType.Player || card.BelongTo == CharacterType.Desk)
        {
            Sprite s = Resources.Load<Sprite>("Pokers/" + card.CardName);
            image.sprite = s;
        }
        else if(card.BelongTo == CharacterType.ComputerLeft)
        {
            Sprite s = Resources.Load<Sprite>("Pokers/CardBack1");
            image.sprite = s;
        }
        else if (card.BelongTo == CharacterType.ComputerRight)
        {
            Sprite s = Resources.Load<Sprite>("Pokers/CardBack1");
            image.sprite = s;
        }
    }

    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool Selected
    {
        get { return isSelected; }
        set
        {
            if (value == isSelected || card.BelongTo != CharacterType.Player)
                 return;
			Vector3 v1 = new Vector3 (transform.localPosition.x,(transform.localPosition+ Vector3.up * 10).y,transform.localPosition.z);
			Vector3 v2 = new Vector3 (transform.localPosition.x,(transform.localPosition- Vector3.up * 10).y,transform.localPosition.z);
			VecUp = v1;
			VecDown = v2;
            if (value == true)
            {
//                transform.localPosition += Vector3.up * 20;
				  selectUp();
            }
            else
            {
//                transform.localPosition -= Vector3.up * 20;
				  selectDown();
            }

            isSelected = value;
        }
    }

	public void selectUp()
	{
		CancelInvoke ("selectDown");
		if (transform.localPosition.y < 9.9) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, VecUp, Time.deltaTime*3);
			Invoke("selectUp",0f);
		}

	}
	public void selectDown()
	{
		CancelInvoke ("selectUp");
		if (transform.localPosition.y > 0.1) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, VecDown, Time.deltaTime*3);
			Invoke("selectDown",0f);
		}
	}

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (card.BelongTo == CharacterType.Player)
                Selected = !Selected;
        }
    }

    /// <summary>
    /// 销毁卡牌
    /// </summary>
    public void Destroy()
    {
        PoolManager.Instance.HideObjet(gameObject);
    }

    /// <summary>
    /// 每次创建时会调用
    /// </summary>
    public override void BeforeGetObject()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 每次销毁时候会调用
    /// </summary>
    public override void BeforeHideObject()
    {
        isSelected = false;
        image.sprite = null;
        card = null;
    }

    /// <summary>
    /// 设置卡牌的位置
    /// </summary>
    /// <param name="parent">父物体</param>
    /// <param name="index">索引</param>
    public void SetPosition(Transform parent, int index)
    {
        transform.SetParent(parent, false);
        transform.SetSiblingIndex(index);
        //右方向
        if ( card.BelongTo == CharacterType.Player)
        {
            transform.localPosition = Vector3.right * 25 * index;
			if (isSelected)
			{
//				Vector3 v = new Vector3 (transform.localPosition.x,(transform.localPosition+ Vector3.up * 100).y,transform.localPosition.z);
//				transform.localPosition = Vector3.Lerp (transform.localPosition, v, Time.deltaTime);
//
                transform.localPosition += Vector3.up * 10;
			}
        }
        else if (card.BelongTo == CharacterType.ComputerLeft || card.BelongTo == CharacterType.ComputerRight)
        {
            transform.localPosition = -Vector3.up * 5 * index;
        }
        else if (card.BelongTo == CharacterType.Desk)
        {
            transform.localPosition = Vector3.right * 25 * index;
        }

    }

}
