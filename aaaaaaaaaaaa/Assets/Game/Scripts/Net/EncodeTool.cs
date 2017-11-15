using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using LitJson;
using Net;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Web.Script.Serialization;
/// <summary>
/// 关于编码的工具类
/// </summary>
public static class EncodeTool
{

    #region 粘包拆包问题 封装一个有规定的数据包

    /// <summary>
    /// 构造数据包 ： 包头 + 包尾
    /// </summary>
    /// <returns></returns>
    public static byte[] EncodePacket(byte[] data)
    {
        //内存流对象
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                //先写入长度
                bw.Write(data.Length);
                //再写入数据
                bw.Write(data);

                byte[] byteArray = new byte[(int)ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, byteArray, 0, (int)ms.Length);

                return byteArray;
            }
        }
    }

    /// <summary>
    /// 解析消息体 从缓存里取出一个一个完整的数据包 
    /// </summary>
    /// <returns></returns>
    public static byte[] DecodePacket(ref List<byte> dataCache)
    {

        using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                int length = dataCache.Count;
         
                byte[] data = br.ReadBytes(length);
                //更新一下数据缓存
                dataCache.Clear();

                return data;
            }
        }
    }

    #endregion

    #region 构造发送的SocketMsg类

    /// <summary>
    /// 把socketMsg类转换成字节数组 发送出去
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] EncodeMsg(SocketMsg msg)
    {
        MemoryStream ms = new MemoryStream();
        BinaryWriter bw = new BinaryWriter(ms);
        bw.Write(msg.OpCode);
        bw.Write(msg.SubCode);
        //如果不等于null  才需要把object 转换成字节数据 存起来
        if (msg.Value != null)
        {
            byte[] valueBytes = EncodeObj(msg.Value);
            bw.Write(valueBytes);
        }
        byte[] data = new byte[ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
        bw.Close();
        ms.Close();
        return data;
    }

    /// <summary>
    /// 将收到的字节数组 转换成 socketMsg对象 供我们使用
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static SocketMsg DecodeMsg(byte[] data)
    {
        MemoryStream ms = new MemoryStream(data);
        BinaryReader br = new BinaryReader(ms);
        SocketMsg msg = new SocketMsg();
        msg.OpCode = br.ReadInt32();
        msg.SubCode = br.ReadInt32();
        //还有剩余的字节没读取 代表 value  有值
        if (ms.Length > ms.Position)
        {
            byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
            object value = DecodeObj(valueBytes);
            msg.Value = value;
        }
        br.Close();
        ms.Close();
        return msg;
    }

    #endregion

    #region 把一个object类型转换成byte[]

    /// <summary>
    /// 序列化对象
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] EncodeObj(object value)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, value);
            byte[] valueBytes = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);
            return valueBytes;
        }
    }

    /// <summary>
    /// 反序列化对象
    /// </summary>
    /// <param name="valueBytes"></param>
    /// <returns></returns>
    public static object DecodeObj(byte[] valueBytes)
    {
        using (MemoryStream ms = new MemoryStream(valueBytes))
        {
            BinaryFormatter bf = new BinaryFormatter();
            object value = bf.Deserialize(ms);
            return value;
        }
    }

    #endregion

//	public void CheckType(object[] data,List<player> list)
//	{
////		string Id=object[0];
////		string Name=Object[1];
////
////
////		类 对象=new 类();
////		对象.Id=Id;
////		对象.Name=Name;
////		list.Add(对象);
//	}

	/// <summary>
	/// 测试登录
	/// </summary>
	/// <returns>The encode.</returns>
	/// <param name="id">Identifier.</param>
	/// <param name="nickname">Nickname.</param>
	/// <param name="sex">Sex.</param>
	/// <param name="avator">Avator.</param>
    public static byte[] UserEncode(string id,string nickname,string sex,string avator,int type,string access_token)
    {
        var data = new PlayerData
        {
            action = 1,
            id = id,
            nickname = nickname,
            sex = sex,
            avator = avator,
            type = type,
            access_token = access_token
        };
         var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }

    /// <summary>
    /// 测试登录
    /// </summary>
    /// <returns>The encode.</returns>
    /// <param name="id">Identifier.</param>
    /// <param name="nickname">Nickname.</param>
    /// <param name="sex">Sex.</param>
    /// <param name="avator">Avator.</param>
    public static byte[] TestEncode(string id, string nickname, string sex, string avator)
    {
        var data = new TestData
        {
            action = 1,
            id = id,
            nickname = nickname,
            sex = sex,
            avator = avator,
            type = 2
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }

	/// <summary>
	/// 创建房间
	/// </summary>
	/// <returns>The room encode.</returns>
	/// <param name="id">Identifier.</param>
	/// <param name="game_type">Game type.</param>
    public static byte[] CreateRoomEncode(string id,int game_type,int inning,int payment,bool excard_visible)
    {
        var data = new CreateData
        {
            action = 3,
            id = id,
            game_type = game_type,
            inning = inning,
            payment = payment,
            excard_visible = excard_visible
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }

	/// <summary>
	/// 加入房间
	/// </summary>
	/// <returns>The room encode.</returns>
	/// <param name="id">Identifier.</param>
	/// <param name="room_id">Room identifier.</param>
	public static byte[] EnterRoomEncode(string id,string room_id)
	{
		var data = new EnterData
		{
			action = 4,
			id = id,
			room_id = room_id
		};
		var serializedData = JsonConvert.SerializeObject(data);
		var deserializedData = JsonConvert.DeserializeObject(serializedData);
		Debug.Log("Deserialized:" + deserializedData);
		byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
		return message;
	}

	/// <summary>
	/// 离开房间
	/// </summary>
	/// <returns>The room encode.</returns>
	/// <param name="id">Identifier.</param>
	/// <param name="room_id">Room identifier.</param>
	public static byte[] LeaveRoomEncode(string id)
	{
		var data = new LeaveData
		{
			action = 5,
			id = id
		};
		var serializedData = JsonConvert.SerializeObject(data);
		var deserializedData = JsonConvert.DeserializeObject(serializedData);
		Debug.Log("Deserialized:" + deserializedData);
		byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
		return message;
	}

	/// <summary>
	/// 玩家准备
	/// </summary>
	/// <returns>The room encode.</returns>
	/// <param name="id">Identifier.</param>
	public static byte[] ReadyEncode(string id)
	{
		var data = new ReadyData
		{
			action = 6,
			id = id
		};
		var serializedData = JsonConvert.SerializeObject(data);
		var deserializedData = JsonConvert.DeserializeObject(serializedData);
		Debug.Log("Deserialized:" + deserializedData);
		byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
		return message;
	}
    /// <summary>
    /// 玩家叫分
    /// </summary>
    /// <returns>The room encode.</returns>
    /// <param name="id">Identifier.</param>
    public static byte[] CallEncode(string id,int bet)
    {
        var data = new CallData
        {
            action = 7,
            id = id,
            bet = bet
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }


	/// <summary>
	/// 卡牌集合转换
	/// </summary>
	/// <returns>The room encode.</returns>
	/// <param name="id">Identifier.</param>
	public static byte[] CardEncode(byte[] array,string id)
	{
      
		var data = new PlayCardData
		{
            action = 8,
            id = id,
            card_list = array
		};
        string str = ObjectToJson(data);
        Debug.Log(str);
        //Debug.Log("第一张牌"+data.card_list[0]);
        //var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(str);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
		return message;
	}
    /// <summary>
    /// 玩家过牌
    /// </summary>
    /// <returns>The room encode.</returns>
    /// <param name="id">Identifier.</param>
    public static byte[] PassEncode(string id)
    {
        var data = new PassData
        {
            action = 9,
            id = id,
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }


    public static string ObjectToJson(object obj)
    { 
       JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
       return jsonSerialize.Serialize(obj);
    }

    /// <summary>
    /// 获取积分变化表
    /// </summary>
    /// <returns>The room encode.</returns>
    /// <param name="id">Identifier.</param>
    public static byte[] ScoreEncode(string id)
    {
        var data = new ScoreData
        {
            action = 10,
            id = id,
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }
    /// <summary>
    /// 获取最终数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static byte[] AllScoreEncode(string id)
    {
        var data = new AllScoreData
        {
            action = 11,
            id = id,
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }

    /// <summary>
    /// 测试重连
    /// </summary>
    /// <returns>The room encode.</returns>
    /// <param name="id">Identifier.</param>
    public static byte[] ReconTestEncode(int action,string id)
    {
        var data = new ReconTestData
        {
            action = action,
            id = id,
            type = 2
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }

    /// <summary>
    /// 重连
    /// </summary>
    /// <returns>The room encode.</returns>
    /// <param name="id">Identifier.</param>
    public static byte[] ReconEncode(int action,string id,int type,string access_token)
    {
        var data = new ReconData
        {
            action = action,
            id = id,
            type = type,
            access_token = access_token
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }
    
    /// <summary>
    /// 投票解散
    /// </summary>
    /// <param name="action"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static byte[] VoteEncode(string id,bool agree)
    {
        var data = new VoteData
        {
            action = 51,
            id = id,
            agree = agree
        };
        var serializedData = JsonConvert.SerializeObject(data);
        var deserializedData = JsonConvert.DeserializeObject(serializedData);
        Debug.Log("Deserialized:" + deserializedData);
        byte[] message = Encoding.UTF8.GetBytes(deserializedData.ToString());
        return message;
    }
}

