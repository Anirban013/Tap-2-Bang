using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FollowPath : MonoBehaviour {

	public enum FollowType
	{
		MoveTorwards,
		Lerp
	}
	public FollowType type = FollowType.MoveTorwards;
	public PathDefinition path;
	public static float speed = 3f;
	public float MaxDToGoal = 0.1f;
	private IEnumerator<Transform>_currPoint;

	public void Start(){
		if (path == null) {
			Debug.LogError ("Path cannot be null!!!!");
			return;
		}
		_currPoint = path.GetPathEnumerator ();
		_currPoint.MoveNext ();
		if (_currPoint.Current == null)
			return;
		transform.position = _currPoint.Current.position;

	}
	public void Update(){
		if (_currPoint == null || _currPoint.Current == null)
			return;
		if (type == FollowType.MoveTorwards)
			transform.position = Vector3.MoveTowards (transform.position, _currPoint.Current.position, Time.deltaTime * speed);
		else if(type == FollowType.Lerp)
			transform.position = Vector3.Lerp (transform.position, _currPoint.Current.position, Time.deltaTime * speed);

		var distanceSquared = (transform.position - _currPoint.Current.position).sqrMagnitude;
		if (distanceSquared < MaxDToGoal * MaxDToGoal)
			_currPoint.MoveNext ();
	}
}
