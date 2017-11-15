using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.IO;
public class StartCommand : EventCommand
{
    [Inject]
    public IntegrationModel IntergrationModel { get; set; }

    [Inject]
    public CardModel CardModel { get; set; }

    [Inject]
    public RoundModel RoundModel { get; set; }

    private byte[] message;

    public override void Execute()
    {
        //Tools.CreateUIPanel(PanelType.StartPanel);


        //初始化Model
        //IntergrationModel.InitIntergration();
       // CardModel.InitCardLibrary();
        //RoundModel.InitRound();

        //读取数据
        //GetData();
    }

    /// <summary>
    /// 读取保存的数据
    /// </summary>
    private void GetData()
    {
		string fileName = Consts.DataPath;
        FileInfo fileInfo = new FileInfo(fileName);
        if (fileInfo.Exists)
        {
            GameData oldData = Tools.GeyDataWithOutBom();
            //保存数据
            IntergrationModel.ComputerLeftIntergration = oldData.ComputerLeftIntergration;
            IntergrationModel.ComputerRightIntergration = oldData.ComputerRightIntergration;
            IntergrationModel.PlayerIntergration = oldData.PlayerIntergration;
        }
    }

}
