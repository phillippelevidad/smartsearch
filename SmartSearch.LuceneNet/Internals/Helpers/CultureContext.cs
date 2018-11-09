using System;
using System.Globalization;
using System.Threading;

namespace SmartSearch.LuceneNet.Internals.Helpers
{
    class CultureContext : IDisposable
    {
        public static CultureContext Invariant => new CultureContext();

        public CultureInfo Culture { get; }

        public CultureInfo BackupCulture { get; }

        public CultureInfo BackupUICulture { get; }

        public CultureContext() : this(CultureInfo.InvariantCulture)
        {
        }

        public CultureContext(string specificCulture) : this(CultureInfo.CreateSpecificCulture(specificCulture))
        {
        }

        public CultureContext(CultureInfo culture)
        {
            Culture = culture;
            BackupCulture = Thread.CurrentThread.CurrentCulture;
            BackupUICulture = Thread.CurrentThread.CurrentUICulture;
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = BackupCulture;
            Thread.CurrentThread.CurrentUICulture = BackupUICulture;
        }
    }
}
