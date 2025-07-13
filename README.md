
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

# DeathCounter for Rust Oxide/uMod

**Current Version:** 1.3.5

DeathCounter is a robust, single-file Rust Oxide/uMod plugin that tracks and displays player death counts via the InfoPanel GUI. It is designed for maximum reliability, seamless InfoPanel integration, and easy server management.

---

## Features

- **Live Death Counter:** Real-time display of player deaths in the InfoPanel GUI
- **InfoPanel Integration:** Full support for InfoPanel by Ghosst (required dependency)
- **Color Coding:** Panel text color changes based on death count
- **Multi-language:** English (default) and German support
- **Permission System:** Fine-grained access for users and admins
- **Persistent Data:** Automatic saving and loading of death stats
- **Configurable:** Panel position, colors, update interval, debug mode, and more
- **Safe Unload/Reload:** Defensive cleanup to prevent InfoPanel errors

---

## Installation

1. **Install InfoPanel**
   - Download and install [InfoPanel](https://umod.org/plugins/info-panel) by Ghosst
2. **Deploy DeathCounter**
   - Place `DeathCounter.cs` in your server's `oxide/plugins/` folder
   - The plugin will compile and load automatically
3. **Grant Permissions**
   - Run:
     ```
     oxide.grant group default deathcounter.use
     oxide.grant group admin deathcounter.admin
     ```

---

## Configuration

The plugin creates `oxide/config/DeathCounter.json` automatically. Example:

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

---

## Color Coding

- **0 Deaths:** Green
- **1-3 Deaths:** Yellow
- **4-7 Deaths:** Orange
- **8-15 Deaths:** Red-Orange
- **16-25 Deaths:** Red
- **26+ Deaths:** Dark Red

---

## Commands

### Chat Commands
- `/deaths` — Show your own death count
- `/deathstop` — Show top 5 players with most deaths
- `/deathpanel show` — Show the DeathCounter panel
- `/deathpanel hide` — Hide the DeathCounter panel
- `/deathpanel reset [playername]` — Reset death count (Admin)

### Console Commands
- `deathcounter.reset [playername]` — Reset death counts
- `deathcounter.reload` — Reload configuration
- `deathcounter.status` — Show plugin status and statistics

---

## Permissions

- `deathcounter.use` — Basic plugin usage
- `deathcounter.admin` — Admin commands and death count resetting
- `deathcounter.viewall` — View all player deaths (future feature)

---

## Data Management

- **Automatic Saving:** After every death and server save
- **Backup System:** Automatic backups on file corruption
- **Validation:** Data integrity checked on startup
- **Migration:** Automatic data structure updates on plugin updates

---

## InfoPanel Integration & Defensive Programming

- All GUI features require InfoPanel to be loaded and available
- Defensive checks and try-catch blocks around all InfoPanel API calls
- Plugin continues to work (death tracking, commands, data) even if InfoPanel is missing or errors occur
- Comprehensive cleanup logic on plugin unload/reload to prevent InfoPanel errors

---

## Troubleshooting

### Plugin won't compile
- Make sure InfoPanel is installed and up to date
- Check for syntax errors in DeathCounter.cs
- Review Oxide compiler logs for details

### InfoPanel not showing
- Ensure InfoPanel is loaded and running (`oxide.plugins`)
- Check player permissions
- Enable debug mode in config for more logs

### Data loss
- Plugin saves after every death and on server save
- Check file permissions in `oxide/data/`
- Backup files are created automatically

---

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

---

### InfoPanel KeyNotFoundException on plugin unload

**Log example:**
```
(03:24:10) | Failed to call hook 'OnPluginUnloaded' on plugin 'InfoPanel v1.0.9' (KeyNotFoundException: The given key 'DeathCounterPanel' was not present in the dictionary.)
  at System.Collections.Generic.Dictionary`2[TKey,TValue].get_Item (TKey key) [0x0001e] in <8ce0bd04a7a04b4b9395538239d3fdd8>:0 
  at Oxide.Plugins.InfoPanel.OnPluginUnloaded (Oxide.Core.Plugins.Plugin plugin) [0x00056] in <5bd7985dcca44fa3a831760b4eb5afae>:0 
  at Oxide.Plugins.InfoPanel.DirectCallHook (System.String name, System.Object& ret, System.Object[] args) [0x00610] in <5bd7985dcca44fa3a831760b4eb5afae>:0 
  at Oxide.Plugins.CSharpPlugin.InvokeMethod (Oxide.Core.Plugins.HookMethod method, System.Object[] args) [0x00079] in <42f9bedc659b4f4786eb778d3cd58968>:0 
  at Oxide.Core.Plugins.Plugin.CallHook (System.String hook, System.Object[] args) [0x00060] in <d59b507fd76240e5b62228d0eae39b73>:0 
```

**Explanation:**
This error is thrown by InfoPanel when it tries to remove a panel that is already gone or was never registered, typically during plugin unload or reload. It is a known issue with InfoPanel's internal dictionary management and does not indicate a problem with DeathCounter itself.

**Why is this okay?**
- DeathCounter handles InfoPanel integration defensively and will not crash or lose data due to this error.
- The error only affects InfoPanel's internal cleanup and does not impact DeathCounter's core features.
- You can safely ignore this error; DeathCounter will continue to function and re-register panels as needed.
- If you want to avoid this error entirely, ensure InfoPanel is updated or patched to check for panel existence before removal.

---

## Support

- **GitHub Issues:** [Report Issues](https://github.com/mleem97/DeathcounterIP/issues)
- **Discord:** Oxide/uMod Community Discord
- **uMod:** [Plugin Page](https://umod.org)

---

## License

MIT License — See [LICENSE](LICENSE) for details.

---

## Changelog

### 1.3.5
- Improved defensive cleanup for InfoPanel panels on plugin unload
- Enhanced error logging and troubleshooting documentation
- Updated Known Issues section for InfoPanel integration
- Minor code and documentation improvements

### 1.3.0
- Fixed InfoPanel integration timing issues
- Improved plugin lifecycle management
- Enhanced error handling for panel registration
- Added comprehensive InfoPanel cleanup on unload
- Resolved KeyNotFoundException errors during plugin reload
- Optimized server initialization checks
- Better defensive programming for external dependencies

### 1.2.0
- Changed default language to English
- German available as secondary language
- Improved language file management
- Enhanced uMod/Oxide compliance
- Updated default death icon

### 1.1.1
- Improved null reference handling
- Optimized InfoPanel integration
- Enhanced error handling
- Automatic file validation

### 1.1.0
- Added InfoPanel integration
- Implemented color coding
- Multi-language support
- Comprehensive configuration options

### 1.0.0
- Initial release
- Basic death display functionality
