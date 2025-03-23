using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Apple : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;  // Apple의 숫자 표시용 텍스트
    [SerializeField] private Image appleImage;      // Apple의 UI Image 컴포넌트

    // ScriptableObject를 통해 Apple 속성을 주입
    [SerializeField] private AppleData appleData;

    private int value;
    public Vector2Int BoardPosition { get; set; }
    public int Value { get { return value; } }

    // AppleData를 기반으로 Apple의 초기화 수행
    public void SetRandomValue()
    {
        // 예: AppleData의 baseValue를 기본으로, 랜덤 요소를 추가할 수 있음
        value = Random.Range(appleData.baseValue, 10);
        if (valueText != null)
            valueText.text = value.ToString();

        // AppleData에 정의된 색상을 적용
        if (appleImage != null)
            appleImage.color = appleData.appleColor;
    }

    public void OnSelect()
    {
        if (appleImage != null)
            appleImage.color = Color.grey;
    }

    public void OnDeselect()
    {
        if (appleImage != null)
            appleImage.color = appleData.appleColor;
    }

    public void PlayRemovalAnimation()
    {
        StartCoroutine(RemoveAnimation());
    }

    private IEnumerator RemoveAnimation()
    {
        float duration = 0.3f;
        Vector3 initialScale = transform.localScale;
        float time = 0f;
        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }

    public void MoveToPosition(Vector2 targetPos)
    {
        StartCoroutine(MoveAnimation(targetPos));
    }

    private IEnumerator MoveAnimation(Vector2 targetPos)
    {
        float duration = 0.3f;
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 initialPos = rect.anchoredPosition;
        float time = 0f;
        while (time < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(initialPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = targetPos;
    }
}
