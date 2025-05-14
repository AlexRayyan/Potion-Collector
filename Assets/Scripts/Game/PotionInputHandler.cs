using UnityEngine;

public class PotionInputHandler : MonoBehaviour
{
    [Header("Raycast Settings")]
    public LayerMask potionLayer;
    public float maxRayDistance = 100f;
    public int maxHits = 4;

    Camera _cam;
    RaycastHit[] _hitBuffer;

    void Awake()
    {
        _cam = Camera.main;
        _hitBuffer = new RaycastHit[maxHits];
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            ProcessTap(Input.mousePosition);
        }
#else
        // Process touch input on mobile
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ProcessTap(touch.position);
            }
        }
#endif
    }

    void ProcessTap(Vector2 screenPos)
    {
        Ray ray = _cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, potionLayer))
        {
            var potion = hit.collider.GetComponent<Potion>();

            if (potion != null && potion.gameObject.activeSelf)
            {
                potion.Collect();
            }
        }
    }
}
