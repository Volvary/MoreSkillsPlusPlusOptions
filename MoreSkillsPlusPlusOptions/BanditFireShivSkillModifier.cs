using EntityStates.Bandit2.Weapon;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using System;
using UnityEngine;

namespace MoreSkillsPlusPlusOptions.BanditSkills
{

    [SkillLevelModifier("Bandit2SerratedShivs", new Type[]
        {
        typeof(Bandit2FireShiv)
        })]
    internal class BanditFireShivSkillModifier : SimpleSkillModifier<Bandit2FireShiv>
    {
        public override void OnSkillEnter(Bandit2FireShiv skillState, int level)
        {
            Chat.AddMessage("Thrown Shiv");

            base.OnSkillEnter(skillState, level);

            skillState.maxShivCount = 1 + level;
            skillState.baseDuration = 1f;
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.1f, level);
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Chat.AddMessage("Shiv Upgrade");

            base.OnSkillLeveledUp(level, characterBody, skillDef);
            skillDef.baseMaxStock = AdditiveScaling(1, 1, level);
        }
    }
}
