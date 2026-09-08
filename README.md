# FXTV Launcher

A lightweight game launcher built with C#.

FXTV Launcher is a personal game launcher project focused on learning and experimenting with software development, networking, authentication, client-server architecture, and real-world application development.

## Features

- Game library and game launching
- User authentication
- Client-server communication
- Custom client-server protocol
- Online lobby system
- Multiple simultaneous clients
- Heartbeat-based connection monitoring
- Graceful disconnection
- Server-side connection cleanup
- Database-backed user management
- Logging and telemetry
- Display settings
- Persistent user preferences
- Quality-of-life improvements

## Architecture

FXTV Launcher uses a client-server architecture.

The client communicates with the server through a custom networking layer.

The server is responsible for authentication, connection management, lobby management, and shared application state.

## Connection Management

FXTV Launcher uses heartbeat messages to monitor active connections.

Graceful disconnection is handled separately from unexpected connection loss.

This allows the server to detect clients that disappear unexpectedly and clean up their associated resources.

## Protocol

FXTV Launcher uses a custom client-server communication protocol.

The protocol defines:

- Message types
- Message structure
- Authentication flow
- Lobby operations
- Heartbeat messages
- Client identification
- Error handling
- Connection lifecycle

## Technology

### Client

- C#
- .NET
- Windows Forms
- TCP sockets
- Custom networking layer

### Server

- C#
- TCP sockets
- Concurrent connection handling
- MySQL

### Development

- Visual Studio
- Git
- GitHub

## Running the Project

### Requirements

- Windows
- .NET SDK
- Visual Studio
- MySQL

### Client

1. Clone the repository.

2. Open the solution in Visual Studio.

3. Configure the server address and port.

4. Start the server.

5. Start one or more client instances.

6. Log in and connect to the server.

### Server

Configure the database connection before starting the server.

Do not commit database credentials or other secrets to the repository.

## Development Goals

FXTV Launcher is primarily a learning and experimentation project.

The project focuses on understanding how real applications behave beyond the happy path:

- What happens when a client suddenly disappears?
- How should the server detect dead connections?
- How should disconnected clients be cleaned up?
- How should multiple clients interact with shared state?
- How should asynchronous operations interact with synchronization?
- How can server behavior be observed through logs and telemetry?
- How should invalid client input be handled?

The goal is not simply to make the application work, but to understand why it works.

## Roadmap

### Core

- [x] Basic launcher UI
- [x] Client-server communication
- [x] Authentication
- [x] Database integration
- [x] Lobby functionality
- [x] Heartbeat
- [x] Graceful disconnection
- [ ] Improved server-side cleanup
- [ ] Authentication token system
- [ ] Improved telemetry
- [ ] Improved server logs

### Games

- [ ] Multiplayer game
- [ ] Additional game modes
- [ ] Match / lobby flow

### Quality of Life

- [ ] Improved UI
- [ ] Keyboard navigation
- [ ] Better error messages
- [ ] Connection status indicators
- [ ] Additional user settings

## Status

Active development.

FXTV Launcher is a work in progress. The architecture, protocol, UI, and feature set may change as development continues.

## Why FXTV?

The project started as a simple question:

"What if I just built my own launcher?"

It has since become a practical way to learn networking, concurrency, databases, software architecture, debugging, and application development by building something that can actually be used.
