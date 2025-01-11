using UnityEngine;
using System.Collections.Generic;

public class LineRenderersManager : MonoBehaviour
{
    public GameObject nodesParent; // Родительский объект для всех нод
    private GameObject lineRenderersParent; // Папка для хранения LineRenderers
    public LineRendererProperties lineRendererProperties;

    // Словарь для хранения всех линий
    private Dictionary<(Node, Node), LineRenderer> lineRenderers = new Dictionary<(Node, Node), LineRenderer>();

    void Start()
    {
        // Проверяем, есть ли папка LineRenderers, если нет — создаём её
        CreateLineRenderersFolder();
    }

    // Метод для создания LineRenderers папки
    private void CreateLineRenderersFolder()
    {
        lineRenderersParent = GameObject.Find("LineRenderers");

        if (lineRenderersParent == null)
        {
            lineRenderersParent = new GameObject("LineRenderers");
            lineRenderersParent.transform.parent = nodesParent.transform; // Делаем её дочерней для nodesParent
        }
    }

    // Метод для добавления LineRenderer к соединениям
    public void AddLineRenderer(Node startNode, Node endNode)
    {
        // Убедимся, что папка LineRenderers существует
        if (lineRenderersParent == null)
        {
            CreateLineRenderersFolder();
        }

        // Проверяем, если линия уже существует, не добавляем её заново
        var key = (startNode, endNode);
        if (lineRenderers.ContainsKey(key))
        {
            Debug.LogWarning($"Линия между {startNode.name} и {endNode.name} уже существует.");
            return;
        }

        // Создаем новый объект для LineRenderer
        GameObject lineObj = new GameObject($"Line_{startNode.name}_to_{endNode.name}");
        lineObj.transform.parent = lineRenderersParent.transform; // Добавляем его в папку

        // Добавляем компонент LineRenderer
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Настраиваем параметры LineRenderer
        lineRendererProperties.setupLineRenderer(lineRenderer);

        // Настраиваем точки LineRenderer
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, startNode.transform.position);
        lineRenderer.SetPosition(1, endNode.transform.position);


        // Добавляем созданный LineRenderer в словарь
        lineRenderers[key] = lineRenderer;
    }

    // Метод для получения всех линий
    public Dictionary<(Node, Node), LineRenderer> GetLineRenderers()
    {
        return lineRenderers;
    }

    public List<LineRenderer> GetLineRenderersForNode(Node node)
    {
        List<LineRenderer> lineRenderersForNode = new List<LineRenderer>();

        // Перебираем все записи в словаре
        foreach (var pair in lineRenderers)
        {
            // Если одна из нод в кортеже совпадает с переданной нодой, добавляем LineRenderer в список
            if (pair.Key.Item1 == node || pair.Key.Item2 == node)
            {
                lineRenderersForNode.Add(pair.Value);
            }
        }

        return lineRenderersForNode; // Возвращаем список найденных LineRenderer
    }


    // Метод для получения линии по паре нод
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
            // Находим объект ArrowTip среди дочерних объектов
            Transform arrowTipTransform = lineRenderer.transform.Find("ArrowTip");

            if (arrowTipTransform != null)
            {
                // Уничтожаем объект ArrowTip
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

    // Метод для удаления линии между двумя нодами
    public void RemoveLineRenderer(Node startNode, Node endNode)
    {
        var key = (startNode, endNode);
        if (lineRenderers.TryGetValue(key, out LineRenderer lineRenderer))
        {
            Destroy(lineRenderer.gameObject);
            lineRenderers.Remove(key);
        }
    }

    // Метод для удаления всех линий
    public void ClearAllLineRenderers()
    {
        foreach (var lineRenderer in lineRenderers.Values)
        {
            Destroy(lineRenderer.gameObject);
        }
        lineRenderers.Clear();
    }
}
