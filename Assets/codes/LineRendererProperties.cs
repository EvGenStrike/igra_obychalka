using UnityEngine;

public class LineRendererProperties : MonoBehaviour
{
    public Gradient lineGradient;
    public float lineWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void setupLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.sortingLayerName = "Default"; 
        lineRenderer.sortingOrder = 0;  
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.colorGradient = lineGradient;
        lineRenderer.widthMultiplier = lineWidth;
    }
}
