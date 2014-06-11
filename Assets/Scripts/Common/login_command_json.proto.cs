//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: login_command_json.proto
// Note: requires additional types generated from: login_command.proto
namespace Pmd
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginJson")]
  public partial class LoginJson : global::ProtoBuf.IExtensible
  {
    public LoginJson() {}
    
    [global::ProtoBuf.ProtoContract(Name=@"Param")]
    public enum Param
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"AccountTokenVerifyLoginJsonPmd_CS", Value=1)]
      AccountTokenVerifyLoginJsonPmd_CS = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"AccountTokenVerifyReturnLoginJsonPmd_S", Value=2)]
      AccountTokenVerifyReturnLoginJsonPmd_S = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ZoneInfoListLoginJsonPmd_S", Value=3)]
      ZoneInfoListLoginJsonPmd_S = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserLoginRequestLoginJsonPmd_C", Value=4)]
      UserLoginRequestLoginJsonPmd_C = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserLoginReturnFailLoginJsonPmd_S", Value=5)]
      UserLoginReturnFailLoginJsonPmd_S = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserLoginReturnOkLoginJsonPmd_S", Value=6)]
      UserLoginReturnOkLoginJsonPmd_S = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserLoginTokenLoginJsonPmd_C", Value=7)]
      UserLoginTokenLoginJsonPmd_C = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ClientLogUrlLoginJsonPmd_S", Value=8)]
      ClientLogUrlLoginJsonPmd_S = 8
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AccountTokenVerifyLoginJsonPmd_CS")]
  public partial class AccountTokenVerifyLoginJsonPmd_CS : global::ProtoBuf.IExtensible
  {
    public AccountTokenVerifyLoginJsonPmd_CS() {}
    
    private string _account;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string account
    {
      get { return _account; }
      set { _account = value; }
    }
    private string _token;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"token", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string token
    {
      get { return _token; }
      set { _token = value; }
    }
    private uint _version;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint version
    {
      get { return _version; }
      set { _version = value; }
    }
    private uint _gameid = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"gameid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint gameid
    {
      get { return _gameid; }
      set { _gameid = value; }
    }
    private string _mid = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"mid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string mid
    {
      get { return _mid; }
      set { _mid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AccountTokenVerifyReturnLoginJsonPmd_S")]
  public partial class AccountTokenVerifyReturnLoginJsonPmd_S : global::ProtoBuf.IExtensible
  {
    public AccountTokenVerifyReturnLoginJsonPmd_S() {}
    
    private Pmd.VerifyReturnReason _retcode;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"retcode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Pmd.VerifyReturnReason retcode
    {
      get { return _retcode; }
      set { _retcode = value; }
    }
    private string _desc = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"desc", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string desc
    {
      get { return _desc; }
      set { _desc = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ZoneInfoListLoginJsonPmd_S")]
  public partial class ZoneInfoListLoginJsonPmd_S : global::ProtoBuf.IExtensible
  {
    public ZoneInfoListLoginJsonPmd_S() {}
    
    private string _gamename;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"gamename", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string gamename
    {
      get { return _gamename; }
      set { _gamename = value; }
    }
    private uint _gameid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"gameid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint gameid
    {
      get { return _gameid; }
      set { _gameid = value; }
    }
    private readonly global::System.Collections.Generic.List<Pmd.ZoneInfo> _zonelist = new global::System.Collections.Generic.List<Pmd.ZoneInfo>();
    [global::ProtoBuf.ProtoMember(3, Name=@"zonelist", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Pmd.ZoneInfo> zonelist
    {
      get { return _zonelist; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserLoginRequestLoginJsonPmd_C")]
  public partial class UserLoginRequestLoginJsonPmd_C : global::ProtoBuf.IExtensible
  {
    public UserLoginRequestLoginJsonPmd_C() {}
    
    private uint _gameid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"gameid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint gameid
    {
      get { return _gameid; }
      set { _gameid = value; }
    }
    private uint _zoneid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"zoneid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint zoneid
    {
      get { return _zoneid; }
      set { _zoneid = value; }
    }
    private uint _gameversion;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"gameversion", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint gameversion
    {
      get { return _gameversion; }
      set { _gameversion = value; }
    }
    private string _mid = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"mid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string mid
    {
      get { return _mid; }
      set { _mid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserLoginReturnFailLoginJsonPmd_S")]
  public partial class UserLoginReturnFailLoginJsonPmd_S : global::ProtoBuf.IExtensible
  {
    public UserLoginReturnFailLoginJsonPmd_S() {}
    
    private Pmd.LoginReturnFailReason _retcode;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"retcode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Pmd.LoginReturnFailReason retcode
    {
      get { return _retcode; }
      set { _retcode = value; }
    }
    private string _desc;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"desc", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string desc
    {
      get { return _desc; }
      set { _desc = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserLoginReturnOkLoginJsonPmd_S")]
  public partial class UserLoginReturnOkLoginJsonPmd_S : global::ProtoBuf.IExtensible
  {
    public UserLoginReturnOkLoginJsonPmd_S() {}
    
    private ulong _accountid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"accountid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong accountid
    {
      get { return _accountid; }
      set { _accountid = value; }
    }
    private ulong _logintempid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"logintempid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong logintempid
    {
      get { return _logintempid; }
      set { _logintempid = value; }
    }
    private ulong _tokenid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"tokenid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong tokenid
    {
      get { return _tokenid; }
      set { _tokenid = value; }
    }
    private string _gatewayurl;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"gatewayurl", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string gatewayurl
    {
      get { return _gatewayurl; }
      set { _gatewayurl = value; }
    }
    private uint _gameid;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"gameid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint gameid
    {
      get { return _gameid; }
      set { _gameid = value; }
    }
    private uint _zoneid;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"zoneid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint zoneid
    {
      get { return _zoneid; }
      set { _zoneid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserLoginTokenLoginJsonPmd_C")]
  public partial class UserLoginTokenLoginJsonPmd_C : global::ProtoBuf.IExtensible
  {
    public UserLoginTokenLoginJsonPmd_C() {}
    
    private uint _gameid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"gameid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint gameid
    {
      get { return _gameid; }
      set { _gameid = value; }
    }
    private uint _zoneid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"zoneid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint zoneid
    {
      get { return _zoneid; }
      set { _zoneid = value; }
    }
    private ulong _accountid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"accountid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong accountid
    {
      get { return _accountid; }
      set { _accountid = value; }
    }
    private ulong _logintempid;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"logintempid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong logintempid
    {
      get { return _logintempid; }
      set { _logintempid = value; }
    }
    private uint _timestamp;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"timestamp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint timestamp
    {
      get { return _timestamp; }
      set { _timestamp = value; }
    }
    private string _tokenmd5;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"tokenmd5", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string tokenmd5
    {
      get { return _tokenmd5; }
      set { _tokenmd5 = value; }
    }
    private string _mid = "";
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"mid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string mid
    {
      get { return _mid; }
      set { _mid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ClientLogUrlLoginJsonPmd_S")]
  public partial class ClientLogUrlLoginJsonPmd_S : global::ProtoBuf.IExtensible
  {
    public ClientLogUrlLoginJsonPmd_S() {}
    
    private string _loglevel;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"loglevel", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string loglevel
    {
      get { return _loglevel; }
      set { _loglevel = value; }
    }
    private string _logurl;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"logurl", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string logurl
    {
      get { return _logurl; }
      set { _logurl = value; }
    }
    private bool _distinct;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"distinct", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool distinct
    {
      get { return _distinct; }
      set { _distinct = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}