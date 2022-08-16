using UnityEngine;
using UnityEngine.UI;

/*

    This script creates a grid from the child obejcts - works best when the number of objects are square numebrs

*/

public class FlexibleGridLayout : LayoutGroup
{

    public int rows;
    public int columns;

    public Vector2 cellSize;
    public Vector2 spacing;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        float sqrRT = Mathf.Sqrt(transform.childCount);
        rows = Mathf.CeilToInt(sqrRT);
        columns = Mathf.CeilToInt(sqrRT);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float) columns) - ((spacing.x / (float)columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = ((parentHeight / (float) rows) - ((spacing.y / (float)rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows));

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        int columnCount = 0;
        int rowCount = 0;

        for(int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }


    }

    public override void CalculateLayoutInputVertical()
    {
        try
        {
            throw new System.NotImplementedException();
        }
        catch
        {
            
        }
    }

    public override void SetLayoutHorizontal()
    { 
        try
        {
            throw new System.NotImplementedException();
        }
        catch
        {
            
        }
    }

    public override void SetLayoutVertical()
    {
        try
        {
            throw new System.NotImplementedException();
        }
        catch
        {
            
        }
    }


}
