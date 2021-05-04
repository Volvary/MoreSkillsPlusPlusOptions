using EntityStates.Bandit2.Weapon;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using System;
using UnityEngine;

namespace MoreSkillsPlusPlusOptions.BanditSkills
{

    [SkillLevelModifier("SlashBlade", new Type[]
    {
    typeof(SlashBlade)
    })]
    internal class BanditBladeSkillModifier : SimpleSkillModifier<SlashBlade>
    {
        Vector3 originalHitboxScale = Vector3.zero;
        public override void OnSkillEnter(SlashBlade slash, int level)
        {
            //Visual Scaling
            Chat.AddMessage("Slash prefab: " + slash.swingEffectPrefab.name + " - " + slash.swingEffectPrefab.transform.localScale.ToString());

            slash.swingEffectPrefab.transform.localScale = new Vector3(1 + level, 1 + level, 1 + level);

            base.OnSkillEnter(slash, level);

            slash.damageCoefficient = MultScaling(slash.damageCoefficient, 0.2f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            Chat.AddMessage("SlashBlade");

            HitBoxGroup hitboxGroup = characterBody.modelLocator.modelTransform.GetComponent<HitBoxGroup>();

            if (hitboxGroup == null || hitboxGroup.groupName != "SlashBlade")
            {
                Debug.LogWarning("didn't get Bandit's slashHitbox. probably got changed?. aborting");

                return;
            }

            Transform hitboxTransform = hitboxGroup.hitBoxes[0].transform;

            if (originalHitboxScale == Vector3.zero)
            {
                originalHitboxScale = hitboxTransform.localScale;
            }
            hitboxTransform.localScale = new Vector3(MultScaling(originalHitboxScale.x, 0.2f, level), MultScaling(originalHitboxScale.y, 0.2f, level), MultScaling(originalHitboxScale.z, 0.3f, level));
        }
    }
}
