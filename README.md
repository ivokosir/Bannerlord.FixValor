# Fix Valor
In Bannerlord the only way to get valor is to fight battles against the odds, but this is broken in current versions of the game. This mod fixes that!

## Details
In newer changes `PlayerEncounter` class calls a function `MapEvent.RecalculateStrengthOfSides()` while
`EncounterState` is still set to `PrepareResults`, but the code that calculates valor xp is connected to
a event that fires after that.

The result of this is:

* strength of a opponent side will always be 0 because you are victorious
* you gain valor only on victory
* no valor for you

The fix in this mod is implemented by not using `CampaignEvents.OnPlayerBattleEndEvent`, but using
`CampaignEvents.OnMissionEndedEvent` which is fired before previous event. This might result in unintended
change that a player now needs to play the battle to get the valor, but to be honest that feels fitting
(and is probably the way _TaleWorlds_ wanted to implement it).
