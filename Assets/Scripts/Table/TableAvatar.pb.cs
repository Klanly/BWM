//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: TableAvatar.proto
namespace table
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TableAvatarItem")]
  public partial class TableAvatarItem : global::ProtoBuf.IExtensible
  {
    public TableAvatarItem() {}
    
    private uint _profession;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"profession", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint profession
    {
      get { return _profession; }
      set { _profession = value; }
    }
    private uint _sex;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"sex", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint sex
    {
      get { return _sex; }
      set { _sex = value; }
    }
    private string _body;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"body", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string body
    {
      get { return _body; }
      set { _body = value; }
    }
    private string _head;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"head", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string head
    {
      get { return _head; }
      set { _head = value; }
    }
    private string _weapon;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"weapon", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string weapon
    {
      get { return _weapon; }
      set { _weapon = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}