using System;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ExactTarget.ETPushSdk
{
    /// <summary>
    /// This is a lightweight SQLite wrapper that handles database interaction. It is only used privately, and shouldn't be used by others.
    /// Most of the methods are self-explainatory, so this class isn't heavily documented.
    /// </summary>
    [BaseType(typeof(NSObject))]
    //[DisableDefaultCtor]
    public partial interface ETSqliteHelper
    {
        // sqlite3 *_db;

        /// <summary>
        /// Constructors this instance.
        /// </summary>
        /// <returns></returns>
        //[Export("init")]
        //IntPtr Constructor();

        /// <summary>
        /// Gets or sets the max retries.
        /// </summary>
        /// <value>
        /// The max retries.
        /// </value>
        [Export("maxRetries")]
        int MaxRetries { get; set; }

        /// <summary>
        /// Gets the max retries.
        /// </summary>
        /// <value>
        /// The max retries.
        /// </value>
        [Export("database")]
        ETSqliteHelper Database { get; }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        [Export("open")]
        bool Open();

        /// <summary>
        /// Closes this instance.
        /// </summary>
        [Export("close")]
        void Close();

        /// <summary>
        /// Gets the last error message.
        /// </summary>
        /// <value>
        /// The last error message.
        /// </value>
        [Export("lastErrorMessage")]
        string LastErrorMessage { get; }

        /// <summary>
        /// Gets the last error code.
        /// </summary>
        /// <value>
        /// The last error code.
        /// </value>
        [Export("lastErrorCode")]
        int LastErrorCode { get; }

        /// <summary>
        /// Gets the rows affected.
        /// </summary>
        /// <value>
        /// The rows affected.
        /// </value>
        [Export("rowsAffected")]
        int RowsAffected { get; }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        //[Obsolete]
        //[Export("executeQuery:")]
        //NSObject[] ExecuteQuery(string sql, params object[] args);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        [Export("executeQuery:arguments:")]
        NSObject[] ExecuteQuery(string sql, params NSObject[] args);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        //[Obsolete]
        //[Export("executeUpdate:")]
        //bool ExecuteUpdate(string sql, params object[] args);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        [Export("executeUpdate:arguments:")]
        bool ExecuteUpdate(string sql, params NSObject[] arguments);

        /// <summary>
        /// Tables the exists.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        [Export("tableExists:")]
        bool TableExists(string tableName);

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        [Export("beginTransaction")]
        bool BeginTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        [Export("commitTransaction")]
        bool CommitTransaction();

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        /// <returns></returns>
        [Export("rollbackTransaction")]
        bool RollbackTransaction();
    }
}
