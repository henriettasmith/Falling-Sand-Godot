using Godot;
using System;

public class ExclusiveCheckbox : Control
{
    [Export]
    String sender;

    [Signal]
    public delegate void checkboxChanged(int newSetting);

    public void changeSelected(int setting, string pathToSender)
    {
        EmitSignal(nameof(checkboxChanged), setting);
        GetNode<Button>(sender).Pressed = false;
        sender = pathToSender;
        GetNode<Button>(sender).Pressed = true;

    }
}
