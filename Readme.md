# Match-3 Game Architecture Overview

This project is a modular, SOLID-compliant match-3 puzzle game built in Unity.  
It features a fully decoupled game loop, object pooling, staggered animations, and special piece logic (bombs).  
All systems are designed for maintainability, extensibility, and performance.

---

## Core Systems

- **BoardService**: Stores gem models in a 2D grid. No Unity logic.
- **GemFactory**: Creates gem models and views. Prevents instant matches via anti-match logic. Injects `TurnService` into each gem input.
- **GemPool**: Reuses gem views to avoid instantiation overhead.
- **AnimationService**: Handles all visual effects and timing. No gameplay logic.
- **TurnService**: Central orchestrator. Handles input swaps with animation, then coordinates full turn lifecycle: swap → match → bomb → destroy → cascade → refill.
- **GemInput**: Detects press/swipe and delegates to `TurnService.RequestSwapAsync`.
- **BombService**: Creates and explodes bombs with configurable delays and blast patterns.
- **CascadeService**: Moves gems downward to fill empty spaces, animates falling.
- **RefillService**: Spawns new gems with staggered animation.
- **DestroyService**: Removes matched gems and returns views to pool with destroy animation.
- **MatchService**: Detects match groups and flat match lists.
- **ScoreService**: Calculates score based on match count and combo multiplier.
- **ComboService**: Tracks combo chains and multipliers.
- **GameInstaller**: Wires up all services, injects dependencies, and initializes the board.

---

## Features Implemented

- **Cascading Gem Drop Logic**
  - Gems fall one by one to fill empty slots.
  - Anti-match logic prevents unintended matches during refill.

- **Gem Pooling System**
  - GemPool reuses destroyed gems.
  - Factory integrates pooling and resets view state.

- **Bomb Piece**
  - Created when matching 4+ same-color gems.
  - Bomb color matches the triggering group.
  - Bomb explodes in a cross + diagonal pattern.
  - Configurable delays before neighbor destruction and bomb removal.
  - Cascade starts only after bomb is fully destroyed.

- **Staggered Gem Drop Animation**
  - Each gem falls with a delay based on its vertical position.
  - Matches Royal Match–style drop effect.

- **Turn Orchestration**
  - Swipes trigger `TurnService.RequestSwapAsync`.
  - Swap is animated before model update.
  - Full pipeline executes automatically (match → bomb → destroy → cascade → refill).

---

## Design Principles

- **SOLID Architecture**: Each service has a single responsibility and clean interface.
- **Dependency Injection**: All services are injected via GameInstaller.
- **Factory Pattern**: GemFactory creates and links model, view, and input.
- **Object Pooling**: GemPool avoids unnecessary instantiation.
- **Decoupled Animation**: AnimationService handles all visuals, separate from logic.
- **Extensibility**: Easy to add new gem types, effects, or board rules.

