﻿using Charmed.Container;
using System;
using System.Reflection;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif // NETFX_CORE

namespace Charmed
{
	public sealed class Navigator : INavigator
	{
		private readonly ISerializer serializer;
		private readonly IContainer container;

#if WINDOWS_PHONE
		private readonly Microsoft.Phone.Controls.PhoneApplicationFrame frame;
#endif // WINDOWS_PHONE

		public Navigator(
			ISerializer serializer,
			IContainer container
#if WINDOWS_PHONE
			, Microsoft.Phone.Controls.PhoneApplicationFrame frame
#endif // WINDOWS_PHONE
			)
		{
			this.serializer = serializer;
			this.container = container;

#if WINDOWS_PHONE
			this.frame = frame;
#endif // WINDOWS_PHONE
		}

		public void NavigateToViewModel<TViewModel>(object parameter = null)
		{
			var viewType = ResolveViewType<TViewModel>();

#if NETFX_CORE
			var frame = (Frame)Window.Current.Content;
#endif // NETFX_CORE

			if (parameter != null)
			{
#if WINDOWS_PHONE
				this.frame.Navigate(ResolveViewUri(viewType, parameter));
#else
				frame.Navigate(viewType, this.serializer.Serialize(parameter));
#endif // WINDOWS_PHONE
			}
			else
			{
#if WINDOWS_PHONE
				this.frame.Navigate(ResolveViewUri(viewType));
#else
				frame.Navigate(viewType);
#endif // WINDOWS_PHONE
			}
		}

		public void GoBack()
		{
#if WINDOWS_PHONE
			this.frame.GoBack();
#else
			((Frame)Window.Current.Content).GoBack();
#endif // WINDOWS_PHONE
		}

		public bool CanGoBack
		{
			get
			{
#if WINDOWS_PHONE
				return this.frame.CanGoBack;
#else
				return ((Frame)Window.Current.Content).CanGoBack;
#endif // WINDOWS_PHONE
			}
		}

		private static Type ResolveViewType<TViewModel>()
		{
			var viewModelType = typeof(TViewModel);

			var viewName = viewModelType.AssemblyQualifiedName.Replace(
				viewModelType.Name,
				viewModelType.Name.Replace("ViewModel", "Page"));

			return Type.GetType(viewName.Replace("Model", string.Empty));
		}

		private Uri ResolveViewUri(Type viewType, object parameter = null)
		{
			var queryString = string.Empty;
			if (parameter != null)
			{
				var serializedParameter = this.serializer.Serialize(parameter);
				queryString = string.Format("?parameter={0}", serializedParameter);
			}

			var match = System.Text.RegularExpressions.Regex.Match(viewType.FullName, @"\.Views.*");
			if (match == null || match.Captures.Count == 0)
			{
				throw new ArgumentException("Views must exist in Views namespace.");
			}
			var path = match.Captures[0].Value.Replace('.', '/');

			return new Uri(string.Format("{0}.xaml{1}", path, queryString), UriKind.Relative);
		}

#if WINDOWS_PHONE
		public void RemoveBackEntry()
		{
			this.frame.RemoveBackEntry();
		}
#endif // WINDOWS_PHONE
	}
}
