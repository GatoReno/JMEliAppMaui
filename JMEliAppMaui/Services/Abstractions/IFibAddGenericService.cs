using System;
namespace JMEliAppMaui.Services.Abstractions
{
	public interface IFibAddGenericService
    {
        Task<object> AddChild<T>(T children,string concept);
        Task UpdateChild<T>(T children, string concept, string id);
        Task<IReadOnlyCollection<object>> GetChilds(string concept);
       Task DeleteChild(string id, string concept);
    }

    public interface IFibGenService
    {

    }
}




