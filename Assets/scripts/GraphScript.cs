using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphScript : MonoBehaviour
{
    [SerializeField]
    private GameObject graphBg;
    [SerializeField]
    private GameObject graphCircleSolid;
    [SerializeField]
    private GameObject graphCircleCurrent;
    [SerializeField]
    private GameObject graphLine;

    [SerializeField]
    private RectTransform canvasRect;
    private float magicDivider;

    [SerializeField]
    private float horPadding;
    [SerializeField]
    private float vertPadding;

    [SerializeField]
    private float maxYValue;
    private bool usingMaxYValue;

    private float width;
    private float height;

    private float distBetweenPoints;
    private float heightRatio;

    private List<float> points;
    private List<Vector2> vPoints;

    private List<GameObject> renderedObjects;

    public void Start()
    {
        renderedObjects = new List<GameObject>();

        RectTransform rt = GetComponent<RectTransform>();
        Vector2 dims = rt.sizeDelta;
        width = dims.x - (2 * horPadding);
        height = dims.y - (2 * vertPadding);

        distBetweenPoints = width;

        points = new List<float>{};
        vPoints = new List<Vector2>();

        if (points.Count > 1)
            distBetweenPoints = width / (points.Count - 1);
        else
            distBetweenPoints = width;

        if (maxYValue > 0)
            usingMaxYValue = true;

        // Work out by what value to divide line lengths... Depends on dimensions
        // of the UI Canvas
        // 2.35f when canvas scale 0.4271
        // 1.32f when              0.7521
        // dy/dx = (1.32 - 2.35) / (0.7521 - 0.4271) = -3.1692
        // -3.1692 * 0.4271 + b = 2.35
        // 2.35 + (3.1692 * 0.4271) = b = 3.7036
        magicDivider = (-3.1692f * canvasRect.localScale.x) + 3.7036f;

        RedrawGraph();
    }

    public void AddPoint(float point)
    {
        points.Add(point);

        RedrawGraph();

        distBetweenPoints = width / points.Count;
    }

    public void SetMaxYValue(float val)
    {
        maxYValue = val;
        if (maxYValue > 0)
            usingMaxYValue = true;
        else
            usingMaxYValue = false;
    }

    public void ChangeLastPoint(float newHeight)
    {
        points[points.Count - 1] = newHeight;
        RedrawGraph();
    }

    private void RedrawGraph()
    {
        // TODO: Could be optimised to reuse already instantiated GOs, and
        //       making more/destroying any extras
        // Destroy rendered objects
        ClearRenderedObjects();
        CalcHeightRatio();
        CalculateVectorPoints();

        for (int i = 1; i < points.Count; i++)
        {
            DrawLine(vPoints[i - 1], vPoints[i]);
        }
        for (int i = 0; i < vPoints.Count; i++)
        {
            DrawPoint(vPoints[i].x, vPoints[i].y, i == vPoints.Count - 1);
        }
    }

    private void CalcHeightRatio()
    {
        if (!usingMaxYValue)
        {
            float maxHeight = 0;
            foreach (float h in points)
            {
                if (h > maxHeight)
                {
                    maxHeight = h;
                }
            }

            if (maxHeight > 0)
                heightRatio = height / maxHeight;
            else
                heightRatio = 1;
        }
        else
        {
            heightRatio = height / maxYValue;
        }
    }

    private void DrawPoint(float x, float y, bool current)
    {
        GameObject cPoint;
        if (current)
            cPoint = Instantiate(graphCircleCurrent);
        else
            cPoint = Instantiate(graphCircleSolid);


        renderedObjects.Add(cPoint);
        cPoint.transform.SetParent(graphBg.transform);

        RectTransform rect = cPoint.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(x, y);
    }

    private void DrawLine(Vector2 p1, Vector2 p2)
    {
        // GameObject line = Instantiate(graphLine);
        GameObject line = new GameObject("Line", typeof(RectTransform));
        RawImage image = line.AddComponent<RawImage>();
        image.color = new Color(0, 0, 0, 0.5f);

        renderedObjects.Add(line);
        line.transform.SetParent(graphBg.transform);

        RectTransform rect = line.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        Vector2 pos = new Vector2((p1.x + p2.x) / 2f, (p1.y + p2.y) / 2f);
        rect.anchoredPosition = pos;

        // Dividing by magic value to make lines the correct length...
        float length = Vector2.Distance(p1, p2) / magicDivider;
        rect.sizeDelta = new Vector2(length, 2);

        Vector2 dir = (p2 - p1).normalized;
        float ang = Vector2.SignedAngle(new Vector2(1, 0), dir);
        rect.localEulerAngles = new Vector3(0, 0, ang);
    }

    private void CalculateVectorPoints()
    {
        vPoints.Clear();
        float xOffset = horPadding;
        for (int i = 0; i < points.Count; i++)
        {
            vPoints.Add(new Vector2(xOffset, vertPadding + heightRatio * points[i]));
            xOffset += distBetweenPoints;
        }
    }

    private void ClearRenderedObjects()
    {
        foreach (GameObject go in renderedObjects)
        {
            Destroy(go);
        }

        renderedObjects.Clear();
    }
}
