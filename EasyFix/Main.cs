using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Networking.Tween;

namespace EasyFix
{
    public static class ModInfo
    {
        public const string Name = "Easy Fix";
        public const string Author = "Discord: BlaBlaName#3854";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/Mopo3eX/EasyFix/releases";
    }
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            LoggerInstance.Msg("Start patching method.");
            try
            {
                HarmonyInstance.Patch(typeof(SyncPhysics).GetMethod("Method_Public_Void_PositionEvent_Single_Boolean_0"), new HarmonyLib.HarmonyMethod(typeof(Main), nameof(OnSync)));
                LoggerInstance.Msg("Method sync physics is patched.");
            }
            catch (Exception e)
            {
                LoggerInstance.Error($"Error on patching method. ({e.Message})\r\nStack Trace: {e.StackTrace}");
            }
        }
        public static Vector3 ClearPos = new Vector3(0, 0, 0);
        public static bool OnSync(SyncPhysics __instance, PositionEvent __0, float __1)
        {
            bool result = true;
            if (Vector3.Distance(ClearPos, __0.field_Public_Vector3_1) > 1000 || Vector3.Distance(ClearPos, __0.field_Public_Vector3_0) > 150)
                result = false;
            if (!result)
            {
                foreach (var col in __instance.gameObject.GetComponents<Collider>())
                {
                    col.enabled = false;
                }
                foreach (var body in __instance.gameObject.GetComponents<Rigidbody>())
                {
                    body.isKinematic = false;
                }
            }
            return result;
        }
    }
}
