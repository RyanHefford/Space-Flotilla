using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * This class is used for optimization when we have
 * multiple agents wanting to do path finding. This will
 * optimize it by NOT making all the calls every frame it
 * will spread it out so that few are called each frame
 * This will make sure that it does NOT lag or cause frame drops.
 * 
 */

public class PathRequestManager : MonoBehaviour
{
	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;

	static PathRequestManager instance;
	Pathfinding pathfinding;

	bool isProcessingPath;

	void Awake()
	{
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
	}

	public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
	{
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

	void TryProcessNext()
	{
		if (!isProcessingPath && pathRequestQueue.Count > 0)
		{
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathfinding.startFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	public void FinishedProcessingPath(Vector2[] path, bool success)
	{
		currentPathRequest.callback(path, success);
		isProcessingPath = false;
		TryProcessNext();
	}

	struct PathRequest
	{
		public Vector2 pathStart;
		public Vector2 pathEnd;
		public Action<Vector2[], bool> callback;

		public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback)
		{
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}

	}
}
