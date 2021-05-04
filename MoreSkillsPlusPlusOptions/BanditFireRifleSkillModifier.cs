using EntityStates.Bandit2.Weapon;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using System;
using UnityEngine;

namespace MoreSkillsPlusPlusOptions.BanditSkills
{

    [SkillLevelModifier("Bandit2Rifle", new Type[]
        {
        typeof(Bandit2FireRifle)
        })]
    internal class BanditFireRifleSkillModifier : SimpleSkillModifier<Bandit2FireRifle>
    {
        float baseBloom = 0;
        public override void OnSkillEnter(Bandit2FireRifle skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            //Chat.AddMessage("Prebuff damage: " + skillState.damageCoefficient + " - procCoefficient: " + skillState.procCoefficient + " - Bloom Value: " + skillState.spreadBloomValue);

            if (Mathf.Abs(baseBloom - 0) < 0.1f)
            {
                baseBloom = skillState.spreadBloomValue;
                Chat.AddMessage("baseBloom: " + baseBloom);
            }

            if (level > 0)
            {
                skillState.procCoefficient = AdditiveScaling(skillState.procCoefficient, 0.1f, level);
                skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.2f, level);

                Chat.AddMessage("basebloom : " + baseBloom + " - " + (4 / ((float)level + 4)));
                skillState.spreadBloomValue = baseBloom * (4 / ((float)level + 4));
            }

            //Chat.AddMessage("Postbuff damage: " + skillState.damageCoefficient + " - procCoefficient: " + skillState.procCoefficient + " - Bloom Value: " + skillState.spreadBloomValue);

        }
    }
}
