using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour{
	public GameObject prefab;
	public int size;
	private Queue<GameObject> pool;

	void Awake() {
		Init();
	}

	public void Init() {
		pool = new Queue<GameObject>();
		for(int i = 0; i < size; i++)
			ExpandPool();
	}

	public void Init(GameObject prefab, int size) {
		this.prefab = prefab;
		this.size = size;
		pool = new Queue<GameObject>();
		for(int i = 0; i < size; i++)
			ExpandPool();
	}

	public GameObject Get() {
		if(pool.Count == 0) ExpandPool();
		GameObject obj = pool.Dequeue();
		obj.SetActive(true);
		return obj;
	}
	
	public GameObject Get(Transform parent) {
		if(pool.Count == 0) ExpandPool();
		GameObject obj = pool.Dequeue();
		obj.SetActive(true);
		obj.transform.SetParent(parent);
		return obj;
	}

	public GameObject Get(Vector3 position, Quaternion rotation) {
		if(pool.Count == 0) ExpandPool();
		GameObject obj = pool.Dequeue();
		obj.SetActive(true);
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		return obj;
	}

	public GameObject Get(Vector3 position, Quaternion rotation, Transform parent) {
		if(pool.Count == 0) ExpandPool();
		GameObject obj = pool.Dequeue();
		obj.SetActive(true);
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		obj.transform.SetParent(parent);
		return obj;
	}

	public GameObject Get(TransformData data) {
		if(pool.Count == 0) ExpandPool();
		GameObject obj = pool.Dequeue();
		obj.SetActive(true);
		obj.transform.position = data.position;
		obj.transform.rotation = data.rotation;
		obj.transform.SetParent(data.parent);
		return obj;
	}

	//Ensure the GameObject belongs to this pool before calling it!!!
	public void Release(GameObject obj) {
		if(obj == null) return;
		SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
		if(sp != null) sp.sprite = null;
		obj.transform.SetParent(transform);
		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
		obj.SetActive(false);
		pool.Enqueue(obj);
	}

	void ExpandPool() {
		GameObject obj = Instantiate(prefab, transform);
		obj.SetActive(false);
		pool.Enqueue(obj);
	}
}
