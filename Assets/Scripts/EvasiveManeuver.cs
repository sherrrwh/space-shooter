using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 控制Enemy在X轴移动
public class EvasiveManeuver : MonoBehaviour {

	public float dodge;
	public float smoothing;
	public float tilt;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	public Boundary boundary;

	private float currentSpeed;
	private float targetManeuver;
	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
		// 记录一开始的z轴速度
		currentSpeed = rb.velocity.z;
		// StartCoroutine是执行一个需要等待的函数
		StartCoroutine(Evade());
	}
	// 需要等待的函数就用IEnumerator
	IEnumerator Evade() {
		// xxx return是等待
		// 随机等待startWait.x - startWait.y秒
		yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

		while (true) {
			targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(GetComponent<Transform>().position.x);
			yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
		}
	}
	// 每0.02s执行一次
	void FixedUpdate() {
		// 游戏里有两种更新频率，一种是每帧更新，一种是个固定时长更新
		// 物理引擎是固定时长更新（2个原因）
		float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
		rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
		// 限制Enemy不要飞出场景
		rb.position = new Vector3
		(
			Mathf.Clamp(rb.position.x, boundary.xmin, boundary.xmax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zmin, boundary.zmax)
		);
		//讲一个欧拉角转换成四元数
		rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt); // 倾斜角度
	}
}
