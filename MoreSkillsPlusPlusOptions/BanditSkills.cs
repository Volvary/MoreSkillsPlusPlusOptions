using EntityStates.Bandit2;
using EntityStates.Bandit2.Weapon;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using System;
using UnityEngine;
using EntityStates;
using R2API.Utils;
using R2API;

namespace MoreSkillsPlusPlusOptions.BanditSkills
{
    class BanditSkills
    {
        public static BuffDef BanditSpeedBuff;

        public static int BanditLightsOutLevel = 0;
        public static int BanditDesperadoLevel = 0;
        public void OnAwake()
        {
            {
                On.EntityStates.Bandit2.Weapon.Bandit2FireShiv.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("FireShiv!!");
                    orig(self);
                };
                On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("BaseFireSidearmRevolverState!! : " + self.GetType().FullName);
                    orig(self);
                };
                On.EntityStates.Bandit2.Weapon.BaseFireSidearmRevolverState.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("BaseFireSidearmRevolverState!! : " + self.GetType().FullName);
                    orig(self);
                };
                On.EntityStates.Bandit2.Weapon.BasePrepSidearmRevolverState.OnEnter += (orig, self) =>
                {
                    Chat.AddMessage("BasePrepSidearmRevolverState!!");
                    if (self is PrepSidearmResetRevolver)
                    {
                        Chat.AddMessage("PrepSidearmResetRevolver!!");
                    }
                    else if (self is PrepSidearmSkullRevolver)
                    {
                        Chat.AddMessage("PrepSidearmSkullRevolver!!");
                    }
                    orig(self);
                };
            }

            BuffDef buffDef = new BuffDef
            {
                buffColor = new Color(153, 0, 0),
                buffIndex = (BuffIndex)63,
                canStack = true,
                eliteDef = null,
                iconPath = "Textures/BuffIcons/texBuffBleedingIcon",
                isDebuff = false,
                name = "BanditSpeed"
            };
            BanditSpeedBuff = buffDef;
            BuffAPI.Add(new CustomBuff(buffDef));

            On.RoR2.CharacterBody.RecalculateStats += new On.RoR2.CharacterBody.hook_RecalculateStats(this.CharacterBody_RecalculateStats);

            On.RoR2.HealthComponent.TakeDamage += new On.RoR2.HealthComponent.hook_TakeDamage(this.HealthComponent_TakeDamage);

            On.EntityStates.Bandit2.StealthMode.OnExit += (orig, self) =>
            {
                self.outer.commonComponents.characterBody.ClearTimedBuffs(BanditSkills.BanditSpeedBuff);
                orig(self);
            };
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig.Invoke(self);

            if (self.HasBuff(BanditSpeedBuff))
            {
                Reflection.SetPropertyValue<float>(self, "moveSpeed", self.moveSpeed + self.GetBuffCount(BanditSpeedBuff));

                //TODO: Integrate this to From The Shadows alternate skill mod
                //Chat.AddMessage("Shadow buff count: " + self.GetBuffCount(BanditSkills.BanditShadowsBuff));
                //Reflection.SetPropertyValue<float>(self, "moveSpeed", self.moveSpeed + self.GetBuffCount(BanditShadowsBuff));
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        { 
            if(di.attacker != null && self != null)
            {
                CharacterBody body = self.GetComponent<CharacterBody>();
                if (body != null)
                {
                    if(di.damageType.HasFlag(RoR2.DamageType.BonusToLowHealth) && di.damageType.HasFlag(RoR2.DamageType.ResetCooldownsOnKill))
                    {
                        di.damage *= Mathf.Lerp(1f + BanditLightsOutLevel * 0.3f, 1.0f, self.combinedHealthFraction);
                    }else if (di.damageType.HasFlag(RoR2.DamageType.GiveSkullOnKill))
                    {
                        float remainingHealth = (self.combinedHealth - di.damage);
                        if (remainingHealth / self.fullCombinedHealth < BanditDesperadoLevel * 0.01f && remainingHealth > 0)
                        {
                            di.damage += remainingHealth;
                            di.damageType |= DamageType.BypassArmor;
                        }
                    }

                    //TODO: Integrate this to From The Shadows alternate skill mod
                    //Chat.AddMessage("Shadow buff count: " + body.GetBuffCount(BanditSkills.BanditShadowsBuff));
                    //di.damage = di.damage * (1 + (body.GetBuffCount(BanditShadowsBuff) * 0.15f));
                }
            }

            orig.Invoke(self, di);
        }
    }

    [SkillLevelModifier("ThrowSmokebomb", new Type[]
        {
        typeof(ThrowSmokebomb)
        })]
    internal class BanditSkillThrowSmokebombModifier : SimpleSkillModifier<ThrowSmokebomb>
    {
        float baseRadius = 0;
        float baseDamage = 0;

        public override void OnSkillEnter(ThrowSmokebomb skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            Chat.AddMessage("Smokebombed");


            //TODO: Replace this with damage + move speed + radius increase and move that to another mod as a new skill.
            //for(int i = 0; i < level; i++)
            //{
            //    Chat.AddMessage("Applying buff");
            //    skillState.outer.commonComponents.characterBody.AddBuff(BanditSkills.BanditShadowsBuff);
            //}
            //Chat.AddMessage("Shadow buff count: " + skillState.outer.commonComponents.characterBody.GetBuffCount(BanditSkills.BanditShadowsBuff));

            for(int i = 0; i < level; i++)
            {
                skillState.outer.commonComponents.characterBody.AddTimedBuff(BanditSkills.BanditSpeedBuff, StealthMode.duration);
            }

        }

        public override void OnSkillExit(ThrowSmokebomb skillState, int level)
        {
            base.OnSkillExit(skillState, level);

            //if(level > 0)
            //{
            //    skillState.outer.commonComponents.characterBody.RemoveBuff(BanditSkills.BanditShadowsBuff);
            //    skillState.outer.commonComponents.characterBody.AddTimedBuff(BanditSkills.BanditShadowsBuff, 0.5f + 0.5f * level);
            //}

            if(level > 0)
            {
                //skillState.outer.commonComponents.characterBody.RemoveBuff(BanditSkills.BanditSpeedBuff);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if(Mathf.Abs(baseRadius - 0) < 0.1f)
            {
                baseRadius = StealthMode.blastAttackRadius;
                baseDamage = StealthMode.blastAttackDamageCoefficient;
            }

            StealthMode.blastAttackRadius = MultScaling(baseRadius, 0.1f, level);
            StealthMode.blastAttackDamageCoefficient = MultScaling(baseDamage, 0.2f, level);
        }
    }

    [SkillLevelModifier("Bandit2.ResetRevolver", new Type[]
        {
        typeof(FireSidearmResetRevolver)
        })]
    internal class BanditSkillResetRevolverModifier : SimpleSkillModifier<FireSidearmResetRevolver>
    {
        public override void OnSkillEnter(FireSidearmResetRevolver skillState, int level)
        {
            Chat.AddMessage("Reset Revolver");

            BanditSkills.BanditLightsOutLevel = level;

            base.OnSkillEnter(skillState, level);
        }
    }

    [SkillLevelModifier("Bandit2Desperado", new Type[]
        {
        typeof(FireSidearmSkullRevolver)
        })]
    internal class BanditSkillSkullRevolverModifier : SimpleSkillModifier<FireSidearmSkullRevolver>
    {
        public override void OnSkillEnter(FireSidearmSkullRevolver skillState, int level)
        {
            Chat.AddMessage("Skull Revolver");

            BanditSkills.BanditDesperadoLevel = level;

            base.OnSkillEnter(skillState, level);
        }
    }
}