# RealtimeChat

ASP.NET Core와 SignalR을 활용한 실시간 채팅 백엔드 API 프로젝트입니다.

JWT 기반 사용자 인증, 채팅방 참여자 권한 관리, 메시지 저장 및 실시간 전달,
메시지 읽음 처리 기능을 구현했습니다.

REST API를 통해 채팅 데이터를 저장 및 조회하고,
SignalR Hub를 통해 같은 채팅방에 참여한 사용자에게 메시지를 실시간으로 전달합니다.

<br>

## Tech Stack

![.NET](https://img.shields.io/badge/.NET_9-512BD4?style=flat&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-512BD4?style=flat)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat&logo=postgresql&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat&logo=jsonwebtokens&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=flat&logo=dotnet&logoColor=white)

<br>

## Features

### Authentication
- 회원가입
- 로그인
- JWT Access Token 기반 인증
- Refresh Token을 통한 Access Token 재발급
- JWT에서 현재 로그인 사용자 정보 조회

### Chat Room
- 채팅방 생성
- 채팅방 목록 조회
- 채팅방 상세 조회
- 채팅방 참여
- 채팅방 나가기
- 채팅방 참여자 조회
- 참여자 기반 접근 권한 검증

### Message
- 메시지 저장
- 채팅방 메시지 조회
- 채팅방 참여자만 메시지 전송 가능
- SignalR을 통한 실시간 메시지 전달

### Message Read
- 메시지 읽음 처리
- 사용자별 메시지 읽음 기록 저장
- 중복 읽음 기록 방지

### Common
- Global Exception Middleware
- Repository / Service 계층 분리
- DTO를 통한 API 요청/응답 모델 분리
- Entity Framework Core Migration

<br>

## Realtime Message Flow

```text
Client A
   |
   | REST API
   | POST /api/messages
   v
MessageController
   |
   v
MessageService
   |
   v
MessageRepository
   |
   v
PostgreSQL
   |
   | SignalR
   v
ChatHub Group
   |
   +-------> Client A
   |
   +-------> Client B
```

메시지는 REST API를 통해 데이터베이스에 저장한 후,
SignalR Group을 이용하여 같은 채팅방에 접속한 사용자들에게 실시간으로 전달합니다.

<br>

## Project Structure

```text
Controllers/
Data/
DTOs/
Hubs/
Interfaces/
Middleware/
Migrations/
Models/
Repositories/
Services/
Program.cs
```

### Layer Structure

```text
Controller
    |
    v
Service
    |
    v
Repository
    |
    v
Database
```

- **Controller**: HTTP 요청 및 응답 처리
- **Service**: 비즈니스 로직 및 권한 검증
- **Repository**: 데이터베이스 접근
- **DTO**: API 요청 및 응답 데이터 분리
- **Hub**: SignalR 실시간 연결 관리

<br>

## Authentication

JWT Bearer Authentication을 사용합니다.

REST API 요청 시 JWT를 Authorization Header에 전달합니다.

```http
Authorization: Bearer {access_token}
```

SignalR 연결에서는 `accessTokenFactory`를 사용하여 JWT를 전달하고,
서버의 `JwtBearerEvents.OnMessageReceived`에서 SignalR 요청의 토큰을 처리합니다.

<br>

## SignalR

SignalR Hub Endpoint:

```text
/chatHub
```

클라이언트는 JWT 인증 후 Hub에 연결하고 채팅방 Group에 참여합니다.

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7147/chatHub", {
        accessTokenFactory: () => token
    })
    .build();

connection.on("ReceiveMessage", message => {
    console.log("실시간 메시지 수신:", message);
});

await connection.start();

await connection.invoke("JoinRoom", "5");
```

`JoinRoom` 호출 시 현재 사용자가 실제 채팅방 참여자인지 확인한 후
SignalR Group에 추가됩니다.

<br>

## Database

- PostgreSQL
- Entity Framework Core
- EF Core Migration

주요 Entity:

```text
User
ChatRoom
ChatRoomUser
Message
MessageRead
RefreshToken
```

`ChatRoomUser`를 통해 사용자와 채팅방의 참여 관계를 관리합니다.

`MessageRead`는 `MessageId + UserId` 복합키를 사용하여
사용자별 메시지 읽음 상태를 관리합니다.

<br>

## Security

- BCrypt를 이용한 비밀번호 해싱
- JWT Bearer Authentication
- JWT Issuer / Audience / Lifetime / Signing Key 검증
- 채팅방 참여자 기반 메시지 전송 권한 검증
- SignalR Hub 연결 JWT 인증
- SignalR Group 참여 시 채팅방 참여 여부 검증
- DB Connection String을 개발 환경 설정으로 분리

<br>

## Local Setup

### 1. PostgreSQL Connection String

`appsettings.Development.json`에 로컬 PostgreSQL 연결 정보를 설정합니다.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=RealtimeChatDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  }
}
```

`appsettings.Development.json`은 Git에 포함하지 않습니다.

### 2. Database Migration

```bash
dotnet ef database update
```

### 3. Run

```bash
dotnet run
```

Swagger를 통해 REST API를 테스트할 수 있습니다.

<br>

## Troubleshooting

### SignalR JWT Authentication

#### 문제

REST API에서는 JWT 인증이 정상적으로 동작했지만,
SignalR 연결에서 인증이 되지 않아 `401 Unauthorized`가 발생했습니다.

#### 해결

SignalR 연결 시 `accessTokenFactory`를 통해 JWT를 전달하고,
`JwtBearerEvents.OnMessageReceived`에서 `/chatHub` 요청의
`access_token`을 인증 토큰으로 처리하도록 구성했습니다.

---

### SignalR Chat Room Authorization

#### 문제

SignalR 연결에 성공한 사용자가 실제 채팅방 참여 여부와 관계없이
임의의 Group에 참여할 수 있는 문제가 있었습니다.

#### 해결

`JoinRoom` 호출 시 JWT에서 현재 사용자 ID를 가져오고,
`ChatRoomUser` 데이터를 조회하여 실제 참여자인 경우에만
SignalR Group에 참가하도록 검증 로직을 추가했습니다.

---

### Message Read Migration

#### 문제

EF Core 모델에서 사용하는 테이블 이름과 기존 Migration의
`MessageRead` 테이블 이름이 일치하지 않아 조회 오류가 발생했습니다.

#### 해결

EF Core Entity Mapping을 명시하여 기존 PostgreSQL 테이블과
모델의 테이블 이름을 일치시켰습니다.

<br>

## Future Improvements

- 읽지 않은 메시지 개수 조회
- SignalR 연결/재연결 처리 개선
- 메시지 페이징
- 채팅방별 최근 메시지 조회
- Unit / Integration Test 추가
- Docker 기반 배포 환경 구성

<br>

## Author

GitHub: https://github.com/MoveHwan
