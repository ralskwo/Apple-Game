using UnityEngine;

[CreateAssetMenu(fileName = "NewAppleData", menuName = "Game/Apple Data")]
public class AppleData : ScriptableObject
{
    public int baseValue = 1;         // 기본 값 (1~9 범위에서 조정 가능)
    public Color appleColor = Color.white; // 기본 색상
    public float spawnProbability = 1.0f;  // 생성 확률 등 추가 속성도 가능
    // 필요에 따라 더 많은 속성 추가 가능 (예: 효과, 애니메이션 클립, 파티클 등)
}
