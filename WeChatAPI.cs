
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WxRobot
{
	/// <summary>
	/// Description of WeChatAPI.
	/// </summary>
	public class WeChatAPI
	{
		public struct UserInfo
		{
			public int Type { get; set; }
			public int List { get; set; }
			public string Wxid { get; set; }
			public string WxNo { get; set; }
			public string Nick { get; set; }
			public string Mark { get; set; }
			public string V1 { get; set; }
			public string HImg { get; set; }
			public string Sex { get; set; }
			public string Address { get; set; }
			public string GetSex(int uSex)
			{
				if(uSex == 1)
					return "男";
				else if(uSex == 2)
					return "女";
				else
					return "未知";
			}
		}
		
		public struct MsgInfo
		{
			public string RecvWxid { get; set; }
			public int Type { get; set; }
			public string Time { get; set; }
			public int IsSend { get; set; }
			public int LocalMsgID { get; set; }
			public string ServerMsgID { get; set; }
			public string Wxid_1 { get; set; }
			public string Wxid_2 { get; set; }
			public string Msg { get; set; }
			public string Source { get; set; }
		}
		
		public int Socket_Handle { get; set; }
		public int Socket_Port { get; set; }
		public int Client_Handle { get; set; }
		public string Client_Wxid { get; set; }
		public string Client_GroupID { get; set; }
		
		public DAPI.dele_Server_CallBack sc;
		public DAPI.dele_Client_CallBack cc;
		
		public WeChatAPI()
		{
			Socket_Handle = 0;
			Socket_Port   = 0;
		}

		
		//微信初始化
		public void Wx_Init(int Client,string pwd)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 1;
			mJson["pwd"] = pwd;
			Socket_Send(Client,mJson.ToString());
		}
		//获取/刷新好友列表
		public void Wx_ReFriendList(int Client)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 2;
			Socket_Send(Client,mJson.ToString());	
		}
		//获取/刷新群员列表
		public void Wx_ReGroupList(int Client, string GroupID)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 3;
			mJson["gid"] = GroupID;
			Socket_Send(Client,mJson.ToString());	
		}
		//下载好友/群员详细信息
		public void Wx_GetUserInfo(int Client, string uWxid,  string GroupID)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 22;
			mJson["wxid"] = uWxid;
			mJson["gid"] = GroupID;
			Socket_Send(Client,mJson.ToString());
		}
		//发送文本消息
		public void Wx_SendTextMsg(int Client, string ToWxid, string AtWxid, string Msg)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 4;
			mJson["wxid"] = ToWxid;
			mJson["atwxid"] = AtWxid;
			mJson["msg"] = Convert.ToBase64String(System.Text.ASCIIEncoding.Default.GetBytes(Msg));
			Socket_Send(Client,mJson.ToString());	
		}
		//发送图片/GIF/文件消息
		//uType : 1=图片 2=GIF动图 3=文件/视频
		public void Wx_SendFileMsg(int Client, int uType, string ToWxid, string uFile)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 4 + uType;
			mJson["wxid"] = ToWxid;
			mJson["file"] = uFile;
			Socket_Send(Client,mJson.ToString());	
		}
		//发送名片消息
		public void Wx_SendCardMsg(int Client, int uType, string ToWxid, string uWxid)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 8;
			mJson["ctype"] = uType;
			mJson["wxid"] = ToWxid;
			mJson["cwxid"] = Convert.ToBase64String(System.Text.ASCIIEncoding.Default.GetBytes(uWxid));
			Socket_Send(Client,mJson.ToString());	
		}
		//发定位消息
		public void Wx_SendMapMsg(int Client, string ToWxid, string uXML)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 29;
			mJson["wxid"] = ToWxid;
			mJson["map"] = Convert.ToBase64String(System.Text.ASCIIEncoding.Default.GetBytes(uXML));
			Socket_Send(Client,mJson.ToString());	
		}
		//发送收藏消息
		public void Wx_SendFavMsg(int Client, string ToWxid, int FavID)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 9;
			mJson["wxid"] = ToWxid;
			mJson["favid"] = FavID;
			Socket_Send(Client,mJson.ToString());	
		}
		//发送链接消息
		public void Wx_SendURLMsg(int Client, string ToWxid, string MyWxid, string uXML)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 10;
			mJson["wxid"] = ToWxid;
			mJson["mywxid"] = MyWxid;
			mJson["xml"] = Convert.ToBase64String(System.Text.ASCIIEncoding.Default.GetBytes(uXML));;
			Socket_Send(Client,mJson.ToString());	
		}
		//转账收款
		public void Wx_GetMoney(int Client, string uWxid, string uTranID, int uTimestamp)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 14;
			mJson["wxid"] = uWxid;
			mJson["time"] = uTimestamp;
			mJson["tranid"] = uTranID;
			Socket_Send(Client,mJson.ToString());	
		}
		//添加好友
		public void Wx_AddFriend(int Client, int uType, int uSource, string uWxid, string uV2, string uMsg)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 11;
			mJson["type"] = uType;
			mJson["source"] = uSource;
			mJson["wxid"] = uWxid;
			mJson["v2"] = uV2;
			mJson["msg"] = uMsg;
			Socket_Send(Client,mJson.ToString());	
		}
		//删除好友
		public void Wx_DelFriend(int Client, string uWxid)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 12;
			mJson["wxid"] = uWxid;
			Socket_Send(Client,mJson.ToString());
		}
		//设置好友备注
		public void Wx_SetMark(int Client, string uWxid, string uMark)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 13;
			mJson["wxid"] = uWxid;
			mJson["mark"] = uMark;
			Socket_Send(Client,mJson.ToString());
		}
		//GetA8Key
		public void Wx_GetA8Key(int Client, int uType, string uWxid, string uURL)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 15;
			mJson["type"] = uType;
			mJson["wxid"] = uWxid;
			mJson["url"] = uURL;
			Socket_Send(Client,mJson.ToString());
		}
		//邀请入群(批量)
		//uType： 1=直接拉群 2=邀请链接
		//Arrar_Wxid：接收消息人wxid数组
		public void WxGroup_Invite(int Client, int uType, string GroupID, string[] Arrar_Wxid)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 16;
			mJson["type"] = uType;
			mJson["gid"] = GroupID;
			JArray mList = new JArray();
			int c = Arrar_Wxid.Length;
			for(int i=0;i<c;i++)
			{
				mList.Add(Arrar_Wxid[i]);
			}
			mJson["list"] = mList;
			Socket_Send(Client,mJson.ToString());
		}
		//踢群成员
		public void WxGroup_Kick(int Client, string GroupID, string uWxid)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 17;
			mJson["gid"] = GroupID;
			mJson["wxid"] = uWxid;
			Socket_Send(Client,mJson.ToString());	
		}
		//退出群聊
		public void WxGroup_Exit(int Client, string GroupID)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 18;
			mJson["gid"] = GroupID;
			Socket_Send(Client,mJson.ToString());	
		}
		//改群名
		public void WxGroup_SetNick(int Client, string GroupID, string uNick)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 19;
			mJson["gid"] = GroupID;
			mJson["nick"] = uNick;
			Socket_Send(Client,mJson.ToString());	
		}
		//发群公告(群主功能，@所有人)
		public void WxGroup_SetNotice(int Client, string mWxid, string GroupID, string uNotice)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 20;
			mJson["mywxid"] = mWxid;
			mJson["gid"] = GroupID;
			mJson["notice"] = uNotice;
			Socket_Send(Client,mJson.ToString());
		}
		//创建群聊
		//Arrar_Wxid ： 2-40人的Wxid数组
		public void WxGroup_Create(int Client, string[] Arrar_Wxid)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 21;
			JArray mList = new JArray();
			int c = Arrar_Wxid.Length;
			for(int i=0;i<c;i++)
			{
				mList.Add(Arrar_Wxid[i]);
			}
			mJson["list"] = mList;
			Socket_Send(Client,mJson.ToString());
		}
		//状态操作
		//uType：1=置顶 2=免打扰 3=保存通讯录 4=显示群员昵称
		public void Wx_StateOpt(int Client, int uType, string uWxid, int Opt)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 28;
			mJson["type"] = uType;
			mJson["wxid"] = uWxid;
			mJson["opt"] = Opt;
			Socket_Send(Client,mJson.ToString());	
		}
		//防撤回
		//Opt：0=取消防撤回 1=防撤回
		public void Wx_DisRevoke(int Client, int Opt)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 25;
			mJson["opt"] = Opt;
			Socket_Send(Client,mJson.ToString());	
		}
		
		public void DownLoadPic(int Client, string picXML, string picFile)
		{
			JObject mJson = new JObject();
			mJson["packtype"] = 27;
			mJson["pic"] = picXML;
			mJson["file"] = picFile;
			Socket_Send(Client,mJson.ToString());	
		}
	}
}
