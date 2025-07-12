# DeathCounter - Changelog

Alle wichtigen Änderungen an diesem Projekt werden in dieser Datei dokumentiert.

Das Format basiert auf [Keep a Changelog](https://keepachangelog.com/de/1.0.0/),
und dieses Projekt folgt [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2025-01-13

### Hinzugefügt
- **Permissions System**: Vollständige Integration des Oxide Permissions-Systems
  - `deathcounter.use` - Grundlegende Nutzung
  - `deathcounter.admin` - Administrative Befehle  
  - `deathcounter.viewall` - Anzeige aller Spielertodesfälle (vorbereitet)
- **Mehrsprachigkeit**: Unterstützung für Deutsch und Englisch
- **Konfigurationsvalidierung**: Automatische Überprüfung und Korrektur ungültiger Werte
- **Debug-Modus**: Detaillierte Logging-Optionen für Entwicklung und Troubleshooting
- **Erweiterte Konsolen-Befehle**:
  - `deathcounter.status` - Plugin-Status anzeigen
  - Verbesserte Fehlerbehandlung in allen Konsolen-Befehlen
- **Bessere Fehlerbehandlung**: Try-catch-Blöcke in allen kritischen Bereichen
- **Auto-Speichern**: Automatisches Speichern nach jedem Todesfall
- **Verbesserte Panel-Updates**: Intelligentere Update-Logik mit Permission-Checks

### Geändert
- **Konfigurationsdatei**: Bessere Dokumentation mit beschreibenden Property-Namen
- **Farbschema**: Angepasste Farbverläufe für bessere Lesbarkeit
- **Code-Struktur**: Aufgeteilt in logische Regionen entsprechend Oxide Best Practices
- **Logging**: Strukturiertes Logging mit verschiedenen Levels (Info, Warning, Error)
- **Performance**: Optimierte Panel-Updates und Datenspeicherung

### Behoben
- **InfoPanel Integration**: Robustere Erkennung und Initialisierung
- **Player Disconnect**: Bessere Behandlung von Spieler-Disconnects
- **Data Loss Prevention**: Mehrfache Sicherheitsmechanismen gegen Datenverlust
- **Memory Leaks**: Proper Timer-Cleanup beim Plugin-Unload

### Sicherheit
- **Input Validation**: Alle Benutzereingaben werden validiert
- **Permission Checks**: Alle sensitiven Operationen sind durch Permissions geschützt
- **Error Handling**: Keine sensitive Information in Fehlermeldungen

## [1.0.0] - 2025-01-13

### Hinzugefügt
- Grundlegende Death Counter Funktionalität
- InfoPanel Integration
- Chat-Befehle für Spieler
- Konsolen-Befehle für Admins
- Persistente Datenspeicherung
- Farbkodierte Todesfallanzahlen
- Konfigurierbare Panel-Einstellungen

### Features
- Echtzeitanzeige der Todesfälle im InfoPanel
- Top 5 Statistiken
- Admin-Funktionen zum Zurücksetzen
- Vollständig konfigurierbar (Position, Farben, Größe)
- Automatische Panel-Updates

---

## Mitwirkende

- **mleem97** - Hauptentwickler
- Basierend auf InfoPanel von **Ghosst**

## Danksagungen

- Oxide/uMod Team für das Framework
- InfoPanel Entwickler Ghosst für die GUI-Basis
- Rust Community für Feedback und Testing
