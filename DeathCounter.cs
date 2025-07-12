using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;
using Oxide.Game.Rust.Cui;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("DeathCounter", "mleem97", "1.1.0")]
    [Description("Zeigt die Anzahl der Todesfälle von Spielern im InfoPanel an")]
    public class DeathCounter : RustPlugin
    {
        #region Fields

        [PluginReference]
        Plugin InfoPanel;

        private Timer updateTimer;
        private Dictionary<ulong, int> playerDeaths = new Dictionary<ulong, int>();
        private List<string> panels = new List<string> { "DeathCounterPanel" };

        // Permission constants
        private const string PERMISSION_ADMIN = "deathcounter.admin";
        private const string PERMISSION_USE = "deathcounter.use";
        private const string PERMISSION_VIEW_ALL = "deathcounter.viewall";

        #endregion

        #region Configuration

        private Configuration config;

        public class Configuration
        {
            [JsonProperty("Update Interval in seconds (Default: 10)")]
            public int UpdateInterval = 10;

            [JsonProperty("Panel Position (TopPanel/BottomPanel/LeftPanel/RightPanel)")]
            public string PanelPosition = "TopPanel";

            [JsonProperty("Show Own Deaths Only (true) or All Players (false)")]
            public bool ShowOwnDeathsOnly = true;

            [JsonProperty("Panel Background Color (RGBA: Red Green Blue Alpha)")]
            public string BackgroundColor = "0.1 0.1 0.1 0.4";

            [JsonProperty("Default Text Color (RGBA: Red Green Blue Alpha)")]
            public string TextColor = "1 0.2 0.2 1";

            [JsonProperty("Font Size (Default: 12)")]
            public int FontSize = 12;

            [JsonProperty("Panel Width (0.0 - 1.0, Default: 0.12)")]
            public float PanelWidth = 0.12f;

            [JsonProperty("Panel Height (0.0 - 1.0, Default: 0.95)")]
            public float PanelHeight = 0.95f;

            [JsonProperty("Auto-show Panel on Player Connect")]
            public bool AutoShowOnConnect = true;

            [JsonProperty("Death Icon URL (leave empty for default skull)")]
            public string DeathIconUrl = "https://i.imgur.com/QvMYvdf.png";

            [JsonProperty("Enable Debug Mode")]
            public bool DebugMode = false;
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<Configuration>();
                if (config == null) 
                {
                    LogWarning("Configuration file is null, creating new one...");
                    throw new Exception();
                }
                ValidateConfig();
            }
            catch (Exception ex)
            {
                LogError($"Error loading configuration: {ex.Message}");
                Config.WriteObject(config = new Configuration(), true);
            }
        }

        private void ValidateConfig()
        {
            bool configChanged = false;

            // Validate update interval
            if (config.UpdateInterval < 1 || config.UpdateInterval > 300)
            {
                LogWarning("Update interval out of range (1-300), resetting to 10");
                config.UpdateInterval = 10;
                configChanged = true;
            }

            // Validate panel dimensions
            if (config.PanelWidth < 0.01f || config.PanelWidth > 1.0f)
            {
                LogWarning("Panel width out of range (0.01-1.0), resetting to 0.12");
                config.PanelWidth = 0.12f;
                configChanged = true;
            }

            if (config.PanelHeight < 0.01f || config.PanelHeight > 1.0f)
            {
                LogWarning("Panel height out of range (0.01-1.0), resetting to 0.95");
                config.PanelHeight = 0.95f;
                configChanged = true;
            }

            // Validate font size
            if (config.FontSize < 8 || config.FontSize > 24)
            {
                LogWarning("Font size out of range (8-24), resetting to 12");
                config.FontSize = 12;
                configChanged = true;
            }

            if (configChanged)
            {
                SaveConfig();
            }
        }

        #endregion

        #region Localization

        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["DeathCount"] = "Todesfälle: {0}",
                ["YourDeaths"] = "Du bist {0} mal gestorben.",
                ["NoDeaths"] = "Noch keine Todesfälle verzeichnet.",
                ["TopDeaths"] = "Top 5 Todesfälle:",
                ["TopDeathsEntry"] = "{0}. {1}: {2} Todesfälle",
                ["UnknownPlayer"] = "Unbekannt",
                ["PanelShown"] = "Death Counter Panel wird angezeigt.",
                ["PanelHidden"] = "Death Counter Panel wurde versteckt.",
                ["DeathsReset"] = "Todesfälle wurden zurückgesetzt.",
                ["DeathsResetTarget"] = "Todesfälle für {0} wurden zurückgesetzt.",
                ["PlayerNotFound"] = "Spieler nicht gefunden.",
                ["NoPermission"] = "Du hast keine Berechtigung für diesen Befehl.",
                ["AllDeathsReset"] = "Alle Todesfälle wurden zurückgesetzt.",
                ["ConfigReloaded"] = "DeathCounter Konfiguration wurde neu geladen.",
                ["AvailableCommands"] = "Verfügbare Befehle:",
                ["CmdDeaths"] = "/deaths - Zeigt deine eigenen Todesfälle an",
                ["CmdDeathsTop"] = "/deathstop - Zeigt die Top 5 Spieler mit den meisten Todesfällen",
                ["CmdPanelShow"] = "/deathpanel show - Zeigt das Death Counter Panel an",
                ["CmdPanelHide"] = "/deathpanel hide - Versteckt das Death Counter Panel",
                ["CmdPanelReset"] = "/deathpanel reset [spielername] - Setzt Todesfälle zurück (Admin)",
                ["UnknownCommand"] = "Unbekannter Befehl. Verwende /deathpanel für Hilfe.",
                ["InfoPanelNotLoaded"] = "InfoPanel Plugin ist nicht geladen oder verfügbar."
            }, this, "en");

            // German translations
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["DeathCount"] = "Todesfälle: {0}",
                ["YourDeaths"] = "Du bist {0} mal gestorben.",
                ["NoDeaths"] = "Noch keine Todesfälle verzeichnet.",
                ["TopDeaths"] = "Top 5 Todesfälle:",
                ["TopDeathsEntry"] = "{0}. {1}: {2} Todesfälle",
                ["UnknownPlayer"] = "Unbekannt",
                ["PanelShown"] = "Death Counter Panel wird angezeigt.",
                ["PanelHidden"] = "Death Counter Panel wurde versteckt.",
                ["DeathsReset"] = "Todesfälle wurden zurückgesetzt.",
                ["DeathsResetTarget"] = "Todesfälle für {0} wurden zurückgesetzt.",
                ["PlayerNotFound"] = "Spieler nicht gefunden.",
                ["NoPermission"] = "Du hast keine Berechtigung für diesen Befehl.",
                ["AllDeathsReset"] = "Alle Todesfälle wurden zurückgesetzt.",
                ["ConfigReloaded"] = "DeathCounter Konfiguration wurde neu geladen.",
                ["AvailableCommands"] = "Verfügbare Befehle:",
                ["CmdDeaths"] = "/deaths - Zeigt deine eigenen Todesfälle an",
                ["CmdDeathsTop"] = "/deathstop - Zeigt die Top 5 Spieler mit den meisten Todesfällen",
                ["CmdPanelShow"] = "/deathpanel show - Zeigt das Death Counter Panel an",
                ["CmdPanelHide"] = "/deathpanel hide - Versteckt das Death Counter Panel",
                ["CmdPanelReset"] = "/deathpanel reset [spielername] - Setzt Todesfälle zurück (Admin)",
                ["UnknownCommand"] = "Unbekannter Befehl. Verwende /deathpanel für Hilfe.",
                ["InfoPanelNotLoaded"] = "InfoPanel Plugin ist nicht geladen oder verfügbar."
            }, this, "de");
        }

        private string Lang(string key, string id = null, params object[] args) => 
            string.Format(lang.GetMessage(key, this, id), args);

        #endregion

        #region Hooks

        void Init()
        {
            // Register permissions
            permission.RegisterPermission(PERMISSION_ADMIN, this);
            permission.RegisterPermission(PERMISSION_USE, this);
            permission.RegisterPermission(PERMISSION_VIEW_ALL, this);

            LoadData();
        }

        void Loaded()
        {
            if (InfoPanel != null)
            {
                InfoPanelInit();
            }
            else
            {
                LogWarning("InfoPanel plugin not found. DeathCounter requires InfoPanel to function.");
            }
        }

        void OnPluginLoaded(Plugin plugin)
        {
            if (plugin?.Title == "InfoPanel")
            {
                LogInfo("InfoPanel detected, initializing DeathCounter integration...");
                InfoPanelInit();
            }
        }

        void Unload()
        {
            SaveData();
            updateTimer?.Destroy();
            
            // Hide panels for all players
            if (InfoPanel != null)
            {
                foreach (var player in BasePlayer.activePlayerList)
                {
                    InfoPanel.Call("HidePanel", "DeathCounter", "DeathCounterPanel", player.UserIDString);
                }
            }
        }

        void OnServerSave()
        {
            SaveData();
        }

        void OnPlayerDeath(BasePlayer player, HitInfo info)
        {
            if (player == null) return;

            try
            {
                var playerId = player.userID;
                
                if (!playerDeaths.ContainsKey(playerId))
                    playerDeaths[playerId] = 0;
                
                playerDeaths[playerId]++;
                
                if (config.DebugMode)
                    LogInfo($"Player {player.displayName} died. Total deaths: {playerDeaths[playerId]}");
                
                UpdatePlayerPanel(player);
                
                // Auto-save after every death to prevent data loss
                timer.Once(1f, () => SaveData());
            }
            catch (Exception ex)
            {
                LogError($"Error in OnPlayerDeath for {player?.displayName}: {ex.Message}");
            }
        }

        void OnPlayerConnected(BasePlayer player)
        {
            if (player == null || !HasPermission(player, PERMISSION_USE)) return;
            
            timer.Once(3f, () =>
            {
                try
                {
                    if (player == null || !player.IsConnected) return;
                    
                    if (InfoPanel != null && config.AutoShowOnConnect)
                    {
                        // Check if InfoPanel GUI is loaded for the player
                        bool isGUILoaded = (bool?)InfoPanel.Call("IsPlayerGUILoaded", player.UserIDString) ?? false;
                        
                        if (isGUILoaded)
                        {
                            ShowPanelToPlayer(player);
                        }
                        else
                        {
                            // Retry after a delay if GUI is not ready
                            timer.Once(2f, () => {
                                if (player != null && player.IsConnected)
                                    ShowPanelToPlayer(player);
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Error in OnPlayerConnected for {player?.displayName}: {ex.Message}");
                }
            });
        }

        void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            if (player == null) return;
            
            try
            {
                if (InfoPanel != null)
                {
                    InfoPanel.Call("HidePanel", "DeathCounter", "DeathCounterPanel", player.UserIDString);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error in OnPlayerDisconnected for {player?.displayName}: {ex.Message}");
            }
        }

        #endregion

        #region InfoPanel Integration

        public void InfoPanelInit()
        {
            try
            {
                InfoPanel.Call("SendPanelInfo", "DeathCounter", panels);
                AddDeathCounterPanel();
                
                updateTimer?.Destroy();
                if (config.UpdateInterval > 0)
                {
                    updateTimer = timer.Repeat(config.UpdateInterval, 0, () => UpdateAllPanels());
                }

                LogInfo("DeathCounter successfully integrated with InfoPanel");
            }
            catch (Exception ex)
            {
                LogError($"Error initializing InfoPanel integration: {ex.Message}");
            }
        }

        public void AddDeathCounterPanel()
        {
            try
            {
                bool success = (bool)InfoPanel.Call("PanelRegister", "DeathCounter", "DeathCounterPanel", GetPanelConfig());
                
                if (success)
                {
                    LogInfo("DeathCounter panel registered successfully");
                    
                    // Show panel to connected players with permission
                    foreach (var player in BasePlayer.activePlayerList)
                    {
                        if (HasPermission(player, PERMISSION_USE))
                        {
                            ShowPanelToPlayer(player);
                        }
                    }
                }
                else
                {
                    LogError("Failed to register DeathCounter panel with InfoPanel");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error adding DeathCounter panel: {ex.Message}");
            }
        }

        private string GetPanelConfig()
        {
            var panelConfig = new
            {
                Autoload = false,
                AnchorX = "Left",
                AnchorY = "Top",
                Available = true,
                BackgroundColor = config.BackgroundColor,
                Dock = config.PanelPosition,
                Width = config.PanelWidth,
                Height = config.PanelHeight,
                Margin = "0.005 0 0 0.005",
                Order = 10,
                Image = new
                {
                    AnchorX = "Left",
                    AnchorY = "Top",
                    Available = true,
                    BackgroundColor = "0.2 0.1 0.1 0.6",
                    Dock = config.PanelPosition,
                    Height = 0.3,
                    Margin = "0.05 0.05 0.35 0.05",
                    Order = 1,
                    Url = !string.IsNullOrEmpty(config.DeathIconUrl) ? config.DeathIconUrl : "https://i.imgur.com/QvMYvdf.png"
                },
                Text = new
                {
                    Align = "MiddleCenter",
                    AnchorX = "Left",
                    AnchorY = "Top",
                    Available = true,
                    BackgroundColor = "0 0 0 0",
                    Dock = config.PanelPosition,
                    FontColor = config.TextColor,
                    FontSize = config.FontSize,
                    Content = Lang("DeathCount", null, 0),
                    Height = 0.65,
                    Margin = "0.35 0.05 0.05 0.05",
                    Order = 2,
                    Width = 0.9
                }
            };

            return JsonConvert.SerializeObject(panelConfig, Formatting.Indented);
        }

        #endregion

        #region Panel Updates

        private void UpdatePlayerPanel(BasePlayer player)
        {
            if (InfoPanel == null || player == null) return;

            try
            {
                var playerId = player.userID;
                var deaths = GetPlayerDeaths(playerId);
                
                string content = Lang("DeathCount", player.UserIDString, deaths);
                string color = GetDeathColor(deaths);

                InfoPanel.Call("SetPanelAttribute", "DeathCounter", "DeathCounterPanelText", "Content", content, player.UserIDString);
                InfoPanel.Call("SetPanelAttribute", "DeathCounter", "DeathCounterPanelText", "FontColor", color, player.UserIDString);
                InfoPanel.Call("RefreshPanel", "DeathCounter", "DeathCounterPanel", player.UserIDString);

                if (config.DebugMode)
                    LogInfo($"Updated panel for {player.displayName}: {deaths} deaths");
            }
            catch (Exception ex)
            {
                LogError($"Error updating panel for {player?.displayName}: {ex.Message}");
            }
        }

        private void UpdateAllPanels()
        {
            if (InfoPanel == null) return;

            try
            {
                foreach (var player in BasePlayer.activePlayerList)
                {
                    if (HasPermission(player, PERMISSION_USE))
                    {
                        UpdatePlayerPanel(player);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error updating all panels: {ex.Message}");
            }
        }

        private void ShowPanelToPlayer(BasePlayer player)
        {
            if (InfoPanel == null || player == null || !HasPermission(player, PERMISSION_USE)) return;
            
            try
            {
                UpdatePlayerPanel(player);
                InfoPanel.Call("ShowPanel", "DeathCounter", "DeathCounterPanel", player.UserIDString);

                if (config.DebugMode)
                    LogInfo($"Showing panel to {player.displayName}");
            }
            catch (Exception ex)
            {
                LogError($"Error showing panel to {player?.displayName}: {ex.Message}");
            }
        }

        private string GetDeathColor(int deaths)
        {
            // Color gradient based on death count
            if (deaths == 0) return "0.2 0.8 0.2 1"; // Green
            if (deaths <= 3) return "0.8 0.8 0.2 1"; // Yellow
            if (deaths <= 7) return "0.8 0.6 0.2 1"; // Orange
            if (deaths <= 15) return "0.8 0.3 0.2 1"; // Red-Orange
            if (deaths <= 25) return "0.8 0.2 0.2 1"; // Red
            return "0.6 0.1 0.1 1"; // Dark Red
        }

        #endregion

        #region Helper Methods

        private bool HasPermission(BasePlayer player, string permission)
        {
            return this.permission.UserHasPermission(player.UserIDString, permission);
        }

        private bool IsAdmin(BasePlayer player)
        {
            return player.IsAdmin || HasPermission(player, PERMISSION_ADMIN);
        }

        private void LogInfo(string message)
        {
            if (config.DebugMode)
                Puts($"[INFO] {message}");
        }

        private void LogWarning(string message)
        {
            PrintWarning($"[WARNING] {message}");
        }

        private void LogError(string message)
        {
            PrintError($"[ERROR] {message}");
        }

        #endregion

        #region Data Management

        private int GetPlayerDeaths(ulong playerId)
        {
            return playerDeaths.ContainsKey(playerId) ? playerDeaths[playerId] : 0;
        }

        void LoadData()
        {
            try
            {
                playerDeaths = Interface.Oxide.DataFileSystem.ReadObject<Dictionary<ulong, int>>("DeathCounter_Deaths") ?? new Dictionary<ulong, int>();
                LogInfo($"Loaded death data for {playerDeaths.Count} players");
            }
            catch (Exception ex)
            {
                LogError($"Error loading death data: {ex.Message}");
                playerDeaths = new Dictionary<ulong, int>();
            }
        }

        void SaveData()
        {
            try
            {
                Interface.Oxide.DataFileSystem.WriteObject("DeathCounter_Deaths", playerDeaths);
                if (config.DebugMode)
                    LogInfo($"Saved death data for {playerDeaths.Count} players");
            }
            catch (Exception ex)
            {
                LogError($"Error saving death data: {ex.Message}");
            }
        }

        #endregion

        #region Chat Commands

        [ChatCommand("deaths")]
        void DeathsCommand(BasePlayer player, string command, string[] args)
        {
            if (player == null) return;

            if (!HasPermission(player, PERMISSION_USE))
            {
                SendReply(player, Lang("NoPermission", player.UserIDString));
                return;
            }

            var deaths = GetPlayerDeaths(player.userID);
            SendReply(player, Lang("YourDeaths", player.UserIDString, deaths));
        }

        [ChatCommand("deathstop")]
        void DeathsTopCommand(BasePlayer player, string command, string[] args)
        {
            if (player == null) return;

            if (!HasPermission(player, PERMISSION_USE))
            {
                SendReply(player, Lang("NoPermission", player.UserIDString));
                return;
            }

            var topDeaths = playerDeaths
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            if (topDeaths.Count == 0)
            {
                SendReply(player, Lang("NoDeaths", player.UserIDString));
                return;
            }

            SendReply(player, Lang("TopDeaths", player.UserIDString));
            for (int i = 0; i < topDeaths.Count; i++)
            {
                var targetPlayer = BasePlayer.FindByID(topDeaths[i].Key);
                var playerName = targetPlayer?.displayName ?? Lang("UnknownPlayer", player.UserIDString);
                SendReply(player, Lang("TopDeathsEntry", player.UserIDString, i + 1, playerName, topDeaths[i].Value));
            }
        }

        [ChatCommand("deathpanel")]
        void DeathPanelCommand(BasePlayer player, string command, string[] args)
        {
            if (player == null) return;

            if (!HasPermission(player, PERMISSION_USE))
            {
                SendReply(player, Lang("NoPermission", player.UserIDString));
                return;
            }

            if (args.Length == 0)
            {
                SendReply(player, Lang("AvailableCommands", player.UserIDString));
                SendReply(player, Lang("CmdDeaths", player.UserIDString));
                SendReply(player, Lang("CmdDeathsTop", player.UserIDString));
                SendReply(player, Lang("CmdPanelShow", player.UserIDString));
                SendReply(player, Lang("CmdPanelHide", player.UserIDString));
                if (IsAdmin(player))
                    SendReply(player, Lang("CmdPanelReset", player.UserIDString));
                return;
            }

            switch (args[0].ToLower())
            {
                case "show":
                    if (InfoPanel != null)
                    {
                        ShowPanelToPlayer(player);
                        SendReply(player, Lang("PanelShown", player.UserIDString));
                    }
                    else
                    {
                        SendReply(player, Lang("InfoPanelNotLoaded", player.UserIDString));
                    }
                    break;

                case "hide":
                    if (InfoPanel != null)
                    {
                        InfoPanel.Call("HidePanel", "DeathCounter", "DeathCounterPanel", player.UserIDString);
                        SendReply(player, Lang("PanelHidden", player.UserIDString));
                    }
                    else
                    {
                        SendReply(player, Lang("InfoPanelNotLoaded", player.UserIDString));
                    }
                    break;

                case "reset":
                    if (!IsAdmin(player))
                    {
                        SendReply(player, Lang("NoPermission", player.UserIDString));
                        return;
                    }

                    if (args.Length > 1)
                    {
                        var targetPlayer = BasePlayer.Find(args[1]);
                        if (targetPlayer != null)
                        {
                            playerDeaths[targetPlayer.userID] = 0;
                            UpdatePlayerPanel(targetPlayer);
                            SendReply(player, Lang("DeathsResetTarget", player.UserIDString, targetPlayer.displayName));
                            SaveData();
                        }
                        else
                        {
                            SendReply(player, Lang("PlayerNotFound", player.UserIDString));
                        }
                    }
                    else
                    {
                        playerDeaths[player.userID] = 0;
                        UpdatePlayerPanel(player);
                        SendReply(player, Lang("DeathsReset", player.UserIDString));
                        SaveData();
                    }
                    break;

                default:
                    SendReply(player, Lang("UnknownCommand", player.UserIDString));
                    break;
            }
        }

        #endregion

        #region Console Commands

        [ConsoleCommand("deathcounter.reset")]
        void ResetDeathsConsole(ConsoleSystem.Arg arg)
        {
            if (arg.Player() != null && !IsAdmin(arg.Player())) 
            {
                arg.ReplyWith("Access denied. Admin permission required.");
                return;
            }

            try
            {
                if (arg.Args.Length > 0)
                {
                    var targetPlayer = BasePlayer.Find(arg.Args[0]);
                    if (targetPlayer != null)
                    {
                        playerDeaths[targetPlayer.userID] = 0;
                        UpdatePlayerPanel(targetPlayer);
                        arg.ReplyWith($"Death count reset for {targetPlayer.displayName}");
                        SaveData();
                    }
                    else
                    {
                        arg.ReplyWith("Player not found.");
                    }
                }
                else
                {
                    int playersReset = playerDeaths.Count;
                    playerDeaths.Clear();
                    SaveData();
                    arg.ReplyWith($"All death counts reset. Affected {playersReset} players.");
                    
                    // Update all active players
                    foreach (var player in BasePlayer.activePlayerList)
                    {
                        UpdatePlayerPanel(player);
                    }
                }
            }
            catch (Exception ex)
            {
                arg.ReplyWith($"Error resetting death counts: {ex.Message}");
                LogError($"Console command error: {ex.Message}");
            }
        }

        [ConsoleCommand("deathcounter.reload")]
        void ReloadPluginConsole(ConsoleSystem.Arg arg)
        {
            if (arg.Player() != null && !IsAdmin(arg.Player())) 
            {
                arg.ReplyWith("Access denied. Admin permission required.");
                return;
            }

            try
            {
                LoadConfig();
                
                // Reinitialize timer with new interval
                updateTimer?.Destroy();
                if (config.UpdateInterval > 0)
                {
                    updateTimer = timer.Repeat(config.UpdateInterval, 0, () => UpdateAllPanels());
                }
                
                arg.ReplyWith("DeathCounter configuration reloaded successfully.");
            }
            catch (Exception ex)
            {
                arg.ReplyWith($"Error reloading configuration: {ex.Message}");
                LogError($"Console reload error: {ex.Message}");
            }
        }

        [ConsoleCommand("deathcounter.status")]
        void StatusConsole(ConsoleSystem.Arg arg)
        {
            if (arg.Player() != null && !IsAdmin(arg.Player())) 
            {
                arg.ReplyWith("Access denied. Admin permission required.");
                return;
            }

            try
            {
                var totalPlayers = playerDeaths.Count;
                var totalDeaths = playerDeaths.Values.Sum();
                var infoPanelStatus = InfoPanel != null ? "Loaded" : "Not Found";
                var activePlayers = BasePlayer.activePlayerList.Count;

                arg.ReplyWith($"=== DeathCounter Status ===");
                arg.ReplyWith($"Plugin Version: 1.1.0");
                arg.ReplyWith($"InfoPanel Status: {infoPanelStatus}");
                arg.ReplyWith($"Total Players Tracked: {totalPlayers}");
                arg.ReplyWith($"Total Deaths Recorded: {totalDeaths}");
                arg.ReplyWith($"Active Players Online: {activePlayers}");
                arg.ReplyWith($"Update Interval: {config.UpdateInterval}s");
                arg.ReplyWith($"Debug Mode: {(config.DebugMode ? "Enabled" : "Disabled")}");
            }
            catch (Exception ex)
            {
                arg.ReplyWith($"Error getting status: {ex.Message}");
                LogError($"Console status error: {ex.Message}");
            }
        }

        #endregion
    }
}
