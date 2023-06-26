using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Application
{
    public class SessionManager
    {
        private readonly DbContext _dbContext;
        private readonly AsyncLocal<int> _sessionId;

        public SessionManager(DbContext dbContext, AsyncLocal<int> sessionId)
        {
            _dbContext = dbContext;
            _sessionId = sessionId;
        }

        public Session BeginAsync()
        {
            // TODO: Enrich log files with session id

            var sessionId = new SqlParameter("@sessionId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            _dbContext.Database.ExecuteSqlRaw("EXEC @sessionId = dbo.spStartSession", sessionId);
            _sessionId.Value = (int)sessionId.Value;

            if (_sessionId.Value == -1) throw new ApplicationException("Unable to start a Lazarus session.");

            return new Session(_sessionId.Value, this);
        }

        public Task EndAsync()
        {
            return _dbContext.Database.ExecuteSqlRawAsync($"EXEC dbo.spEndSession @sessionId={_sessionId.Value}");
        }
    }

    public class Session : IDisposable, IAsyncDisposable
    {
        private readonly SessionManager _manager;
        private bool _isDisposed;

        internal Session(int sessionId, SessionManager manager)
        {
            SessionId = sessionId;
            _manager = manager;
        }

        ~Session()
        {
            _manager.EndAsync().RunSynchronously();
            _isDisposed = true;
        }

        public int SessionId { get; }

        public void Dispose()
        {
            if (_isDisposed) return;

            _manager.EndAsync().RunSynchronously();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed) return;

            _isDisposed = true;
            GC.SuppressFinalize(this);

            await _manager.EndAsync();
        }
    }
}