//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: MoveCommand.proto
// Note: requires additional types generated from: MapCommand.proto
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
      UserGotoMoveUserCmd_S = 3
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserMoveUpMoveUserCmd_C")]
  public partial class UserMoveUpMoveUserCmd_C : global::ProtoBuf.IExtensible
  {
    public UserMoveUpMoveUserCmd_C() {}
    
    private Cmd.Pos _pos;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"pos", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos pos
    {
      get { return _pos; }
      set { _pos = value; }
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
    private Cmd.Pos _pos;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"pos", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos pos
    {
      get { return _pos; }
      set { _pos = value; }
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
    private Cmd.Pos _pos;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"pos", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.Pos pos
    {
      get { return _pos; }
      set { _pos = value; }
    }
    private uint _mapid;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"mapid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint mapid
    {
      get { return _mapid; }
      set { _mapid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}