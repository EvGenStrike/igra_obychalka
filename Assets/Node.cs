using UnityEngine;

public class Node : MonoBehaviour
{
    public Node connectedNode; // Объект, к которому будет соединение
    public LineRenderer lineRenderer;
    public LineRendererProperties lineRendererProperties;
    

    private static int step = 0;
    private int localStep = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRendererProperties.setupLineRenderer(lineRenderer);
        Debug.Log($"line renderer: {lineRenderer}");
        Debug.Log($"connected node: {connectedNode}");
        if (lineRenderer != null && connectedNode != null)
        {
            UpdateLine();
        }
        localStep = step;
        step += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer != null && connectedNode != null)
        {
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        lineRenderer.positionCount = 2;

        // RectTransform rectTransform1 = connectedNode.gameObject.GetComponent<RectTransform>();
        // RectTransform rectTransform2 = transform.gameObject.GetComponent<RectTransform>();

        // // Получаем мировые координаты
        // Vector3 worldPos1 = rectTransform1.position;
        // Vector3 worldPos2 = rectTransform2.position;

        // lineRenderer_.SetPosition(localStep + 1, worldPos1);
        // lineRenderer_.SetPosition(localStep, worldPos2);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, connectedNode.transform.position);

        Debug.Log($"Node1: {transform.position}, Node2: {connectedNode.transform.position}");
    }
}
