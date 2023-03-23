using System.Data.Common;

namespace WpfApp.Data.Services;

public class FluentDbReaderFactory : IFluentDbReaderFactory
{
	public IFluentDbReader Create(DbDataReader reader)
	{
		return new FluentDbReader(reader, 0);
	}

	public IFluentDbReader Create(DbDataReader reader, int ordinal)
	{
		return new FluentDbReader(reader, ordinal);
	}
}
