# DeathCounter Plugin für Rust Oxide

Ein Rust Oxide Plugin, das die Anzahl der Todesfälle von Spielern im InfoPanel anzeigt.

## Features

- **Echtzeit Death Counter**: Zeigt die Anzahl der Todesfälle jedes Spielers in einem InfoPanel an
- **Automatische Updates**: Das Panel wird automatisch aktualisiert, wenn ein Spieler stirbt
- **Persistente Daten**: Todesfälle werden gespeichert und überleben Server-Neustarts
- **Farbkodierung**: Die Anzahl der Todesfälle wird farblich dargestellt (grün für wenige, rot für viele)
- **Chat-Befehle**: Verschiedene Befehle zur Verwaltung und Anzeige von Statistiken
- **Admin-Funktionen**: Admins können Todesfälle zurücksetzen
- **Vollständig konfigurierbar**: Position, Farben, Größe und Update-Intervall können angepasst werden

## Abhängigkeiten

- **InfoPanel Plugin**: Dieses Plugin benötigt das InfoPanel Plugin von Ghosst
- Rust Oxide Framework

## Installation

1. Stelle sicher, dass das InfoPanel Plugin installiert und funktionsfähig ist
2. Lade `DeathCounter.cs` in den `oxide/plugins/` Ordner deines Rust-Servers hoch
3. Das Plugin wird automatisch geladen und erstellt eine Konfigurationsdatei

## Konfiguration

Die Konfiguration findest du unter `oxide/config/DeathCounter.json`:

```json
{
  "Update Interval (seconds)": 10,
  "Panel Position": "TopPanel",
  "Show Own Deaths Only": true,
  "Panel Background Color": "0.1 0.1 0.1 0.4",
  "Text Color": "1 0.2 0.2 1",
  "Font Size": 12,
  "Panel Width": 0.12,
  "Panel Height": 0.95
}
```

### Konfigurationsoptionen

- **Update Interval**: Wie oft das Panel aktualisiert wird (in Sekunden)
- **Panel Position**: Position des Panels ("TopPanel", "BottomPanel", etc.)
- **Show Own Deaths Only**: Ob nur eigene Todesfälle oder die aller Spieler angezeigt werden
- **Panel Background Color**: Hintergrundfarbe des Panels (RGBA Format)
- **Text Color**: Standardtextfarbe (RGBA Format)
- **Font Size**: Schriftgröße
- **Panel Width/Height**: Breite und Höhe des Panels (0-1)

## Chat-Befehle

### Für alle Spieler:
- `/deaths` - Zeigt deine eigenen Todesfälle an
- `/deathstop` - Zeigt die Top 5 Spieler mit den meisten Todesfällen
- `/deathpanel show` - Zeigt das Death Counter Panel an
- `/deathpanel hide` - Versteckt das Death Counter Panel

### Für Admins:
- `/deathpanel reset [spielername]` - Setzt Todesfälle für einen Spieler zurück (oder eigene, wenn kein Name angegeben)

## Konsolen-Befehle

- `deathcounter.reset [spielername]` - Setzt Todesfälle zurück (ohne Namen = alle Spieler)
- `deathcounter.reload` - Lädt die Konfiguration neu

## Farbkodierung

Das Plugin verwendet eine automatische Farbkodierung basierend auf der Anzahl der Todesfälle:

- **0 Todesfälle**: Grün
- **1-5 Todesfälle**: Gelb
- **6-10 Todesfälle**: Orange
- **11-20 Todesfälle**: Rot
- **20+ Todesfälle**: Dunkelrot

## InfoPanel Integration

Das Plugin nutzt die InfoPanel API für die GUI-Darstellung:

- Automatische Registrierung beim InfoPanel
- Unterstützung für alle InfoPanel-Features (Docking, Anker, etc.)
- Responsive Updates bei Spieler-Events
- Individualisierung pro Spieler möglich

## Datenverarbeitung

- Todesfälle werden in `oxide/data/DeathCounter_Deaths.json` gespeichert
- Automatisches Speichern bei Server-Saves und Plugin-Entladung
- Backup-System verhindert Datenverlust

## Events

Das Plugin reagiert auf folgende Rust-Events:
- `OnPlayerDeath`: Erhöht den Death Counter
- `OnPlayerConnected`: Zeigt das Panel neuen Spielern
- `OnPlayerDisconnected`: Versteckt das Panel
- `OnServerSave`: Speichert die Daten

## Troubleshooting

1. **Panel wird nicht angezeigt**: 
   - Stelle sicher, dass InfoPanel installiert und geladen ist
   - Überprüfe die Plugin-Logs auf Fehler

2. **Daten gehen verloren**:
   - Überprüfe die Schreibrechte im `oxide/data/` Ordner

3. **Performance-Probleme**:
   - Erhöhe das Update-Intervall in der Konfiguration

## Version History

- **1.0.0**: Erste Version mit grundlegenden Features

## Support

Bei Problemen oder Feature-Requests erstelle ein Issue auf GitHub oder kontaktiere den Plugin-Entwickler.

## Lizenz

Dieses Plugin steht unter einer Open-Source-Lizenz und kann frei modifiziert und verteilt werden.
