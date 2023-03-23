using System.Data.Common;

namespace WpfApp.Data.Services;

public interface IFluentDbReaderFactory
{
	IFluentDbReader Create(DbDataReader reader);
	IFluentDbReader Create(DbDataReader reader, int ordinal);
}
