# Fantastic Duo

Fantastic duo는 두 명의 플레이어가 주기적으로 변하는 두 개의 역할을 분담하여 한 개의 유닛을 조종하는 배틀 로얄 게임입니다.

## Authors

[박준현](https://github.com/channelsucre)(고려대학교 20)

[윤태영](https://github.com/taeyoung-1)(한양대학교 18)

[정영훈](https://github.com/andyj0927)(KAIST 20)

## Introduction

- 8인 배틀 로얄
- 총 네 개의 유닛이 존재하며, 두 명씩 팀을 구성해 하나의 유닛을 조종
- 이 때 한 팀을 이루는 두 명은 두 개의 역할을 15초를 주기로 바꿔가며 분담
    - 플레이어 1(상단바 초록색): 마우스를 이용해 조준, 마우스 좌클릭으로 발사, 마우스 우클릭으로 대쉬
    - 플레이어 2(상단바 노란색): WASD를 이용해 상하좌우 이동, R을 이용해 탄창이 모두 소모되었을 때 재장전

## Development Environment

- Framework: Unity 2021.3.6f1
- Language: C#
- IDE: Visual Studio Code

## Implementation

### Co-operation

- Fantastic Duo의 핵심이 되는 협동 요소를 구현하기 위해 Photon Unity Network 2를 사용하였습니다.
- 이 때 한 개의 PhotonView를 두 명이 조종할 수 있도록 하기 위하여 PunRPC를 이용해 remote procedure call로 플레이어의 입력 정보를 master에게 전달하였습니다. 이를 통해 탄창 정보, 체력 등을 동기화했습니다.
- PUN이 자체적으로 제공하는 custom properties 기능을 사용하여 플레이어의 팀, 생사 정보, 역할 등을 동기화했습니다. PUN 서버를 사용하였기 때문에 추가적인 백엔드는 구현하지 않았습니다.
- Unity의 Slider를 사용하여 체력, 탄창 등을 표기했습니다.

### Map Design

- Fantastic Duo는 우주인의 몸에 외계인이 침입하여 우주인과 외계인이 듀오가 되어 플레이하는 게임입니다. 그렇기에 배경을 우주로 설정하였으며, 우주에 있는 신전을 모티브로 맵을 만들었습니다.
- 원기둥형태의 벽도 만들어 총알을 피하면서 상대를 맞출 수 있도록 만들었습니다.
- 4팀이 각 모서리에서 시작할 수 있도록 위치를 설정해 주었으며 사방이 모두 막힌 벽도 만들어 대쉬 기능으로 빠져나오거나 들어갈 수 있도록 만들었습니다.

### Physics

- Unity의 RigidBody와 Collider를 사용해서 충돌과 피격 판정을 구현했습니다.
- 이때 RigidBody가 Collider를 무시하고 지나가는 현상이 발생하여 많은 시행착오를 겪었으나, 세부 조정을 통해 문제를 해결하였습니다.

### Camera Control

- 카메라는 플레이어의 큐브를 자동으로 추적합니다. 같은 큐브를 조종하는 두 명의 사용자는 같은 큐브를 바라보게 됩니다.
