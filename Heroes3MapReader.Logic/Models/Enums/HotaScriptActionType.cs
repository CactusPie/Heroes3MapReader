namespace Heroes3MapReader.Logic.Models.Enums;

/// <summary>
///
/// </summary>
public enum HotaScriptActionType
{
    /// <summary>
    /// Represents a conditional chain action.
    /// </summary>
    ConditionalChain = 1,

    /// <summary>
    /// Sets a variable conditionally.
    /// </summary>
    SetVariableConditional = 2,

    /// <summary>
    /// Modifies a variable.
    /// </summary>
    ModifyVariable = 3,

    /// <summary>
    /// Grants or removes resources.
    /// </summary>
    Resources = 4,

    /// <summary>
    /// Removes the current object or finishes a quest.
    /// </summary>
    RemoveCurrentObjectOrFinishQuest = 5,

    /// <summary>
    /// Shows a rewards message.
    /// </summary>
    ShowRewardsMessage = 6,

    /// <summary>
    /// Executes a quest action.
    /// </summary>
    QuestAction = 7,

    /// <summary>
    /// Grants or modifies creatures.
    /// </summary>
    Creatures = 8,

    /// <summary>
    /// Grants or modifies an artifact.
    /// </summary>
    Artifact = 9,

    /// <summary>
    /// Constructs a building.
    /// </summary>
    ConstructBuilding = 10,

    /// <summary>
    /// Sets a quest hint.
    /// </summary>
    SetQuestHint = 11,

    /// <summary>
    /// Shows a question to the player.
    /// </summary>
    ShowQuestion = 12,

    /// <summary>
    /// Represents a conditional action.
    /// </summary>
    Conditional = 13,

    /// <summary>
    /// Allows creatures to be hired.
    /// </summary>
    CreaturesToHire = 14,

    /// <summary>
    /// Grants or modifies a spell.
    /// </summary>
    Spell = 15,

    /// <summary>
    /// Grants experience points.
    /// </summary>
    Experience = 16,

    /// <summary>
    /// Grants spell points.
    /// </summary>
    SpellPoints = 17,

    /// <summary>
    /// Grants movement points.
    /// </summary>
    MovementPoints = 18,

    /// <summary>
    /// Grants or modifies a primary skill.
    /// </summary>
    PrimarySkill = 19,

    /// <summary>
    /// Grants or modifies a secondary skill.
    /// </summary>
    SecondarySkill = 20,

    /// <summary>
    /// Modifies luck.
    /// </summary>
    Luck = 21,

    /// <summary>
    /// Modifies morale.
    /// </summary>
    Morale = 22,

    /// <summary>
    /// Starts combat.
    /// </summary>
    StartCombat = 23,

    /// <summary>
    /// Executes an event.
    /// </summary>
    ExecuteEvent = 24,

    /// <summary>
    /// Grants or modifies a war machine.
    /// </summary>
    WarMachine = 25,

    /// <summary>
    /// Grants a spellbook.
    /// </summary>
    Spellbook = 26,

    /// <summary>
    /// Disables an event.
    /// </summary>
    DisableEvent = 27,

    /// <summary>
    /// Starts a loop for a specified number of times.
    /// </summary>
    LoopFor = 28,

    /// <summary>
    /// Shows a message to the player.
    /// </summary>
    ShowMessage = 29,
}