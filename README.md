
# DeathCounter - Rust Oxide Plugin

**Current Version: 1.3.5**

A comprehensive Rust Oxide plugin that displays player death counts through the InfoPanel GUI system.

## Features

- **Real-time Death Display**: Shows current death count for each player
- **InfoPanel Integration**: Uses InfoPanel plugin for elegant GUI presentation
- **Color Coding**: Automatic color changes based on death count
- **Multi-language**: Full support for English (default) and German
- **Permission System**: Granular control over access and admin functions
- **Persistent Data Storage**: Automatic saving and loading of death statistics
- **Configurable**: Extensive customization options for position, colors and behavior

## Installation

1. **Install Dependencies**:
   - [InfoPanel Plugin](https://umod.org/plugins/info-panel) by Ghosst (required)

2. **Install Plugin**:
   - Upload `DeathCounter.cs` to your Rust server's `oxide/plugins/` folder
   - Plugin compiles automatically on server start

3. **Grant Permissions**:
   ```
   oxide.grant group default deathcounter.use
   oxide.grant group admin deathcounter.admin
   ```

## Commands

### Chat Commands
- `/deaths` - Shows your own death count
- `/deathstop` - Shows top 5 players with most deaths
- `/deathpanel show` - Shows the Death Counter panel
- `/deathpanel hide` - Hides the Death Counter panel
- `/deathpanel reset [playername]` - Resets death count (Admin)

### Console Commands
- `deathcounter.reset [playername]` - Resets death counts
- `deathcounter.reload` - Reloads configuration
- `deathcounter.status` - Shows plugin status and statistics

## Permissions

- `deathcounter.use` - Basic plugin usage
- `deathcounter.admin` - Admin commands and death count resetting
- `deathcounter.viewall` - View all player deaths (future feature)

## Configuration

The plugin automatically creates a configuration file at `oxide/config/DeathCounter.json`:

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
  "Death Icon URL (leave empty for default skull)": "https://i.imgur.com/YCmvxMN.png",
  "Enable Debug Mode": false
}
```

## Color Coding

The plugin uses automatic color coding based on death count:
- **0 Deaths**: Green
- **1-3 Deaths**: Yellow
- **4-7 Deaths**: Orange
- **8-15 Deaths**: Red-Orange
- **16-25 Deaths**: Red
- **26+ Deaths**: Dark Red

## Data Management

- **Automatic Saving**: After every death and server save
- **Backup System**: Automatic backups on file corruption
- **Validation**: Data integrity checked on startup
- **Migration**: Automatic data structure updates on plugin updates

## Troubleshooting

### Plugin won't compile

### InfoPanel not showing

### Data loss

## Known Issues

### Panel registration reported success but verification failed

**Log example:**
```
(03:18:46) | [DeathCounter] [WARNING] Panel registration reported success but verification failed
```

**Explanation:**
This warning means that the InfoPanel API returned a success response when registering the DeathCounter panel, but a subsequent verification check did not confirm the panel as registered. This can happen due to timing issues, InfoPanel reloads, or race conditions in the Rust plugin environment.

**Why is this okay?**
- The plugin automatically detects this state and disables GUI features until InfoPanel is ready again.
- No data loss or crash occurs; all core DeathCounter features (death tracking, chat/console commands, data persistence) continue to work.
- The warning is informational and helps with troubleshooting InfoPanel integration, but does not interfere with plugin functionality.
- If InfoPanel becomes available again, the panel will be re-registered automatically.


## Support

- **GitHub Issues**: [Report Issues](https://github.com/mleem97/DeathcounterIP/issues)
- **Discord**: Oxide/uMod Community Discord
- **uMod**: [Plugin Page](https://umod.org)

## License

MIT License - See [LICENSE](LICENSE) for details.


## Changelog

### Version 1.3.5
- Improved defensive cleanup for InfoPanel panels on plugin unload
- Enhanced error logging and troubleshooting documentation
- Updated Known Issues section for InfoPanel integration
- Minor code and documentation improvements

### Version 1.3.0
- Fixed InfoPanel integration timing issues
- Improved plugin lifecycle management
- Enhanced error handling for panel registration
- Added comprehensive InfoPanel cleanup on unload
- Resolved KeyNotFoundException errors during plugin reload
- Optimized server initialization checks
- Better defensive programming for external dependencies

### Version 1.2.0
- Changed default language to English
- German available as secondary language
- Improved language file management
- Enhanced uMod/Oxide compliance
- Updated default death icon

### Version 1.1.1
- Improved null reference handling
- Optimized InfoPanel integration
- Enhanced error handling
- Automatic file validation

### Version 1.1.0
- Added InfoPanel integration
- Implemented color coding
- Multi-language support
- Comprehensive configuration options

### Version 1.0.0
- Initial release
- Basic death display functionality
