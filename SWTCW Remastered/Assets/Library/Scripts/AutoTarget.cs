using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoTarget : MonoBehaviour {

	public float maxTargetDist = 100f;
	public float reticleRecenterSpeed = 100f;
	public RectTransform reticle;
	private Image reticleImage;
	public float reticleSizeWithNoTarget = 80f;
	public Color reticleColorWithNoTarget;
	public float reticleSizeWithTarget = 150f;
	public Color reticleColorWithTarget;

	public List<GameObject> targets = new List<GameObject>();
	public float targetUpdateWait;
	private GameObject closestTarget = null;

	public GameObject selectedObject; // TODO make private
	private Camera cam;
	private int layerMask;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		reticleImage = reticle.gameObject.GetComponent<Image>();

		InvokeRepeating("UpdateTargets", 0f, targetUpdateWait);
	}
	
	private void UpdateTargets()
	{
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Target"))
		{
			if (!targets.Contains(obj))
			{
				targets.Add(obj);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		SelectObject(FindClosestEnemy());

		if(selectedObject != null)
		{
			Rect visualRect = RendererBoundsInScreenSpace(selectedObject.GetComponent<Renderer>());
			Vector2 newPos = new Vector2((visualRect.xMax - visualRect.xMin) / 2 + visualRect.xMin, (visualRect.yMax - visualRect.yMin) / 2 + visualRect.yMin);
			reticle.position = Vector2.MoveTowards(reticle.position, newPos, reticleRecenterSpeed);
			reticle.sizeDelta = Vector2.MoveTowards(reticle.sizeDelta, new Vector2(reticleSizeWithTarget, reticleSizeWithTarget), reticleRecenterSpeed);
			reticleImage.color = Color.Lerp(reticleImage.color, reticleColorWithTarget, reticleRecenterSpeed);
		}
		else
		{
			float screenX = (Screen.width / 2);
			float screenY = ((Screen.height / 2.5f) * 1.5f);
			reticle.transform.position = Vector2.MoveTowards(reticle.position, new Vector2(screenX, screenY), reticleRecenterSpeed);
			reticle.sizeDelta = Vector2.MoveTowards(reticle.sizeDelta, new Vector2(reticleSizeWithNoTarget, reticleSizeWithNoTarget), reticleRecenterSpeed);
			reticleImage.color = Color.Lerp(reticleImage.color, reticleColorWithNoTarget, reticleRecenterSpeed);
		}
	}

	public GameObject FindClosestEnemy() // TODO return closest target within FOV
	{
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject obj in targets)
		{
			Vector3 diff = obj.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance && IsInView(this.gameObject, obj))
			{
				closestTarget = obj;
				distance = curDistance;
			}
		}
		return closestTarget;
	}

	private bool IsInView(GameObject origin, GameObject toCheck)
	{
		Vector3 pointOnScreen = cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

		//Is in front
		if (pointOnScreen.z < 0)
		{
			//Debug.Log("Behind: " + toCheck.name);
			return false;
		}

		//Is in FOV
		if ((pointOnScreen.x < (Screen.width / 5) * 2) || (pointOnScreen.x > (Screen.width / 5) * 3) ||
				(pointOnScreen.y < (Screen.height / 3)) || (pointOnScreen.y > Screen.height))
		{
			//Debug.Log("OutOfBounds: " + toCheck.name);
			return false;
		}

		RaycastHit hit;
		Vector3 heading = toCheck.transform.position - origin.transform.position;
		Vector3 direction = heading.normalized;// / heading.magnitude;

		layerMask = 1 << LayerMask.NameToLayer("Player");
		layerMask = ~layerMask;
		if (Physics.Linecast(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, out hit, layerMask))
		{
			if (hit.transform.name != toCheck.name)
			{
				/* -->
				Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
				Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
				*/
				//Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
				return false;
			}
		}
		return true;
	}

	private void SelectObject(GameObject obj)
	{
		if(obj != null) // check to ensure object has not been destroyed
		{
			Vector3 diff = obj.transform.position - transform.position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < maxTargetDist * maxTargetDist && IsInView(this.gameObject, obj)) // have to muliply by itselft because curDistance is the is a sqrMagnitude
			{
				//print("Within target distance");
				selectedObject = obj;
			}
			else
			{
				//print("Either not within range or not visible.");
				ClearSelection();
			}
		}
		
	}

	public GameObject GetSelectedObj()
	{
		return selectedObject;
	}

	public void ClearSelection()
	{
		selectedObject = null;
	}

	static Vector3[] screenSpaceCorners = new Vector3[8];
	public Rect RendererBoundsInScreenSpace(Renderer r)
	{
		Bounds selectedObjBounds = r.GetComponent<Renderer>().bounds; // Space occupied by selected object in WORLD space
		if(screenSpaceCorners == null)
		{
			screenSpaceCorners = new Vector3[8];
		}
		

		screenSpaceCorners[0] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x + selectedObjBounds.extents.x, selectedObjBounds.center.y + selectedObjBounds.extents.y, selectedObjBounds.center.z + selectedObjBounds.extents.z));
		screenSpaceCorners[1] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x + selectedObjBounds.extents.x, selectedObjBounds.center.y + selectedObjBounds.extents.y, selectedObjBounds.center.z - selectedObjBounds.extents.z));
		screenSpaceCorners[2] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x + selectedObjBounds.extents.x, selectedObjBounds.center.y - selectedObjBounds.extents.y, selectedObjBounds.center.z + selectedObjBounds.extents.z));
		screenSpaceCorners[3] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x + selectedObjBounds.extents.x, selectedObjBounds.center.y - selectedObjBounds.extents.y, selectedObjBounds.center.z - selectedObjBounds.extents.z));

		screenSpaceCorners[4] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x - selectedObjBounds.extents.x, selectedObjBounds.center.y + selectedObjBounds.extents.y, selectedObjBounds.center.z + selectedObjBounds.extents.z));
		screenSpaceCorners[5] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x - selectedObjBounds.extents.x, selectedObjBounds.center.y + selectedObjBounds.extents.y, selectedObjBounds.center.z - selectedObjBounds.extents.z));
		screenSpaceCorners[6] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x - selectedObjBounds.extents.x, selectedObjBounds.center.y - selectedObjBounds.extents.y, selectedObjBounds.center.z + selectedObjBounds.extents.z));
		screenSpaceCorners[7] = cam.WorldToScreenPoint(new Vector3(selectedObjBounds.center.x - selectedObjBounds.extents.x, selectedObjBounds.center.y - selectedObjBounds.extents.y, selectedObjBounds.center.z - selectedObjBounds.extents.z));

		float minX = screenSpaceCorners[0].x;
		float minY = screenSpaceCorners[0].y;
		float maxX = screenSpaceCorners[0].x;
		float maxY = screenSpaceCorners[0].y;

		for (int i = 1; i < 8; i++)
		{
			if (screenSpaceCorners[i].x < minX)
			{
				minX = screenSpaceCorners[i].x;
			}
			if (screenSpaceCorners[i].y < minY)
			{
				minY = screenSpaceCorners[i].y;
			}
			if (screenSpaceCorners[i].x > maxX)
			{
				maxX = screenSpaceCorners[i].x;
			}
			if (screenSpaceCorners[i].y > maxY)
			{
				maxY = screenSpaceCorners[i].y;
			}
		}

		return Rect.MinMaxRect(minX, minY, maxX, maxY);
	}
}
