using MvvmCross.ViewModels;
using MvxPoet.Core.ViewModels;

namespace MvxPoet.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<WritePoemViewModel>();
        }
    }
}
