using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float Speed = 10;
	public Vector3 Direction = Vector3.up;

	void Update()
	{
		transform.Rotate(Direction * Speed * Time.deltaTime);
	}
}