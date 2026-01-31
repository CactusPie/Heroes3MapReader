namespace Heroes3MapReader.Logic.Models.Enums;

/// <summary>
/// Spell types in Heroes 3
/// Based on VCMI spell definitions
/// </summary>
public enum SpellType
{
    // Adventure Spells (0-9)
    /// <summary>Summon Boat - Adventure spell to summon a boat</summary>
    SummonBoat = 0,
    /// <summary>Scuttle Boat - Adventure spell to destroy a boat</summary>
    ScuttleBoat = 1,
    /// <summary>Visions - Adventure spell to reveal puzzle map</summary>
    Visions = 2,
    /// <summary>View Earth - Adventure spell to view terrain</summary>
    ViewEarth = 3,
    /// <summary>Disguise - Adventure spell to hide army</summary>
    Disguise = 4,
    /// <summary>View Air - Adventure spell to view air</summary>
    ViewAir = 5,
    /// <summary>Fly - Adventure spell to fly over terrain</summary>
    Fly = 6,
    /// <summary>Water Walk - Adventure spell to walk on water</summary>
    WaterWalk = 7,
    /// <summary>Dimension Door - Adventure spell to teleport</summary>
    DimensionDoor = 8,
    /// <summary>Town Portal - Adventure spell to teleport to town</summary>
    TownPortal = 9,

    // Battlefield Obstacles (10-14)
    /// <summary>Quicksand - Combat spell creating quicksand</summary>
    Quicksand = 10,
    /// <summary>Land Mine - Combat spell creating a mine</summary>
    LandMine = 11,
    /// <summary>Force Field - Combat spell creating a force field</summary>
    ForceField = 12,
    /// <summary>Fire Wall - Combat spell creating a fire wall</summary>
    FireWall = 13,
    /// <summary>Earthquake - Combat spell destroying walls</summary>
    Earthquake = 14,

    // Direct Damage Spells (15-26)
    /// <summary>Magic Arrow - Basic damage spell</summary>
    MagicArrow = 15,
    /// <summary>Ice Bolt - Water damage spell</summary>
    IceBolt = 16,
    /// <summary>Lightning Bolt - Air damage spell</summary>
    LightningBolt = 17,
    /// <summary>Implosion - Earth damage spell</summary>
    Implosion = 18,
    /// <summary>Chain Lightning - Air damage spell hitting multiple targets</summary>
    ChainLightning = 19,
    /// <summary>Frost Ring - Water area damage spell</summary>
    FrostRing = 20,
    /// <summary>Fireball - Fire area damage spell</summary>
    Fireball = 21,
    /// <summary>Inferno - Fire area damage spell</summary>
    Inferno = 22,
    /// <summary>Meteor Shower - Fire area damage spell</summary>
    MeteorShower = 23,
    /// <summary>Death Ripple - Damage to living creatures</summary>
    DeathRipple = 24,
    /// <summary>Destroy Undead - Damage to undead creatures</summary>
    DestroyUndead = 25,
    /// <summary>Armageddon - Massive damage to all units</summary>
    Armageddon = 26,

    // Protection & Shield Spells (27-34, 36)
    /// <summary>Shield - Reduces ranged damage</summary>
    Shield = 27,
    /// <summary>Air Shield - Reduces ranged damage</summary>
    AirShield = 28,
    /// <summary>Fire Shield - Reflects melee damage as fire</summary>
    FireShield = 29,
    /// <summary>Protection from Air - Reduces air spell damage</summary>
    ProtectionFromAir = 30,
    /// <summary>Protection from Fire - Reduces fire spell damage</summary>
    ProtectionFromFire = 31,
    /// <summary>Protection from Water - Reduces water spell damage</summary>
    ProtectionFromWater = 32,
    /// <summary>Protection from Earth - Reduces earth spell damage</summary>
    ProtectionFromEarth = 33,
    /// <summary>Anti-Magic - Makes unit immune to spells</summary>
    AntiMagic = 34,

    // Utility Spells (35, 37-40)
    /// <summary>Dispel - Removes effects from units</summary>
    Dispel = 35,
    /// <summary>Magic Mirror - Reflects spells</summary>
    MagicMirror = 36,
    /// <summary>Cure - Removes negative effects</summary>
    Cure = 37,
    /// <summary>Resurrection - Brings units back to life</summary>
    Resurrection = 38,
    /// <summary>Animate Dead - Temporarily resurrects undead</summary>
    AnimateDead = 39,
    /// <summary>Sacrifice - Sacrifices units to resurrect others</summary>
    Sacrifice = 40,

    // Buff Spells (41, 43-44, 46, 48-49, 51, 53, 55-56, 58)
    /// <summary>Bless - Increases damage (max damage)</summary>
    Bless = 41,
    /// <summary>Bloodlust - Increases attack</summary>
    Bloodlust = 43,
    /// <summary>Precision - Increases ranged attack</summary>
    Precision = 44,
    /// <summary>Stone Skin - Increases defense</summary>
    StoneSkin = 46,
    /// <summary>Prayer - Increases attack and defense</summary>
    Prayer = 48,
    /// <summary>Mirth - Increases morale</summary>
    Mirth = 49,
    /// <summary>Fortune - Increases luck</summary>
    Fortune = 51,
    /// <summary>Haste - Increases speed</summary>
    Haste = 53,
    /// <summary>Slayer - Greatly increases attack</summary>
    Slayer = 55,
    /// <summary>Frenzy - Greatly increases attack, removes defense</summary>
    Frenzy = 56,
    /// <summary>Counterstrike - Allows unlimited retaliation</summary>
    Counterstrike = 58,

    // Debuff Spells (42, 45, 47, 50, 52, 54, 59-62)
    /// <summary>Curse - Decreases damage (min damage)</summary>
    Curse = 42,
    /// <summary>Weakness - Decreases attack</summary>
    Weakness = 45,
    /// <summary>Disrupting Ray - Decreases defense</summary>
    DisruptingRay = 47,
    /// <summary>Sorrow - Decreases morale</summary>
    Sorrow = 50,
    /// <summary>Misfortune - Decreases luck</summary>
    Misfortune = 52,
    /// <summary>Slow - Decreases speed</summary>
    Slow = 54,
    /// <summary>Berserk - Makes unit attack allies</summary>
    Berserk = 59,
    /// <summary>Hypnotize - Takes control of enemy unit</summary>
    Hypnotize = 60,
    /// <summary>Forgetfulness - Unit cannot cast spells</summary>
    Forgetfulness = 61,
    /// <summary>Blind - Unit cannot move or attack</summary>
    Blind = 62,

    // Advanced Utility Spells (63-69)
    /// <summary>Teleport - Teleports unit on battlefield</summary>
    Teleport = 63,
    /// <summary>Remove Obstacle - Removes battlefield obstacle</summary>
    RemoveObstacle = 64,
    /// <summary>Clone - Creates a copy of a unit</summary>
    Clone = 65,
    /// <summary>Summon Fire Elemental - Summons fire elementals</summary>
    SummonFireElemental = 66,
    /// <summary>Summon Earth Elemental - Summons earth elementals</summary>
    SummonEarthElemental = 67,
    /// <summary>Summon Water Elemental - Summons water elementals</summary>
    SummonWaterElemental = 68,
    /// <summary>Summon Air Elemental - Summons air elementals</summary>
    SummonAirElemental = 69,

    // Special Creature Abilities (70-81) - AB/SoD+
    /// <summary>Stone Gaze - Basilisk/Gorgon petrification ability</summary>
    StoneGaze = 70,
    /// <summary>Poison - Wyvern poison ability</summary>
    Poison = 71,
    /// <summary>Bind - Scorpicore binding ability</summary>
    Bind = 72,
    /// <summary>Disease - Zombie disease ability</summary>
    Disease = 73,
    /// <summary>Paralyze - Paralysis ability</summary>
    Paralyze = 74,
    /// <summary>Age - Ghost Dragon aging ability</summary>
    Age = 75,
    /// <summary>Death Cloud - Lich death cloud ability</summary>
    DeathCloud = 76,
    /// <summary>Thunderbolt - Thunderbird ability</summary>
    Thunderbolt = 77,
    /// <summary>Dispel Helpful - Magic Elemental ability to remove buffs</summary>
    DispelHelpful = 78,
    /// <summary>Death Stare - Mighty Gorgon ability</summary>
    DeathStare = 79,
    /// <summary>Acid Breath - Dragon acid breath ability</summary>
    AcidBreath = 80,
    /// <summary>Acid Breath Damage - Acid breath damage effect</summary>
    AcidBreathDamage = 81,
}
