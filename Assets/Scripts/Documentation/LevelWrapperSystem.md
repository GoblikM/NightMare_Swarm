# LevelWrapper and PlayerPlaceholder System

## Overview
This system provides a mechanism to track and clean up controllers (weapons, passive items) when transitioning between levels, preventing controller accumulation and memory leaks.

## Components

### LevelWrapper
- **Purpose**: Manages level lifecycle and tracks controllers
- **Location**: `Assets/Scripts/Managers/LevelWrapper.cs`
- **Key Features**:
  - Tracks controllers transferred from PlayerPlaceholder to Player
  - Handles runtime-spawned controllers
  - Provides cleanup methods for level exit
  - Uses reflection to update controller references

### PlayerPlaceholder
- **Purpose**: Temporary holder for controllers before transferring to actual Player
- **Location**: `Assets/Scripts/Player/PlayerPlaceholder.cs`
- **Key Features**:
  - Holds controllers as child objects
  - Provides methods to add/remove controllers
  - Validation methods for controller integrity
  - Debug visualization in Scene view

## Setup Instructions

### 1. Scene Setup
1. Create a GameObject and attach the `LevelWrapper` script
2. Create a GameObject and attach the `PlayerPlaceholder` script
3. Assign the Player prefab to the LevelWrapper's `playerPrefab` field
4. Add any initial controllers as children of the PlayerPlaceholder

### 2. Controller Setup
Controllers (weapons, passive items) should be added as children of the PlayerPlaceholder GameObject:
```
Scene Hierarchy:
├── LevelWrapper
├── PlayerPlaceholder
│   ├── WeaponController1
│   ├── WeaponController2
│   └── PassiveItem1
└── Other GameObjects...
```

### 3. Integration with Existing Systems
The system automatically integrates with:
- `PlayerStats.SpawnWeapon()` - Tracks newly spawned weapons
- `PlayerStats.SpawnPassiveItem()` - Tracks newly spawned passive items
- `InventoryManager.LevelUpWeapon()` - Updates tracking when weapons are upgraded
- `InventoryManager.LevelUpPassiveItem()` - Updates tracking when passive items are upgraded
- `SceneController.SceneChange()` - Cleans up controllers when changing scenes

## Usage

### Automatic Usage
Once set up in the scene, the system works automatically:
1. When the level starts, the LevelWrapper initializes
2. Controllers are transferred from PlayerPlaceholder to Player
3. Runtime-spawned controllers are automatically tracked
4. When the level exits, all tracked controllers are cleaned up

### Manual Usage
You can also control the system manually:

```csharp
// Exit level and clean up controllers
LevelWrapper.instance.ExitLevel();

// Get list of tracked controllers
var controllers = LevelWrapper.instance.GetTransferredControllers();

// Add a controller to tracking
LevelWrapper.instance.AddTransferredController(controllerGameObject);

// Remove a controller from tracking
LevelWrapper.instance.RemoveTransferredController(controllerGameObject);
```

### Testing
Use the `LevelWrapperTest` script to test the system:
1. Attach it to any GameObject in the scene
2. Press `T` to spawn test controllers
3. Press `E` to exit level
4. Press `I` to show tracking info

## Key Methods

### LevelWrapper
- `InitializeLevel()` - Initialize the level and replace placeholder
- `ExitLevel()` - Clean up controllers and exit level
- `AddTransferredController(GameObject)` - Add controller to tracking
- `RemoveTransferredController(GameObject)` - Remove controller from tracking
- `GetTransferredControllers()` - Get list of tracked controllers

### PlayerPlaceholder
- `AddController(GameObject)` - Add controller to placeholder
- `RemoveController(GameObject)` - Remove controller from placeholder
- `GetControllers()` - Get all controllers in placeholder
- `ValidateControllers()` - Validate all controllers are valid

## Memory Management
The system ensures proper memory management by:
- Tracking all controllers transferred from placeholder
- Tracking runtime-spawned controllers
- Cleaning up all tracked controllers on level exit
- Removing destroyed controllers from tracking lists
- Proper cleanup on scene changes

## Technical Details

### Controller Reference Updates
When controllers are transferred from placeholder to player, their internal references are updated using reflection:
- `WeaponController.playerMovement` is updated to reference the new Player
- `PassiveItem.player` is updated to reference the new PlayerStats

### Integration Points
The system integrates with existing code at these points:
- `PlayerStats.SpawnWeapon()` - Adds tracking for new weapons
- `PlayerStats.SpawnPassiveItem()` - Adds tracking for new passive items
- `InventoryManager.LevelUpWeapon()` - Updates tracking when weapons are upgraded
- `InventoryManager.LevelUpPassiveItem()` - Updates tracking when passive items are upgraded
- `SceneController.SceneChange()` - Cleans up before scene changes

## Error Handling
The system includes comprehensive error handling:
- Null reference checks for all operations
- Validation of controller components
- Graceful handling of missing components
- Debug logging for tracking operations
- Warning messages for invalid operations

## Debugging
Enable debug mode in PlayerPlaceholder to see:
- Gizmos in Scene view showing placeholder position
- Debug messages for all operations
- Validation warnings for invalid controllers
- Visual connections between placeholder and controllers