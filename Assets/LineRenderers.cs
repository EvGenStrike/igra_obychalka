using UnityEngine;
using System.Collections.Generic;

public class LineRenderersManager : MonoBehaviour
{
    public GameObject nodesParent; // ������������ ������ ��� ���� ���
    private GameObject lineRenderersParent; // ����� ��� �������� LineRenderers
    public LineRendererProperties lineRendererProperties;

    // ������� ��� �������� ���� �����
    private Dictionary<(Node, Node), LineRenderer> lineRenderers = new Dictionary<(Node, Node), LineRenderer>();

    void Start()
    {
        // ���������, ���� �� ����� LineRenderers, ���� ��� � ������ �
        CreateLineRenderersFolder();
    }

    // ����� ��� �������� LineRenderers �����
    private void CreateLineRenderersFolder()
    {
        lineRenderersParent = GameObject.Find("LineRenderers");

        if (lineRenderersParent == null)
        {
            lineRenderersParent = new GameObject("LineRenderers");
            lineRenderersParent.transform.parent = nodesParent.transform; // ������ � �������� ��� nodesParent
        }
    }

    // ����� ��� ���������� LineRenderer � �����������
    public void AddLineRenderer(Node startNode, Node endNode)
    {
        // ��������, ��� ����� LineRenderers ����������
        if (lineRenderersParent == null)
        {
            CreateLineRenderersFolder();
        }

        // ���������, ���� ����� ��� ����������, �� ��������� � ������
        var key = (startNode, endNode);
        if (lineRenderers.ContainsKey(key))
        {
            Debug.LogWarning($"����� ����� {startNode.name} � {endNode.name} ��� ����������.");
            return;
        }

        // ������� ����� ������ ��� LineRenderer
        GameObject lineObj = new GameObject($"Line_{startNode.name}_to_{endNode.name}");
        lineObj.transform.parent = lineRenderersParent.transform; // ��������� ��� � �����

        // ��������� ��������� LineRenderer
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // ����������� ��������� LineRenderer
        lineRendererProperties.setupLineRenderer(lineRenderer);

        // ����������� ����� LineRenderer
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, startNode.transform.position);
        lineRenderer.SetPosition(1, endNode.transform.position);


        // ��������� ��������� LineRenderer � �������
        lineRenderers[key] = lineRenderer;
    }

    // ����� ��� ��������� ���� �����
    public Dictionary<(Node, Node), LineRenderer> GetLineRenderers()
    {
        return lineRenderers;
    }

    public List<LineRenderer> GetLineRenderersForNode(Node node)
    {
        List<LineRenderer> lineRenderersForNode = new List<LineRenderer>();

        // ���������� ��� ������ � �������
        foreach (var pair in lineRenderers)
        {
            // ���� ���� �� ��� � ������� ��������� � ���������� �����, ��������� LineRenderer � ������
            if (pair.Key.Item1 == node || pair.Key.Item2 == node)
            {
                lineRenderersForNode.Add(pair.Value);
            }
        }

        return lineRenderersForNode; // ���������� ������ ��������� LineRenderer
    }


    // ����� ��� ��������� ����� �� ���� ���
    public LineRenderer GetLineRenderer(Node startNode, Node endNode)
    {
        var key1 = (startNode, endNode);
        var key2 = (endNode, startNode);

        if (lineRenderers.TryGetValue(key1, out LineRenderer lineRenderer1))
        {
            return lineRenderer1;
        }
        else if (lineRenderers.TryGetValue(key2, out LineRenderer lineRenderer2))
        {
            return lineRenderer2;
        }
        return null;
    }

    public bool haveLine(Node startNode, Node endNode)
    {
        LineRenderer lineRenderer = GetLineRenderer(startNode, endNode);
        return lineRenderer != null;
    }

    public void SetPathLineRenderer(Node startNode, Node endNode)
    {
        LineRenderer lineRenderer = GetLineRenderer(startNode, endNode);
        if (lineRenderer == null)
        {
            return;
        }

        lineRenderer.colorGradient = lineRendererProperties.greenGradient;

        ArrowTipGenerator arrowTipGenerator = FindObjectOfType<ArrowTipGenerator>();
        if (arrowTipGenerator != null)
        {
            arrowTipGenerator.DrawArrowTip(lineRenderer, startNode, endNode);
        }
    }

    public void RemoveArrowTip(LineRenderer lineRenderer)
    {
        if (lineRenderer != null)
        {
            // ������� ������ ArrowTip ����� �������� ��������
            Transform arrowTipTransform = lineRenderer.transform.Find("ArrowTip");

            if (arrowTipTransform != null)
            {
                // ���������� ������ ArrowTip
                Destroy(arrowTipTransform.gameObject);
            }
        }
    }

    public void SetNormalLineRenderer(Node node)
    {
        var lineRenderersForNode = GetLineRenderersForNode(node);
        foreach (var lineRenderer in lineRenderersForNode)
        {
            lineRenderer.colorGradient = lineRendererProperties.lineGradient;
            RemoveArrowTip(lineRenderer);
        }
    }

    // ����� ��� �������� ����� ����� ����� ������
    public void RemoveLineRenderer(Node startNode, Node endNode)
    {
        var key = (startNode, endNode);
        if (lineRenderers.TryGetValue(key, out LineRenderer lineRenderer))
        {
            Destroy(lineRenderer.gameObject);
            lineRenderers.Remove(key);
        }
    }

    // ����� ��� �������� ���� �����
    public void ClearAllLineRenderers()
    {
        foreach (var lineRenderer in lineRenderers.Values)
        {
            Destroy(lineRenderer.gameObject);
        }
        lineRenderers.Clear();
    }
}
