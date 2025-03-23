# 사과 게임

**사과 게임**은 Unity와 C#을 기반으로 개발한 퍼즐 게임입니다.  
플레이어는 캔버스 UI 상의 Apple 프리팹을 드래그하여 선택하고, 선택한 Apple들의 값의 합이 정확히 10이 되는 조합을 만들어 점수를 획득합니다.  
이 프로젝트는 SOLID 원칙 및 여러 디자인 패턴(Service Locator, Observer, Factory, Object Pooling 등)을 적용하여 유지보수와 확장성이 뛰어난 구조로 개발되었습니다.

---

## Demo ScreenShot

![screenshot1](https://github.com/user-attachments/assets/e6bac2c6-075f-45ca-999a-45e16b442bb6)
![screenshot2](https://github.com/user-attachments/assets/c7d3627c-f735-4530-abc5-5a193887d09f)
![screenshot3](https://github.com/user-attachments/assets/49a8c797-a39b-4387-a498-955fc5ba6f40)

## 주요 기능

-   **드래그 기반 선택**

    -   플레이어는 마우스 드래그를 통해 Apple 프리팹을 선택할 수 있습니다.
    -   드래그 영역에 포함된 Apple들은 실시간으로 하이라이트되어 선택 여부를 확인할 수 있습니다.

-   **조합 검증 및 점수 시스템**

    -   선택된 Apple들의 값의 합이 10이면, 조합이 유효한 것으로 간주되어 해당 Apple들은 제거(또는 오브젝트 풀로 반환)되고 점수가 업데이트됩니다.

-   **오브젝트 풀링 & 팩토리 패턴**

    -   Apple 생성은 IAppleFactory 인터페이스와 ApplePool(또는 AppleFactory)을 통해 관리되어, Instantiate/Destroy의 오버헤드를 줄이고 성능을 최적화하였습니다.

-   **이벤트 기반 통신 (Observer 패턴)**

    -   GameEvents를 통해 점수 업데이트, 게임 종료 등의 이벤트를 발행하고, UIManager 등은 이를 구독하여 UI를 자동 갱신합니다.

-   **Service Locator 패턴**

    -   모든 주요 매니저(GameManager, UIManager, BoardManager, TimerManager, InputHandler, SoundManager 등)는 자신의 Awake()에서 ServiceLocator에 등록되며, 필요 시 ServiceLocator.Get<T>()를 통해 동적으로 연결됩니다.
    -   이로써 매니저 간의 직접적인 Inspector 참조를 제거하여 결합도를 낮추었습니다.

-   **ScriptableObject를 통한 데이터 관리**
    -   AppleData ScriptableObject를 활용하여 Apple의 기본 속성(기본 값, 색상, 생성 확률 등)을 에셋으로 관리합니다.
    -   이를 통해 다양한 Apple 타입을 손쉽게 추가하거나 수정할 수 있습니다.

---

## 아키텍처 및 디자인 패턴

### Service Locator 패턴

-   각 매니저는 자신의 Awake()에서 `ServiceLocator.Register<T>(this)`를 호출하여 자신을 등록합니다.
-   다른 매니저들은 `ServiceLocator.Get<T>()`를 통해 동적으로 참조하여, 직접적인 Inspector 연결을 제거했습니다.

### Observer 패턴

-   `GameEvents` 클래스를 사용하여 게임 내 주요 상태 변화(점수 업데이트, 게임 종료 등)를 이벤트로 발행합니다.
-   UIManager 등은 이 이벤트를 구독하여, UI를 업데이트합니다.

### Factory 패턴 & 오브젝트 풀링

-   `IAppleFactory` 인터페이스를 통해 Apple 생성 로직을 추상화하였으며, 이를 구현한 `ApplePool` 또는 `AppleFactory`를 통해 Apple 프리팹을 생성하고 재사용합니다.
-   이 구조는 Apple 생성 방식을 쉽게 교체하거나 최적화할 수 있도록 합니다.

### SOLID 원칙 적용

-   **단일 책임 (SRP):** 각 클래스는 한 가지 역할(예: 게임 흐름, UI 업데이트, 보드 관리 등)에 집중합니다.
-   **개방/폐쇄 (OCP):** 인터페이스와 이벤트를 통해 새로운 기능 추가 시 기존 코드를 최소한으로 수정합니다.
-   **리스코프 치환 (LSP):** 인터페이스(IAppleFactory)를 통해 구현체를 자유롭게 교체할 수 있습니다.
-   **인터페이스 분리 (ISP):** 필요한 기능만을 제공하는 작고 구체적인 인터페이스를 사용합니다.
-   **의존성 역전 (DIP):** 고수준 모듈은 ServiceLocator, 인터페이스, 이벤트를 통해 추상화된 객체에 의존하여 결합도를 낮춥니다.

---

## 사용 기술

-   **Unity Engine:**  
    Unity를 사용하여 게임을 개발하였으며, WebGL 및 데스크톱 플랫폼 모두 지원합니다.

-   **C#:**  
    최신 C# 기능을 활용해 명확하고 유지보수하기 쉬운 코드를 작성했습니다.

-   **TextMeshPro:**  
    고품질 UI 텍스트 렌더링을 위해 사용되었습니다.

-   **ScriptableObject:**  
    Apple의 속성을 관리하기 위해 사용되어 데이터와 로직을 분리하였습니다.

---

## 실행 방법

1. **Unity 프로젝트 열기:**  
   Unity Editor에서 프로젝트를 열고 메인 씬을 실행합니다.

2. **서비스 등록 및 초기화:**  
   각 매니저는 자신의 Awake()에서 ServiceLocator에 등록되며, GameManager의 Start()에서 게임이 초기화되고 보드가 생성되며 타이머가 시작됩니다.

3. **게임 플레이:**  
   플레이어는 InputHandler를 통해 Apple 프리팹들을 드래그하여 선택하고, CombinationValidator가 선택된 Apple들의 합을 검증합니다.
    - 올바른 조합일 경우 BoardManager가 Apple들을 제거(또는 풀로 반환)하고, GameManager가 점수를 업데이트합니다.
    - UIManager는 GameEvents를 구독하여 점수와 타이머를 실시간으로 갱신합니다.

---

## 향후 개선 계획

-   **상태 관리 확장:**  
    필요에 따라 상태 패턴을 재도입하여 게임 일시정지, 보너스 모드, 재시작 등 다양한 상태를 관리할 수 있습니다.

-   **오브젝트 풀링 최적화:**  
    ApplePool을 더욱 정교하게 구현하여 Apple 재사용 및 풀 관리 성능을 극대화할 수 있습니다.

-   **DI 컨테이너 도입:**  
    Zenject와 같은 DI 컨테이너를 도입하여 ServiceLocator 기반 의존성 관리를 보다 체계적으로 개선할 수 있습니다.

-   **유닛 테스트 확장:**  
    각 매니저와 게임 로직에 대해 단위 테스트를 추가하여 코드 안정성을 강화할 수 있습니다.

-   **추가 게임 기능:**  
    새로운 Apple 타입, 콤보 시스템, 파워업 및 보너스 요소 등 추가 기능을 도입하여 게임의 다양성과 재미를 높일 수 있습니다.

---

## 데모 및 저장소 링크

-   🕹️ Web Demo: https://mayquartet.com/my_htmls/Apple_Game/index.html
-   📦 GitHub Repository: https://github.com/ralskwo/Apple-Game
