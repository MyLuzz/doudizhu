using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

/// <summary>
/// 改变倍数的命令
/// </summary>
public class ChangeMultipleCommand : EventCommand
{
    [Inject]
    public IntegrationModel IntegrationModel { get; set; }

    public override void Execute()
    {
        int multiple = (int)evt.data;
        IntegrationModel.Multiples = multiple;

        Tools.CreateUIPanel(PanelType.BackgroundPanel);
        Tools.CreateUIPanel(PanelType.CharacterPanel);
        Tools.CreateUIPanel(PanelType.InteractionPanel);
    }
}
