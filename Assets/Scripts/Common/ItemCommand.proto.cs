//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: ItemCommand.proto
// Note: requires additional types generated from: Common.proto
// Note: requires additional types generated from: SaveData.proto
namespace Cmd
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Item")]
  public partial class Item : global::ProtoBuf.IExtensible
  {
    public Item() {}
    
    [global::ProtoBuf.ProtoContract(Name=@"Param")]
    public enum Param
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"AddItemItemUserCmd_S", Value=1)]
      AddItemItemUserCmd_S = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ReplaceItemListItemUserCmd_S", Value=2)]
      ReplaceItemListItemUserCmd_S = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RemoveItemItemUserCmd_CS", Value=3)]
      RemoveItemItemUserCmd_CS = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SwapItemItemUserCmd_CS", Value=4)]
      SwapItemItemUserCmd_CS = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SplitItemItemUserCmd_CS", Value=5)]
      SplitItemItemUserCmd_CS = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UnionItemItemUserCmd_CS", Value=6)]
      UnionItemItemUserCmd_CS = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UseItemItemUserCmd_CS", Value=7)]
      UseItemItemUserCmd_CS = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RefreshPosItemUserCmd_CS", Value=8)]
      RefreshPosItemUserCmd_CS = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RefreshCountItemItemUserCmd_CS", Value=9)]
      RefreshCountItemItemUserCmd_CS = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TidyItemItemUserCmd_C", Value=10)]
      TidyItemItemUserCmd_C = 10
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AddItemItemUserCmd_S")]
  public partial class AddItemItemUserCmd_S : global::ProtoBuf.IExtensible
  {
    public AddItemItemUserCmd_S() {}
    
    private Cmd.SaveItem _item;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"item", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.SaveItem item
    {
      get { return _item; }
      set { _item = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReplaceItemListItemUserCmd_S")]
  public partial class ReplaceItemListItemUserCmd_S : global::ProtoBuf.IExtensible
  {
    public ReplaceItemListItemUserCmd_S() {}
    
    private readonly global::System.Collections.Generic.List<Cmd.SaveItem> _itemlist = new global::System.Collections.Generic.List<Cmd.SaveItem>();
    [global::ProtoBuf.ProtoMember(1, Name=@"itemlist", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Cmd.SaveItem> itemlist
    {
      get { return _itemlist; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RemoveItemItemUserCmd_CS")]
  public partial class RemoveItemItemUserCmd_CS : global::ProtoBuf.IExtensible
  {
    public RemoveItemItemUserCmd_CS() {}
    
    private ulong _thisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"thisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong thisid
    {
      get { return _thisid; }
      set { _thisid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SwapItemItemUserCmd_CS")]
  public partial class SwapItemItemUserCmd_CS : global::ProtoBuf.IExtensible
  {
    public SwapItemItemUserCmd_CS() {}
    
    private ulong _srcThisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"srcThisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong srcThisid
    {
      get { return _srcThisid; }
      set { _srcThisid = value; }
    }
    private ulong _dstThisid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"dstThisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong dstThisid
    {
      get { return _dstThisid; }
      set { _dstThisid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SplitItemItemUserCmd_CS")]
  public partial class SplitItemItemUserCmd_CS : global::ProtoBuf.IExtensible
  {
    public SplitItemItemUserCmd_CS() {}
    
    private ulong _thisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"thisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong thisid
    {
      get { return _thisid; }
      set { _thisid = value; }
    }
    private int _num;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private Cmd.ItemLocation _dst;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"dst", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.ItemLocation dst
    {
      get { return _dst; }
      set { _dst = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UnionItemItemUserCmd_CS")]
  public partial class UnionItemItemUserCmd_CS : global::ProtoBuf.IExtensible
  {
    public UnionItemItemUserCmd_CS() {}
    
    private ulong _srcThisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"srcThisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong srcThisid
    {
      get { return _srcThisid; }
      set { _srcThisid = value; }
    }
    private ulong _dstThisid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"dstThisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong dstThisid
    {
      get { return _dstThisid; }
      set { _dstThisid = value; }
    }
    private int _num;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UseItemItemUserCmd_CS")]
  public partial class UseItemItemUserCmd_CS : global::ProtoBuf.IExtensible
  {
    public UseItemItemUserCmd_CS() {}
    
    private ulong _thisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"thisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong thisid
    {
      get { return _thisid; }
      set { _thisid = value; }
    }
    private ulong _targetid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"targetid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong targetid
    {
      get { return _targetid; }
      set { _targetid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RefreshPosItemUserCmd_CS")]
  public partial class RefreshPosItemUserCmd_CS : global::ProtoBuf.IExtensible
  {
    public RefreshPosItemUserCmd_CS() {}
    
    private ulong _thisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"thisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong thisid
    {
      get { return _thisid; }
      set { _thisid = value; }
    }
    private Cmd.ItemLocation _dst;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"dst", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Cmd.ItemLocation dst
    {
      get { return _dst; }
      set { _dst = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RefreshCountItemItemUserCmd_CS")]
  public partial class RefreshCountItemItemUserCmd_CS : global::ProtoBuf.IExtensible
  {
    public RefreshCountItemItemUserCmd_CS() {}
    
    private ulong _thisid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"thisid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong thisid
    {
      get { return _thisid; }
      set { _thisid = value; }
    }
    private int _count;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int count
    {
      get { return _count; }
      set { _count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TidyItemItemUserCmd_C")]
  public partial class TidyItemItemUserCmd_C : global::ProtoBuf.IExtensible
  {
    public TidyItemItemUserCmd_C() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}