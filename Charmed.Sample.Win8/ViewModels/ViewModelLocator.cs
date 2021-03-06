﻿using Charmed.Container;
using Charmed.Helpers;
using Charmed.Messaging;
using Charmed.Sample.Services;

namespace Charmed.Sample.ViewModels
{
	public sealed class ViewModelLocator
	{
		public ViewModelLocator()
		{
			Ioc.Container.Register<MainViewModel>();

			Ioc.Container.Register<ISerializer, Serializer>();
			Ioc.Container.Register<ISettings, Settings>();
			Ioc.Container.Register<IStorage, Storage>();
			Ioc.Container.RegisterInstance<IMessageBus>(new MessageBus());
			Ioc.Container.RegisterInstance<IContainer>(Ioc.Container);

#if NETFX_CORE
			Ioc.Container.Register<FeedItemViewModel, Win8FeedItemViewModel>();

			Ioc.Container.Register<ISecondaryPinner, Win8SecondaryPinner>();

			Ioc.Container.Register<INavigator, Navigator>();
			Ioc.Container.Register<IRssFeedService, Win8RssFeedService>();
			Ioc.Container.Register<ShellViewModel>();
			Ioc.Container.Register<IShareManager, ShareManager>();
			Ioc.Container.Register<ISettingsManager, SettingsManager>();
			Ioc.Container.Register<SettingsViewModel>();
#else
			Ioc.Container.Register<ISecondaryPinner, WP8SecondaryPinner>();
			Ioc.Container.Register<IRssFeedService, WP8RssFeedService>();
			Ioc.Container.Register<FeedItemViewModel, WP8FeedItemViewModel>();
			Ioc.Container.Register<SplashViewModel>();
#endif // NETFX_CORE
		}

		public MainViewModel Main
		{
			get { return Ioc.Container.Resolve<MainViewModel>();}
		}

		public FeedItemViewModel FeedItem
		{
			get { return Ioc.Container.Resolve<FeedItemViewModel>(); }
		}

#if NETFX_CORE
		public SettingsViewModel Settings
		{
			get { return Ioc.Container.Resolve<SettingsViewModel>(); }
		}
#else
		public SplashViewModel Splash
		{
			get { return Ioc.Container.Resolve<SplashViewModel>(); }
		}
#endif // NETFX_CORE

		public static bool IsInDesignMode
		{
			get
			{
#if NETFX_CORE
				return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#else
				return System.ComponentModel.DesignerProperties.IsInDesignTool;
#endif // NETFX_CORE
			}
		}
	}
}
