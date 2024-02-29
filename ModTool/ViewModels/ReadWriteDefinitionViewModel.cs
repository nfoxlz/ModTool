using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace ModTool.ViewModels
{
    internal sealed partial class ReadWriteDefinitionViewModel : DialogViewModel
    {
        public static IEnumerable<Models.Function> Functions { get; private set; }

        static ReadWriteDefinitionViewModel() => Functions = Enum.GetValues<Models.Function>();

        [ObservableProperty]
        private Models.ReadWriteSetting _setting;

        public ReadWriteDefinitionViewModel(Models.ReadWriteSetting setting) => Setting = setting;
    }
}
