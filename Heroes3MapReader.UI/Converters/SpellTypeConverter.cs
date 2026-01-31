using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Heroes3MapReader.Logic.Models.Enums;

namespace Heroes3MapReader.UI.Converters;

public sealed class SpellTypeConverter : IValueConverter
{
    public static readonly SpellTypeConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            SpellType.SummonBoat => "Summon Boat",
            SpellType.ScuttleBoat => "Scuttle Boat",
            SpellType.Visions => "Visions",
            SpellType.ViewEarth => "View Earth",
            SpellType.Disguise => "Disguise",
            SpellType.ViewAir => "View Air",
            SpellType.Fly => "Fly",
            SpellType.WaterWalk => "Water Walk",
            SpellType.DimensionDoor => "Dimension Door",
            SpellType.TownPortal => "Town Portal",
            SpellType.Quicksand => "Quicksand",
            SpellType.LandMine => "Land Mine",
            SpellType.ForceField => "Force Field",
            SpellType.FireWall => "Fire Wall",
            SpellType.Earthquake => "Earthquake",
            SpellType.MagicArrow => "Magic Arrow",
            SpellType.IceBolt => "Ice Bolt",
            SpellType.LightningBolt => "Lightning Bolt",
            SpellType.Implosion => "Implosion",
            SpellType.ChainLightning => "Chain Lightning",
            SpellType.FrostRing => "Frost Ring",
            SpellType.Fireball => "Fireball",
            SpellType.Inferno => "Inferno",
            SpellType.MeteorShower => "Meteor Shower",
            SpellType.DeathRipple => "Death Ripple",
            SpellType.DestroyUndead => "Destroy Undead",
            SpellType.Armageddon => "Armageddon",
            SpellType.Shield => "Shield",
            SpellType.AirShield => "Air Shield",
            SpellType.FireShield => "Fire Shield",
            SpellType.ProtectionFromAir => "Protection from Air",
            SpellType.ProtectionFromFire => "Protection from Fire",
            SpellType.ProtectionFromWater => "Protection from Water",
            SpellType.ProtectionFromEarth => "Protection from Earth",
            SpellType.AntiMagic => "Anti-Magic",
            SpellType.Dispel => "Dispel",
            SpellType.MagicMirror => "Magic Mirror",
            SpellType.Cure => "Cure",
            SpellType.Resurrection => "Resurrection",
            SpellType.AnimateDead => "Animate Dead",
            SpellType.Sacrifice => "Sacrifice",
            SpellType.Bless => "Bless",
            SpellType.Bloodlust => "Bloodlust",
            SpellType.Precision => "Precision",
            SpellType.StoneSkin => "Stone Skin",
            SpellType.Prayer => "Prayer",
            SpellType.Mirth => "Mirth",
            SpellType.Fortune => "Fortune",
            SpellType.Haste => "Haste",
            SpellType.Slayer => "Slayer",
            SpellType.Frenzy => "Frenzy",
            SpellType.Counterstrike => "Counterstrike",
            SpellType.Curse => "Curse",
            SpellType.Weakness => "Weakness",
            SpellType.DisruptingRay => "Disrupting Ray",
            SpellType.Sorrow => "Sorrow",
            SpellType.Misfortune => "Misfortune",
            SpellType.Slow => "Slow",
            SpellType.Berserk => "Berserk",
            SpellType.Hypnotize => "Hypnotize",
            SpellType.Forgetfulness => "Forgetfulness",
            SpellType.Blind => "Blind",
            SpellType.Teleport => "Teleport",
            SpellType.RemoveObstacle => "Remove Obstacle",
            SpellType.Clone => "Clone",
            SpellType.SummonFireElemental => "Summon Fire Elemental",
            SpellType.SummonEarthElemental => "Summon Earth Elemental",
            SpellType.SummonWaterElemental => "Summon Water Elemental",
            SpellType.SummonAirElemental => "Summon Air Elemental",
            SpellType.StoneGaze => "Stone Gaze",
            SpellType.Poison => "Poison",
            SpellType.Bind => "Bind",
            SpellType.Disease => "Disease",
            SpellType.Paralyze => "Paralyze",
            SpellType.Age => "Age",
            SpellType.DeathCloud => "Death Cloud",
            SpellType.Thunderbolt => "Thunderbolt",
            SpellType.DispelHelpful => "Dispel Helpful",
            SpellType.DeathStare => "Death Stare",
            SpellType.AcidBreath => "Acid Breath",
            SpellType.AcidBreathDamage => "Acid Breath Damage",
            _ => value?.ToString() ?? "Unknown",
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
