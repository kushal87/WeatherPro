using System;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;


namespace WeatherPro.Core
{
    public class App: MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }

}
