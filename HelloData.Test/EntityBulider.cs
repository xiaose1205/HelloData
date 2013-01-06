/*
	以下代码为T4自动生成的代码，请不要擅自修改
	生成时间:2012-11-28 15:16:09.6083
	生成机器：WANGJUN
	author：xiaose
*/
using System;
using System.Collections.Generic; 
using System.Text;
using HelloData.FrameWork.Data;

 
	
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_Account_channel: BaseEntity
    {
	  
	    /// <summary>
		///	帐号ID
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_Account_channel()
        {
            base.SetIni(this,"Sms_Account_channel","ID");
        }	
	    /// <summary>
		///	
		/// </summary>
 
		public int AccountID { get; set; }		
	    /// <summary>
		///	通道ID
		/// </summary>
 
		public int ChannelID { get; set; }		
	    /// <summary>
		///	通道运营商类别	1：移动，2：电信，3：联通
		/// </summary>
 
		public int? Channeltype { get; set; }		
	    /// <summary>
		///	发送比例	数字0到100，例：80=80%
		/// </summary>
 
		public int? Sendproportion { get; set; }		
	    /// <summary>
		///	创建时间
		/// </summary>
 
		public DateTime? CreateTime { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string AccountID="AccountID";					
			public const string ChannelID="ChannelID";					
			public const string Channeltype="Channeltype";					
			public const string Sendproportion="Sendproportion";					
			public const string CreateTime="CreateTime";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_AddRecord: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true)]
		public int ID { get; set; }	
		public  Sms_AddRecord()
        {
            base.SetIni(this,"Sms_AddRecord","ID");
        }	
	    /// <summary>
		///	帐号ID
		/// </summary>
 
		public int AccountID { get; set; }		
	    /// <summary>
		///	充值前的剩余余额
		/// </summary>
 
		public float? BeforeAdd { get; set; }		
	    /// <summary>
		///	充值金额
		/// </summary>
 
		public float? AddMount { get; set; }		
	    /// <summary>
		///	充值后的余额
		/// </summary>
 
		public float? AfterAdd { get; set; }		
	    /// <summary>
		///	创建时间
		/// </summary>
 
		public DateTime? CreateTime { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string AccountID="AccountID";					
			public const string BeforeAdd="BeforeAdd";					
			public const string AddMount="AddMount";					
			public const string AfterAdd="AfterAdd";					
			public const string CreateTime="CreateTime";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_Batch: BaseEntity
    {
	  
	    /// <summary>
		///	批次ID
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_Batch()
        {
            base.SetIni(this,"Sms_Batch","ID");
        }	
	    /// <summary>
		///	帐号ID
		/// </summary>
 
		public int AccountID { get; set; }		
	    /// <summary>
		///	信息状态	0:未发送1:已发送2:暂停发送3:正在发送4:停止发送
		/// </summary>
 
		public int? MessageState { get; set; }		
	    /// <summary>
		///	信息内容
		/// </summary>
 
		public string SmsContent { get; set; }		
	    /// <summary>
		///	包含条数
		/// </summary>
 
		public int? Msgcount { get; set; }		
	    /// <summary>
		///	信息类型	短信:1彩信:2WAPPUSH:3
		/// </summary>
 
		public int? Msg_type { get; set; }		
	    /// <summary>
		///	优先级	1最低，5最高
		/// </summary>
 
		public int? Level { get; set; }		
	    /// <summary>
		///	是否需要状态报告
		/// </summary>
 
		public bool? State_report { get; set; }		
	    /// <summary>
		///	客户扩展码
		/// </summary>
 
		public string Custom_num { get; set; }		
	    /// <summary>
		///	发送时间段开始时间
		/// </summary>
 
		public DateTime? Begin_time { get; set; }		
	    /// <summary>
		///	发送时间段结束时间
		/// </summary>
 
		public DateTime? End_time { get; set; }		
	    /// <summary>
		///	处理完成时间
		/// </summary>
 
		public DateTime? Commit_time { get; set; }		
	    /// <summary>
		///	提交时间
		/// </summary>
 
		public DateTime? Post_time { get; set; }		
	    /// <summary>
		///	批次状态	0：初始状态1：处理完成
		/// </summary>
 
		public int? BatchState { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string AccountID="AccountID";					
			public const string MessageState="MessageState";					
			public const string SmsContent="SmsContent";					
			public const string Msgcount="Msgcount";					
			public const string Msg_type="Msg_type";					
			public const string Level="Level";					
			public const string State_report="State_report";					
			public const string Custom_num="Custom_num";					
			public const string Begin_time="Begin_time";					
			public const string End_time="End_time";					
			public const string Commit_time="Commit_time";					
			public const string Post_time="Post_time";					
			public const string BatchState="BatchState";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_Batch_Details: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_Batch_Details()
        {
            base.SetIni(this,"Sms_Batch_Details","ID");
        }	
	    /// <summary>
		///	批次ID
		/// </summary>
 
		public int BatchID { get; set; }		
	    /// <summary>
		///	消息ID	匹配状态报告
		/// </summary>
 
		public string Msg_id { get; set; }		
	    /// <summary>
		///	信息ID	"信息类型:0:普通短信3：wappush4:彩信"
		/// </summary>
 
		public int? Sms_type { get; set; }		
	    /// <summary>
		///	通道ID
		/// </summary>
 
		public int? ChannelID { get; set; }		
	    /// <summary>
		///	手机号码
		/// </summary>
 
		public string phone { get; set; }		
	    /// <summary>
		///	状态	"等待发送:0发送成功:1被拒绝:2数据格式错误:3多次发送失败:4帧结束标志:5序列号错误:6系统拒绝发送:7"
		/// </summary>
 
		public int? State { get; set; }		
	    /// <summary>
		///	状态报告
		/// </summary>
 
		public string State_report { get; set; }		
	    /// <summary>
		///	提交时间
		/// </summary>
 
		public DateTime? Submit_time { get; set; }		
	    /// <summary>
		///	状态报告返回时间
		/// </summary>
 
		public DateTime? Report_time { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string BatchID="BatchID";					
			public const string Msg_id="Msg_id";					
			public const string Sms_type="Sms_type";					
			public const string ChannelID="ChannelID";					
			public const string phone="phone";					
			public const string State="State";					
			public const string State_report="State_report";					
			public const string Submit_time="Submit_time";					
			public const string Report_time="Report_time";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_BlackPhone: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_BlackPhone()
        {
            base.SetIni(this,"Sms_BlackPhone","ID");
        }	
	    /// <summary>
		///	手机号
		/// </summary>
 
		public string Phone { get; set; }		
	    /// <summary>
		///	创建时间
		/// </summary>
 
		public DateTime? CreateTime { get; set; }		
	    /// <summary>
		///	通道提供商ID	局部黑名单有效
		/// </summary>
 
		public int? OperatorID { get; set; }		
	    /// <summary>
		///	黑名单类别	0：全局黑名单1：局部黑名单（针对某个通道提供商）
		/// </summary>
 
		public int? Blacktype { get; set; }		
	    /// <summary>
		///	备注
		/// </summary>
 
		public string Comment { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string Phone="Phone";					
			public const string CreateTime="CreateTime";					
			public const string OperatorID="OperatorID";					
			public const string Blacktype="Blacktype";					
			public const string Comment="Comment";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_Channel: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_Channel()
        {
            base.SetIni(this,"Sms_Channel","ID");
        }	
	    /// <summary>
		///	提供商ID
		/// </summary>
 
		public int OperatorID { get; set; }		
	    /// <summary>
		///	运营商类别	1:移动，2:电信，3:联通
		/// </summary>
 
		public int? ChannelType { get; set; }		
	    /// <summary>
		///	通道名	自定义
		/// </summary>
 
		public string ChannelName { get; set; }		
	    /// <summary>
		///	通道号
		/// </summary>
 
		public string ChannelNum { get; set; }		
	    /// <summary>
		///	是否有效标志	0:否,1:是
		/// </summary>
 
		public int? AvailFlag { get; set; }		
	    /// <summary>
		///	服务器IP	可能是IP地址，亦可能是webservice或者http协议地址
		/// </summary>
 
		public string HostName { get; set; }		
	    /// <summary>
		///	端口
		/// </summary>
 
		public string Port { get; set; }		
	    /// <summary>
		///	通道方是否添加企业签名
		/// </summary>
 
		public bool? Issignal { get; set; }		
	    /// <summary>
		///	通道识别号	做程序内部调用唯一标识
		/// </summary>
 
		public string Identity { get; set; }		
	    /// <summary>
		///	是否支持彩信
		/// </summary>
 
		public bool? Ismms { get; set; }		
	    /// <summary>
		///	是否支持短信
		/// </summary>
 
		public bool? Issms { get; set; }		
	    /// <summary>
		///	是否支持长短信
		/// </summary>
 
		public bool? Islong_sms { get; set; }		
	    /// <summary>
		///	是否支持wappush
		/// </summary>
 
		public bool? Iswappush { get; set; }		
	    /// <summary>
		///	是否支持状态报告
		/// </summary>
 
		public bool? Isstate_report { get; set; }		
	    /// <summary>
		///	是否支持群发
		/// </summary>
 
		public bool? Ismass_commit { get; set; }		
	    /// <summary>
		///	是否支持上行
		/// </summary>
 
		public bool? IsMo { get; set; }		
	    /// <summary>
		///	是否支持扩展
		/// </summary>
 
		public bool? IsExpand { get; set; }		
	    /// <summary>
		///	扩展最大位数
		/// </summary>
 
		public int? Max_Expand { get; set; }		
	    /// <summary>
		///	单个短信的最大字节长度
		/// </summary>
 
		public int? Max_onelength { get; set; }		
	    /// <summary>
		///	最长支持最大字节数
		/// </summary>
 
		public int? Max_length { get; set; }		
	    /// <summary>
		///	月最高发送量
		/// </summary>
 
		public int? MouthMax { get; set; }		
	    /// <summary>
		///	帐号
		/// </summary>
 
		public string Account { get; set; }		
	    /// <summary>
		///	密码
		/// </summary>
 
		public string Password { get; set; }		
	    /// <summary>
		///	通讯方式
		/// </summary>
 
		public string Communicatetype { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string OperatorID="OperatorID";					
			public const string ChannelType="ChannelType";					
			public const string ChannelName="ChannelName";					
			public const string ChannelNum="ChannelNum";					
			public const string AvailFlag="AvailFlag";					
			public const string HostName="HostName";					
			public const string Port="Port";					
			public const string Issignal="Issignal";					
			public const string Identity="Identity";					
			public const string Ismms="Ismms";					
			public const string Issms="Issms";					
			public const string Islong_sms="Islong_sms";					
			public const string Iswappush="Iswappush";					
			public const string Isstate_report="Isstate_report";					
			public const string Ismass_commit="Ismass_commit";					
			public const string IsMo="IsMo";					
			public const string IsExpand="IsExpand";					
			public const string Max_Expand="Max_Expand";					
			public const string Max_onelength="Max_onelength";					
			public const string Max_length="Max_length";					
			public const string MouthMax="MouthMax";					
			public const string Account="Account";					
			public const string Password="Password";					
			public const string Communicatetype="Communicatetype";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_ContentFilterKey: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_ContentFilterKey()
        {
            base.SetIni(this,"Sms_ContentFilterKey","ID");
        }	
	    /// <summary>
		///	通道提供商ID	局部敏感字有效
		/// </summary>
 
		public int? OperatorID { get; set; }		
	    /// <summary>
		///	关键字内容
		/// </summary>
 
		public string Key { get; set; }		
	    /// <summary>
		///	非法关键字类别	0：全局敏感字1：局部敏感字（针对某个通道提供商）
		/// </summary>
 
		public int? Keytype { get; set; }		
	    /// <summary>
		///	创建时间
		/// </summary>
 
		public DateTime? CreateTime { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string OperatorID="OperatorID";					
			public const string Key="Key";					
			public const string Keytype="Keytype";					
			public const string CreateTime="CreateTime";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_DeductRecord: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_DeductRecord()
        {
            base.SetIni(this,"Sms_DeductRecord","ID");
        }	
	    /// <summary>
		///	
		/// </summary>
 
		public int AccountID { get; set; }		
	    /// <summary>
		///	批次ID
		/// </summary>
 
		public int? BatchID { get; set; }		
	    /// <summary>
		///	单价
		/// </summary>
 
		public float? Price { get; set; }		
	    /// <summary>
		///	扣费时间
		/// </summary>
 
		public DateTime? Deduct_time { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string AccountID="AccountID";					
			public const string BatchID="BatchID";					
			public const string Price="Price";					
			public const string Deduct_time="Deduct_time";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_Enterprise: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_Enterprise()
        {
            base.SetIni(this,"Sms_Enterprise","ID");
        }	
	    /// <summary>
		///	企业名称
		/// </summary>
 
		public string Enterprise_Name { get; set; }		
	    /// <summary>
		///	企业介绍
		/// </summary>
 
		public string Introduction { get; set; }		
	    /// <summary>
		///	是否有效标志	0:否,1:是
		/// </summary>
 
		public int? AvailFlag { get; set; }		
	    /// <summary>
		///	创建时间
		/// </summary>
 
		public DateTime? CreateTime { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string Enterprise_Name="Enterprise_Name";					
			public const string Introduction="Introduction";					
			public const string AvailFlag="AvailFlag";					
			public const string CreateTime="CreateTime";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_Operator: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_Operator()
        {
            base.SetIni(this,"Sms_Operator","ID");
        }	
	    /// <summary>
		///	提供商名称
		/// </summary>
 
		public string OperatorName { get; set; }		
	    /// <summary>
		///	提供商介绍
		/// </summary>
 
		public string Introduction { get; set; }		
	    /// <summary>
		///	提供商所在地区
		/// </summary>
 
		public string OperatorArea { get; set; }		
	    /// <summary>
		///	联系人
		/// </summary>
 
		public string ContactNmae { get; set; }		
	    /// <summary>
		///	联系号码
		/// </summary>
 
		public string Telphone { get; set; }		
	    /// <summary>
		///	备注
		/// </summary>
 
		public string Remark { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string OperatorName="OperatorName";					
			public const string Introduction="Introduction";					
			public const string OperatorArea="OperatorArea";					
			public const string ContactNmae="ContactNmae";					
			public const string Telphone="Telphone";					
			public const string Remark="Remark";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_teleseg: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_teleseg()
        {
            base.SetIni(this,"Sms_teleseg","ID");
        }	
	    /// <summary>
		///	运营商号码段	(如：158,133)
		/// </summary>
 
		public string Phone { get; set; }		
	    /// <summary>
		///	运营商ID	1移动2电信3联通
		/// </summary>
 
		public int? Carrier_ID { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string Phone="Phone";					
			public const string Carrier_ID="Carrier_ID";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_Account: BaseEntity
    {
	  
	    /// <summary>
		///	帐号ID
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_Account()
        {
            base.SetIni(this,"Sms_Account","ID");
        }	
	    /// <summary>
		///	企业ID
		/// </summary>
 
		public int EnterpriseID { get; set; }		
	    /// <summary>
		///	帐号名称
		/// </summary>
 
		public string Account { get; set; }		
	    /// <summary>
		///	帐号密码
		/// </summary>
 
		public string Password { get; set; }		
	    /// <summary>
		///	签名
		/// </summary>
 
		public string Signature { get; set; }		
	    /// <summary>
		///	帐号优先级
		/// </summary>
 
		public int? Level { get; set; }		
	    /// <summary>
		///	帐号状态
		/// </summary>
 
		public int? State { get; set; }		
	    /// <summary>
		///	wappush单价
		/// </summary>
 
		public float? wappush_price { get; set; }		
	    /// <summary>
		///	短信单价
		/// </summary>
 
		public float sms_price { get; set; }		
	    /// <summary>
		///	彩信单价
		/// </summary>
 
		public float? mms_price { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string EnterpriseID="EnterpriseID";					
			public const string Account="Account";					
			public const string Password="Password";					
			public const string Signature="Signature";					
			public const string Level="Level";					
			public const string State="State";					
			public const string wappush_price="wappush_price";					
			public const string sms_price="sms_price";					
			public const string mms_price="mms_price";					
		}
				 
	}
					
    /// <summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sms_MO: BaseEntity
    {
	  
	    /// <summary>
		///	
		/// </summary>
        [Column(IsKeyProperty = true,AutoIncrement=true)]
		public int ID { get; set; }	
		public  Sms_MO()
        {
            base.SetIni(this,"Sms_MO","ID");
        }	
	    /// <summary>
		///	帐号ID
		/// </summary>
 
		public int? AccountID { get; set; }		
	    /// <summary>
		///	接收上行的通道
		/// </summary>
 
		public string ReceiveSpid { get; set; }		
	    /// <summary>
		///	手机号
		/// </summary>
 
		public string Phone { get; set; }		
	    /// <summary>
		///	上行内容
		/// </summary>
 
		public string Content { get; set; }		
	    /// <summary>
		///	接收时间
		/// </summary>
 
		public DateTime? ReceiveTime { get; set; }		
	    /// <summary>
		///	是否已读	0:否,1:是
		/// </summary>
 
		public int? Readed { get; set; }		
	    /// <summary>
		///	是否已回复	0:否,1:是
		/// </summary>
 
		public int? Responsed { get; set; }		
	    /// <summary>
		///	创建时间
		/// </summary>
 
		public DateTime? CreateTime { get; set; }		
				
		public static class Columns
		{
			public const string ID="ID";					
			public const string AccountID="AccountID";					
			public const string ReceiveSpid="ReceiveSpid";					
			public const string Phone="Phone";					
			public const string Content="Content";					
			public const string ReceiveTime="ReceiveTime";					
			public const string Readed="Readed";					
			public const string Responsed="Responsed";					
			public const string CreateTime="CreateTime";					
		}
				 
	}
				 

