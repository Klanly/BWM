using UnityEngine;
using System.Collections;

public static class Avatar
{
	public static GameObject CreateAvatar(string sk, string body, string head, string weapon)
	{
		if (string.IsNullOrEmpty(sk) || string.IsNullOrEmpty(body))
			return null;

		var res = Resources.Load(sk);
		if (res == null)
		{
			Debug.LogError("Load Resources error: " + sk);
			return null;
		}
		var gosk = Object.Instantiate(res) as GameObject;
		ChangeBody(gosk, body);
		ChangeHead(gosk, head);
		LinkWeapon(gosk, weapon);
		return gosk;
	}

	public static void ChangeBody(GameObject go, string body)
	{
		if (!string.IsNullOrEmpty(body))
		{
			var gobody = Object.Instantiate(Resources.Load(body)) as GameObject;
			go.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = gobody.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
			go.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = gobody.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;
			Object.Destroy(gobody);
		}
	}

	public static void ChangeHead(GameObject go, string head)
	{
		if (!string.IsNullOrEmpty(head))
		{
			var gohead = Object.Instantiate(Resources.Load(head)) as GameObject;
			var ktop = go.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 Head").Find("k_top");
			ktop.gameObject.AddComponent<MeshFilter>().sharedMesh = gohead.GetComponentInChildren<MeshFilter>().sharedMesh;
			ktop.gameObject.AddComponent<MeshRenderer>().sharedMaterials = gohead.GetComponentInChildren<MeshRenderer>().sharedMaterials;
			Object.Destroy(gohead);
		}
	}

	public static void LinkWeapon(GameObject go, string weapon)
	{
		if (!string.IsNullOrEmpty(weapon))
		{
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
}
