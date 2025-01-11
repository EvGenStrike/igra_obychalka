using UnityEngine;

public class LineRendererProperties : MonoBehaviour
{
    public Gradient lineGradient;
    public Gradient greenGradient;
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
        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = lineMaterial;
        lineRenderer.colorGradient = lineGradient;
        lineRenderer.widthMultiplier = lineWidth;
    }
}
