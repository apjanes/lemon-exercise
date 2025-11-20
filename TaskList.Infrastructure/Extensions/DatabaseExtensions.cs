using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TaskList.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    private const int ErrorCodeConstraintUnique = 2067;
    private const int ErrorCodePrimaryKey = 1555;

    public static bool IsUniqueKeyViolation(this DbUpdateException exception)
    {
        if (exception.InnerException is SqliteException sqliteException)
        {
            return sqliteException.SqliteErrorCode == ErrorCodeConstraintUnique
                   || sqliteException.SqliteErrorCode == ErrorCodePrimaryKey;
        }

        return false;
    }
}