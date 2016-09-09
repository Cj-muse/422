﻿using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace CS422	
{
	public class ThreadPoolSleepSorter : IDisposable
	{
		private TextWriter _textWriter;
		private BlockingCollection<byte> _collection;
		private ushort _threadCount;
		private Thread[] _readyThreadPool;


		public ThreadPoolSleepSorter ()
		{}

		public ThreadPoolSleepSorter(TextWriter output, ushort threadCount)
		{
			_collection = new BlockingCollection<byte>();
			_textWriter = output;

			if (threadCount == 0) 
			{_threadCount = 64;	}
			else
			{_threadCount = threadCount;}

			// Create and start all threads
			_readyThreadPool = new Thread[_threadCount];
			for (int i = 0; i < _threadCount; i++)
			{
				_readyThreadPool [i] = new Thread(() => ThreadWorkFunc());
				_readyThreadPool [i].Start ();
			}

		}

		public void Sort(byte[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				//awaken thread
				_collection.Add(values[i]);
			}			
		}

		// ThreadWorkFunc (performs sleeping and displaying of values)
		void ThreadWorkFunc() 
		{
			byte value = 0;
			while (true) {
				value = _collection.Take ();
				Thread.Sleep (value*1000);
				_textWriter.WriteLine (value);
			}
		}

		public void Dispose()
		{				
			Dispose();
			GC.SuppressFinalize(this);
		}
	}
}

