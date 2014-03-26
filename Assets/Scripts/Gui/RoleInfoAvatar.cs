using UnityEngine;
using System.Collections;

public class RoleInfoAvatar : MonoBehaviour
{
	private GameObject avatar;

	void OnEnable()
	{
		NGUITools.BringForward(this.gameObject);

		avatar = Avatar.Create(table.TableAvatar.Select(MainRole.ServerInfo.profession, MainRole.ServerInfo.sexman));
		avatar.name = "RoleInfoAvatar.avatar";
		avatar.transform.position = new Vector3(-10, 0, 0);
		avatar.transform.Rotate(0, 180, 0);
		avatar.transform.localScale = new Vector3(5, 5, 5);
	}

	void OnDisable()
	{
		if (avatar)
		{
			GameObject.Destroy(avatar);
			avatar = null;
		}
	}
}
