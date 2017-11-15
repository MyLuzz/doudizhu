using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
using UnityEngine.UI;

public class GameOverView : View
{
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

	public Text LeftRoundScore;
	public Text RightRoundScore;
	public Text PlayerRoundScore;
	public Button Restart;
	public GameObject TotalPanel;
	public Text LeftTotalScore;
	public Text RightTotalScore;
	public Text PlayerTotalScore;
    public PlayerControl Player;
    public DeskControl Desk;
    public ComputerControl Left;
    public ComputerControl Right;

    public SoundManager soundManager;
    //最终结算面板的属性
    public Button back_main;
    public Image img_00;
    public Image img_11;
    public Image img_22;
    public Text land0;
    public Text land1;
    public Text land2;
    public Text score0;
    public Text score1;
    public Text score2;
    public Image img_k0;
    public Image img_k1;
    public Image img_k2;
    public Image player_ide;
    public Image left_ide;
    public Image right_ide;

    public Button btn_deal;
}
