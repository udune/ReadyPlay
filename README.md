# ReadyPlay

## **Avatar Custom 및 Parts Change에 관련한 샘플 코드입니다. Patrol NPC는 캐릭터 이외에 돌아다니는 NPC를 말하며 Strategy 패턴으로 구현했습니다.**
###### *이외 디테일한 코드는 유출이 불가하여 참조하지 못 하는 점 양해 부탁드립니다.*

### Patrol NPC
1. 객체 지향적인 설계
  BasePatrolNpc 클래스를 활용한 상속 구조를 통해, 공통 로직을 정의하고 확장성을 높였습니다.
  IPatrolNpc 인터페이스를 사용하여, 일관된 인터페이스를 유지하며 다형성을 활용할 수 있도록 했습니다.
2. 유지보수와 확장성을 고려한 상태 관리
  ChangeState<T>()를 활용하여 NPC의 상태를 변경하며 State Pattern을 구현했습니다.
  PatrolNpcIdle, PatrolNpcMove, PatrolNpcContact 등의 클래스로 역할을 명확히 분리하여 각 상태별 책임을 분리했습니다.
  OnStart(), OnUpdate(), OnEnd() 메서드를 오버라이딩하여 상태 간 전환을 명확하게 관리할 수 있도록 했습니다.
3. 효율적인 경로 탐색 및 이동 처리
  NavMeshAgent를 활용하여 네비게이션 및 경로 이동을 자동화했습니다.
  SetDestination()을 활용해 다음 목적지를 설정하고, IsArrive()를 통해 도착 여부를 판별하는 구조가 깔끔합니다.
  Idle() 및 Move() 메서드로 정지 및 이동 애니메이션을 자연스럽게 처리했습니다.
4. 랜덤한 순찰 경로 지정
  StartPatrol()에서 무작위 순찰 경로를 생성하여 NPC가 매번 다른 경로를 이동할 수 있도록 구현했습니다.
  FindWayPoint()에서 다음 목적지를 자동으로 탐색하도록 설계했습니다.
5. 플레이어 탐지 및 상호작용 로직 구현
  SearchPlayer()를 통해 플레이어를 탐지하고, 감지 시 LookPlayer()를 통해 NPC가 자연스럽게 플레이어를 바라보게 했습니다.
  IsContact()로 플레이어와의 접촉 여부를 판단하고, 이에 따라 상태를 변경하도록 했습니다.
6. 유연한 NPC 생성 및 관리
  PatrolNpcManager에서 patrolNpcAvatars 배열을 활용하여 여러 개의 NPC를 손쉽게 생성하고 관리할 수 있도록 했습니다.
  OnDestroy()에서 NPC를 정리하여 메모리 관리와 리소스 낭비를 방지했습니다.
