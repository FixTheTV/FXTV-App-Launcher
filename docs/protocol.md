# FXTVGame Binary Protocol

Every packet starts with a fixed 6-byte header:

- `[0..3] int32`: total packet length, including header
- `[4..5] uint16`: opcode

All strings are UTF-8.

## Auth

### `1001` C2S_Login

- `byte usernameLength`
- `byte[] username`
- `byte passwordLength`
- `byte[] password`

### `2001` S2C_LoginResult

Fail:

- `byte result = 0`

Success:

- `byte result = 1`
- `byte usernameLength`
- `byte[] username`
- `int64 userId`

### `1002` C2S_Register

- `byte usernameLength`
- `byte[] username`
- `byte passwordLength`
- `byte[] password`

### `2002` S2C_RegisterResult

Fail:

- `byte result = 0`

Success:

- `byte result = 1`
- `byte usernameLength`
- `byte[] username`

### `1003` C2S_Logout

No payload.

### `2003` S2C_LogoutResult

- `byte result`

## Lobby

### `1101` C2S_JoinLobby

- `int32 lobbyId`

### `2101` S2C_JoinLobbyResult

- `byte result`
- `int32 lobbyId`
- `int32 playerCount`

### `1102` C2S_LobbyChat

- `uint16 messageLength`
- `byte[] message`

### `2102` S2C_LobbyChat

- `uint16 usernameLength`
- `byte[] username`
- `uint16 messageLength`
- `byte[] message`

### `1103` C2S_LobbyReady

- `byte isReady`

Ready state values:

- `0`: not ready
- `1`: ready

The client sends this when the player clicks the Ready/Unready button.

### `2103` S2C_UpdateLobbyCount

- `int32 playerCount`

The server broadcasts this when the lobby player count changes.

### `1104` C2S_LeaveLobby

No payload.

The client sends this when the player leaves the current lobby.

### `2104` S2C_LeaveLobby

No payload.

The server sends this back to the leaving client after processing the leave request.

### `2105` S2C_LobbyTabUpdate

- `uint16 usernameLength`
- `byte[] username`
- `byte slot`
- `byte isReady`

Slot values:

- `0`: first player slot
- `1`: second player slot

Ready state values:

- `0`: not ready
- `1`: ready

If `usernameLength` is `0`, that slot is empty.
