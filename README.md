# DeathCounter Plugin for Rust Oxide

A Rust Oxide plugin that displays player death counts in the InfoPanel GUI.

## Features

- **Real-time Death Counter**: Shows each player's death count in an InfoPanel display
- **Automatic Updates**: Panel automatically updates when a player dies
- **Persistent Data**: Death counts are saved and survive server restarts
- **Color Coding**: Death count is color-coded (green for few deaths, red for many)
- **Chat Commands**: Various commands for managing and displaying statistics
- **Admin Functions**: Admins can reset death counts
- **Fully Configurable**: Position, colors, size, and update interval can be customized
- **Permissions System**: Full Oxide permissions integration for access control
- **Multilingual Support**: English and German language support
- **Error Handling**: Robust error handling and debug mode

## Dependencies

- **InfoPanel Plugin**: This plugin requires the InfoPanel plugin by Ghosst
- Rust Oxide Framework

## Installation

1. Ensure the InfoPanel plugin is installed and functional
2. Upload `DeathCounter.cs` to the `oxide/plugins/` folder of your Rust server
3. The plugin will automatically load and create a configuration file

## Permissions

The plugin uses Oxide's permission system for access control:

- `deathcounter.use` - Basic plugin usage (view panel, chat commands)
- `deathcounter.admin` - Administrative commands (reset, reload)
- `deathcounter.viewall` - View all player deaths (future feature)

**Grant permissions:**
```
oxide.grant group default deathcounter.use
oxide.grant group admin deathcounter.admin
```

## Configuration

Configuration can be found at `oxide/config/DeathCounter.json`:

```json
{
  "Update Interval in seconds (Default: 10)": 10,
  "Panel Position (TopPanel/BottomPanel/LeftPanel/RightPanel)": "TopPanel",
  "Show Own Deaths Only (true) or All Players (false)": true,
  "Panel Background Color (RGBA: Red Green Blue Alpha)": "0.1 0.1 0.1 0.4",
  "Default Text Color (RGBA: Red Green Blue Alpha)": "1 0.2 0.2 1",
  "Font Size (Default: 12)": 12,
  "Panel Width (0.0 - 1.0, Default: 0.12)": 0.12,
  "Panel Height (0.0 - 1.0, Default: 0.95)": 0.95,
  "Auto-show Panel on Player Connect": true,
  "Death Icon URL (leave empty for default skull)": "https://i.imgur.com/QvMYvdf.png",
  "Enable Debug Mode": false
}
```

### Configuration Options

- **Update Interval**: How often the panel updates (in seconds)
- **Panel Position**: Panel position ("TopPanel", "BottomPanel", etc.)
- **Show Own Deaths Only**: Whether to show only own deaths or all players
- **Panel Background Color**: Panel background color (RGBA format)
- **Default Text Color**: Default text color (RGBA format)
- **Font Size**: Font size for the text
- **Panel Width/Height**: Panel dimensions (0-1)
- **Auto-show Panel**: Automatically show panel when players connect
- **Death Icon URL**: Custom death icon URL
- **Debug Mode**: Enable detailed logging for troubleshooting

## Chat Commands

### For all players:
- `/deaths` - Shows your own death count
- `/deathstop` - Shows top 5 players with most deaths
- `/deathpanel show` - Shows the Death Counter panel
- `/deathpanel hide` - Hides the Death Counter panel

### For admins:
- `/deathpanel reset [playername]` - Resets death count for a player (or yourself if no name given)

## Console Commands

- `deathcounter.reset [playername]` - Reset death counts (no name = all players)
- `deathcounter.reload` - Reload configuration
- `deathcounter.status` - Show plugin status and statistics

## Color Coding

The plugin uses automatic color coding based on death count:

- **0 Deaths**: Green
- **1-3 Deaths**: Yellow
- **4-7 Deaths**: Orange
- **8-15 Deaths**: Red-Orange
- **16-25 Deaths**: Red
- **25+ Deaths**: Dark Red

## InfoPanel Integration

The plugin uses the InfoPanel API for GUI display:

- Automatic registration with InfoPanel
- Support for all InfoPanel features (docking, anchors, etc.)
- Responsive updates on player events
- Individual customization per player possible

## Data Processing

- Death counts are stored in `oxide/data/DeathCounter_Deaths.json`
- Automatic saving on server saves and plugin unload
- Backup system prevents data loss

## Events

The plugin responds to the following Rust events:
- `OnPlayerDeath`: Increments the death counter
- `OnPlayerConnected`: Shows panel to new players
- `OnPlayerDisconnected`: Hides the panel
- `OnServerSave`: Saves the data

## Troubleshooting

1. **Panel not showing**: 
   - Make sure InfoPanel is installed and loaded
   - Check plugin logs for errors
   - Verify player has `deathcounter.use` permission

2. **Data loss**:
   - Check write permissions in `oxide/data/` folder
   - Enable debug mode for detailed logging

3. **Performance issues**:
   - Increase update interval in configuration
   - Check server performance during peak times

4. **Permission issues**:
   - Verify permissions are granted correctly
   - Use `oxide.show perms <player>` to check player permissions

## Localization

The plugin supports multiple languages:
- **English** (en) - Default
- **German** (de) - Included

Language files are stored in `oxide/lang/[language]/DeathCounter.json`

To add custom translations:
1. Create a new language file in the appropriate folder
2. Copy the structure from an existing language file
3. Translate the messages

## API for Other Plugins

The plugin stores death data that other plugins can access:

```csharp
// Get death count for a player
var deathCount = DeathCounter?.Call("GetPlayerDeaths", player.userID);

// Reset death count for a player
DeathCounter?.Call("ResetPlayerDeaths", player.userID);
```

## Version History

- **1.1.0**: Added permissions system, localization, improved error handling
- **1.0.0**: Initial release with basic features

## Support

For issues or feature requests, please create an issue on GitHub or contact the plugin developer.

## License

This plugin is open source and can be freely modified and distributed.

## Credits

- **mleem97** - Plugin Developer
- **Ghosst** - InfoPanel Plugin (dependency)
- **Oxide/uMod Team** - Framework
- **Rust Community** - Feedback and testing
