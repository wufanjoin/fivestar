
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Direction
{
    Horizontal,
    Vertical
}
[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseMeshEffect
{
    [SerializeField] private Color32 topColor = Color.white;

    [SerializeField] private Color32 bottomColor = Color.black;

    [SerializeField] private Direction direction = Direction.Vertical;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        var vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);
        int count = vertexList.Count;

        ApplyGradient(vertexList, 0, count);
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);
    }

    private void ApplyGradient(List<UIVertex> vertexList, int start, int end)
    {
        float positionPoint=0f;
        switch (direction)
        {
            case Direction.Horizontal:
                positionPoint = vertexList[0].position.x;
                break;
           case  Direction.Vertical:
               positionPoint = vertexList[0].position.y;
                break;
        }
        float bottomY = positionPoint;
        float topY = positionPoint;
        
        for (int i = start; i < end; ++i)
        {
            float vertexPoint = 0f;
            switch (direction)
            {
                case Direction.Horizontal:
                    vertexPoint = vertexList[i].position.x;
                    break;
                case Direction.Vertical:
                    vertexPoint = vertexList[i].position.y;
                    break;
            }
            float y = vertexPoint;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        float uiElementHeight = topY - bottomY;
        for (int i = start; i < end; ++i)
        {
            UIVertex uiVertex = vertexList[i];
            float vertexPoint = 0f;
            switch (direction)
            {
                case Direction.Horizontal:
                    vertexPoint = vertexList[i].position.x;
                    break;
                case Direction.Vertical:
                    vertexPoint = vertexList[i].position.y;
                    break;
            }
            uiVertex.color = Color32.Lerp(bottomColor, topColor, (vertexPoint - bottomY) / uiElementHeight);
            vertexList[i] = uiVertex;
        }
    }
}