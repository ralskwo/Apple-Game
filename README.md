# 사과 게임

**사과 게임**은 Unity와 C#을 활용하여 제작한 퍼즐 게임입니다.  
플레이어는 캔버스 UI 상의 Apple 프리팹들을 드래그하여 선택하고, 선택한 Apple들의 값의 합이 정확히 10이 되는 조합을 만들어 점수를 획득합니다.  
이 게임은 유지보수와 확장성을 고려해 SOLID 원칙 및 여러 디자인 패턴(Service Locator, Observer, Factory, Object Pooling 등)을 적용하여 개발되었습니다.

---

## Demo ScreenShot

![스크린샷1](ScreenShot\screenshot1.png)
![스크린샷2](ScreenShot\screenshot2.png)
![스크린샷3](ScreenShot\screenshot3.png)

## 주요 기능

-   **드래그 기반 선택:**  
    플레이어는 마우스 드래그를 통해 화면 상의 Apple들을 선택할 수 있습니다.

-   **조합 검증:**  
    사용자가 선택한 Apple들의 값의 합이 10이면 유효한 조합으로 간주되어, 해당 Apple들은 제거(또는 오브젝트 풀로 반환)되고 점수가 업데이트됩니다.

-   **오브젝트 풀링:**  
    Apple 생성 및 제거에 있어 오브젝트 풀링을 적용하여, Instantiate/Destroy에 의한 성능 저하를 최소화하였습니다. ApplePool(팩토리 패턴 구현체)을 통해 Apple 프리팹을 재사용합니다.

-   **이벤트 기반 통신:**  
    GameEvents 클래스를 통해 게임 내 주요 이벤트(점수 업데이트, 게임 종료 등)를 Observer 패턴으로 전달합니다. 이를 통해 매니저 간의 직접적인 호출을 줄이고, 느슨한 결합을 유지합니다.

-   **Service Locator:**  
    모든 주요 매니저(GameManager, UIManager, BoardManager, TimerManager, InputHandler, SoundManager 등)는 ServiceLocator에 등록되어, 다른 매니저와의 연결을 런타임에 동적으로 해결합니다. 이 방식은 Inspector를 통한 직접 참조를 제거하여, 코드의 결합도를 낮추고 테스트와 유지보수를 쉽게 만듭니다.

-   **ScriptableObject 활용:**  
    Apple의 속성(예: 기본 값, 색상, 생성 확률 등)은 ScriptableObject(AppleData)를 통해 관리됩니다. 이를 통해 디자이너가 데이터를 에셋 형태로 쉽게 조정할 수 있으며, 다양한 Apple 타입을 손쉽게 확장할 수 있습니다.

---

## 아키텍처 및 디자인 패턴

### Service Locator 패턴

각 매니저는 자신의 Awake()에서 ServiceLocator.Register<T>(this)를 호출해 자신을 등록합니다.  
필요한 경우 다른 매니저는 ServiceLocator.Get<T>()를 통해 동적으로 참조합니다.  
이렇게 하면 각 매니저가 Inspector에 직접 연결되지 않고도 서로 통신할 수 있어, 의존성이 크게 줄어듭니다.

### Observer 패턴

GameEvents라는 정적 클래스를 사용하여, 게임 내 상태 변화(예: 점수 업데이트, 게임 종료)를 이벤트로 발행합니다.  
UIManager는 이 이벤트들을 구독하여, UI를 자동으로 업데이트합니다.  
이로써 매니저 간 직접 호출 대신 이벤트 기반 통신이 가능해졌습니다.

### Factory 패턴 & 오브젝트 풀링

Apple 생성은 IAppleFactory 인터페이스를 통해 추상화되었으며, ApplePool 또는 AppleFactory를 통해 구체적인 생성/재사용 로직을 분리했습니다.  
이 패턴은 Apple 생성 방식을 쉽게 변경할 수 있도록 하며, 특히 오브젝트 풀링을 통해 성능 최적화에 기여합니다.

---

## 알고리즘 및 SOLID 원칙 적용

### 알고리즘

-   **드래그 영역 계산:**  
    InputHandler는 사용자의 드래그 동작으로부터 사각형 영역을 계산한 후, 해당 영역에 포함된 Apple들을 탐색하여 선택합니다.
-   **조합 검증:**  
    CombinationValidator는 선택된 Apple들의 값의 합을 계산하여, 그 합이 10이면 유효한 조합으로 간주합니다.

### SOLID 원칙

-   **단일 책임 원칙 (SRP):**  
    각 클래스는 하나의 역할에 집중합니다. 예를 들어, GameManager는 게임 흐름 관리, UIManager는 UI 업데이트, BoardManager는 보드와 Apple 관리, TimerManager는 타이머 관리 등으로 역할이 명확히 분리되어 있습니다.

-   **개방/폐쇄 원칙 (OCP):**  
    IAppleFactory와 GameEvents 같은 추상화 계층을 통해, 새로운 기능이나 Apple 생성 방식이 추가되더라도 기존 코드를 최소한으로 수정할 수 있도록 설계되었습니다.

-   **리스코프 치환 원칙 (LSP):**  
    IAppleFactory 인터페이스를 사용함으로써, ApplePool과 같은 구체적 구현체는 언제든지 교체 가능하며, BoardManager는 이를 신경 쓸 필요가 없습니다.

-   **인터페이스 분리 원칙 (ISP):**  
    필요한 기능만을 제공하는 작고 구체적인 인터페이스(IAppleFactory 등)를 사용하여, 클라이언트가 불필요한 메소드에 의존하지 않도록 설계했습니다.

-   **의존성 역전 원칙 (DIP):**  
    고수준 모듈(GameManager, UIManager 등)은 구체적인 구현 대신 ServiceLocator, 인터페이스, 이벤트 등을 통해 추상화된 객체에 의존합니다. 이로 인해 전체적인 결합도가 낮아지고, 테스트와 유지보수가 용이해졌습니다.

---

## 사용 기술

-   **Unity Engine:**  
    Unity를 사용하여 게임을 개발하였으며, WebGL과 데스크톱 플랫폼 모두 지원합니다.

-   **C#:**  
    최신 C# 기능을 사용하여 명확하고 유지보수하기 쉬운 코드를 작성했습니다.

-   **TextMeshPro:**  
    고품질의 UI 텍스트 렌더링을 위해 TextMeshPro를 사용했습니다.

-   **ScriptableObject:**  
    AppleData ScriptableObject를 통해 Apple의 속성을 관리하여, 데이터와 로직의 분리를 구현했습니다.

---

## How to Run

1. **Unity 프로젝트 열기:**  
   Unity Editor에서 프로젝트를 열고 메인 씬을 실행합니다.

2. **서비스 등록 및 초기화:**  
   각 매니저는 자신의 Awake()에서 ServiceLocator에 등록됩니다. GameManager의 Start() 메소드가 게임을 초기화하며, 보드를 생성하고 타이머를 시작합니다.

3. **게임 플레이:**  
   플레이어는 InputHandler를 통해 드래그로 Apple들을 선택하고, CombinationValidator를 통해 선택된 조합이 검증됩니다.  
   유효한 조합일 경우 BoardManager는 Apple들을 제거(또는 풀로 반환)하고, GameManager는 점수를 업데이트합니다.

4. **빌드 및 배포:**  
   WebGL 빌드 시, Brotli 또는 Gzip 압축 파일을 올바른 HTTP 헤더와 함께 제공하도록 웹 서버를 구성합니다.

---

## Future Improvements

-   **상태 관리 확장:**  
    필요에 따라 상태 패턴을 다시 도입하여 게임 일시정지, 보너스 모드, 재시작 등 다양한 게임 상태를 관리할 수 있습니다.

-   **오브젝트 풀링 최적화:**  
    ApplePool을 더욱 정교하게 구현하여 Apple 재사용 및 풀링 성능을 극대화할 수 있습니다.

-   **DI 컨테이너 도입:**  
    Zenject와 같은 DI 컨테이너를 도입하여 ServiceLocator 기반의 의존성 관리를 보다 체계적으로 개선할 수 있습니다.

-   **유닛 테스트:**  
    각 매니저와 게임 로직에 대해 단위 테스트를 추가하여 코드 안정성을 더욱 강화할 수 있습니다.

-   **추가 게임 기능:**  
    콤보 시스템, 파워업, 다양한 Apple 타입, 추가 효과 등을 도입하여 게임의 다양성과 재미를 높일 수 있습니다.

---

## 데모 및 저장소 링크

-   🕹️ Web Demo: ![실행하기](https://mayquartet.com/my_htmls/Apple_Game/index.html)
-   📦 GitHub Repository: https://github.com/ralskwo/Apple-Game
