//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: TableNpc.proto
namespace table
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableNpc")]
  public partial class TableNpc : global::ProtoBuf.IExtensible
  {
    public TableNpc() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private uint _level;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint level
    {
      get { return _level; }
      set { _level = value; }
    }
    private string _label;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"label", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string label
    {
      get { return _label; }
      set { _label = value; }
    }
    private string _model;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"model", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string model
    {
      get { return _model; }
      set { _model = value; }
    }
    private uint _baseType;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"baseType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint baseType
    {
      get { return _baseType; }
      set { _baseType = value; }
    }
    private uint _clickType;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"clickType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint clickType
    {
      get { return _clickType; }
      set { _clickType = value; }
    }
    private readonly global::System.Collections.Generic.List<string> _hpBar = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(8, Name=@"hpBar", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> hpBar
    {
      get { return _hpBar; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}