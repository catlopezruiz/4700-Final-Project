using UnityEngine;

public class TimingBarUI : MonoBehaviour
{
    public RectTransform barRect;
    public RectTransform markerRect;
    public RectTransform hitZoneRect;

    public float markerSpeed = 300f;

    public float maxHitZoneWidth = 220f;
    public float minHitZoneWidth = 120f;

    private bool isActive = false;
    private float direction = 1f;

    void Start()
    {
        SetHitZoneWidth(maxHitZoneWidth);
        HideBar();
    }

    void Update()
    {
        if (!isActive) return;
        MoveMarker();
    }

    void MoveMarker()
    {
        Vector2 pos = markerRect.anchoredPosition;
        pos.x += direction * markerSpeed * Time.deltaTime;

        float halfBarWidth = barRect.rect.width / 2f;
        float halfMarkerWidth = markerRect.rect.width / 2f;

        float leftLimit = -halfBarWidth + halfMarkerWidth;
        float rightLimit = halfBarWidth - halfMarkerWidth;

        if (pos.x >= rightLimit)
        {
            pos.x = rightLimit;
            direction = -1f;
        }
        else if (pos.x <= leftLimit)
        {
            pos.x = leftLimit;
            direction = 1f;
        }

        markerRect.anchoredPosition = pos;
    }

    public void ShowBar()
    {
        gameObject.SetActive(true);
        isActive = true;
        ResetMarker();
    }

    public void HideBar()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    public void ResetMarker()
    {
        direction = 1f;
        markerRect.anchoredPosition = new Vector2(-barRect.rect.width / 2f, markerRect.anchoredPosition.y);
    }

    public bool IsInHitZone()
    {
        float markerX = markerRect.anchoredPosition.x;
        float hitZoneX = hitZoneRect.anchoredPosition.x;
        float halfHitZoneWidth = hitZoneRect.rect.width / 2f;

        return markerX >= hitZoneX - halfHitZoneWidth &&
               markerX <= hitZoneX + halfHitZoneWidth;
    }

    public float GetMarkerNormalizedPosition()
    {
        float halfBarWidth = barRect.rect.width / 2f;
        float markerX = markerRect.anchoredPosition.x;

        return Mathf.InverseLerp(-halfBarWidth, halfBarWidth, markerX);
    }

    public float GetMarkerNormalizedPositionInHitZone()
    {
        float markerX = markerRect.anchoredPosition.x;
        float hitZoneX = hitZoneRect.anchoredPosition.x;
        float halfHitZoneWidth = hitZoneRect.rect.width / 2f;

        float leftEdge = hitZoneX - halfHitZoneWidth;
        float rightEdge = hitZoneX + halfHitZoneWidth;

        return Mathf.InverseLerp(leftEdge, rightEdge, markerX);
    }

    public void SetHitZoneWidth(float width)
    {
        width = Mathf.Clamp(width, minHitZoneWidth, maxHitZoneWidth);
        hitZoneRect.sizeDelta = new Vector2(width, hitZoneRect.sizeDelta.y);
    }

    public void SetMarkerSpeed(float speed)
    {
        markerSpeed = speed;
    }
}