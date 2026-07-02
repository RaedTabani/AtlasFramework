# Atlas Framework Architecture

## Studio

DeviGames

## Framework

Atlas

## Namespace

DeviGames.Atlas


# Principles

- Modular packages
- Reusable systems
- Clear responsibilities
- Event driven communication
- Data driven design
- Mobile first


# Dependency Rule

Gameplay
    ↓
Services
    ↓
Core


Core never depends on:
- Gameplay
- Services
- UI


# Package Rules

Every package must have:

- Purpose
- Responsibilities
- Non-responsibilities
- Dependencies
- Public API
- Documentation
- Tests