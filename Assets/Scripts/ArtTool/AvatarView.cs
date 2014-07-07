using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarView : MonoBehaviour
{

	public GameObject prefabSkeleton;
	public GameObject prefabCoat;
	public GameObject prefabHead;
	public GameObject prefabWeapon;

	private string strSkeleton = "Prefabs/Models/Body/Sk_Male_001";
	private string strCoat = "Prefabs/Models/Body/Male_Body_7000";
	private string strHead = "Prefabs/Models/Head/Male_Head_7000";
	private string strWeapon = "Prefabs/Models/Weapon/Weapon_Sword_1006";

	/*

	// Use this for initialization
	void Start () {

		var role = Object.Instantiate(prefabSkeleton) as Transform;
		var coat  = Object.Instantiate(prefabCoat) as Transform;

		role.GetComponentsInChildren<Transform>();

		Debug.Log(role);
		//Debug.Log(role.GetComponent<Transform>());
		role.position = new Vector3(10,0,10);
		role.localScale = new Vector3(5,5,5);
		role.name = "role";

		// The SkinnedMeshRenderers that will make up a character will be
		// combined into one SkinnedMeshRenderers using multiple materials.
		// This will speed up rendering the resulting character.
		List<CombineInstance> combineInstances = new List<CombineInstance>();
		List<Material> materials = new List<Material>();
		List<Transform> bones = new List<Transform>();
		
		//获得构成骨架的所有Transform
		Transform[] transforms = role.GetComponentsInChildren<Transform>();
		
		//一次处理构成身体的各部分
		//foreach (CharacterElement element in currentConfiguration.Values)
		{
			//GetSkinnedMeshRenderer()内部Instantiat了一个由该部分肢体Assets构成的
			//GameObject，并返回Unity自动为其创建SinkedMeshRender。
			//SkinnedMeshRenderer smr = element.GetSkinnedMeshRenderer();
			SkinnedMeshRenderer smr = coat.GetComponentInChildren<SkinnedMeshRenderer>();
			
			//注意smr.materials中包含的材质数量和顺序与下面的sub mesh是对应的
			materials.AddRange(smr.materials);
			for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
			{
				CombineInstance ci = new CombineInstance();
				ci.mesh = smr.sharedMesh;
				ci.subMeshIndex = sub;
				combineInstances.Add(ci);
			}
			
			// As the SkinnedMeshRenders are stored in assetbundles that do not
			// contain their bones (those are stored in the characterbase assetbundles)
			// we need to collect references to the bones we are using
			// 网格点与骨骼的对应关系是通过Mesh数据结构中的BoneWeight数组来实现的。该数组
			// 与网格顶点数组对应，记录了每个网格点受骨骼（骨骼记录在SinkedMeshRender的bones
			// 数组中，按下标索引）影响的权重。
			// 而此处，示例程序提供的肢体Assets并不包含骨骼，而是返回骨骼名称。因此，推断
			// GetBoneNames()返回的骨骼名称应该与实际骨骼数组的顺序相同。
			foreach (Transform bone in smr.bones)
			{
				foreach (Transform transform in transforms)
				{
					//通过名字找到实际的骨骼
					if (transform.name != bone.name) continue;
					bones.Add(transform);
					break;
				}
			}
			
			//Object.Destroy(coat.gameObject);
		}
		
		// Obtain and configure the SkinnedMeshRenderer attached to
		// the character base.
		// 至此，combineInstances、bones和materials三个数组中的数据对应关系是正确的。
		// 合并时，第二个参数是fals，表示保持子网格不变，只不过将它们统一到一个Mesh里
		// 来管理，这样只需采用一个SkinedMeshRender绘制，效率较高。
		SkinnedMeshRenderer r = role.GetComponentInChildren<SkinnedMeshRenderer>();
		r.sharedMesh = new Mesh();
		r.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
		r.bones = bones.ToArray();
		r.materials = materials.ToArray();

	}

*/

	private GameObject role, role1;
	void Start()
	{
		role = Instantiate(prefabSkeleton) as GameObject;
		role.transform.position = new Vector3(10, 0, 10);
		role.transform.localScale = new Vector3(5, 5, 5);
		role.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

		role1 = Avatar.Create(strSkeleton, strCoat, strHead, strWeapon);
		role1.transform.position = new Vector3(15, 0, 10);
		role1.transform.localScale = new Vector3(5, 5, 5);
		role1.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
		role1.name = "role1";


		/*
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, strSkeleton);
		if (filePath.Contains("://")) {
			WWW www = new WWW(filePath);
			yield return www;
			role1 = www.;
		} else
			result = System.IO.File.ReadAllText(filePath);
		*/
	}

	// Update is called once per frame
	private bool bTouched = false;
	void Update()
	{

		if (Input.GetKeyDown("space") || (!bTouched && Input.touchCount > 0) || (!bTouched && Input.GetMouseButton(0)))
		{
			bTouched = true;

			var coat = Instantiate(prefabCoat) as GameObject;
			var head = Instantiate(prefabHead) as GameObject;
			var weapon = Instantiate(prefabWeapon) as GameObject;

			role.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = coat.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
			role.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = coat.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;

			Transform[] transforms = role.GetComponentsInChildren<Transform>();
			List<Transform> bones = new List<Transform>();
			foreach (Transform bone in coat.GetComponentInChildren<SkinnedMeshRenderer>().bones)
			{
				foreach (Transform transform in transforms)
				{
					//通过名字找到实际的骨骼
					if (transform.name != bone.name) continue;
					bones.Add(transform);
					break;
				}
			}
			role.GetComponentInChildren<SkinnedMeshRenderer>().bones = bones.ToArray();
			Destroy(coat);

			/*
			var ktop = role.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 Head").Find("k_top");
			ktop.gameObject.AddComponent<MeshFilter>().sharedMesh = head.GetComponentInChildren<MeshFilter>().sharedMesh;
			ktop.gameObject.AddComponent<MeshRenderer>().sharedMaterials = head.GetComponentInChildren<MeshRenderer>().sharedMaterials;
			Destroy(head);
			*/
			var ktop = role.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 Head").Find("k_top");
			var headroot = head.transform;
			headroot.parent = ktop;
			var headma = head.transform.Find("m_a");
			var invert1 = Matrix4x4.TRS(headma.localPosition, headma.localRotation, headma.localScale).inverse;
			var v41 = invert1.GetColumn(3);
			headroot.localPosition = new Vector3(v41.x, v41.y, v41.z);
			headroot.localScale = headma.localScale;
			var rotate1 = headma.localRotation.eulerAngles;
			headroot.localRotation = Quaternion.Inverse(Quaternion.Euler(rotate1));


			/* 以scene_root和k_armright对齐
			var karmright = role.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 R Clavicle").Find("Bip01 R UpperArm").Find("Bip01 R Forearm").Find("Bip01 R Hand").Find("k_armright");
			var weaponroot = weapon.transform;
			weaponroot.parent = karmright;
			weaponroot.localPosition = new Vector3(0,0,0);
			weaponroot.localScale = new Vector3(1,1,1);
			weaponroot.localRotation = Quaternion.Euler(new Vector3(90,0,0));
			*/

			// 以m_a和k_armright对齐，scene_root点为m_a的inverse
			var karmright = role.transform.Find("Bip01").Find("Bip01 Pelvis").Find("Bip01 Spine").Find("Bip01 Spine1").Find("Bip01 Spine2").Find("Bip01 Neck").Find("Bip01 R Clavicle").Find("Bip01 R UpperArm").Find("Bip01 R Forearm").Find("Bip01 R Hand").Find("k_armright");
			var weaponroot = weapon.transform;
			weaponroot.parent = karmright;
			var weaponma = weapon.transform.Find("m_a");
			var invert = Matrix4x4.TRS(weaponma.localPosition, weaponma.localRotation, weaponma.localScale).inverse;
			var v4 = invert.GetColumn(3);
			weaponroot.localPosition = new Vector3(v4.x, v4.y, v4.z);
			weaponroot.localScale = weaponma.localScale;
			var rotate = weaponma.localRotation.eulerAngles;
			weaponroot.localRotation = Quaternion.Inverse(Quaternion.Euler(rotate));
		}
	}
}
