using UnityEngine;
using System.Collections.Generic;

public class NodesController : MonoBehaviour
{

    public Node gamerNode;
    // ��������� ���� ��� �������� ����
    private Stack<Node> path = new Stack<Node>();

    // ������ ��� ����
    public Stack<Node> Path
    {
        get { return path; }
    }

    // ������ ��� ���� (��������� ����� ���� � �������� �������)
    public void SetPath(Stack<Node> newPath)
    {
        path = newPath;
    }

    // ����� ��� ���������� ���� � ����
    public void AddNodeToPath(Node newNode)
    {
        if (newNode != null && !path.Contains(newNode))  // ���������, ��� ���� �� null � ��� �� � ����
        {
            path.Push(newNode);  // ��������� ������� � ����
            Debug.Log($"���� {newNode.name} ��������� � ����.");
        }
        else
        {
            Debug.LogWarning("���� ��� � ���� ��� null.");
        }
    }

    // ����� ��� �������� ��������� ���� �� ���� � ��������� ������ ���������� ��������
    public Node RemoveLastNodeFromPath()
    {
        if (path.Count > 0)  // ���������, ��� ���� �� ������
        {
            Node removedNode = path.Pop();  // ������� � ���������� ��������� �������
            Debug.Log($"���� {removedNode.name} ������� �� ����.");

            if (path.Count > 0)  // ���� ���� �� ������, �������� ����� ��������� �������
            {
                Node newLastNode = path.Peek();
                Debug.Log($"����� ��������� ���� � ����: {newLastNode.name}");
                return newLastNode;
            }
            else
            {
                Debug.LogWarning("���� ������.");
                return null;  // ���� ���� ������, ���������� null
            }
        }
        else
        {
            Debug.LogWarning("���� ������.");
            return null;  // ���� ���� ������, ���������� null
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
