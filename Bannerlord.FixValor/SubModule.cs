using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.FixValor
{
    internal class FixValorAfterBattleBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, this.OnMissionEnded);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnMissionEnded(IMission mission)
        {
            MapEvent mapEvent = MapEvent.PlayerMapEvent;
            if (mapEvent == null) return;
            MapEventSide playerSide = mapEvent.Winner;
            if (playerSide == null || !playerSide.IsMainPartyAmongParties()) return;

            TraitLevelingHelper.OnBattleWon(mapEvent, playerSide.GetPlayerPartyContributionRate());
        }
    }

    public class SubModule : MBSubModuleBase
    {
        private static readonly Harmony Harmony = new("Bannerlord.FixValor");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Harmony.TryPatch(
                AccessTools2.DeclaredMethod("TaleWorlds.CampaignSystem.CampaignBehaviors.PlayerVariablesBehavior:OnPlayerBattleEnd"),
                prefix: AccessTools2.DeclaredMethod(typeof(SubModule), nameof(SkipMethod)));
        }

        private static bool SkipMethod()
        {
            return false;
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (gameStarter is CampaignGameStarter campaignGameStarter)
            {
                campaignGameStarter.AddBehavior(new FixValorAfterBattleBehavior());
            }
        }
    }
}
