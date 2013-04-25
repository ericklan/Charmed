﻿using Charmed.Sample.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Charmed.Sample.ViewModels
{
	public sealed class SettingsViewModel : ViewModelBase
	{
		private readonly ISettings settings;

		public SettingsViewModel(ISettings settings)
		{
			this.settings = settings;

			this.Feeds = new ObservableCollection<string>();

			string[] feedData;
			if (this.settings.TryGetValue<string[]>(Constants.FeedsKey, out feedData))
			{
				foreach (var feed in feedData)
				{
					this.Feeds.Add(feed);
				}
			}
		}

		public void AddFeed()
		{
			this.Feeds.Add(this.NewFeed);
			this.NewFeed = string.Empty;

			SaveFeeds();
		}

		public void RemoveFeed(string feed)
		{
			this.Feeds.Remove(feed);

			SaveFeeds();
		}

		private void SaveFeeds()
		{
			this.settings.AddOrUpdate(Constants.FeedsKey, this.Feeds.ToArray());
		}

		public ObservableCollection<string> Feeds { get; private set; }

		private string newFeed = string.Empty;
		public string NewFeed
		{
			get { return this.newFeed; }
			set { this.SetProperty(ref this.newFeed, value); }
		}
	}
}
