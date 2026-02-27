using UnityEngine;
using UnityEngine.UI;

namespace Siege.Gameplay.UI
{
    public class FlexibleGridViewLayout : LayoutGroup
    {
        public enum CountMode
        {
            FixedColumns = 0,
            FixedRows = 1,
        }

        public enum StartCorner
        {
            UpperLeft = 0,
            UpperRight = 1,
            LowerLeft = 2,
            LowerRight = 3,
        }

        public enum StartAxis
        {
            Horizontal = 0,
            Vertical = 1,
        }

        [SerializeField] CountMode _countMode = CountMode.FixedColumns;
        [SerializeField] int _count = 1;
        [SerializeField] Vector2 _spacing = Vector2.zero;
        [SerializeField] StartCorner _startCorner = StartCorner.UpperLeft;
        [SerializeField] StartAxis _startAxis = StartAxis.Horizontal;
        [SerializeField] bool _childControlWidth = true;
        [SerializeField] bool _childControlHeight = true;
        [SerializeField] bool _childForceExpandWidth = true;
        [SerializeField] bool _childForceExpandHeight = true;

        public CountMode Mode
        {
            get => _countMode;
            set
            {
                if (_countMode == value)
                    return;
                _countMode = value;
                SetDirty();
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                var v = Mathf.Max(1, value);
                if (_count == v)
                    return;
                _count = v;
                SetDirty();
            }
        }

        public Vector2 Spacing
        {
            get => _spacing;
            set
            {
                _spacing = new Vector2(Mathf.Max(0, value.x), Mathf.Max(0, value.y));
                SetDirty();
            }
        }

        public StartCorner Corner
        {
            get => _startCorner;
            set
            {
                if (_startCorner == value)
                    return;
                _startCorner = value;
                SetDirty();
            }
        }

        public StartAxis Axis
        {
            get => _startAxis;
            set
            {
                if (_startAxis == value)
                    return;
                _startAxis = value;
                SetDirty();
            }
        }

        public bool ChildControlWidth
        {
            get => _childControlWidth;
            set
            {
                if (_childControlWidth == value)
                    return;
                _childControlWidth = value;
                SetDirty();
            }
        }

        public bool ChildControlHeight
        {
            get => _childControlHeight;
            set
            {
                if (_childControlHeight == value)
                    return;
                _childControlHeight = value;
                SetDirty();
            }
        }

        public bool ChildForceExpandWidth
        {
            get => _childForceExpandWidth;
            set
            {
                if (_childForceExpandWidth == value)
                    return;
                _childForceExpandWidth = value;
                SetDirty();
            }
        }

        public bool ChildForceExpandHeight
        {
            get => _childForceExpandHeight;
            set
            {
                if (_childForceExpandHeight == value)
                    return;
                _childForceExpandHeight = value;
                SetDirty();
            }
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (_countMode == CountMode.FixedColumns)
            {
                CalculateLayoutForFixedColumns(0);
            }
            else
            {
                CalculateLayoutForFixedRows(0);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            if (_countMode == CountMode.FixedColumns)
            {
                CalculateLayoutForFixedColumns(1);
            }
            else
            {
                CalculateLayoutForFixedRows(1);
            }
        }

        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0);
        }

        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1);
        }

        private void CalculateLayoutForFixedColumns(int axis)
        {
            int columnCount = Mathf.Max(1, _count);
            int rowCount = rectChildren.Count > 0 ? Mathf.CeilToInt((float)rectChildren.Count / columnCount) : 1;

            if (axis == 0)
            {
                float totalMin = padding.horizontal;
                float totalPreferred = padding.horizontal;
                float totalFlexible = 0;

                for (int col = 0; col < columnCount; col++)
                {
                    float colMin = 0;
                    float colPreferred = 0;
                    float colFlexible = 0;

                    for (int row = 0; row < rowCount; row++)
                    {
                        int index = _startAxis == StartAxis.Horizontal ? row * columnCount + col : col * rowCount + row;
                        if (index >= rectChildren.Count)
                            break;

                        GetChildSizes(rectChildren[index], 0, out float min, out float preferred, out float flexible);
                        colMin = Mathf.Max(colMin, min);
                        colPreferred = Mathf.Max(colPreferred, preferred);
                        colFlexible = Mathf.Max(colFlexible, flexible);
                    }

                    totalMin += colMin + (col > 0 ? _spacing.x : 0);
                    totalPreferred += colPreferred + (col > 0 ? _spacing.x : 0);
                    totalFlexible += colFlexible;
                }

                SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, 0);
            }
            else
            {
                float totalMin = padding.vertical;
                float totalPreferred = padding.vertical;
                float totalFlexible = 0;

                for (int row = 0; row < rowCount; row++)
                {
                    float rowMin = 0;
                    float rowPreferred = 0;
                    float rowFlexible = 0;

                    for (int col = 0; col < columnCount; col++)
                    {
                        int index = _startAxis == StartAxis.Horizontal ? row * columnCount + col : col * rowCount + row;
                        if (index >= rectChildren.Count)
                            break;

                        GetChildSizes(rectChildren[index], 1, out float min, out float preferred, out float flexible);
                        rowMin = Mathf.Max(rowMin, min);
                        rowPreferred = Mathf.Max(rowPreferred, preferred);
                        rowFlexible = Mathf.Max(rowFlexible, flexible);
                    }

                    totalMin += rowMin + (row > 0 ? _spacing.y : 0);
                    totalPreferred += rowPreferred + (row > 0 ? _spacing.y : 0);
                    totalFlexible += rowFlexible;
                }

                SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, 1);
            }
        }

        private void CalculateLayoutForFixedRows(int axis)
        {
            int rowCount = Mathf.Max(1, _count);
            int columnCount = rectChildren.Count > 0 ? Mathf.CeilToInt((float)rectChildren.Count / rowCount) : 1;

            if (axis == 0)
            {
                float totalMin = padding.horizontal;
                float totalPreferred = padding.horizontal;
                float totalFlexible = 0;

                for (int col = 0; col < columnCount; col++)
                {
                    float colMin = 0;
                    float colPreferred = 0;
                    float colFlexible = 0;

                    for (int row = 0; row < rowCount; row++)
                    {
                        int index = _startAxis == StartAxis.Horizontal ? row * columnCount + col : col * rowCount + row;
                        if (index >= rectChildren.Count)
                            break;

                        GetChildSizes(rectChildren[index], 0, out float min, out float preferred, out float flexible);
                        colMin = Mathf.Max(colMin, min);
                        colPreferred = Mathf.Max(colPreferred, preferred);
                        colFlexible = Mathf.Max(colFlexible, flexible);
                    }

                    totalMin += colMin + (col > 0 ? _spacing.x : 0);
                    totalPreferred += colPreferred + (col > 0 ? _spacing.x : 0);
                    totalFlexible += colFlexible;
                }

                SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, 0);
            }
            else
            {
                float totalMin = padding.vertical;
                float totalPreferred = padding.vertical;
                float totalFlexible = 0;

                for (int row = 0; row < rowCount; row++)
                {
                    float rowMin = 0;
                    float rowPreferred = 0;
                    float rowFlexible = 0;

                    for (int col = 0; col < columnCount; col++)
                    {
                        int index = _startAxis == StartAxis.Horizontal ? row * columnCount + col : col * rowCount + row;
                        if (index >= rectChildren.Count)
                            break;

                        GetChildSizes(rectChildren[index], 1, out float min, out float preferred, out float flexible);
                        rowMin = Mathf.Max(rowMin, min);
                        rowPreferred = Mathf.Max(rowPreferred, preferred);
                        rowFlexible = Mathf.Max(rowFlexible, flexible);
                    }

                    totalMin += rowMin + (row > 0 ? _spacing.y : 0);
                    totalPreferred += rowPreferred + (row > 0 ? _spacing.y : 0);
                    totalFlexible += rowFlexible;
                }

                SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, 1);
            }
        }

        private void SetChildrenAlongAxis(int axis)
        {
            int columnCount, rowCount;
        
            if (_countMode == CountMode.FixedColumns)
            {
                columnCount = Mathf.Max(1, _count);
                rowCount = rectChildren.Count > 0 ? Mathf.CeilToInt((float)rectChildren.Count / columnCount) : 1;
            }
            else
            {
                rowCount = Mathf.Max(1, _count);
                columnCount = rectChildren.Count > 0 ? Mathf.CeilToInt((float)rectChildren.Count / rowCount) : 1;
            }

            float[] columnWidths = new float[columnCount];
            float[] rowHeights = new float[rowCount];

            CalculateCellSizes(columnCount, rowCount, columnWidths, rowHeights);

            if (axis == 0)
            {
                for (int i = 0; i < rectChildren.Count; i++)
                {
                    int col, row;
                    GetCellPosition(i, columnCount, rowCount, out col, out row);

                    float xPos = padding.left;
                    for (int c = 0; c < col; c++)
                    {
                        xPos += columnWidths[c] + _spacing.x;
                    }

                    if (_childControlWidth)
                    {
                        SetChildAlongAxis(rectChildren[i], 0, xPos, columnWidths[col]);
                    }
                    else
                    {
                        float offsetInCell = (columnWidths[col] - rectChildren[i].sizeDelta.x) * GetAlignmentOnAxis(0);
                        SetChildAlongAxis(rectChildren[i], 0, xPos + offsetInCell);
                    }
                }
            }
            else
            {
                for (int i = 0; i < rectChildren.Count; i++)
                {
                    int col, row;
                    GetCellPosition(i, columnCount, rowCount, out col, out row);

                    float yPos = padding.top;
                    for (int r = 0; r < row; r++)
                    {
                        yPos += rowHeights[r] + _spacing.y;
                    }

                    if (_childControlHeight)
                    {
                        SetChildAlongAxis(rectChildren[i], 1, yPos, rowHeights[row]);
                    }
                    else
                    {
                        float offsetInCell = (rowHeights[row] - rectChildren[i].sizeDelta.y) * GetAlignmentOnAxis(1);
                        SetChildAlongAxis(rectChildren[i], 1, yPos + offsetInCell);
                    }
                }
            }
        }

        private void CalculateCellSizes(int columnCount, int rowCount, float[] columnWidths, float[] rowHeights)
        {
            for (int col = 0; col < columnCount; col++)
            {
                float colMin = 0;
                float colPreferred = 0;
                float colFlexible = 0;

                for (int row = 0; row < rowCount; row++)
                {
                    int index;
                    if (!TryGetChildIndex(col, row, columnCount, rowCount, out index))
                        continue;

                    GetChildSizes(rectChildren[index], 0, out float min, out float preferred, out float flexible);
                    colMin = Mathf.Max(colMin, min);
                    colPreferred = Mathf.Max(colPreferred, preferred);
                    colFlexible = Mathf.Max(colFlexible, flexible);
                }

                columnWidths[col] = colPreferred;
            }

            for (int row = 0; row < rowCount; row++)
            {
                float rowMin = 0;
                float rowPreferred = 0;
                float rowFlexible = 0;

                for (int col = 0; col < columnCount; col++)
                {
                    int index;
                    if (!TryGetChildIndex(col, row, columnCount, rowCount, out index))
                        continue;

                    GetChildSizes(rectChildren[index], 1, out float min, out float preferred, out float flexible);
                    rowMin = Mathf.Max(rowMin, min);
                    rowPreferred = Mathf.Max(rowPreferred, preferred);
                    rowFlexible = Mathf.Max(rowFlexible, flexible);
                }

                rowHeights[row] = rowPreferred;
            }

            float availableWidth = rectTransform.rect.width - padding.horizontal;
            float totalSpacingX = Mathf.Max(0, columnCount - 1) * _spacing.x;
            float totalPreferredWidth = 0;
            for (int col = 0; col < columnCount; col++)
                totalPreferredWidth += columnWidths[col];

            if (totalPreferredWidth + totalSpacingX < availableWidth && _childForceExpandWidth)
            {
                float extraSpace = availableWidth - totalSpacingX - totalPreferredWidth;
                float extraPerColumn = extraSpace / columnCount;
                for (int col = 0; col < columnCount; col++)
                    columnWidths[col] += extraPerColumn;
            }

            float availableHeight = rectTransform.rect.height - padding.vertical;
            float totalSpacingY = Mathf.Max(0, rowCount - 1) * _spacing.y;
            float totalPreferredHeight = 0;
            for (int row = 0; row < rowCount; row++)
                totalPreferredHeight += rowHeights[row];

            if (totalPreferredHeight + totalSpacingY < availableHeight && _childForceExpandHeight)
            {
                float extraSpace = availableHeight - totalSpacingY - totalPreferredHeight;
                float extraPerRow = extraSpace / rowCount;
                for (int row = 0; row < rowCount; row++)
                    rowHeights[row] += extraPerRow;
            }
        }

        private void GetChildSizes(RectTransform child, int axis, out float min, out float preferred, out float flexible)
        {
            bool controlSize = axis == 0 ? _childControlWidth : _childControlHeight;
            bool childForceExpand = axis == 0 ? _childForceExpandWidth : _childForceExpandHeight;

            if (!controlSize)
            {
                min = child.sizeDelta[axis];
                preferred = min;
                flexible = 0;
            }
            else
            {
                min = LayoutUtility.GetMinSize(child, axis);
                preferred = LayoutUtility.GetPreferredSize(child, axis);
                flexible = LayoutUtility.GetFlexibleSize(child, axis);
            }

            if (childForceExpand)
                flexible = Mathf.Max(flexible, 1);
        }

        private void GetCellPosition(int childIndex, int columnCount, int rowCount, out int col, out int row)
        {
            if (_startAxis == StartAxis.Horizontal)
            {
                col = childIndex % columnCount;
                row = childIndex / columnCount;
            }
            else
            {
                col = childIndex / rowCount;
                row = childIndex % rowCount;
            }

            int cornerX = (int)_startCorner % 2;
            int cornerY = (int)_startCorner / 2;

            if (cornerX == 1)
                col = columnCount - 1 - col;
            if (cornerY == 1)
                row = rowCount - 1 - row;
        }

        private bool TryGetChildIndex(int col, int row, int columnCount, int rowCount, out int index)
        {
            int cornerX = (int)_startCorner % 2;
            int cornerY = (int)_startCorner / 2;

            int actualCol = cornerX == 1 ? columnCount - 1 - col : col;
            int actualRow = cornerY == 1 ? rowCount - 1 - row : row;

            if (_startAxis == StartAxis.Horizontal)
            {
                index = actualRow * columnCount + actualCol;
            }
            else
            {
                index = actualCol * rowCount + actualRow;
            }

            return index >= 0 && index < rectChildren.Count;
        }
    }
}