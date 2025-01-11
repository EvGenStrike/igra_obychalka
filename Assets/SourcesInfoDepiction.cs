using UnityEngine;
using TMPro;

public class SourcesInfoDepiction : MonoBehaviour
{
    public TextMeshProUGUI headline;
    public TextMeshProUGUI description;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeInfo(string title, string description){
        headline.text = title;
        this.description.text = description;
    }
}
