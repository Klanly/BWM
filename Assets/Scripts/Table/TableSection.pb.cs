//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: TableSection.proto
namespace table
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableSection")]
  public partial class TableSection : global::ProtoBuf.IExtensible
  {
    public TableSection() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _level;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint level
    {
      get { return _level; }
      set { _level = value; }
    }
    private uint _stageIdNormal;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"stageIdNormal", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint stageIdNormal
    {
      get { return _stageIdNormal; }
      set { _stageIdNormal = value; }
    }
    private uint _stageIdSenior;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"stageIdSenior", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint stageIdSenior
    {
      get { return _stageIdSenior; }
      set { _stageIdSenior = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}