using UnityEngine;
using System.Collections.Generic;

public class NodesController : MonoBehaviour
{

    public Node gamerNode;
    // Приватное поле для хранения пути
    private Stack<Node> path = new Stack<Node>();

    // Геттер для пути
    public Stack<Node> Path
    {
        get { return path; }
    }

    // Сеттер для пути (принимает новый стек и заменяет текущий)
    public void SetPath(Stack<Node> newPath)
    {
        path = newPath;
    }

    // Метод для добавления ноды в путь
    public void AddNodeToPath(Node newNode)
    {
        if (newNode != null && !path.Contains(newNode))  // Проверяем, что нода не null и ещё не в пути
        {
            path.Push(newNode);  // Добавляем элемент в стек
            Debug.Log($"Нода {newNode.name} добавлена в путь.");
        }
        else
        {
            Debug.LogWarning("Нода уже в пути или null.");
        }
    }

    // Метод для удаления последней ноды из пути и получения нового последнего элемента
    public Node RemoveLastNodeFromPath()
    {
        if (path.Count > 0)  // Проверяем, что стек не пустой
        {
            Node removedNode = path.Pop();  // Удаляем и возвращаем последний элемент
            Debug.Log($"Нода {removedNode.name} удалена из пути.");

            if (path.Count > 0)  // Если стек не пустой, получаем новый последний элемент
            {
                Node newLastNode = path.Peek();
                Debug.Log($"Новая последняя нода в пути: {newLastNode.name}");
                return newLastNode;
            }
            else
            {
                Debug.LogWarning("Стек пустой.");
                return null;  // Если стек пустой, возвращаем null
            }
        }
        else
        {
            Debug.LogWarning("Стек пустой.");
            return null;  // Если стек пустой, возвращаем null
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
