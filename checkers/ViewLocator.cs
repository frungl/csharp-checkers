using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using checkers.ViewModels;

namespace checkers
{
    /// <inheritdoc />
    public class ViewLocator : IDataTemplate
    {
        /// <inheritdoc />
        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        /// <inheritdoc />
        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}