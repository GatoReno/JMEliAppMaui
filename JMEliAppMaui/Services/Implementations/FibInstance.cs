using System;
using Firebase.Database;
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
}

