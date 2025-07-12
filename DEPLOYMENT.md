# DeathCounter Plugin - Deployment Guide

## Automatic File Management

The DeathCounter plugin now includes robust automatic file management to ensure smooth deployment and operation.

### Features

#### 1. Automatic File Creation
The plugin automatically creates all required files during initialization:
- **Configuration file** (`oxide/config/DeathCounter.json`)
- **Language files** (English and German)
- **Data file** (`oxide/data/DeathCounter_Deaths.json`)

#### 2. File Validation & Recovery
- **Configuration validation**: Checks for corrupted or outdated config files
- **Data integrity checks**: Validates death data file and creates backups if corrupted
- **Automatic backups**: Creates timestamped backups before replacing corrupted files

#### 3. Dependency Management
- **InfoPanel detection**: Automatically detects when InfoPanel plugin is loaded/unloaded
- **Graceful degradation**: Continues operating without GUI features if InfoPanel is unavailable
- **Dynamic integration**: Reestablishes InfoPanel connection when the plugin becomes available

### Installation Process

1. **Copy plugin file**:
   ```
   DeathCounter.cs → oxide/plugins/DeathCounter.cs
   ```

2. **Restart server or reload plugin**:
   ```
   o.reload DeathCounter
   ```

3. **Verify installation**:
   - Check console for initialization messages
   - Run `deathcounter.status` to verify all components are working

### File Structure After Installation

```
oxide/
├── plugins/
│   └── DeathCounter.cs
├── config/
│   └── DeathCounter.json (auto-created)
├── data/
│   ├── DeathCounter_Deaths.json (auto-created)
│   └── lang/
│       ├── en/
│       │   └── DeathCounter.json (auto-created)
│       └── de/
│           └── DeathCounter.json (auto-created)
└── logs/
    └── (check for any error messages)
```

### Troubleshooting

#### Plugin Won't Load
1. Check `oxide/logs/` for compilation errors
2. Ensure file permissions allow reading/writing in oxide directories
3. Verify Oxide framework is up to date

#### InfoPanel Integration Issues
1. Install InfoPanel plugin by Ghosst
2. Ensure InfoPanel loads before DeathCounter
3. Check console for InfoPanel API version messages

#### Data Loss Prevention
- Plugin automatically creates backups of corrupted files
- Backups are timestamped: `filename.backup.yyyyMMdd_HHmmss`
- Manual backups recommended before major server updates

### Performance Considerations

- **File checks**: Only performed during plugin initialization
- **Backup creation**: Only when corruption is detected
- **InfoPanel integration**: Gracefully handles plugin load/unload scenarios
- **Timer management**: Properly cleaned up during plugin unload

### Logging

Enable debug mode in configuration for detailed logging:
```json
{
  "Debug Mode": true
}
```

Debug logs include:
- File creation/validation status
- InfoPanel integration steps
- Configuration validation results
- Data integrity checks

### Migration from Older Versions

The plugin automatically handles migration:
1. Detects outdated configuration files
2. Creates backups of existing files
3. Generates new configuration with all current options
4. Preserves existing death count data

No manual intervention required for upgrades.
