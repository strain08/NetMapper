using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NetMapper.Extensions;
using NetMapper.ViewModels;
using Splat;
using System;

namespace NetMapper;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        var name = data!.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            if (Design.IsDesignMode)
                return (Control)Activator.CreateInstance(type)!;
            else
                return Locator.Current.CreateControlWithConstructorInjection(type)!;
        }
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}