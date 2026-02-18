using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T> {
	public const bool ASCENDING = true;
    public const bool DESCENDING = false;

    List<KeyValuePair<T, float>> heap = new List<KeyValuePair<T, float>>();
	bool order;

    public int Count {
		get {
			return heap.Count;
		}
	}

	public PriorityQueue(bool order = ASCENDING) {
        this.order = order;
    }

    public void Enqueue(T item, float priority) {
        heap.Add(new KeyValuePair<T, float>(item, priority));
        HeapifyUp(heap.Count - 1);
    }

    public T Dequeue() {
        if(heap.Count == 0) throw new InvalidOperationException("Priority queue is empty.");
        T root = heap[0].Key;
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        if(heap.Count > 0) HeapifyDown(0);
        return root;
    }

    public T Peek() {
        if(heap.Count == 0) throw new InvalidOperationException("Priority queue is empty.");
        return heap[0].Key;
    }

    public List<T> ToList() {
        List<T> values = new List<T>();
        PriorityQueue<T> pq = this;
        while(pq.Count > 0) values.Add(pq.Dequeue());
        return values;
    }

	bool Compare(float a, float b) {
        if(order == ASCENDING) return a < b;
        else return a > b;
    }

    void HeapifyUp(int index) {
        while(index > 0) {
            int parent = (index - 1) / 2;
            if(!Compare(heap[index].Value, heap[parent].Value)) break;
            Swap(index, parent);
            index = parent;
        }
    }

    void HeapifyDown(int index) {
        int lastIndex = heap.Count - 1;
        while(true) {
            int left = index * 2 + 1;
            int right = index * 2 + 2;
            int best = index;
            if(left <= lastIndex && Compare(heap[left].Value, heap[best].Value))
                best = left;
            if(right <= lastIndex && Compare(heap[right].Value, heap[best].Value))
                best = right;
            if(best == index) break;
            Swap(index, best);
            index = best;
        }
    }

    void Swap(int i, int j) {
        var temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}

