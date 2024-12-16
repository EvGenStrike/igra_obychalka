using System.Collections.Generic;
using UnityEngine;

public class ArrowTipGenerator : MonoBehaviour
{
    public float tipLength = 0.2f; // ����� �����������
    public float tipWidth = 0.1f;  // ������ �����������
    public Material arrowTipMaterial; // �������� ��� ����������� ������
    public string colorHEX = "#0FFD36";
    public float arrowBaseOffset = 0.5f; // ����� ������ ������� �� ����� �����

    public void DrawArrowTip(LineRenderer lineRenderer, Node startNode, Node endNode)
    {
        if (lineRenderer == null || lineRenderer.positionCount < 2)
        {
            Debug.LogWarning("LineRenderer ������ ����� ��� ������� 2 �����.");
            return;
        }

        // �������� ��������� ��� �������� �����
        Vector3 endPoint = startNode.transform.position;
        Vector3 secondToLastPoint = endNode.transform.position;

        // ������������ ����������� � ���������� �����������
        Vector3 direction = (endPoint - secondToLastPoint).normalized;
        Vector3 right = Vector3.Cross(direction, Vector3.forward).normalized;

        // ���������� ����������� �����������
        List<Vector3> vertices = new List<Vector3>
        {
            endPoint - direction * arrowBaseOffset, // �������� ������ �����������
             endPoint - direction * (tipLength + arrowBaseOffset) + right * (tipWidth / 2), // ����� ���� ��������
            endPoint - direction * (tipLength + arrowBaseOffset) - right * (tipWidth / 2)  // ������ ���� ��������
        };

        List<int> triangles = new List<int> { 0, 1, 2 };

        // ������� ������ ��� ����������� ������
        GameObject arrowTipObject = new GameObject("ArrowTip");
        arrowTipObject.transform.SetParent(lineRenderer.transform);

        MeshFilter meshFilter = arrowTipObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = arrowTipObject.AddComponent<MeshRenderer>();

        // ������������� �������� ��� �����������
        if (arrowTipMaterial != null)
            meshRenderer.material = arrowTipMaterial;

        Color colorFromHex;
        if (ColorUtility.TryParseHtmlString(colorHEX, out colorFromHex))
        {
            meshRenderer.material.color = colorFromHex;
        }

        // ������� ��� ��� �����������
        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };
        mesh.RecalculateNormals();

        // ��������� ��� � �������
        meshFilter.mesh = mesh;
    }
}
