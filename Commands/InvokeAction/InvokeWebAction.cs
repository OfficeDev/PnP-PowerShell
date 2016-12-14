using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using SharePointPnP.PowerShell.Commands.Extensions;
using SharePointPnP.PowerShell.Commands.Base;

namespace SharePointPnP.PowerShell.Commands.InvokeAction
{
	internal class InvokeWebAction
	{
		private const int WebProgressBarId = 1;
		private const int ListProgressBarId = 2;
		private const int ListItemProgressBarId = 3;
		
		private readonly Cmdlet _cmdlet;
		
		private InvokeWebActionResult _result;
		private int _currentWebsProcessed;
		private int _currentListsProcessed;
		private int _currentListItemsProcessed;
		
		private int _currentPostWebsProcessed;
		private int _currentPostListsProcessed;

		private double _averageShouldProcessWebTime;
		private double _averageWebTime;

		private double _averageShouldProcessListTime;
		private double _averageListTime;

		private double _averageShouldProcessListItemTime;
		private double _averageListItemTime;

		private double _averageShouldProcessPostWebTime;
		private double _averagePostWebTime;

		private double _averageShouldProcessPostListTime;
		private double _averagePostListTime;
		
		private readonly IEnumerable<Web> _webs;
		private readonly bool _subWebs;
		private readonly string _listName;
		private readonly bool _isListNameSpecified;
		private readonly InvokeActionParameter<Web> _webActions;
		private readonly InvokeActionParameter<List> _listActions;
		private readonly InvokeActionParameter<ListItem> _listItemActions;
		
		private readonly bool _skipCounting;
		
		private Stopwatch _totalExecutionTimeStopWatch;

		private InvokeWebAction(Cmdlet cmdlet, bool subWebs, InvokeActionParameter<Web> webActions, InvokeActionParameter<List> listActions, InvokeActionParameter<ListItem> listItemActions, bool skipCounting)
		{
			_cmdlet = cmdlet;
			_subWebs = subWebs;

			//We will update the InvokeActionParameter parameters, thats why we create a copies.
			_webActions = webActions.ShallowCopy();
			_listActions = listActions.ShallowCopy();
			_listItemActions = listItemActions.ShallowCopy();

			_skipCounting = skipCounting;
		}

		public InvokeWebAction(Cmdlet cmdlet, IEnumerable<Web> webs, bool subWebs, InvokeActionParameter<Web> webActions, InvokeActionParameter<List> listActions, InvokeActionParameter<ListItem> listItemActions, bool skipCounting) : 
			this(cmdlet, subWebs, webActions, listActions, listItemActions, skipCounting)
		{
			_webs = webs;

			_isListNameSpecified = false;           
		}

		public InvokeWebAction(Cmdlet cmdlet, Web web, string listName, InvokeActionParameter<Web> webActions, InvokeActionParameter<List> listActions, InvokeActionParameter<ListItem> listItemActions, bool skipCounting) :
			this(cmdlet, false, webActions, listActions, listItemActions, skipCounting)
		{
			_listName = listName;
			_webs = new List<Web>() { web };

			_isListNameSpecified = true;
		}

		public InvokeWebActionResult StartProcessAction()
		{
			_totalExecutionTimeStopWatch = Stopwatch.StartNew();

			_result = new InvokeWebActionResult();
			_result.StartDate = DateTime.Now;

			ClientContext previousContext = SPOnlineConnection.CurrentConnection.Context;

			UpdatePropertiesToLoad();

			List<Web> webs;
			if (_subWebs)
				webs = GetSubWebs(_webs.ToList());
			else
				webs = _webs.ToList();

			if (!_skipCounting)
				CountItems(webs);

			ProcessAction(webs);

			UpdateResult();

			//Reset context to where the user were before.
			SPOnlineConnection.CurrentConnection.Context = previousContext;

			return _result;
		}

		private void UpdatePropertiesToLoad()
		{
			_webActions.Properties = AddProperties(_webActions.Properties, "Title", "Url");

			if (_listActions.HasAnyAction || _listItemActions.HasAnyAction)
			{
				_webActions.Properties = AddProperties(_webActions.Properties, "Lists");
				_listActions.Properties = AddProperties(_listActions.Properties, "Title", "ItemCount");
				_listItemActions.Properties = AddProperties(_listItemActions.Properties, "Id");
			}
		}

		private List<Web> GetSubWebs(List<Web> webs)
		{
			List<Web> allWebs = new List<Web>();
			int webCount = webs.Count;
			for (int webIndex = 0; webIndex < webCount; webIndex++)
			{
				Web currentWeb = webs[webIndex];
				currentWeb.LoadProperties(_webActions.Properties);

				WriteIterationProgress(WebProgressBarId, null, "Fetching sub webs", GetTitle(currentWeb), webIndex, webCount);

				allWebs.AddRange(currentWeb.GetAllWebsRecursive());
				allWebs.Add(currentWeb);
			}

			CompleteProgressBar(WebProgressBarId);

			return allWebs;
		}

		private void CountItems(List<Web> webs)
		{
			_result.TotalWebCount = webs.Count;

			if (_listActions.HasAnyAction || _listItemActions.HasAnyAction)
			{
				int webCount = webs.Count;
				for (int webIndex = 0; webIndex < webCount; webIndex++)
				{
					Web currentWeb = webs[webIndex];
					currentWeb.LoadProperties(_webActions.Properties);

					WriteIterationProgress(WebProgressBarId, null, "Couting webs", GetTitle(currentWeb), webIndex, webCount, null, _totalExecutionTimeStopWatch);

					_result.TotalListCount = currentWeb.Lists.Count;
					_result.TotalListItemCount = currentWeb.Lists.Sum(item => item.ItemCount);

					bool processWeb = _webActions.ShouldProcessAnyAction(currentWeb);

					if (!processWeb)
						continue;

					_result.ProcessedWebCount++;

					if (!_listActions.HasAnyAction && !_listItemActions.HasAnyAction)
						continue;

					ListCollection lists = currentWeb.Lists;
					int listCount = lists.Count;

					for (int listIndex = 0; listIndex < listCount; listIndex++)
					{
						List currentList = lists[listIndex];
						currentList.LoadProperties(_listActions.Properties);

						WriteIterationProgress(ListProgressBarId, WebProgressBarId, "Counting list", GetTitle(currentList), listIndex, listCount);

						if ((_isListNameSpecified && currentList.Title.Equals(_listName, StringComparison.CurrentCultureIgnoreCase)) || _listActions.ShouldProcessAnyAction(currentList))
						{
							_result.ProcessedListCount++;
							_result.ProcessedListItemCount += currentList.ItemCount;
						}
					}

					CompleteProgressBar(ListProgressBarId);
				}

				CompleteProgressBar(WebProgressBarId);
			}
		}

		private void ProcessAction(List<Web> webs)
		{
			bool processAction;
			int webCount = webs.Count;
			for (int webIndex = 0; webIndex < webCount; webIndex++)
			{
				Web currentWeb = webs[webIndex];

				//Update current connection context to the web that is beeing process
				//So commands like Get-PnPList returns the correct list for the current web beeing proccess
				SPOnlineConnection.CurrentConnection.Context = (ClientContext) currentWeb.Context;

				currentWeb.LoadProperties(_webActions.Properties);

				UpdateWebProgressBar(webs, webIndex, webCount, 0, _totalExecutionTimeStopWatch);

				if (!_webActions.ShouldProcessAnyAction(currentWeb))
					continue;

				processAction = ProcessAction(currentWeb, GetTitle, _webActions.Properties, _webActions.ShouldProcessAction, _webActions.Action, ref _currentWebsProcessed, ref _averageWebTime, ref _averageShouldProcessWebTime);

				if (!processAction)
					continue;

				if (_listActions.HasAnyAction || _listItemActions.HasAnyAction)
				{
					ListCollection lists = currentWeb.Lists;
					int listCount = lists.Count;

					for (int listIndex = 0; listIndex < listCount; listIndex++)
					{
						List currentList = lists[listIndex];
						currentList.LoadProperties(_listActions.Properties);

						if (_isListNameSpecified && !currentList.Title.Equals(_listName, StringComparison.CurrentCultureIgnoreCase))
							continue;

						UpdateWebProgressBar(webs, webIndex, webCount, listIndex, _totalExecutionTimeStopWatch);

						UpdateListProgressBar(lists, listIndex, listCount);

						processAction = ProcessAction(currentList, GetTitle, _listActions.Properties, _listActions.ShouldProcessAction, _listActions.Action, ref _currentListsProcessed, ref _averageListTime, ref _averageShouldProcessListTime);

						if (!processAction)
							continue;

						if (_listItemActions.HasAnyAction)
						{
							ListItemCollection listItems = currentList.GetItems(CamlQuery.CreateAllItemsQuery());
							currentList.Context.Load(listItems);
							currentList.Context.ExecuteQueryRetry();

							int listItemCount = listItems.Count;

							for (int listItemIndex = 0; listItemIndex < listItemCount; listItemIndex++)
							{
								ListItem currentListItem = listItems[listItemIndex];

								currentListItem.LoadProperties(_listItemActions.Properties);

								WriteIterationProgress(ListItemProgressBarId, ListProgressBarId, "Iterating list items", GetTitle(currentListItem), listItemIndex, listItemCount, CalculateRemainingTimeForListItems(listItemCount, listItemIndex));

								ProcessAction(currentListItem, GetTitle, _listItemActions.Properties, _listItemActions.ShouldProcessAction, _listItemActions.Action, ref _currentListItemsProcessed, ref _averageListItemTime, ref _averageShouldProcessListItemTime);
							}

							CompleteProgressBar(ListItemProgressBarId);
						}

						processAction = ProcessAction(currentList, GetTitle, _listActions.Properties, _listActions.ShouldProcessPostAction, _listActions.PostAction, ref _currentPostListsProcessed, ref _averagePostListTime, ref _averageShouldProcessPostListTime);
					}

					CompleteProgressBar(ListProgressBarId);
				}

				processAction = ProcessAction(currentWeb, GetTitle, _webActions.Properties, _webActions.ShouldProcessPost, _webActions.PostAction, ref _currentPostWebsProcessed, ref _averagePostWebTime, ref _averageShouldProcessPostWebTime);
			}

			CompleteProgressBar(WebProgressBarId);
		}

		private void UpdateResult()
		{
			_result.ProcessedWebCount = _currentWebsProcessed;

			if(_webActions.HasPostAction)
				_result.ProcessedPostWebCount = _currentPostWebsProcessed;

			_result.ProcessedListCount = _currentListsProcessed;

			if (_listActions.HasPostAction)
				_result.ProcessedPostListCount = _currentPostListsProcessed;

			_result.ProcessedListItemCount = _currentListItemsProcessed;

			if(_webActions.HasAction)
				_result.AverageWebTime = _averageWebTime;

			if(_webActions.HasPostAction)
				_result.AveragePostWebTime = _averagePostWebTime;

			if (_listActions.HasAction)
				_result.AverageListTime = _averageListTime;

			if(_listActions.HasPostAction)
				_result.AveragePostListTime = _averagePostListTime;

			if (_listItemActions.HasAction)
				_result.AverageListItemTime = _averageListItemTime;

			_result.EndDate = DateTime.Now;
		}

		private void UpdateWebProgressBar(List<Web> webs, int index, int count, int listProcessedOnCurrentWeb, Stopwatch totalExecutionTimeStopWatch)
		{
			WriteIterationProgress(WebProgressBarId, null, "Iterating webs", GetTitle(webs[index]), index, count, CalculateRemainingTimeForWeb(webs, index, listProcessedOnCurrentWeb), totalExecutionTimeStopWatch);
		}

		private void UpdateListProgressBar(ListCollection lists, int index, int count)
		{
			WriteIterationProgress(ListProgressBarId, WebProgressBarId, "Iterating lists", GetTitle(lists[index]), index, count, CalculateRemainingTimeForList(lists, index));
		}

		private bool ProcessAction<T>(T item, Func<T, string> getTitle, string[] properties, Func<T, bool> shouldProcessAction, Action<T> action, ref int itemsProcessed, ref double averageActionTime, ref double averageShouldProcessTime) where T : SecurableObject
		{
			Stopwatch shouldProcessStopWatch = Stopwatch.StartNew();

			item.LoadProperties(properties);

			string title = getTitle(item);

			bool processAction;

			if (shouldProcessAction == null)
				processAction = true;
			else
				processAction = shouldProcessAction(item);

			averageShouldProcessTime = CalculateAverage(averageShouldProcessTime, shouldProcessStopWatch.Elapsed.TotalSeconds, itemsProcessed);

			if (processAction && action != null)
			{
				if (_cmdlet.ShouldProcess(title, "Process action"))
				{
					Stopwatch actionStopWatch = Stopwatch.StartNew();

					action(item);
					averageActionTime = CalculateAverage(averageActionTime, actionStopWatch.Elapsed.TotalSeconds, itemsProcessed);
				}
			}

			if(processAction)
				itemsProcessed++;

			return processAction;
		}

		private string GetTitle(Web web) => $"{web.Title} - {web.Url}";

		private string GetTitle(List list) => $"{list.Title}";

		private string GetTitle(ListItem listItem)
		{
			string statusText;

			if (!string.IsNullOrEmpty(listItem["Title"]?.ToString()) &&
				listItem.File != null &&
				listItem.File.IsObjectPropertyInstantiated(item => item.Name) &&
				string.IsNullOrEmpty(listItem.File.Name))
			{
				statusText = $"{listItem["Title"]} - {listItem.File.Name}";
			}
			else if (!string.IsNullOrEmpty(listItem["Title"]?.ToString()))
				statusText = listItem["Title"].ToString();
			else
				statusText = listItem.Id.ToString();
			
			return statusText;
		}

		private double CalculateRemainingTimeForWeb(List<Web> webs, int websProcessed = 0, int listProcessedOnCurrentWeb = 0)
		{
			if (_skipCounting)
				return -1;

			double timeRemaining = ((_averageWebTime + _averagePostWebTime) + (_averageShouldProcessWebTime + _averageShouldProcessPostWebTime)) * (webs.Count - websProcessed);

			//We have already processed the WebAction on current web, this will make the WebProgressBar and ListProgressBar in sync when iteration one web.
			if (listProcessedOnCurrentWeb > 0)
			{
				timeRemaining -= _averageWebTime;
				timeRemaining -= _averageShouldProcessWebTime;
			}

			if (_listActions.HasAnyAction || _listItemActions.HasAnyAction)
			{
				timeRemaining += CalculateRemainingTimeForList(webs[websProcessed].Lists, listProcessedOnCurrentWeb);
				timeRemaining += webs.Skip(websProcessed + 1).Sum(item => CalculateRemainingTimeForList(item.Lists));
			}

			return timeRemaining;
		}

		private double CalculateRemainingTimeForList(ListCollection lists, int itemsProcessed = 0)
		{
			if (_skipCounting)
				return -1;

			double timeRemaining = ((_averageListTime + _averagePostListTime) + (_averageShouldProcessListTime + _averageShouldProcessPostListTime)) * (lists.Count - itemsProcessed);
			int remainingListItems = lists.Cast<List>().Skip(itemsProcessed).Sum(item => item.ItemCount);

			if(_listItemActions.HasAnyAction)
				timeRemaining += CalculateRemainingTimeForListItems(remainingListItems);

			return timeRemaining; 
		}

		private double CalculateRemainingTimeForListItems(int listItemsCount, int itemsProcessed = 0)
		{
			if (_skipCounting)
				return -1;

			return (_averageListItemTime + _averageShouldProcessListItemTime) * (listItemsCount - itemsProcessed);
		}

		private double CalculateAverage(double averageTime, double totalSeconds, int currentItemsProcessed)
		{
			if (currentItemsProcessed == 0)
				return totalSeconds;

			return averageTime + ((totalSeconds - averageTime) / currentItemsProcessed);
		}

			private void WriteIterationProgress(int id, int? parentId, string activity, string status, int currentIndex, int itemCount, double? secondsRemaining = null, Stopwatch stopwatch = null)
		{
			string activityText = $"{activity}, {currentIndex + 1}/{itemCount}";

			if (stopwatch != null)
			{
				activityText += ", ";

				if (stopwatch.Elapsed.Days > 0)
					activityText += $"{stopwatch.Elapsed.Days} days ";

				activityText += stopwatch.Elapsed.ToString("hh\\:mm\\:ss");
			}

			string statusText = status;

			if (string.IsNullOrEmpty(statusText))
				statusText = " ";

			int procentComplete = (int) (((double)currentIndex / itemCount) * 100);

			ProgressRecord progressRecord = new ProgressRecord(id, activityText, statusText)
			{
				PercentComplete = procentComplete
			};

			if (parentId.HasValue)
				progressRecord.ParentActivityId = parentId.Value;

			if(secondsRemaining.HasValue && !_skipCounting)
				progressRecord.SecondsRemaining = (int) secondsRemaining.Value;

			_cmdlet.WriteProgress(progressRecord);
		}

		private void CompleteProgressBar(int id)
		{
			//HACK: Since you need the Activity name to close the progress bar we write an empty temporary activity first.
			_cmdlet.WriteProgress(new ProgressRecord(id, " ", " ")
			{
				PercentComplete = 100
			});

			_cmdlet.WriteProgress(new ProgressRecord(id, " ", " ")
			{
				RecordType = ProgressRecordType.Completed
			});
		}

		private string[] AddProperties(string[] properties, params string[] propertiesToAdd)
		{
			if (properties == null)
				return propertiesToAdd;

			return properties.Union(propertiesToAdd).ToArray();
		}
	}
}
