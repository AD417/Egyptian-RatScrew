using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Linq;

namespace EgyptianRatScrew.DevcadeExtension;

static class InputManager {
    private static readonly Dictionary<Input.ArcadeButtons, Action> OnPress = new();
    private static readonly Dictionary<Input.ArcadeButtons, Action> WhileHeld = new();
    private static readonly Dictionary<Input.ArcadeButtons, Action> OnRelease = new();

    private static readonly Dictionary<Input.ArcadeButtons, Keys> ButtonMap = new();

    // TODO: get rid of this if/when it gets integrated with Devcade.Input
    private static HashSet<Keys> PressedLastFrame = new();

    /// <summary>
    /// For debugging, make a keyboard key coorespond to a devcade button.
    /// Only one keyboard key can be mapped to a button at a time. 
    /// </summary>
    /// <param name="key">
    ///     The key that the programmer can press to emulate a button.
    /// </param>
    /// <param name="button">
    ///     The button that should coorespond to the relevant key press.
    /// </param>
    public static void MapKeyToButton(Keys key, Input.ArcadeButtons button) {
        ButtonMap[button] = key;
    }

    /// <summary>
    /// Set an action to be executed whenever a specific button is being held.
    /// This action will be executed on the first frame the button is detected
    /// as being held, and will trigger every time the button goes from being
    /// released to being held. 
    /// </summary>
    /// <param name="button">
    ///     The ID of the button to bind this action to.
    /// </param>
    /// <param name="action">
    ///     A function to be exeucted when the button is pressed. This function
    ///     must take no arguments and return nothing. 
    /// </param>
    public static void SetOnPress(Input.ArcadeButtons button, Action action) {
        OnPress[button] = action;
    }

    /// <summary>
    /// Set an action to be executed on every frame while a specific button is
    /// being held. This action will be executed after the <c>SetOnPress</c>
    /// action (if any), and will be executed on every frame up until, but not 
    /// including, the frame when the button is released. 
    /// </summary>
    /// 
    /// <param name="button">
    ///     The ID of the button to bind this action to.
    /// </param>
    /// <param name="action">
    ///     A function to be exeucted on every frame the button is held. This
    ///     function must take no arguments and return nothing. 
    /// </param>
    public static void SetWhileHeld(Input.ArcadeButtons button, Action action) {
        WhileHeld[button] = action;
    }

    /// <summary>
    /// Set an action to be executed whenever a held button is being released.
    /// This action will be executed on the frame after the last frame the 
    /// button is detected as being held, and will trigger every time the 
    /// button goes from being held to being released.
    /// </summary>
    /// <param name="button">
    ///     The ID of the button to bind this action to.
    /// </param>
    /// <param name="action">
    ///     A function to be exeucted when the button is released. This
    ///     function must take no arguments and return nothing.
    /// </param>
    public static void SetOnRelease(Input.ArcadeButtons button, Action action) {
        OnRelease[button] = action;
    }

    private static bool GetButtonDown(Input.ArcadeButtons button) {
        if (Input.GetButtonDown(1, button)) return true;
        if (!ButtonMap.ContainsKey(button)) return false;

        Keys key = ButtonMap[button];
        if (PressedLastFrame.Contains(key)) return false;
        return Keyboard.GetState().IsKeyDown(ButtonMap[button]);
    }

    private static bool GetButton(Input.ArcadeButtons button) {
        if (Input.GetButton(1, button)) return true;
        if (!ButtonMap.ContainsKey(button)) return false;

        return Keyboard.GetState().IsKeyDown(ButtonMap[button]);
    }

    private static bool GetButtonUp(Input.ArcadeButtons button) {
        if (Input.GetButtonUp(1, button)) return true;
        if (!ButtonMap.ContainsKey(button)) return false;

        Keys key = ButtonMap[button];
        if (!PressedLastFrame.Contains(key)) return false;
        return !Keyboard.GetState().IsKeyDown(key);
    }

    public static void TickActions() {
        Input.ArcadeButtons[] allButtons = 
                (Input.ArcadeButtons[])Enum.GetValues(typeof(Input.ArcadeButtons));

        foreach (Input.ArcadeButtons button in allButtons) {
            if (GetButtonDown(button)) {
                if (OnPress.ContainsKey(button)) OnPress[button]();
            }
            if (GetButton(button)) {
                if (WhileHeld.ContainsKey(button)) WhileHeld[button]();
            }
            if (GetButtonUp(button)) {
                if (OnRelease.ContainsKey(button)) OnRelease[button]();
            }
        }

        PressedLastFrame = Keyboard.GetState().GetPressedKeys().ToHashSet();
    }
}