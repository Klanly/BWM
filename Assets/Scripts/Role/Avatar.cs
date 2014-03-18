using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Avatar
{
	private static readonly string[] Skeleton = new string[]
	{
		null,
		"Prefabs/Models/Body/Sk_Male_001", 
		"Prefabs/Models/Body/Sk_Female_001"
	};

	public static GameObject Create(table.TableAvatar item)
	{
		return Create(Skeleton[item.sex], item.body, item.head, item.weapon);
	}

	public static GameObject Create(string skeleton, string body, string head, string weapon)
	{
		if (string.IsNullOrEmpty(skeleton) || string.IsNullOrEmpty(body))
			return null;

		var res = Resources.Load(skeleton);
		if (res == null)
		{
			Debug.LogError("Load Resources error: " + skeleton);
			return null;
		}
		var gosk = Object.Instantiate(res) as GameObject;
		ChangeBody(gosk, body);
		LinkHead(gosk, head);
		LinkWeapon(gosk, weapon);
		return gosk;
	}

	public static void ChangeBody(GameObject go, string body)
	{
		if (string.IsNullOrEmpty(body))
			return;

		var gobody = Object.Instantiate(Resources.Load(body)) as GameObject;
		go.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = gobody.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		go.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = gobody.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;

		Transform[] transforms = go.GetComponentsInChildren<Transform>();
		List<Transform> bones = new List<Transform>();
		foreach (Transform bone in gobody.GetComponentInChildren<SkinnedMeshRenderer>().bones)
		{
			foreach (Transform transform in transforms)
			{
				//通过名字找到实际的骨骼
				if (transform.name != bone.name) continue;
				bones.Add(transform);
				break;
			}
		}
		go.GetComponentInChildren<SkinnedMeshRenderer>().bones = bones.ToArray();
		Object.Destroy(gobody);
	}

	public static void LinkHead(GameObject go, string head)
	{
		if (string.IsNullOrEmpty(head))
			return;

		var gohead = Object.Instantiate(Resources.Load(head)) as GameObject;

		var ktop = go.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 Head").Find("k_top");
		var headroot = gohead.transform;
		headroot.parent = ktop;
		var headma = gohead.transform.Find("m_a");
		var invert = Matrix4x4.TRS(headma.localPosition, headma.localRotation, headma.localScale).inverse;
		var v4 = invert.GetColumn(3);
		headroot.localPosition = new Vector3(v4.x, v4.y, v4.z);
		headroot.localScale = headma.localScale;
		var rotate = headma.localRotation.eulerAngles;
		headroot.localRotation = Quaternion.Inverse(Quaternion.Euler(rotate));
	}

	public static void LinkWeapon(GameObject go, string weapon)
	{
		if (string.IsNullOrEmpty(weapon))
			return;

		var goweapon = Object.Instantiate(Resources.Load(weapon)) as GameObject;

		/*
		// 以scene_root和k_armright对齐
		var karmright = go.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 R Clavicle").Find("Bip01 R UpperArm").Find("Bip01 R Forearm").Find("Bip01 R Hand").Find("k_armright");
		var weaponroot = goweapon.transform;
		weaponroot.parent = karmright;
		weaponroot.localPosition = new Vector3(0,0,0);
		weaponroot.localScale = new Vector3(1,1,1);
		weaponroot.localRotation = Quaternion.Euler(new Vector3(90,0,0));
		*/

		// 以m_a和k_armright对齐，scene_root点为m_a的inverse
		var karmright = go.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 R Clavicle").Find("Bip01 R UpperArm").Find("Bip01 R Forearm").Find("Bip01 R Hand").Find("k_armright");
		var weaponroot = goweapon.transform;
		weaponroot.parent = karmright;
		var weaponma = goweapon.transform.Find("m_a");
		var invert = Matrix4x4.TRS(weaponma.localPosition, weaponma.localRotation, weaponma.localScale).inverse;
		var v4 = invert.GetColumn(3);
		weaponroot.localPosition = new Vector3(v4.x, v4.y, v4.z);
		weaponroot.localScale = weaponma.localScale;
		var rotate = weaponma.localRotation.eulerAngles;
		weaponroot.localRotation = Quaternion.Inverse(Quaternion.Euler(rotate));
	}
}
