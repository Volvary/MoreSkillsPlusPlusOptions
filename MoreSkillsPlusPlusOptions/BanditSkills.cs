using EntityStates.Bandit2;
using EntityStates.Bandit2.Weapon;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using System;
using UnityEngine;
using EntityStates;

namespace MoreSkillsPlusPlusOptions.BanditSkills
{
    class BanditSkills
    {
        public static BuffDef BanditShadowsBuff;
        public void OnAwake()
        {
            {
                On.EntityStates.Bandit2.Weapon.Bandit2FireShiv.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("FireShiv!!");
                    orig(self);
                };
                On.EntityStates.Bandit2.Weapon.Bandit2FirePrimaryBase.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("Bandit2FirePrimaryBase!!");
                    orig(self);
                }; 
                On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("BaseFireSidearmRevolverState!!");
                    orig(self);
                };
                On.EntityStates.Bandit2.Weapon.BasePrepSidearmRevolverState.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("BasePrepSidearmRevolverState!!");
                    orig(self);
                };
                On.EntityStates.Bandit2.Weapon.Reload.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("Reload!!");
                    orig(self);
                };
                On.EntityStates.Bandit2.Weapon.SlashBlade.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("Slashed!!");
                    orig(self);
                };
            }

            //BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            //buffDef.name = "FromTheShadows";
            //buffDef.iconPath = "Textures/MiscIcons/texMysteryIcon";
            //buffDef.buffColor = new Color(153, 0, 0);
            //buffDef.canStack = false;
            //buffDef.isDebuff = false;
            //buffDef.eliteDef = null;
            //BanditShadowsBuff = buffDef;

            //CharacterBody.RecalculateStats += new CharacterBody.hook_RecalculateStats(this.CharacterBody_RecalculateStats);
        }

        //private void CharacterBody_RecalculateStats(CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        //{
        //}
        }

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

    [SkillLevelModifier("ThrowSmokebomb", new Type[]
        {
        typeof(ThrowSmokebomb)
        })]
    internal class BanditSkillThrowSmokebombModifier : SimpleSkillModifier<ThrowSmokebomb>
    {
        public override void OnSkillExit(ThrowSmokebomb skillState, int level)
        {
            CharacterBody body = skillState.outer.commonComponents.characterBody;

            //body.AddTimedBuff(BanditSkills.BanditShadowsBuff, 1.0f);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            characterBody.baseArmor += 1.0f;
        }
    }

    [SkillLevelModifier("FireShotgun2", new Type[]
        {
        typeof(FireShotgun2)
        })]
    internal class BanditSkillShotgunModifier : SimpleSkillModifier<FireShotgun2>
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

    //Allows to level the skill but doesn't hit OnSkillEnter
    [SkillLevelModifier("Bandit2FireRifle", new Type[]
        {
        typeof(Bandit2FireRifle)
        })]
    internal class BanditSkillRifleModifier : SimpleSkillModifier<Bandit2FireRifle>
    {
        public override void OnSkillEnter(Bandit2FireRifle skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
        }
    }


    //NOT FUNCTIONAL
    [SkillLevelModifier("Bandit2FirePrimaryBase", new Type[]
        {
        typeof(Bandit2FirePrimaryBase)
        })]
    internal class BanditSkillPrimaryBaseModifier : SimpleSkillModifier<Bandit2FirePrimaryBase>
    {
        public override void OnSkillEnter(Bandit2FirePrimaryBase skillState, int level)
        {
            if(skillState is Bandit2FireRifle)
            {
                Chat.AddMessage("Rifle");

                base.OnSkillEnter(skillState, level);

                Chat.AddMessage("skill damage: " + skillState.damageCoefficient);


                skillState.procCoefficient = AdditiveScaling(skillState.procCoefficient, 0.1f, level);
                skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.2f, level);
                skillState.spreadBloomValue = skillState.spreadBloomValue * (5 / level + 5);

                Chat.AddMessage("skill damage: " + skillState.damageCoefficient);
            }

        }
    }


}