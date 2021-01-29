using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IState
{
    private BombController _controller;
    private LineRenderer _lineRenderer;

    private readonly float lifeTime = 1;
    private readonly float height = 1f;
    private float gravity;
    private Transform _start;
    private readonly int resolution = 15;
    private readonly string Path = "Hips_jnt/Spine_jnt/Spine_jnt 1/Chest_jnt/Shoulder_Right_jnt/Arm_Right_jnt/Forearm_Right_jnt/Hand_Right_jnt/BombPos";

    public void Action()
    {
        BombAction();
    }

    public void Init()
    {
        _controller = GetComponent<BombController>();
        _lineRenderer = GetComponent<LineRenderer>();

        _start = transform.Find(Path);
        gravity = Physics.gravity.y;
    }

    private void BombAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);

        if (ground.Raycast(ray, out float rayDistance))
        {
            Vector3 pos = ray.GetPoint(rayDistance);
            _lineRenderer.enabled = true;
            DrawLine(pos);
        }
    }

    private void DrawLine(Vector3 pos)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = pos;

        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        Vector3 initVelocity = velocityY + velocityXZ * -Mathf.Sign(gravity);

        for (int i = 0; i <= resolution; ++i)
        {
            float simTime = i / (float)resolution * time;
            Vector3 displacement = initVelocity * simTime + Vector3.up * gravity * simTime * simTime / 2f;
            Vector3 position = startPos + displacement;
            _lineRenderer.SetPosition(i, position);
        }
    }

    private IEnumerator IEBomb(Vector3 pos)
    {
        GameObject bomb = PoolMng.Instance.Pop(PoolType.ItemPool, "Grenade");
        bomb.gameObject.SetActive(true);
        bomb.transform.position = _start.transform.position;
        Vector3 startPos = bomb.transform.position;
        Vector3 targetPos = pos;

        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        Vector3 finalVelocity = velocityY + velocityXZ * -Mathf.Sign(gravity);

        bomb.GetComponent<Rigidbody>().velocity = finalVelocity;

        yield return new WaitForSeconds(lifeTime);

        if (bomb != null)
            bomb.GetComponent<Grenade>().Detonate();

        Destroy(bomb.gameObject);
        PoolMng.Instance.Push(PoolType.ItemPool, bomb, "Grenage");
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
