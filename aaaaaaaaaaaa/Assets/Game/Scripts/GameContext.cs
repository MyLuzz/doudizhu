using strange.extensions.context.impl;
using UnityEngine;
using System.Collections.Generic;
using strange.extensions.context.api;

public class GameContext : MVCSContext
{
    public GameContext(MonoBehaviour view, bool autoMapping) : base(view, autoMapping)
    {

    }

    /// <summary>
    /// 绑定映射
    /// </summary>
    protected override void mapBindings()
    {
        injectionBinder.Bind<IntegrationModel>().To<IntegrationModel>().ToSingleton();
        injectionBinder.Bind<CardModel>().To<CardModel>().ToSingleton();
        injectionBinder.Bind<RoundModel>().To<RoundModel>().ToSingleton();

        mediationBinder.BindView<StartView>().ToMediator<StartMediator>();
        mediationBinder.Bind<InteractionView>().To<InteractionMediator>();
        mediationBinder.Bind<CharacterView>().To<CharacterMediator>();
		mediationBinder.Bind<GameOverView>().To<GameOverMediator> ();

        commandBinder.Bind(CommandEvent.ChangeMultiple).To<ChangeMultipleCommand>();
        commandBinder.Bind(CommandEvent.RequestDeal).To<RequestDealCommand>();
        commandBinder.Bind(CommandEvent.GrabLandLord).To<GrabLandlordCommand>();
        commandBinder.Bind(CommandEvent.PlayCard).To<PlayCardCommand>();
        commandBinder.Bind(CommandEvent.PassCard).To<PassCardCommand>();
        commandBinder.Bind(CommandEvent.GameOver).To<GameOverCommand>();
        commandBinder.Bind(CommandEvent.RequestUpdate).To<RequestUpdateCommand>();

        //开始命令
        commandBinder.Bind(ContextEvent.START).To<StartCommand>().Once();

    }
}

