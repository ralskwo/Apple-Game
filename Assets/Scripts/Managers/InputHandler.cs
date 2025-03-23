using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    private CombinationValidator combinationValidator;
    private BoardManager boardManager;
    private GameManager gameManager;

    [SerializeField] private RectTransform dragSelectionRect;
    [SerializeField] private RectTransform dragParentRect;

    private List<Apple> selectedApples = new List<Apple>();
    private bool isDragging = false;
    private Vector2 dragStartPos;

    void Awake()
    {
        ServiceLocator.Register<InputHandler>(this);
    }

    void Start()
    {
        combinationValidator = ServiceLocator.Get<CombinationValidator>();
        boardManager = ServiceLocator.Get<BoardManager>();
        gameManager = ServiceLocator.Get<GameManager>();
    }

    void Update()
    {
        if (!gameManager.IsGameActive())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPos = GetCanvasLocalPosition(Input.mousePosition);
            if (dragSelectionRect != null)
            {
                dragSelectionRect.gameObject.SetActive(true);
                dragSelectionRect.anchoredPosition = dragStartPos;
                dragSelectionRect.sizeDelta = Vector2.zero;
            }
            ClearSelection();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPos = GetCanvasLocalPosition(Input.mousePosition);
            UpdateDragSelectionRect(dragStartPos, currentPos);
            UpdateSelectionWithinRect(GetSelectionRect(dragStartPos, currentPos));
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            if (dragSelectionRect != null)
                dragSelectionRect.gameObject.SetActive(false);
            EndDrag();
        }
    }

    private Vector2 GetCanvasLocalPosition(Vector2 screenPosition)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dragParentRect, screenPosition, null, out localPos);
        return localPos;
    }

    private void UpdateDragSelectionRect(Vector2 start, Vector2 end)
    {
        if (dragSelectionRect == null) return;
        Vector2 center = (start + end) / 2;
        dragSelectionRect.anchoredPosition = center;
        Vector2 size = new Vector2(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y));
        dragSelectionRect.sizeDelta = size;
    }

    private Rect GetSelectionRect(Vector2 start, Vector2 end)
    {
        float x = Mathf.Min(start.x, end.x);
        float y = Mathf.Min(start.y, end.y);
        float width = Mathf.Abs(end.x - start.x);
        float height = Mathf.Abs(end.y - start.y);
        return new Rect(x, y, width, height);
    }

    private void UpdateSelectionWithinRect(Rect selectionRect)
    {
        Apple[] allApples = boardManager.boardTransform.GetComponentsInChildren<Apple>();
        foreach (Apple apple in allApples)
        {
            RectTransform appleRect = apple.GetComponent<RectTransform>();
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, appleRect.position);
            Vector2 appleLocalPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(dragParentRect, screenPos, null, out appleLocalPos);

            if (selectionRect.Contains(appleLocalPos))
            {
                if (!selectedApples.Contains(apple))
                {
                    selectedApples.Add(apple);
                    apple.OnSelect();
                }
            }
            else
            {
                apple.OnDeselect();
            }
        }
    }

    private void ClearSelection()
    {
        Apple[] allApples = boardManager.GetComponentsInChildren<Apple>();
        foreach (Apple apple in allApples)
        {
            apple.OnDeselect();
        }
        selectedApples.Clear();
    }

    private void EndDrag()
    {
        if (combinationValidator.CheckCombination(selectedApples))
        {
            boardManager.RemoveApples(selectedApples);
            gameManager.UpdateScore(selectedApples.Count);
        }
        else
        {
            foreach (var apple in selectedApples)
            {
                apple.OnDeselect();
            }
        }
        selectedApples.Clear();
    }
}
