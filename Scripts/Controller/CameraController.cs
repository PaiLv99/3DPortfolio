using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public enum CameraState { Follow, Shake, }

    public  bool _hasTarget;
    private Player _player;
    [SerializeField]
    private Transform _target;
    private Camera _camera;
    private float _smoothTime = 0.001f;
    private bool _isShaking = false;
    private Vector3 _camerapos;
    private Quaternion _camerarotate;
    private Quaternion _targetrotation;
    private float _camerasize;

    public SelectCharater _selectChar;

    public CameraState _cameraState = CameraState.Follow;

    public void CharInit()
    {
        _player = FindObjectOfType<Player>();
        _target = _player.GetComponent<Transform>();
    }

    private void FollowCamera()
    {
        Vector3 refVel = Vector3.zero;
        Vector3 targetPos = _target.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref refVel, _smoothTime);
    }

    public void Shake()
    {
        if (!_isShaking)
        {
            StartCoroutine(IEShake());
        }
    }

    private IEnumerator IEShake()
    {
        Vector3 originRot = transform.eulerAngles;

       _isShaking = true;

        float elapsedTime = 0f;
        float duration = .2f;

        while(elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            Vector3 rot = originRot + new Vector3(x, y, z);
            Quaternion qRot = Quaternion.Euler(rot);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, qRot, Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isShaking = false;
        transform.rotation = Quaternion.Euler(originRot);
        _cameraState = CameraState.Follow;
    }

    /*
    private void CameraPan()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * Time.deltaTime * mouseX * 100);
    }
    */

    public void Init()
    {
        _camera = FindObjectOfType<Camera>();

        _camerapos = _camera.transform.position;
        _camerarotate = _camera.transform.rotation;
        _camerasize = _camera.orthographicSize;
        _targetrotation = Quaternion.Euler(new Vector3(10, 0, 0));
    }

    public void CutSceneCamera(Vector3 position)
    {
        StartCoroutine(IECameraMoveV(new Vector3(0, 2.5f, -10), 2));
        StartCoroutine(IECameraMoveH(new Vector3(10,0,0), position, 2));
        StartCoroutine(IECameraRotation(_camerarotate, _targetrotation, 2));
        StartCoroutine(IECameraZoom(5f, 1.2f, 2));
    }

    private void SelectPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        _selectChar = FindObjectOfType<SelectCharater>();

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, 1 << 13))
        {
            Vector3 pos = hit.transform.position;
            _selectChar._selectPlayer = hit.transform.gameObject;
            _selectChar.Open();
            StartCoroutine(IECameraMoveV(new Vector3(0, 2.5f, -10)));
            StartCoroutine(IECameraMoveH(transform.position, pos));
            StartCoroutine(IECameraRotation(_camerarotate, _targetrotation));
            StartCoroutine(IECameraZoom(5f, 1.5f));
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !_hasTarget)
        {
            // UI뒤에 있는 오브젝트 무시하기
            if (!EventSystem.current.IsPointerOverGameObject())
                SelectPlayer();
        }
    }

    void LateUpdate()
    {
        if (_target != null)
        {
            switch(_cameraState)
            {
                case CameraState.Follow:
                    FollowCamera();
                    break;
                //case CameraState.Shake:
                    //Shake();
                    //CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
                   //break;
            }
            //if (Input.GetMouseButton(2))
                //CameraPan();
        }
    }

    #region 카메라 워킹관련

    public void ClearCameraTransform()
    {
        StartCoroutine(IECameraMoveV(_camerapos ));
        StartCoroutine(IECameraMoveH(transform.position, Vector3.zero));
        StartCoroutine(IECameraRotation(_camera.transform.rotation, _camerarotate));
        StartCoroutine(IECameraZoom(_camera.orthographicSize, _camerasize));
    }

    private IEnumerator IECameraZoom(float start, float end, float speed = 3)
    {
        float interpolation = 0;
        while (interpolation <= 1)
        {
            interpolation += Time.deltaTime * speed;
            _camera.orthographicSize = Mathf.Lerp(start, end, interpolation);
            yield return null;
        }
    }

    private IEnumerator IECameraRotation(Quaternion start, Quaternion end, float speed = 3)
    {
        float interpolation = 0;
        while (interpolation <= 1)
        {
            interpolation += Time.deltaTime * speed;
            Quaternion angle = Quaternion.Lerp(start, end, interpolation);
            _camera.transform.rotation = angle;
            yield return null;
        }
    }

    private IEnumerator IECameraMoveV(Vector3 target, float speed = 3)
    {
        float interpolation = 0;
        Vector3 start = _camera.transform.localPosition;

        while (interpolation <= 1)
        {
            interpolation += Time.deltaTime * speed;
            _camera.transform.localPosition = Vector3.Lerp(start, target, interpolation);
            yield return null;
        }
    }

    private IEnumerator IECameraMoveH(Vector3 start, Vector3 target, float speed = 3)
    {
        float interpolation = 0;

        while (interpolation <= 1)
        {
            interpolation += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, target, interpolation);
            yield return null;
        }
    }
    #endregion
}
