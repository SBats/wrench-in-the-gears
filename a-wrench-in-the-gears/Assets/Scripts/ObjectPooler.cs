using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem {
	public GameObject objectToPool;
	public int amountToPool;
	public bool shouldExpand;
}

public class ObjectPooler : MonoBehaviour {
	public static ObjectPooler SharedInstance;
	public List<ObjectPoolItem> itemsToPool;
	[HideInInspector]
	public List<GameObject> pooledObjects;

	// Life Cycles

	private void Awake() {
		SharedInstance = this;
	}

	private void Start() {
		this.InitPooledObjectsList();
	}

	// Public

	public GameObject GetPooledObject(string tag) {
		for (int i = 0; i < this.pooledObjects.Count; i++) {
			GameObject currentObject = this.pooledObjects[i];
			if (!currentObject.activeInHierarchy && currentObject.tag == tag) {
				return currentObject;
			}
		}
		foreach (ObjectPoolItem item in itemsToPool) {
			if (item.objectToPool.tag == tag && item.shouldExpand) {
				GameObject obj = this.InstantiateInactiveObject(item);
				pooledObjects.Add(obj);
				return obj;
			}
		}
		return null;
	}

	// Private
	private void InitPooledObjectsList() {
		this.pooledObjects = new List<GameObject>();
		foreach (ObjectPoolItem item in this.itemsToPool) {
			for (int i = 0; i < item.amountToPool; i++) {
				GameObject obj = this.InstantiateInactiveObject(item);
				pooledObjects.Add(obj);
			}
		}
	}

	private GameObject InstantiateInactiveObject(ObjectPoolItem item) {
		GameObject obj = Instantiate(item.objectToPool);
		obj.SetActive(false);
		return obj;
	}
}
