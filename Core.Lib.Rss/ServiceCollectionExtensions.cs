using Core.Lib.RSS.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Core.Lib.Rss
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRss(this IServiceCollection services)
            => services;

    }

    public interface IRssStore
    {
        RSSItem GetItemById(int Id);
        IEnumerable<RSSItem> GetItemsByFeed(RSSFeed feed);
    }

    public interface IFeedStore
    {
        IEnumerable<RSSFeed> GetFeeds();
        void Register(RSSFeed feed);
        void Remove(RSSFeed feed);
    }
    public interface IState<T>
    {
        bool IsRead { get; }
        bool IsNew { get; }
        T Target { get; }
    }

    public interface ICollectionState<T> : IEnumerable<IState<T>>
    {
        int News { get; }
    }

    public interface ITargetInfo<T>
    {
        RSSFeed Feed { get; }
        DateTime CreateDate { get; }
        int Id { get; }
    }
}
