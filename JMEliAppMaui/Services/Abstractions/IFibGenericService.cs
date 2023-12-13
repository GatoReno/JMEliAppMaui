using System;
namespace JMEliAppMaui.Services.Implementations
{
	public interface IFibGenericService<T>
	{
        Task<T> AddClient(T children);
    }
}

