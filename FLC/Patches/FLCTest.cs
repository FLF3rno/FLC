using MonoMod.Cil;
using System.Collections;
using HarmonyLib;
using MonoMod.RuntimeDetour;

On.GameNetcodeStuff.PlayerControllerB.PlayerJump += OnJump;
// ...
static IEnumerator OnJump(On.GameNetcodeStuff.PlayerControllerB.orig_PlayerJump orig, GameNetcodeStuff. PlayerControllerB self)
{
    // code here runs before the original method
    IEnumerator origIEnumerator = orig(self);
    while (origIEnumerator.MoveNext())
    {
        yield return origIEnumerator.Current;
    }
    // code here runs after the original method
}

On.GameNetcodeStuff.PlayerControllerB.Update += DieADeath;

 static void DieADeath(On.GameNetcodeStuff.PlayerControllerB.orig_Update orig, GameNetcodeStuff.PlayerControllerB self)
{
    // Code here runs before the original method
    orig(self);
    // Code here runs after the original method
    if (self.isExhausted)
    {
        self.KillPlayer(UnityEngine.Vector3.zero, true, CauseOfDeath.Inertia);
    }
}

// Somewhere in our code we subscribe to the event once:
On.GameNetcodeStuff.PlayerControllerB.CheckConditionsForEmote += PlayerControllerB_CheckConditionsForEmote;
// ...
 static bool PlayerControllerB_CheckConditionsForEmote(On.GameNetcodeStuff.PlayerControllerB.orig_CheckConditionsForEmote orig, GameNetcodeStuff.PlayerControllerB self)
{
    // Since we are patching a method that returns a boolean,
    // we can get the return value by calling the original method.
    bool originalResult = orig(self);

    // What we return from a patch will override the original return value.
    // Since we want to be able to emote all the time, we will return true.
    // We could also return the original return value to do nothing.
    return true;
}