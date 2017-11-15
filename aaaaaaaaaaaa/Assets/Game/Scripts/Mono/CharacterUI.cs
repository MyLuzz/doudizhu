using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 控制UI显示
/// </summary>
public class CharacterUI : MonoBehaviour
{
    public Image img_Head;
    public Image img_Identity;
    public Text txt_Int;//总积分
    public Text txt_Remain;//剩余手牌数
	public Text txt_Round;//回合得分

    /// <summary>
    /// 设置身份（地主/农民）
    /// </summary>
    public void SetIdentity(Identity identity)
    {
        Sprite head = null;
        Sprite iden = null;
        switch (identity)
        {
            case Identity.Farmer:
                head = Resources.Load<Sprite>("Pokers/Role_Farmer");
                iden = Resources.Load<Sprite>("Pokers/Identity_Farmer");
                break;
            case Identity.Landlord:
                head = Resources.Load<Sprite>("Pokers/Role_Landlord");
                iden = Resources.Load<Sprite>("Pokers/Identity_Landlord");
                break;
            default:
                break;
        }
        if (head == null || iden == null)
        {
            Debug.LogError("设置身份的图片不存在");
            return;
        }
        img_Head.sprite = head;
        img_Identity.sprite = iden;
    }

    /// <summary>
    /// 设置积分(根据不同的积分换算成相应的段位)
    /// </summary>
    /// <param name="intergration">要设置的积分</param>
    public void SetIntergration(int intergration)
    {
		txt_Int.text = Tools.GetRanking(intergration);
    }

	/// <summary>
	/// 设置回合分
	/// </summary>
	/// <param name="intergration">Intergration.</param>
	public void RoundScore(int intergration)
	{
		txt_Round.text = "积分：" + intergration;
	}

    /// <summary>
    /// 设置剩余手牌
    /// </summary>
    /// <param name="number"></param>
    public void SetRemain(int number)
    {
        txt_Remain.text = "剩余牌数：" + number.ToString();
    }

}
