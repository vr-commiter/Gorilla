using MelonLoader;
using HarmonyLib;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using UnityEngine;
using System.Threading;
using MyTrueGear;

namespace GorillaTag_TrueGear
{
    public static class BuildInfo
    {
        public const string Name = "GorillaTag_TrueGear"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "TrueGear Mod for GorillaTag"; // Description for the Mod.  (Set as null if none)
        public const string Author = "HuangLvYuan"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class GorillaTag_TrueGear : MelonMod
    {
        private static Vector3 lastHitPoint = Vector3.zero;
        private static bool isLeftHandTouch = false;
        private static bool isRightHandTouch = false;
        private static bool canFallDamage = true;
        private static Vector3 lastPosition = new Vector3();
        private static TrueGearMod _TrueGear = null;

        private static Timer leftTimer = new Timer(LeftTimer, null, Timeout.Infinite, Timeout.Infinite);
        private static Timer rightTimer = new Timer(RightTimer, null, Timeout.Infinite, Timeout.Infinite);

        public override void OnInitializeMelon() {
            //MelonLogger.Msg("OnApplicationStart");
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(GorillaTag_TrueGear));
            _TrueGear = new TrueGearMod();
        }

        [HarmonyPostfix,HarmonyPatch(typeof(GorillaTagger), "UpdateColor")]
        private static void GorillaTagger_UpdateColor_Postfix()
        {
            //MelonLogger.Msg("------------------------------");
            //MelonLogger.Msg("ChangeColor");
            _TrueGear.Play("RefreshVisibleClothing");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GorillaTagger), "ApplyStatusEffect")]
        private static void GorillaTagger_ApplyStatusEffect_Postfix()
        {
            //MelonLogger.Msg("------------------------------");
            //MelonLogger.Msg("ChangeColor");
            _TrueGear.Play("RefreshVisibleClothing");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Player), "CheckWaterSurfaceJump")]
        private static void Player_CheckWaterSurfaceJump_Postfix(Player __instance,bool __result)
        {
            if (__result)
            {
                //MelonLogger.Msg("------------------------------");
                //MelonLogger.Msg("WaterSurfaceJump");
                _TrueGear.Play("Move");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Player), "OnEnterWaterVolume")]
        private static void Player_OnEnterWaterVolume_Postfix(Player __instance, Collider playerCollider, WaterVolume volume)
        {
            if (playerCollider == __instance.headCollider)
            {
                if (!__instance.headOverlappingWaterVolumes.Contains(volume))
                {
                    //MelonLogger.Msg("------------------------------");
                    //MelonLogger.Msg("EnterWater");
                    _TrueGear.Play("EnterWater");
                    return;
                }
            }
            else if (playerCollider == __instance.bodyCollider && !__instance.bodyOverlappingWaterVolumes.Contains(volume))
            {
                //MelonLogger.Msg("------------------------------");
                //MelonLogger.Msg("EnterWater");
                _TrueGear.Play("EnterWater");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Player), "OnExitWaterVolume")]
        private static void Player_OnExitWaterVolume_Postfix(Player __instance, Collider playerCollider)
        {
            if (playerCollider == __instance.headCollider)
            {
                //MelonLogger.Msg("------------------------------");
                //MelonLogger.Msg("ExitWater");
                _TrueGear.Play("ExitWater");
                return;
            }
            if (playerCollider == __instance.bodyCollider)
            {
                //MelonLogger.Msg("------------------------------");
                //MelonLogger.Msg("ExitWater");
                _TrueGear.Play("ExitWater");
            }
        }


        [HarmonyPostfix, HarmonyPatch(typeof(Player), "IsHandTouching")]
        private static void Player_IsHandTouching_Postfix(bool forLeftHand, bool __result)
        {
            if (__result)
            {
                if (forLeftHand)
                {
                    leftTimer.Change(150, Timeout.Infinite);
                    if (isLeftHandTouch)
                    {
                        return;
                    }
                    isLeftHandTouch = true;
                    //MelonLogger.Msg("------------------------------");
                    //MelonLogger.Msg("LeftHandTouching");
                    _TrueGear.Play("LeftHandPickupItem");
                    canFallDamage = true;                                    
                }
                else if(!forLeftHand)
                {
                    rightTimer.Change(150, Timeout.Infinite);
                    if (isRightHandTouch)
                    {
                        return;
                    }
                    isRightHandTouch = true;
                    //MelonLogger.Msg("------------------------------");
                    //MelonLogger.Msg("RightHandTouching");
                    _TrueGear.Play("RightHandPickupItem");
                    canFallDamage = true;              
                }
                lastPosition = GameObject.FindAnyObjectByType<Player>().transform.position;
            }
        }
        
        private static void LeftTimer(System.Object o)
        {
            isLeftHandTouch = false;
        }

        private static void RightTimer(System.Object o)
        {
            isRightHandTouch = false;
        }

        

        [HarmonyPrefix, HarmonyPatch(typeof(Player), "BodyCollider")]
        private static void Player_BodyCollider_Prefix(Player __instance)
        {
            if (__instance.MaxSphereSizeForNoOverlap(__instance.bodyInitialRadius * __instance.scale, __instance.PositionWithOffset(__instance.headCollider.transform, __instance.bodyOffset), false, out __instance.bodyMaxRadius))
            {
                if (Physics.SphereCast(__instance.PositionWithOffset(__instance.headCollider.transform, __instance.bodyOffset), __instance.bodyMaxRadius, Vector3.down, out __instance.bodyHitInfo, __instance.bodyInitialHeight * __instance.scale - __instance.bodyMaxRadius, __instance.locomotionEnabledLayers))
                {
                    if (canFallDamage && lastPosition != __instance.transform.position)
                    {                        
                        canFallDamage = false;
                        var distance = Vector3.Distance(lastPosition, __instance.transform.position);
                        
                        if (Mathf.Abs(__instance.transform.position.y - lastPosition.y) > 7f)
                        {
                            //MelonLogger.Msg("------------------------------");
                            //MelonLogger.Msg("FallDamage");
                            _TrueGear.Play("FallDamage");
                        }
                        else if(Mathf.Abs(distance) > 0.5f)
                        {
                            //MelonLogger.Msg("------------------------------");
                            //MelonLogger.Msg("Move");
                            _TrueGear.Play("Move");
                        }
                        //MelonLogger.Msg(distance);
                        //MelonLogger.Msg($"Now   |{__instance.transform.position.x},{__instance.transform.position.y},{__instance.transform.position.z}");
                        //MelonLogger.Msg($"Old   |{lastPosition.x},{lastPosition.y},{lastPosition.z}");
                        lastPosition = __instance.transform.position;
                    }
                }
            }
        }



    }
}