using UnityEngine;

public class Node : MonoBehaviour
{
    public Node connectedNode; // Объект, к которому будет соединение
    public GameObject lineRenderer;

    private LineRenderer lineRenderer_;

    private static int step = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer_ = lineRenderer.GetComponent<LineRenderer>();
        Debug.Log($"line renderer: {lineRenderer_}");
        Debug.Log($"connected node: {connectedNode}");
        if (lineRenderer_ != null && connectedNode != null)
        {
            UpdateLine();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer_ != null && connectedNode != null)
        {
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        lineRenderer_.positionCount = 11;
        lineRenderer_.SetPosition(step, transform.position);
        lineRenderer_.SetPosition(step + 1, connectedNode.transform.position);
        Debug.Log($"Node1: {transform.position}, Node2: {connectedNode.transform.position}");
        step += 1;
    }
}
