using System;
public class UIEvents {
    public static event Action OnButtonStartPressed;
    public static void ButtonStartPressed() {
        OnButtonStartPressed?.Invoke();
    }

    public static event Action OnButtonShotPressed;
    public static void ButtonShotPressed() {
        OnButtonShotPressed?.Invoke();
    }
}
