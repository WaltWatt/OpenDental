using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase {
	///<summary>Creates a thread-safe queue with a size limit. 
	///The queue will automatically dequeue any items in excess of the size limit when a new item is enqueued.</summary>
	public class LimitedSizeQueue<T>:IEnumerable<T> {
		public int Limit;
		protected ConcurrentQueue<T> _queue;

		public LimitedSizeQueue(int limit) {
			_queue=new ConcurrentQueue<T>();
			Limit=limit;
		}
		///<summary>Adds an item to the queue and automatically dequeues any items in excess of the specified size limit.
		///Returns true if successful.</summary>
		public bool Enqueue(T item) {
			if(_queue.Contains(item)) {
				return false;
			}
			while(_queue.Count>=Limit) {
				T dequeued;
				if(!_queue.TryDequeue(out dequeued)) {
					return false;
				}
			}
			_queue.Enqueue(item);
			return true;
		}

		///<summary>Adds an item to the queue and automatically dequeues any items in excess of the specified size limit.
		///Returns true if successful. If an item is dequeued, returns the object as an out parameter.</summary>
		public bool Enqueue(T item, out T dequeued) {
			//there might be no value to dequeue, if the size is still smaller than the limit
			dequeued=default(T); //Default(T) returns 0 for numeric types, null for reference types. For structs, it returns a struct with each member initialized to its default
			if(_queue.Count>=Limit && !_queue.TryDequeue(out dequeued)) { //only try to dequeue if the count exceeds the size limit
				return false;
			}
			_queue.Enqueue(item);
			return true;
		}

		///<summary>Tries to dequeue an item. Returns true if successful.</summary>
		public bool Dequeue() {
			T dequeued;
			return _queue.TryDequeue(out dequeued);
		}

		///<summary>Tries to dequeue an item. Returns true if successful. Returns the dequeued object as an out parameter.</summary>
		public bool Dequeue(out T dequeued) {
			return _queue.TryDequeue(out dequeued);
		}

		///<summary>See if the queue contains an item/</summary>
		public bool Contains(T item) {
			return _queue.Contains(item);
		}

		public List<T> ToList() {
			return _queue.ToList();
		}

		///<summary>Implement IEnumerator to allow iteration.</summary>
		public IEnumerator<T> GetEnumerator() {
			return _queue.GetEnumerator();
		}

		///<summary>IEnumerator[T] inherits from IEnumerable and expects IEnumerable.GetEnumerator. Delegates to the generic implementation."/></summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
