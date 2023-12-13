using System;
using Firebase.Database;
using Firebase.Storage;
using JMEliAppMaui.ProgramHelpers;

namespace JMEliAppMaui.Services.Implementations
{
	public sealed class FibInstance : ISingletonDependency
	{
        private static FirebaseClient _instance;

       
        public static FirebaseClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FirebaseClient(ProgramHelpers.Contants.FibConstants.fibRef);
            }
            return _instance;
        }
    }


    public sealed class FibStoreInstance : ISingletonDependency
	{
        private static FirebaseStorage _instance;

       
        public static FirebaseStorage GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FirebaseStorage(ProgramHelpers.Contants.FibConstants.fibAppSpot);
            }
            return _instance;
        }
    }
}

