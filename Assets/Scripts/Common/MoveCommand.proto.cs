//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: MoveCommand.proto
// Note: requires additional types generated from: Common.proto
namespace Cmd
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Move")]
  public partial class Move : global::ProtoBuf.IExtensible
  {
    public Move() {}
    
    [global::ProtoBuf.ProtoContract(Name=@"Param")]
    public enum Param
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserMoveUpMoveUserCmd_C", Value=1)]
      UserMoveUpMoveUserCmd_C = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserMoveDownMoveUserCmd_S", Value=2)]
      UserMoveDownMoveUserCmd_S = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserGotoMoveUserCmd_S", Value=3)]
      UserGotoMoveUserCmd_S = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ReturnUserMoveFailMoveUserCmd_S", Value=4)]
      ReturnUserMoveFailMoveUserCmd_S = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"NpcMoveDownMoveUserCmd_S", Value=5)]
      NpcMoveDownMoveUserCmd_S = 5
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserMoveUpMoveUserCmd_C")]
  public partial class UserMoveUpMoveUserCmd_C : global::ProtoBuf.IExtensible
  {
    public UserMoveUpMoveUserCmd_C() {}
    
    private Cmd.Pos _poscm;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"poscm", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos poscm
    {
      get { return _poscm; }
      set { _poscm = value; }
    }
    private uint _angle;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"angle", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint angle
    {
      get { return _angle; }
      set { _angle = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserMoveDownMoveUserCmd_S")]
  public partial class UserMoveDownMoveUserCmd_S : global::ProtoBuf.IExtensible
  {
    public UserMoveDownMoveUserCmd_S() {}
    
    private ulong _charid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"charid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong charid
    {
      get { return _charid; }
      set { _charid = value; }
    }
    private Cmd.Pos _poscm;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"poscm", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos poscm
    {
      get { return _poscm; }
      set { _poscm = value; }
    }
    private uint _angle;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"angle", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint angle
    {
      get { return _angle; }
      set { _angle = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserGotoMoveUserCmd_S")]
  public partial class UserGotoMoveUserCmd_S : global::ProtoBuf.IExtensible
  {
    public UserGotoMoveUserCmd_S() {}
    
    private ulong _charid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"charid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong charid
    {
      get { return _charid; }
      set { _charid = value; }
    }
    private Cmd.Pos _poscm;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"poscm", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos poscm
    {
      get { return _poscm; }
      set { _poscm = value; }
    }
    private uint _angle;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"angle", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint angle
    {
      get { return _angle; }
      set { _angle = value; }
    }
    private uint _mapid;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"mapid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint mapid
    {
      get { return _mapid; }
      set { _mapid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReturnUserMoveFailMoveUserCmd_S")]
  public partial class ReturnUserMoveFailMoveUserCmd_S : global::ProtoBuf.IExtensible
  {
    public ReturnUserMoveFailMoveUserCmd_S() {}
    
    private Cmd.Pos _poscm;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"poscm", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos poscm
    {
      get { return _poscm; }
      set { _poscm = value; }
    }
    private uint _angle;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"angle", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint angle
    {
      get { return _angle; }
      set { _angle = value; }
    }
    private Cmd.MoveFailType _reason;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"reason", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Cmd.MoveFailType reason
    {
      get { return _reason; }
      set { _reason = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"NpcMoveDownMoveUserCmd_S")]
  public partial class NpcMoveDownMoveUserCmd_S : global::ProtoBuf.IExtensible
  {
    public NpcMoveDownMoveUserCmd_S() {}
    
    private ulong _tempid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"tempid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong tempid
    {
      get { return _tempid; }
      set { _tempid = value; }
    }
    private Cmd.Pos _poscm;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"poscm", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos poscm
    {
      get { return _poscm; }
      set { _poscm = value; }
    }
    private uint _angle;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"angle", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint angle
    {
      get { return _angle; }
      set { _angle = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"MoveFailType")]
    public enum MoveFailType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"MoveFailType_Goto", Value=1)]
      MoveFailType_Goto = 1
    }
  
}