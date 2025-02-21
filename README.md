# NoMoMOMO

## Main Classes
### GameManager (Singleton)
- `+ instance: GameManager`
- `+ StartGame()`
- `+ UpdateGame()`
- `+ LoadLevel(level: int)`
- `+ GameOver()`

### PlayerController (MonoBehaviour)
- `- speed: float`
- `- lives: int`
- `- score: int`
- `+ Move(direction: Vector2)`
- `+ UseSkill(skill: PowerUp)`
- `+ TakeDamage(amount: int)`

### LevelManager (MonoBehaviour)
- `- currentLevel: int`
- `- platforms: List<GameObject>`
- `- obstacles: List<GameObject>`
- `+ LoadLevel(level: int)`
- `+ SpawnObstacles()`
- `+ SpawnPowerUps()`

### PowerUp (MonoBehaviour) [Abstract]
- `- duration: float`
- `+ Activate(player: PlayerController)`

#### PowerUp Variants (inherit from PowerUp)
- **AirPowerUp** `: PowerUp`
  - `+ Activate(player: PlayerController)`
- **WaterPowerUp** `: PowerUp`
  - `+ Activate(player: PlayerController)`
- **EarthPowerUp** `: PowerUp`
  - `+ Activate(player: PlayerController)`
- **FirePowerUp** `: PowerUp`
  - `+ Activate(player: PlayerController)`

### Obstacle (MonoBehaviour) [Abstract]
- `+ Interact(player: PlayerController)`

#### Obstacle Variants (inherit from Obstacle)
- **Projectile** `: Obstacle`
  - `+ Interact(player: PlayerController)`
- **Platform** `: Obstacle`
  - `+ Interact(player: PlayerController)`

### ScoreSystem (MonoBehaviour)
- `- score: int`
- `+ AddPoints(points: int)`
- `+ CalculateFinalScore()`

## Relationships
- `GameManager` **controls** `LevelManager`
- `LevelManager` **spawns** `Obstacles` and `PowerUps`
- `PlayerController` **collects** `PowerUps`
- `PlayerController` **collides with** `Obstacles`
- `ScoreSystem` **tracks** `PlayerController` progress
