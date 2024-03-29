﻿using System.Collections.Specialized;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using RedmineWorkingHours.ConsoleApp.Configuration;

namespace RedmineWorkingHours.ConsoleApp.Communication.Redmine;

internal class RedmineCommunication : IHoursReader
{
    #region Constants

    private const string RedmineDateFormat = "yyyy-MM-dd";

    #endregion

    #region Members

    private readonly RedmineManager _manager;
    private readonly int _targetUserId;

    #endregion

    #region Constructor

    public RedmineCommunication(AppConfiguration configuration)
    {
        _manager = new RedmineManager(configuration.RedmineConfiguration.ServerUrl, configuration.RedmineConfiguration.ApiKey);
        _targetUserId = configuration.RedmineConfiguration.TargetUserId;
    }

    #endregion

    #region Get data from Redmine

    public double GetWorkedHoursBetween(DateTime begin, DateTime end)
    {
        var parameters = new NameValueCollection
        {
            { RedmineKeys.USER_ID, _targetUserId.ToString() },
            { RedmineKeys.SPENT_ON, $"><{begin.ToString(RedmineDateFormat)}|{end.ToString(RedmineDateFormat)}" }
        };
        IEnumerable<TimeEntry>? timeEntries = _manager.GetObjects<TimeEntry>(parameters);
        if (timeEntries == null)
            return 0.0;
        return (double)timeEntries.Select(w => w.Hours).Sum();
    }

    public double GetVacationDaysBetween(DateTime begin, DateTime end)
    {
        var parameters = new NameValueCollection
        {
            { RedmineKeys.ASSIGNED_TO_ID, _targetUserId.ToString() },
            { RedmineKeys.PROJECT_ID, 10.ToString() },
            { RedmineKeys.TRACKER_ID, 7.ToString() }
        };
        IEnumerable<Issue>? allVacations = _manager.GetObjects<Issue>(parameters);
        if (allVacations == null)
            return 0.0;
        IEnumerable<Issue> vacationsInTimeFrame = allVacations
            .Where(i => i.StartDate >= begin && i.DueDate <= end);

        double vacationDays = 0.0;
        foreach (Issue vacation in vacationsInTimeFrame)
        {
            if (vacation.DueDate == null || vacation.StartDate == null)
                continue;

            TimeSpan? vacationDuration = vacation.DueDate.Value.AddDays(1.0) - vacation.StartDate;
            if (vacationDuration == null)
                throw new NotSupportedException($"Inconsistent vacation issue date (start: {vacation.StartDate}, due: {vacation.DueDate})");
            vacationDays += vacationDuration.Value.Days;
        }
        return vacationDays;
    }

    #endregion
}