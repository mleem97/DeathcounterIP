# DeathCounter Installation Guide

## Voraussetzungen

1. **Rust Server** mit Oxide Framework
2. **InfoPanel Plugin** von Ghosst (muss bereits installiert sein)

## Schritt-für-Schritt Installation

### 1. InfoPanel Plugin installieren
Falls noch nicht installiert, lade das InfoPanel Plugin herunter und installiere es:
- Datei: `InfoPanel.cs` in `oxide/plugins/` kopieren
- Server neuladen oder Plugin mit `oxide.reload InfoPanel` laden

### 2. DeathCounter Plugin installieren
- Kopiere `DeathCounter.cs` in den `oxide/plugins/` Ordner deines Servers
- Das Plugin wird automatisch geladen

### 3. Permissions konfigurieren
Das Plugin verwendet ein Permissions-System für bessere Kontrolle:

**Standardberechtigungen:**
- `deathcounter.use` - Grundlegende Nutzung des Plugins (Panel sehen, Chat-Befehle)
- `deathcounter.admin` - Administrative Befehle (Reset, Reload)
- `deathcounter.viewall` - Anzeige aller Spielertodesfälle (zukünftig)

**Permissions vergeben:**
```
oxide.grant group default deathcounter.use
oxide.grant group admin deathcounter.admin
oxide.grant user <steamid> deathcounter.use
```

### 4. Konfiguration anpassen (optional)
- Die Konfigurationsdatei wird automatisch unter `oxide/config/DeathCounter.json` erstellt
- Passe die Einstellungen nach deinen Wünschen an
- Verwende `deathcounter.reload` in der Konsole um Änderungen zu übernehmen

### 5. Test
- Verbinde dich mit dem Server
- Das Death Counter Panel sollte automatisch erscheinen (bei entsprechender Berechtigung)
- Teste die Chat-Befehle: `/deaths`, `/deathpanel show/hide`

## Neue Features in Version 1.1.0

### Permissions System
- Vollständige Integration des Oxide Permissions-Systems
- Feingranuläre Kontrolle über Zugriff und Funktionen
- Admin-spezifische Befehle sind geschützt

### Mehrsprachigkeit
- Unterstützung für Deutsch und Englisch
- Lokalisierbare Nachrichten über Oxide's Language-System
- Einfache Erweiterung um weitere Sprachen

### Verbesserte Fehlerbehandlung
- Robuste Exception-Behandlung
- Detaillierte Logging-Optionen
- Debug-Modus für Entwicklung und Troubleshooting

### Konfigurationsvalidierung
- Automatische Überprüfung von Konfigurationswerten
- Fallback auf Standardwerte bei ungültigen Eingaben
- Bessere Dokumentation der Konfigurationsoptionen

## Erste Schritte

Nach der Installation:

1. **Panel testen**: Verwende `/deathpanel show` um das Panel anzuzeigen
2. **Deaths testen**: Stirb absichtlich um zu sehen, ob der Counter funktioniert
3. **Konfiguration anpassen**: Ändere Position, Farbe oder Größe nach deinen Wünschen

## Häufige Probleme

### Panel wird nicht angezeigt
- Überprüfe ob InfoPanel geladen ist: `oxide.plugins`
- Schaue in die Logs: `oxide.log` für Fehlermeldungen
- Teste InfoPanel mit anderen Plugins

### Counter zählt nicht
- Überprüfe die Plugin-Logs auf Fehler
- Stelle sicher, dass OnPlayerDeath Event funktioniert
- Teste mit `/deathpanel reset` und stirb erneut

### Konfiguration wird nicht übernommen
- Verwende `deathcounter.reload` nach Änderungen
- Überprüfe JSON-Syntax der Konfigurationsdatei
- Bei Syntaxfehlern wird die Standard-Konfiguration verwendet

## Anpassungen

Du kannst das Plugin leicht anpassen:

- **Farben ändern**: Bearbeite die `GetDeathColor()` Methode
- **Text ändern**: Ändere die deutschen Texte in der entsprechenden Zeile
- **Position ändern**: Modifiziere die Panel-Konfiguration in `GetPanelConfig()`
- **Icon ändern**: Ändere die URL in der Image-Konfiguration

## Support

Bei Problemen:
1. Überprüfe die Server-Logs
2. Teste mit minimaler Konfiguration
3. Kontaktiere den Entwickler mit detaillierter Fehlerbeschreibung
