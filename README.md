# Entity Interaction System

Unity version : 6.5 (6000.5.9f1)

Game scene - 'Assets/Scenes/SampleScene'

## Architecture
- 'ActionConfig' \ 'FactionConfig' \ 'UnitConfig' - shared ScriptableObject. Can be created by 'right click' GameTool/Create/
- New faction and base unit can be created by - GameTool/CreateNewFaction
- New effects will be created by GameTool/UpdateEffects - this tool creates all new Effects.
- 'Selection' - availability + pick best.
- 'Interaction Controller' - start\tick\complete\cancel - action with run time Interactions, also added methods : OnStart, OnEnd, OnCancel, OnComplete to draw debug lines and logs (can be changed to Analytics)
- Unit - only view of Interaction in system, had - IChangeColorEffector to change visual.
- Unit -> Status - not finished state machine to show unit state. 
	CurrentState - showing generic state of entity - None, Idle, Dead. 
	StatusEffect - showing all status on entity : Shielded - effect status, Interaction - another entity making interaction to this entity.
	Revived - temp state to change Status from Dead -> Idle.
	InAction - entity perfoming any action.

ActionEffectRuntime - this is runtime element of all effects.
	OnStart, OnUpdate, OnCancel, OnEnd - called from InteractionController.
	ShouldInterrupt - called from effect when this effect needs to be interrupted while effect working (enemy status changed \ enemy Died).
	
ActionConfig - has all needed parameters, but all parameters for UI are not used, only work in Debug.Logs.
	FactionPair - factionPairs allow to use some action for any Factions.
	effect - any SO with base BaseEffect
	isSelfCast - in 2.Domain model -> Cannot interact with itself was described. This works, but i added this as parameter, because it can be used by 'Buff' effect.
	canCastOnDead - used to revive any dead entity, base rule, can use Action only on entity with Status - Idle.

FactionConfig - has factionName and description but they are not used. It can be used for showing Name on "Base" and in "Ui".
	units -> UnitData, used for set up base Faction entity that will be spawned on game start. unitConfig - for entity config, overriddenPrefab - if need to change visual of entity.
	basePrefab - used as regular prefab for all entity if it is not ovveridden.
	
UnitConfig - base unit config.
	factionConfig - faction associated.
	actions - entity actions.

## GamePlay

In scene - 2 Faction (One and NewFraction).

Faction One - 1 attacker, 2 pinger (just waiting), 3 self shield caster.
Faction NewFaction - 1 cast shield to any unit, 2 and 3 - wait and cast resurrection to dead unit.

## Notes
No ui implemented.
In future UI can be connected by DI \ EventBus by Faction Controller.
In WorldController - game root start.
Fraction Controller - support multiple Fractions.
All Scriptable object used [SerializeField] private to public get property. In future if need to add Localization - it can be used by this property.
I did not use a custom editor for Action Config, but it will be very good to use them with Is Self Cast - if it 'true' - hide - 'Faction Pairs'.
If Action Type Immediate - hide Action Duration field.
I prefer to use ScriptableObject because i have my own assets to change ScriptableObject data from the server by Json. 
It's very useful, because if need remote config - you just use already created SO.
In build it will be -> Start game -> GET remote config -> ApplyConfig - all SO will be changed on this run. 
If internet not available - use default (in build config).


##Known issue
Initially, I misspelled 'Faction' as 'Fraction' in the codebase.
I noticed this late after finished task,
I fixed the naming in the ScriptableObject and the Scene, but deliberately skipped refactoring the core scripts to save time and avoid regression testing.