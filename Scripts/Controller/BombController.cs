using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    //private Grenade _bombPrefab;
    private readonly float lifeTime = 1;
    private readonly float height = 1f;
    private float gravity;
    private Transform _start;
    private readonly int resolution = 15;
    public LineRenderer line;

    private InventoryUI _inventory;
    private Rigidbody _rigidbody;
    private TrailRenderer _trail;

    public void Init()
    {
        _start = transform.Find("Hips_jnt/Spine_jnt/Spine_jnt 1/Chest_jnt/Shoulder_Right_jnt/Arm_Right_jnt/Forearm_Right_jnt/Hand_Right_jnt/BombPos");
        gravity = Physics.gravity.y;

        line = GetComponent<LineRenderer>();
        line.material = Resources.Load<Material>("Material/BombTrace");
        line.startColor = new Color(1, .92f, .016f, .1f);
        line.endColor = new Color(1, 1, 1, .3f);
        line.startWidth = .01f;
        line.endWidth = .2f;
        line.positionCount = resolution + 1;
        line.numCornerVertices = 90;
        line.numCapVertices = 4;
        line.enabled = false;

        _inventory = FindObjectOfType<InventoryUI>();
        InitGrenade();
    }

    private void InitGrenade()
    {
        GameObject grenade = PoolMng.Instance.Pop(PoolType.ItemPool, "Grenade");
        Item item = grenade.GetComponent<Item>();
        item._itemCount = 3;
        _inventory.AddItem(item);
        item._itemCount = 1;
        PoolMng.Instance.Push(PoolType.ItemPool, grenade, "Grenade");
    }

    public IEnumerator IEBomb(Vector3 pos)
    {
        // Enter
        GameObject bomb = PoolMng.Instance.Pop(PoolType.ProjectilePool, "GrenadeThrow");
        bomb.gameObject.SetActive(true);
        bomb.transform.position = _start.transform.position;

        _trail = bomb.GetComponent<TrailRenderer>();
        _trail.enabled = true;

        _rigidbody = bomb.GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;

        // Update
        Vector3 startPos = bomb.transform.position;
        Vector3 targetPos = pos;

        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        Vector3 finalVelocity = velocityY + velocityXZ * -Mathf.Sign(gravity);

        _rigidbody.velocity = finalVelocity;

        yield return new WaitForSeconds(lifeTime);

        if (bomb != null)
            bomb.GetComponent<Grenade>().Detonate();

        // Exit
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.useGravity = false;
        _trail.enabled = false;

        bomb.transform.position = Vector3.zero;
        PoolMng.Instance.Push(PoolType.ProjectilePool, bomb, "GrenadeThrow");
    }

    public void DrawLine(Vector3 pos)
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
            line.SetPosition(i, position);
        }
    }
}
