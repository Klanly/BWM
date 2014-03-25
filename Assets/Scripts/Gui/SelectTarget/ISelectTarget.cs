using UnityEngine;
using System.Collections;

/// <summary>
/// 可被点选的场景对象
/// </summary>
public interface ISelectTarget
{
	string Name { get;}
	string RoleHeadSprite { get; }
}
