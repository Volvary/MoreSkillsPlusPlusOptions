using EntityStates.Bandit2.Weapon;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using System;
using UnityEngine;

namespace MoreSkillsPlusPlusOptions.BanditSkills
{

    [SkillLevelModifier("FireShotgun2", new Type[]
        {
        typeof(FireShotgun2)
        })]
    internal class BanditFireShotgunSkillModifier : SimpleSkillModifier<FireShotgun2>
    {
        public override void OnSkillEnter(FireShotgun2 skillState, int level)
        {
            Chat.AddMessage("Shotgun");

            base.OnSkillEnter(skillState, level);

            skillState.bulletCount = skillState.bulletCount + level;
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.05f, level);

            skillState.minFixedSpreadYaw += level * 0.5f;
            skillState.maxFixedSpreadYaw += level;
        }
    }
}
