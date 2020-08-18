using Optional;
using MOP.Infra.Domain.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using Serilog;
using MOP.Infra.Services;
using System.Runtime.CompilerServices;

using static MOP.Infra.Optional.Static;

[assembly: InternalsVisibleTo("MOP.Host.Test")]
namespace MOP.Host.Events
{
    internal class EventStorage : IEventStorage, IDisposable
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<EventDb> _data;
        private readonly ILogger _logger;

        public EventStorage(FileInfo dbFile, ILogService log)
        {
            _logger = log.GetContextLogger<IEventStorage>();
            _db = new LiteDatabase($"FileName={dbFile.FullName};Connection=shared");
            _data = _db.GetCollection<EventDb>("EVENTS");
            BsonMapper.Global
                .Entity<EventDb>()
                .Id(e => e.Id);
        }

        public void Dispose() => _db?.Dispose();

        public IEnumerable<IEvent<object>> GetAllEvents()
        {
            return _data.FindAll()
                .Select(e => e.ToEvent());
        }

        public Option<IEvent<object>> GetEvent(Guid id)
        {
            return Some(_data.FindOne(e => e.Id == id))
                .Map(e => e.ToEvent());
        }

        public IEnumerable<IEvent<object>> GetEvents(Guid startId)
        {
            var @event = GetEvent(startId);
            return @event.Match(
                some: e => GetEvents(e.DateTime),
                none: () => new List<IEvent<object>>()
            );
        }

        public IEnumerable<IEvent<object>> GetEvents(DateTime from)
        {
            return _data.Find(e => e.DateTime >= from)
                .Select(e => e.ToEvent());
        }

        public void WriteEvent<T>(IEvent<T> @event)
        {
            _logger.Debug("Writing event {@Type} with id: {@Id}", @event.Type, @event.Id);
            if (_data.Upsert(EventDb.From(@event)))
                _logger.Debug("Saved event: {@Id}", @event.Id);
            else
                _logger.Warning("Can't save event: {@Id}", @event.Id);
        }
    }
}
