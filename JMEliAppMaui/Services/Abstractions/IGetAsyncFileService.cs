namespace JMEliAppMaui.Services.Abstractions;

public interface IGetAsyncFileService
{ 
        Task SaveAndView(string fileName,MemoryStream stream,OpenOption context,string contentType = "application/pdf"); 
}

public enum OpenOption
{
    InApp,
    ChooseApp
}   