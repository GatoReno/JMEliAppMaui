﻿using System;
namespace JMEliAppMaui.Services.Abstractions
{
	public interface IFibAddGenericService<T>
    {
        Task<T> AddChild(T children,string concept);
        Task UpdateChild(T children, string concept, string id);
        Task<T> GetChilds(string concept);
    }
}




