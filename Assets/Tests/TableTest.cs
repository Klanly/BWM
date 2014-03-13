using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GX;

public class TableTest : MonoBehaviour
{
	void Start()
	{
		foreach (var item in Table.Query<table.TableItemItem>())
		{
			Debug.Log(item.ToStringDebug());
		}
	}
}
