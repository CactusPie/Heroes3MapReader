using System.Text;
using Heroes3MapReader.Logic.MapSpecificationLogic;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.Logic;

/// <summary>
/// Static class for reading and skipping HotA9+ script data from map files.
/// </summary>
internal static class HotaScriptReader
{
    /// <summary>
    /// Skips the HotA9+ scripts section of the map file.
    /// This section contains event scripting data that we don't need for map info extraction.
    /// </summary>
    public static void SkipHotaScripts(BinaryReader reader, MapSpecification features, Encoding encoding)
    {
        if (!features.VersionFlags.IsHotA9OrHigher)
        {
            return;
        }

        bool eventsSystemActive = reader.ReadBoolean();
        if (!eventsSystemActive)
        {
            return;
        }

        // Load event lists: hero event, player event, town event, quest event
        for (int listIndex = 0; listIndex < 4; listIndex++)
        {
            int eventsCount = reader.ReadInt32();
            for (int i = 0; i < eventsCount; i++)
            {
                reader.ReadInt32(); // eventID
                SkipHotaScriptActions(reader, encoding);
                BinaryReaderExtensions.ReadString(reader, encoding); // eventName
            }
        }

        // Next ID values
        reader.ReadInt32(); // nextVariableID
        reader.ReadInt32(); // nextHeroEventID
        reader.ReadInt32(); // nextPlayerEventID
        reader.ReadInt32(); // nextTownEventID
        reader.ReadInt32(); // nextQuestEventID

        // Variables
        int variablesCount = reader.ReadInt32();
        for (int i = 0; i < variablesCount; i++)
        {
            reader.ReadInt32(); // uniqueID
            BinaryReaderExtensions.ReadString(reader, encoding); // variableID
            reader.ReadBoolean(); // save in campaign
            reader.ReadBoolean(); // import from prev map
            reader.ReadInt32(); // initialValue
        }

        // Load event maps: hero event, player event, town event, quest event, variable
        for (int mapIndex = 0; mapIndex < 5; mapIndex++)
        {
            int mappingSize = reader.ReadInt32();
            for (int i = 0; i < mappingSize; i++)
            {
                reader.ReadInt32(); // UID of event
            }
        }
    }

    private static void SkipHotaScriptActions(BinaryReader reader, Encoding encoding)
    {
        reader.ReadInt32(); // event type (expected: 1)
        reader.ReadByte(); // unknown (expected: 0)

        int actionsCount = reader.ReadInt32();
        for (int j = 0; j < actionsCount; j++)
        {
            var actionType = (HotaScriptActionType)reader.ReadInt32();
            SkipHotaScriptAction(reader, actionType, encoding);
        }
    }

    private static void SkipHotaScriptAction(BinaryReader reader, HotaScriptActionType actionType, Encoding encoding)
    {
        switch (actionType)
        {
            case HotaScriptActionType.ConditionalChain:
                while (true)
                {
                    SkipHotaScriptCondition(reader);
                    SkipHotaScriptActions(reader, encoding);
                    reader.ReadBoolean(); // unknown (expected: 1)
                    int continueChain = reader.ReadInt32();
                    if (continueChain == 0)
                    {
                        break;
                    }
                }

                reader.ReadInt32(); // unknown
                break;

            case HotaScriptActionType.SetVariableConditional:
                reader.ReadInt32(); // variableID
                SkipHotaScriptCondition(reader);
                SkipHotaScriptExpression(reader);
                SkipHotaScriptExpression(reader);
                break;

            case HotaScriptActionType.ModifyVariable:
                reader.ReadInt32(); // variableID
                reader.ReadByte(); // mode
                SkipHotaScriptExpressionInternal(reader);
                break;

            case HotaScriptActionType.Resources:
                reader.ReadByte(); // mode
                for (int i = 0; i < 7; i++)
                {
                    SkipHotaScriptExpression(reader);
                }

                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.RemoveCurrentObjectOrFinishQuest:
                // No data
                break;

            case HotaScriptActionType.ShowRewardsMessage:
                BinaryReaderExtensions.ReadString(reader, encoding); // textID
                SkipHotaScriptActions(reader, encoding);
                break;

            case HotaScriptActionType.QuestAction:
                SkipHotaScriptCondition(reader);
                BinaryReaderExtensions.ReadString(reader, encoding); // proposalTextID
                BinaryReaderExtensions.ReadString(reader, encoding); // progressionTextID
                BinaryReaderExtensions.ReadString(reader, encoding); // completionTextID
                BinaryReaderExtensions.ReadString(reader, encoding); // hintTextID
                SkipHotaScriptActions(reader, encoding);
                reader.ReadBoolean(); // unknown
                break;

            case HotaScriptActionType.Creatures:
                reader.ReadBoolean(); // takeCreatures
                reader.ReadInt32(); // creature
                SkipHotaScriptExpression(reader);
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.Artifact:
                reader.ReadBoolean(); // takeArtifact
                reader.ReadInt32(); // artifact
                reader.ReadInt32(); // scrollSpellID
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.ConstructBuilding:
                reader.ReadInt32(); // buildingID
                reader.ReadInt16(); // unknownA (faction ID?)
                reader.ReadInt16(); // unknownB (faction building ID?)
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.SetQuestHint:
                BinaryReaderExtensions.ReadString(reader, encoding); // messageTextID
                int hintImageCount = reader.ReadInt32();
                for (int i = 0; i < hintImageCount; i++)
                {
                    reader.ReadInt32(); // imageType
                    reader.ReadInt32(); // imageSubtype
                    SkipHotaScriptExpression(reader);
                }

                reader.ReadBoolean(); // showInLog
                break;

            case HotaScriptActionType.ShowQuestion:
                int imageShowType = reader.ReadByte();
                BinaryReaderExtensions.ReadString(reader, encoding); // messageTextID
                SkipHotaScriptActions(reader, encoding);
                SkipHotaScriptActions(reader, encoding);

                if (imageShowType == 2)
                {
                    SkipHotaScriptActions(reader, encoding);
                }

                int numberOfImages = 2;
                if (imageShowType == 0 || imageShowType == 3)
                {
                    numberOfImages = reader.ReadInt32();
                }

                for (int i = 0; i < numberOfImages; i++)
                {
                    reader.ReadInt32(); // imageType
                    reader.ReadInt32(); // imageSubtype
                    SkipHotaScriptExpression(reader);
                }

                if (imageShowType == 1 || imageShowType == 2)
                {
                    reader.ReadBoolean(); // showOrBetweenImages
                    reader.ReadInt32(); // unknown
                }

                break;

            case HotaScriptActionType.Conditional:
                SkipHotaScriptCondition(reader);
                SkipHotaScriptActions(reader, encoding);
                SkipHotaScriptActions(reader, encoding);
                break;

            case HotaScriptActionType.CreaturesToHire:
                reader.ReadInt32(); // dwelling
                SkipHotaScriptExpression(reader); // amount
                reader.ReadInt32(); // unknown
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.Spell:
                reader.ReadInt32(); // spellID
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.Experience:
                SkipHotaScriptExpression(reader);
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.SpellPoints:
                SkipHotaScriptExpression(reader);
                reader.ReadInt32(); // mode
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.MovementPoints:
                SkipHotaScriptExpression(reader);
                reader.ReadInt32(); // mode
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.PrimarySkill:
                SkipHotaScriptExpression(reader);
                reader.ReadInt32(); // skillToGive
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.SecondarySkill:
                reader.ReadInt32(); // masteryLevel
                reader.ReadInt32(); // skill
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.Luck:
                reader.ReadInt32(); // amount
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.Morale:
                reader.ReadInt32(); // amount
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.StartCombat:
                for (int i = 0; i < 7; i++)
                {
                    SkipHotaScriptExpression(reader);
                    reader.ReadInt32(); // creature
                }

                break;

            case HotaScriptActionType.ExecuteEvent:
                reader.ReadInt32(); // eventType
                reader.ReadInt32(); // eventID
                break;

            case HotaScriptActionType.WarMachine:
                reader.ReadBoolean(); // takeMachine
                reader.ReadInt32(); // machine
                reader.ReadBytes(4); // garbage
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.Spellbook:
                reader.ReadBoolean(); // takeSpellbook
                reader.ReadBytes(8); // garbage
                reader.ReadBoolean(); // showMessage
                break;

            case HotaScriptActionType.DisableEvent:
                // No data
                break;

            case HotaScriptActionType.LoopFor:
                SkipHotaScriptActions(reader, encoding); // loop body
                SkipHotaScriptExpression(reader); // initial value
                SkipHotaScriptExpression(reader); // final value
                reader.ReadInt32(); // variableID
                break;

            case HotaScriptActionType.ShowMessage:
                BinaryReaderExtensions.ReadString(reader, encoding); // textID
                int msgImageCount = reader.ReadInt32();
                for (int i = 0; i < msgImageCount; i++)
                {
                    reader.ReadInt32(); // imageType
                    reader.ReadInt32(); // imageSubtype
                    SkipHotaScriptExpression(reader);
                }

                break;

            default:
                throw new InvalidDataException($"Unknown HotA script action type: {actionType}");
        }
    }

    private static void SkipHotaScriptCondition(BinaryReader reader)
    {
        reader.ReadBoolean(); // unknown (expected: true)
        SkipHotaScriptConditionInternal(reader);
    }

    private static void SkipHotaScriptConditionInternal(BinaryReader reader)
    {
        int conditionCode = reader.ReadInt32();
        switch (conditionCode)
        {
            case 0: // CONSTANT
                reader.ReadBoolean(); // value
                break;

            case 1: // ALL_OF
            case 2: // ANY_OF
                int argumentsCount = reader.ReadInt32();
                for (int i = 0; i < argumentsCount; i++)
                {
                    SkipHotaScriptConditionInternal(reader);
                }

                break;

            case 3: // LESSER
            case 4: // GREATER
            case 5: // EQUAL
            case 8: // GREATER_OR_EQUAL
            case 9: // LESSER_OR_EQUAL
            case 10: // NOT_EQUAL
                SkipHotaScriptExpression(reader);
                SkipHotaScriptExpression(reader);
                break;

            case 6: // NOT
                SkipHotaScriptCondition(reader);
                break;

            case 7: // HAS_ARTIFACT
                reader.ReadInt32(); // artifact
                reader.ReadInt32(); // scrollSpellID
                break;

            case 11: // CURRENT_PLAYER
                reader.ReadInt32(); // expectedPlayer
                break;

            case 12: // HERO_OWNER
                reader.ReadInt32(); // expectedHero
                reader.ReadInt32(); // expectedPlayer
                break;

            case 14: // PLAYER_DEFEATED_MONSTER
                reader.ReadInt32(); // expectedPlayer
                reader.ReadInt32(); // targetObjectID
                break;

            case 15: // PLAYER_DEFEATED_HERO
                reader.ReadInt32(); // expectedPlayer
                reader.ReadInt32(); // targetObjectID
                break;

            case 16: // HERO_SECONDARY_SKILL
                reader.ReadInt32(); // expectedSkill
                reader.ReadInt32(); // expectedMastery
                break;

            case 17: // PLAYER_DEFEATED
                reader.ReadInt32(); // expectedPlayer
                break;

            case 18: // PLAYER_OWNS_TOWN
                reader.ReadInt32(); // expectedPlayer
                reader.ReadInt32(); // targetObjectID
                break;

            case 19: // PLAYER_IS_HUMAN
                reader.ReadInt32(); // expectedPlayer
                break;

            case 20: // PLAYER_STARTING_FACTION
                reader.ReadInt32(); // expectedPlayer
                reader.ReadInt32(); // expectedFaction
                break;

            case 21: // TOWN_IS_NEUTRAL
                // No data
                break;

            default:
                throw new InvalidDataException($"Unknown HotA script condition code: {conditionCode}");
        }
    }

    private static void SkipHotaScriptExpression(BinaryReader reader)
    {
        bool isExpression = reader.ReadBoolean();
        if (!isExpression)
        {
            reader.ReadInt32(); // rawValue
            return;
        }

        SkipHotaScriptExpressionInternal(reader);
    }

    private static void SkipHotaScriptExpressionInternal(BinaryReader reader)
    {
        reader.ReadBoolean(); // unknown (expected: true)

        int expressionCode = reader.ReadInt32();
        switch (expressionCode)
        {
            case 0: // INTEGER_VALUE
                reader.ReadInt32(); // value
                break;

            case 1: // VARIABLE_VALUE
                reader.ReadInt32(); // variableIndex
                break;

            case 2: // NEGATE
                reader.ReadInt32(); // unknown
                SkipHotaScriptExpression(reader);
                break;

            case 3: // ADD
            case 4: // SUBSTRACT
            case 6: // MULTIPLY
            case 7: // DIVIDE
            case 8: // REMAINDER
                SkipHotaScriptExpressionInternal(reader);
                SkipHotaScriptExpressionInternal(reader);
                break;

            case 5: // RESOURCE
                reader.ReadByte(); // player (special value for current player)
                reader.ReadInt32(); // resource
                break;

            case 9: // CREATURE_COUNT_IN_ARMY
                reader.ReadInt32(); // creature
                break;

            case 10: // CURRENT_DIFFICULTY
                // No data
                break;

            case 11: // COMPARE_DIFFICULTY
                reader.ReadInt32(); // difficultyToCompare
                break;

            case 12: // CURRENT_DATE
                // No data
                break;

            case 13: // HERO_EXPERIENCE
                // No data
                break;

            case 14: // HERO_LEVEL
                // No data
                break;

            case 15: // HERO_PRIMARY_SKILL
                reader.ReadInt32(); // skill
                break;

            case 16: // RANDOM_NUMBER
                SkipHotaScriptExpression(reader);
                SkipHotaScriptExpression(reader);
                break;

            case 17: // HERO_OWNED_ARTIFACTS
                reader.ReadInt32(); // artifact
                reader.ReadInt32(); // scrollSpell
                break;

            default:
                throw new InvalidDataException($"Unknown HotA script expression code: {expressionCode}");
        }
    }
}
