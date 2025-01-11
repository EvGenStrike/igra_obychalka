using System.Collections.Generic;
using UnityEngine;

public class ArrowTipGenerator : MonoBehaviour
{
    public float tipLength = 0.2f; // Длина наконечника
    public float tipWidth = 0.1f;  // Ширина наконечника
    public Material arrowTipMaterial; // Материал для наконечника стрелы
    public string colorHEX = "#0FFD36";
    public float arrowBaseOffset = 0.5f; // Сдвиг начала стрелки от конца линии

    public void DrawArrowTip(LineRenderer lineRenderer, Node startNode, Node endNode)
    {
        if (lineRenderer == null || lineRenderer.positionCount < 2)
        {
            Debug.LogWarning("LineRenderer должен иметь как минимум 2 точки.");
            return;
        }

        // Получаем последние два сегмента линии
        Vector3 endPoint = startNode.transform.position;
        Vector3 secondToLastPoint = endNode.transform.position;

        // Рассчитываем направление и ориентацию наконечника
        Vector3 direction = (endPoint - secondToLastPoint).normalized;
        Vector3 right = Vector3.Cross(direction, Vector3.forward).normalized;

        // Генерируем треугольник наконечника
        List<Vector3> vertices = new List<Vector3>
        {
            endPoint - direction * arrowBaseOffset, // Сдвигаем начало наконечника
             endPoint - direction * (tipLength + arrowBaseOffset) + right * (tipWidth / 2), // Левый угол сдвигаем
            endPoint - direction * (tipLength + arrowBaseOffset) - right * (tipWidth / 2)  // Правый угол сдвигаем
        };

        List<int> triangles = new List<int> { 0, 1, 2 };

        // Создаем объект для наконечника стрелы
        GameObject arrowTipObject = new GameObject("ArrowTip");
        arrowTipObject.transform.SetParent(lineRenderer.transform);

        MeshFilter meshFilter = arrowTipObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = arrowTipObject.AddComponent<MeshRenderer>();

        // Устанавливаем материал для наконечника
        if (arrowTipMaterial != null)
            meshRenderer.material = arrowTipMaterial;

        Color colorFromHex;
        if (ColorUtility.TryParseHtmlString(colorHEX, out colorFromHex))
        {
            meshRenderer.material.color = colorFromHex;
        }

        // Создаем меш для наконечника
        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };
        mesh.RecalculateNormals();

        // Применяем меш к объекту
        meshFilter.mesh = mesh;
    }
}
