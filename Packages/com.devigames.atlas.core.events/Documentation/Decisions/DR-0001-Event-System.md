# ADR-0001 - Event System

## Status
Accepted

## Decision

Atlas will expose a static EventBus API while internally using an IEventBus implementation.

Gameplay code will use:

EventBus.Publish(...)
EventBus.Subscribe(...)
EventBus.Unsubscribe(...)

The internal implementation can be replaced without affecting gameplay code.

## Why

- Very easy to use.
- Clean gameplay code.
- Testable.
- Future-proof.
- Supports dependency injection later without changing the API.

## Consequences

Gameplay never knows how events are stored or dispatched.