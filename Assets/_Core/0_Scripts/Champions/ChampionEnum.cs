using UnityEngine;
namespace DucDevGame
{

    public enum ChampionName
    {
        JarvanIV = 0,
        Poppy = 1,
        Vayne = 2,
        Garen = 3,
        Galio = 4,
    }

    public enum OriginType
    {
        None = 0,
        // Regions (Khu vực chính)
        Demacia = 1,
        Noxus = 2,
        Ionia = 3,
        Freljord = 4,
        Shurima = 5,
        Void = 6,
        Piltover = 7,
        Zaun = 8,
        ShadowIsles = 9,
        Targon = 10,
        Ixtal = 11,
        Bilgewater = 12,
        Yordle = 13,

        // Unique Origins (Tộc đặc biệt thường dành cho tướng 5-cost)
        Ascendant = 14,      // Xerath (Charms)
        Assimilator = 15,    // Kai'Sa (AD/AP Adaptation)
        Blacksmith = 16,     // Ornn (Artifact Forging)
        Caretaker = 17,      // Bard (Free Rerolls/Units)
        Chainbreaker = 18,   // Sylas (Ability Rotation)
        Chronokeeper = 19,   // Zilean (XP Storage)
        DarkChild = 20,      // Annie (Tibbers)
        Darkin = 21,         // Aatrox/Zaahen (Omnivamp)
        Dragonborn = 22,     // Shyvana (Transform)
        Wanderer = 23        // Ryze (World Walker)
    }

    public enum ClassType
    {
        None = 0,
        // Frontline (Chống chịu)
        Warden = 1,         // Shielding (Hộ vệ)
        Bruiser = 2,        // Bonus Health (Đấu sĩ)
        Defender = 3,       // Armor/MR (Vệ binh)
        Juggernaut = 4,     // Durability/DR (Đại chiến binh)
        Bastion = 5,        // Defense (Can trường)

        // Magic Damage (Phép thuật)
        Arcanist = 6,       // Team AP (Pháp sư)
        Invoker = 7,        // Mana Regen (Thuật sĩ)
        Visionary = 8,      // Ability Power (Tiên tri)

        // Physical Damage (Vật lý)
        Gunslinger = 9,     // Bonus Physical DMG (Pháo thủ)
        Longshot = 10,      // Range Scaling (Thiện xạ)
        Quickstriker = 11,  // Attack Speed (Song đấu)
        Vanquisher = 12,    // Crit Chance/DMG (Chinh phục)
        Slayer = 13,        // Omnivamp/AD (Đồ tể)
        Huntress = 14,      // Thợ săn (Nidalee)

        // Utility (Đa dụng)
        Disruptor = 15,     // Crowd Control (Quấy rối)
        Strategist = 16,    // Buff Front/Backline (Chiến thuật gia)
        Rogue = 17          // Dash/Backline access (Sát thủ/Vô diện)
    }

    public enum RoleType
    {
        PhysicalTank = 0,
        MagicTank = 1,
        MagicAssassin = 2,
        PhysicalAssassin = 3,
        MagicRanger = 4,
        PhysicalRanger = 5,
        PhysicalFighter = 6,
        MagicFighter = 7,
        PhysicalMage = 8,
        MagicMage = 9
    }

    public enum LevelStar
    {
        Lv1 = 0,
        Lv2 = 1,
        Lv3 = 2,
        Lv4 = 3,
    }

    public enum LockStatus
    {
        Unlocked = 0,
        Locked = 1
    }

    public enum ChampionAction
    {
        Idle = 0,
        Walk = 1,
        BasicAttack = 2,
        CastSkill = 3,
        Death = 4,
        Dance = 5
    }
}