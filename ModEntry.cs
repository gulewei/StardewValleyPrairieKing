using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Minigames;

namespace StardewValleyPrairieKing
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
         ** Public methods
         *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Player.Warped += OnPlayerWarped;
        }

        /*********
         ** Private methods
         *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnPlayerWarped(object? sender, WarpedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // Monitor.Log($">>>> {Game1.player.Name} now in {e.NewLocation.Name}.", LogLevel.Debug);

            // 进入酒吧
            if (e.OldLocation.Name != "Saloon" && e.NewLocation.Name == "Saloon")
            {
                Helper.Events.GameLoop.OneSecondUpdateTicked += OnSaloonOneSecondUpdateTicked;
            }
        }

        private void OnSaloonOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs args)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            var minigame = Game1.currentMinigame;
            if (minigame is not AbigailGame abigail || AbigailGame.onStartMenu) return;

            InitAbigailGamePowerups(abigail);
            Helper.Events.GameLoop.OneSecondUpdateTicked -= OnSaloonOneSecondUpdateTicked;
        }

        private static void InitAbigailGamePowerups(AbigailGame abigail)
        {
            // 开始草原大王
            // abigail.powerupDuration *= 1000;
            // abigail.usePowerup(AbigailGame.POWERUP_SPEED);
            // abigail.usePowerup(AbigailGame.POWERUP_SPREAD);
            var duration = abigail.powerupDuration * 1000;
            AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(AbigailGame.POWERUP_SPEED, GetRandomPoint(), duration));
            AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(AbigailGame.POWERUP_SPREAD, GetRandomPoint(), duration));
            // Monitor.Log($">>>> 开始享受吧.", LogLevel.Debug);
        }

        private static Point GetRandomPoint()
        {
            var rnd = new Random();
            var x = rnd.Next(1, 15) * AbigailGame.TileSize;
            var y = rnd.Next(1, 15) * AbigailGame.TileSize;
            return new Point(x, y);
        }
    }
}