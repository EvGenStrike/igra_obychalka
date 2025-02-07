using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public List<Node> connectedNodes = new List<Node>(); // Список нод, с которыми будет соединение
    public UnityEvent OnSwitchCliked;
    public LineRenderersManager lineRenderers;
    public Sprite normalNodeSprite; // Изображение для обычной ноды
    public Sprite sourceNodeSprite; // Изображение для ноды-источника
    public bool isSourceNode;
    public int sourceId;
    public bool isStartNode;
    private bool hasLineRendererConnections;
    private Button nodeButton;  // Ссылка на кнопку ноды
    public Node gamerNode;
    private NodesController nodesController;
    private bool clicked;

    private static Node lastClickedNode = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Получаем ссылку на NodesController
        nodesController = FindObjectOfType<NodesController>();

        if (nodesController == null)
        {
            Debug.LogError("NodesController not found!");
            return;
        }

        

        // Проверяем текущее количество источников
        int currentSourceCount = nodesController.allNodes.FindAll(node => node.isSourceNode).Count;

        // Устанавливаем ноду как источник, если текущих источников меньше 2-3
        if (!isSourceNode && currentSourceCount < Random.Range(2, 4))
        {
            isSourceNode = true;
            sourceId = Random.Range(1, 9); // Генерация значения от 1 до 8 включительно
        }
        else
        {
            isSourceNode = false; // Если лимит источников достигнут
        }
        // Убедимся, что стартовая нода не является источником
        if (isStartNode)
        {
            isSourceNode = false;
        }


        OnSwitchCliked.AddListener(OnSwitchClikedEvent);
        nodeButton = GetComponentInChildren<Button>(); // Button внутри Node
        if (nodeButton != null)
        {
            nodeButton.onClick.AddListener(OnNodeButtonClick); // Подписываемся на событие нажатия
        }

        GameObject nodesObject = GameObject.Find("Nodes");
        if (nodesObject != null)
        {
            var nodesController = nodesObject.GetComponent<NodesController>();
            this.nodesController = nodesController;
        }

        foreach (var connectedNode in connectedNodes)
        {
            if (connectedNode != null && !connectedNode.hasLineRendererConnections) {
                lineRenderers.AddLineRenderer(this, connectedNode);
                hasLineRendererConnections = true;
            }
        }

        UpdateButtonSprite();
    }

    // Метод для обновления изображения кнопки
    private void UpdateButtonSprite()
    {
        if (nodeButton != null)
        {
            // Выбираем изображение в зависимости от флага isSourceNode
            nodeButton.image.sprite = isSourceNode ? sourceNodeSprite : normalNodeSprite;
        }
        else
        {
            Debug.LogWarning("nodeButton is not assigned on Node: " + gameObject.name);
        }
    }

    private Node GetPlayerNode()
    {
        GameObject nodesObject = GameObject.Find("Nodes");
        if (nodesObject != null)
        {
            var nodesController = nodesObject.GetComponent<NodesController>();
            if (nodesController != null)
            {
                //Debug.Log("Получена игровая нода: " + nodesController.gamerNode);
                return nodesController.gamerNode;
            }
        }

        return null;
    }

    public void ClearPath()
    {
        nodesController.path = new Stack<Node>();
        lastClickedNode = null;
    }

    private void OnSwitchClikedEvent()
    {
        if (nodesController.path.Count == 0)
        {
            lastClickedNode = null;
        }

        Color targetColor = nodeButton.GetComponent<Image>().color;
        if (!isStartNode){
            targetColor = Color.white;
        }

        if (lastClickedNode == null && this == GetPlayerNode())
        {
            if (!isStartNode){
                targetColor = new Color(0.6f, 0.6f, 0f);
            }
            nodesController.AddNodeToPath(this);
            lastClickedNode = this;
        }
        else if (this == lastClickedNode && !clicked)
        {
            lastClickedNode = nodesController.RemoveLastNodeFromPath();
            if (lastClickedNode == GetPlayerNode())
            {
                this.clicked = false;
            }
            lineRenderers.SetNormalLineRenderer(this);
        }
        else if (lastClickedNode != null && nodesController.Path.Contains(lastClickedNode))
        {
            if (!isStartNode){
                targetColor = new Color(0.6f, 0.6f, 0f);
            }
            nodesController.AddNodeToPath(this);
            lineRenderers.SetPathLineRenderer(this, lastClickedNode);
            lastClickedNode = this;
        }
        nodeButton.GetComponent<Image>().color = targetColor;

    }

    private void switchClicked()
    {
        this.clicked = !this.clicked;
        OnSwitchCliked?.Invoke();
    }

    private void OnNodeButtonClick()
    {
        switchClicked();
    }

    // Update is called once per frame
    void Update()
    {
        if (connectedNodes.Count > 0)
        {
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        //lineRenderer.positionCount = 2;

        //// RectTransform rectTransform1 = connectedNode.gameObject.GetComponent<RectTransform>();
        //// RectTransform rectTransform2 = transform.gameObject.GetComponent<RectTransform>();

        //// // Получаем мировые координаты
        //// Vector3 worldPos1 = rectTransform1.position;
        //// Vector3 worldPos2 = rectTransform2.position;

        //// lineRenderer_.SetPosition(localStep + 1, worldPos1);
        //// lineRenderer_.SetPosition(localStep, worldPos2);
        //lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(1, connectedNode.transform.position);

        //Debug.Log($"Node1: {transform.position}, Node2: {connectedNode.transform.position}");
    }
}
