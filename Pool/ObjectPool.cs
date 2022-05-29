using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
	public class ObjectPool<T> where T:new()
	{
		private static ObjectPool<T> mInstance = null;

		public static ObjectPool<T> Instance
        {
            get
            {
				if(mInstance == null)
                {
					mInstance = new ObjectPool<T>();

					mInstance.Init(0);
                }

				return mInstance;
            }
        }

		private List<ObjectPoolContainer<T>> list;
		private Dictionary<T, ObjectPoolContainer<T>> InUsing;
		private int lastIndex = 0;

		public ObjectPool()
        {

        }

		public void Init(int initialSize)
		{
			list = new List<ObjectPoolContainer<T>>(initialSize);
			InUsing = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

			Warm(initialSize);
		}

		private void Warm(int capacity)
		{
			for (int i = 0; i < capacity; i++)
			{
				CreateContainer();
			}
		}

		private ObjectPoolContainer<T> CreateContainer()
		{
			var container = new ObjectPoolContainer<T>();
			container.Item = new T();
			list.Add(container);
			return container;
		}

		public T GetItem()
		{
			ObjectPoolContainer<T> container = null;
			for (int i = 0; i < list.Count; i++)
			{
				lastIndex++;
				if (lastIndex > list.Count - 1) lastIndex = 0;

				if (list[lastIndex].Used)
				{
					continue;
				}
				else
				{
					container = list[lastIndex];
					break;
				}
			}

			if (container == null)
			{
				container = CreateContainer();
			}

			container.Consume();
			InUsing.Add(container.Item, container);
			return container.Item;
		}

		public void ReleaseItem(object item)
		{
			ReleaseItem((T)item);
		}

		public void ReleaseItem(T item)
		{
			if (InUsing.ContainsKey(item))
			{
				var container = InUsing[item];
				container.Release();
				InUsing.Remove(item);
			}
			else
			{
				Debug.LogWarning("This object pool does not contain the item provided: " + item);
			}
		}

		public int Count
		{
			get { return list.Count; }
		}

		public int CountUsedItems
		{
			get { return InUsing.Count; }
		}
	}
}

