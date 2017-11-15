using strange.extensions.command.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RequestUpdateCommand : EventCommand
{
    [Inject]
    public IntegrationModel IntegrationModel { get; set; }
    
    public override void Execute()
    {
        GameData gameData = new GameData();
        gameData.PlayerIntergration = IntegrationModel.PlayerIntergration;
        gameData.ComputerLeftIntergration = IntegrationModel.ComputerLeftIntergration;
        gameData.ComputerRightIntergration = IntegrationModel.ComputerRightIntergration;
        
       dispatcher.Dispatch(ViewEvent.UPDATE_INTEGRATION,gameData);
    }
}
