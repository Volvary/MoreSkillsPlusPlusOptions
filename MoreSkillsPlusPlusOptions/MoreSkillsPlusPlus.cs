using System;

using BepInEx;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using EntityStates.Bandit2.Weapon;
using SkillsPlusPlus;
using EntityStates;
using UnityEngine;
using R2API;
using R2API.Utils;

namespace MoreSkillsPlusPlusOptions
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.cwmlolzlz.skills")]
    [R2APISubmoduleDependency(nameof(BuffAPI))]
    //Change these
    [BepInPlugin("com.Volvary.MoreSkillsPlusPlusOptions", "More SkillPlusPlus Options.", "0.0.1")]
    public class MoreSkillsPlusPlusOptions : BaseUnityPlugin
    {
        public void Awake()
        {
            Logger.LogMessage("Loaded MoreSkillPlusPlusOptions!!!!");

            BanditSkills.BanditSkills banditSkills = new BanditSkills.BanditSkills();
            banditSkills.OnAwake();

            SkillModifierManager.LoadSkillModifiers();
        }
    }
}