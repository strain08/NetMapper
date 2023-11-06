using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetMapper.ViewModels;
using Splat;

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