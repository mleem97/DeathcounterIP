# AI Coding Agent Instructions for DeathCounter Plugin

## Project Overview
This is a **Rust Oxide plugin** that displays player death counts via InfoPanel GUI integration. Single-file C# plugin architecture following Oxide/uMod conventions.

## Architecture & Dependencies
- **Core File**: `DeathCounter.cs` (829 lines) - Complete plugin implementation
- **Critical Dependency**: InfoPanel plugin by Ghosst (external dependency, not in codebase)
- **Framework**: Rust Oxide/uMod plugin system with Newtonsoft.Json serialization
- **Data Flow**: Game events → Plugin hooks → InfoPanel API calls → GUI updates

## Code Organization Patterns
The plugin follows strict **region-based organization**:
```csharp
#region Fields           // Plugin references, timers, data storage
#region Configuration    // JsonProperty-decorated config class with validation
#region Localization     // Oxide lang system with DE/EN support
#region Hooks           // Oxide event handlers (OnPlayerDeath, OnPlayerConnected, etc.)
#region InfoPanel Integration  // External API integration with error handling
#region Panel Updates    // GUI update logic with color coding
#region Helper Methods   // Permission checks, logging utilities
#region Data Management  // JSON persistence via Oxide.DataFileSystem
#region Chat Commands    // [ChatCommand] decorated methods
#region Console Commands // [ConsoleCommand] decorated methods
```

## InfoPanel Integration Pattern
**Critical**: All GUI functionality depends on InfoPanel plugin being loaded:
```csharp
// Always check InfoPanel availability before API calls
if (InfoPanel != null)
{
    InfoPanel.Call("ShowPanel", "DeathCounter", "DeathCounterPanel", player.UserIDString);
}
```

Key InfoPanel API calls used:
- `PanelRegister` - Register panel configuration (JSON serialized)
- `SetPanelAttribute` - Update panel content/colors
- `ShowPanel/HidePanel` - Visibility control
- `RefreshPanel` - Force panel update

## Permission System Implementation
Uses Oxide's permission system with three levels:
- `PERMISSION_USE = "deathcounter.use"` - Basic usage
- `PERMISSION_ADMIN = "deathcounter.admin"` - Admin commands
- `PERMISSION_VIEW_ALL = "deathcounter.viewall"` - Future feature

**Pattern**: Always validate permissions before operations:
```csharp
if (!HasPermission(player, PERMISSION_USE)) return;
```

## Configuration & Validation
- Uses `JsonProperty` attributes with descriptive names for server admin clarity
- **Critical**: `ValidateConfig()` method prevents crashes from invalid values
- Range validation pattern: Reset to defaults if out of bounds
- Auto-save on validation changes

## Error Handling Strategy
- **Comprehensive try-catch** in all external API calls and critical operations
- **Structured logging**: `LogInfo()`, `LogWarning()`, `LogError()` with debug mode
- **Graceful degradation**: Plugin continues functioning if InfoPanel unavailable
- **Data loss prevention**: Auto-save after deaths, multiple save points

## Development Workflow
- **No build process** - Direct C# file deployment to `oxide/plugins/`
- **Testing**: Deploy to Rust server, monitor `oxide/logs/` for errors
- **Debug mode**: Enable via config for detailed operation logging
- **Console commands**: Use `deathcounter.status` for runtime diagnostics

## Data Persistence Pattern
Uses Oxide's DataFileSystem with Dictionary<ulong, int> for player data:
```csharp
Interface.Oxide.DataFileSystem.ReadObject<Dictionary<ulong, int>>("DeathCounter_Deaths")
```
**Always** wrap in try-catch and provide empty dictionary fallback.

## Localization Implementation
- Override `LoadDefaultMessages()` with language dictionaries
- Use `Lang(key, playerId, args)` helper for all user-facing text
- Support pattern: Register same messages for multiple language codes

## Key Constraints
- **Single file limitation** - Everything must be in `DeathCounter.cs`
- **External dependency** - InfoPanel must be present and loaded
- **Oxide lifecycle** - Respect plugin loading order and cleanup in `Unload()`
- **Performance consideration** - Timer-based updates, avoid excessive InfoPanel calls

## Testing Approach
- **Manual testing** on Rust server required (no unit test framework)
- **Debug mode** essential for troubleshooting InfoPanel integration
- **Permission testing** with different player roles
- **Data persistence testing** across server restarts
