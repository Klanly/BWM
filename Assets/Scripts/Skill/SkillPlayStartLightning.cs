using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Skill))]
public class SkillPlayStartLightning : SkillBase
{

	public float time;
	public string mountOfStartGo;
	public string mountOfTargetGo;

	public int branchesPerReceiver = 1;
	public int sectionsPerBranch = 10;
	public float lightningBoltJitter = 5f;
	public float lightningWidth = 1f;
	public float lightningFrequency = 60f;

	public Material lightningMaterial;
	private GameObject[] lightningReceivers;

	private List<LineRenderer> lineRenderers = new List<LineRenderer>();
	private float timeCount = 0f;

	private float timeTotal = 0.0f;
	private List<GameObject> lightningGo = new List<GameObject>();
	private Transform mountStartGo;

	override public void StartSkill()
	{
		var skill = gameObject.GetComponent<Skill>();
		if (!skill || !skill.startGo || !skill.targetGo)
		{
			Finish();
			return;
		}

		mountStartGo = SkillBase.Find(skill.startGo.transform, mountOfStartGo);
		if (!mountStartGo)
			mountStartGo = skill.startGo.transform;

		var mountTargetGo = SkillBase.Find(skill.targetGo.transform, mountOfTargetGo);
		if (!mountTargetGo)
			mountTargetGo = skill.targetGo.transform;
		lightningReceivers = new GameObject[1] { mountTargetGo.gameObject };

		//Initialise the line renderers
		InitialiseLineRenderers();
	}

	void Finish()
	{
		foreach (GameObject t in lightningGo)
			Destroy(t);
		Destroy(this);
	}

	// Update is called once per frame
	void Update()
	{
		timeTotal += Time.deltaTime;
		if (timeTotal > time)
		{
			Finish();
			return;
		}

		if (!mountStartGo || !lightningReceivers[0])
		{
			Finish();
			return;
		}

		//Check if it's time to change the lightning bolt
		timeCount += Time.deltaTime;
		if (timeCount < (1 / lightningFrequency))
			return;
		else
			timeCount = 0f;

		//Keep a count of which renderer we're currently working on
		int rendererIndex = 0;

		for (int i = 0; i < lightningReceivers.Length; i++)
		{
			//Determine the length of a section of the bolt
			Vector3 sectionVector = (lightningReceivers[i].transform.position - mountStartGo.position) / sectionsPerBranch;

			//Initialise an array of vectors for the bolt
			Vector3[] lineVectors = new Vector3[sectionsPerBranch];

			//Calculate the vectors for the middle sections
			for (int j = 1; j < lineVectors.Length - 1; j++)
				lineVectors[j] = mountStartGo.position + (sectionVector * j);

			int startIndex = 0;
			//Set the values in the line renderer for ecah bolt
			for (int j = 0; j < branchesPerReceiver; j++)
			{

				if (lineRenderers[rendererIndex])
				{

					if (j % 2 == 0)
					{
						//Set the beginning and end of each branch to be on the game objects
						lineRenderers[rendererIndex].SetPosition(startIndex + 0, mountStartGo.position);
						lineRenderers[rendererIndex].SetPosition(startIndex + lineVectors.Length - 1, lightningReceivers[i].transform.position);
						lineRenderers[rendererIndex].SetWidth(lightningWidth, lightningWidth);

						//Set vectors for the rest of the sections adding jitter in the process
						for (int k = 1; k < (sectionsPerBranch - 1); k++)
							lineRenderers[rendererIndex].SetPosition(startIndex + k, AddVectorJitter(lineVectors[k], lightningBoltJitter));
					}
					else
					{
						//Set the beginning and end of each branch to be on the game objects
						lineRenderers[rendererIndex].SetPosition(startIndex + 0, lightningReceivers[i].transform.position);
						lineRenderers[rendererIndex].SetPosition(startIndex + lineVectors.Length - 1, mountStartGo.position);
						lineRenderers[rendererIndex].SetWidth(lightningWidth, lightningWidth);

						//Set vectors for the rest of the sections adding jitter in the process
						for (int k = 1; k < (sectionsPerBranch - 1); k++)
							lineRenderers[rendererIndex].SetPosition(startIndex + k, AddVectorJitter(lineVectors[sectionsPerBranch - k - 1], lightningBoltJitter));
					}

					startIndex += sectionsPerBranch;
				}

			}
			rendererIndex++;
		}
	}

	//Add a bit of random jitter to a vector
	public Vector3 AddVectorJitter(Vector3 vector, float jitter)
	{
		vector += Vector3.left * Random.Range(-jitter, jitter);
		vector += Vector3.up * Random.Range(-jitter, jitter);
		vector += Vector3.forward * Random.Range(-jitter, jitter);

		return vector;
	}

	public void InitialiseLineRenderers()
	{
		//Create a game object for each line renderer and parent it to this game object
		for (int i = 0; i < lightningReceivers.Length; i++)
		{
			GameObject temp = new GameObject();
			temp.transform.parent = gameObject.transform;
			temp.transform.localPosition = Vector3.zero;
			temp.name = "Line Renderer " + (i + 1);
			var renderer = temp.AddComponent<LineRenderer>();
			lightningGo.Add(temp);
			lineRenderers.Add(renderer);
		}

		//Set initial settings for each renderer
		for (int i = 0; i < lineRenderers.Count; i++)
		{
			lineRenderers[i].castShadows = false;
			lineRenderers[i].receiveShadows = false;
			lineRenderers[i].material = lightningMaterial;

			lineRenderers[i].SetVertexCount(sectionsPerBranch * branchesPerReceiver);
			lineRenderers[i].SetWidth(lightningWidth, lightningWidth);
		}
	}

	private void InitialiseLights()
	{

	}

	private void RandomiseLights()
	{

	}
}
