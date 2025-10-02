using Microsoft.Extensions.DependencyInjection;
using QuotationSystem2.PresentationLayer.Services.Interfaces;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuotationSystem2.PresentationLayer.Services.Localization
{
    [MarkupExtensionReturnType(typeof(BindingExpression))] // Return type for this extension  
    public class LocExtension : MarkupExtension
    {
        public required string Key { get; set; }
        public bool ForceStatic { get; set; } = false;

        public LocExtension() { }

        public LocExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Return empty if key is not provided
            if (string.IsNullOrWhiteSpace(Key))
                return string.Empty;

            var keys = Key.Split('|');
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            // Case: multiple keys, no dependency object
            if (keys.Length > 1)
            {
                var staticText = FormatKeys(keys);

                // Try to get target object and property
                if (target?.TargetObject != null && target.TargetProperty != null)
                {
                    var weakTarget = new WeakReference(target.TargetObject);
                    var targetProperty = target.TargetProperty;

                    // Update value on language change
                    LanguageService.Instance.LanguageChanged += (sender, e) =>
                    {
                        if (!weakTarget.IsAlive) return;

                        var instance = weakTarget.Target;
                        var value = FormatKeys(keys);

                        // Use reflection to set property value
                        var propertyInfo = instance?.GetType().GetProperty(targetProperty.ToString() ?? string.Empty);
                        propertyInfo?.SetValue(instance, value);
                    };
                }

                return staticText;
            }

            // Case: single key (normal binding)
            return new Binding($"[{Key}]")
            {
                Source = new LocBindingSource(),
                Mode = BindingMode.OneWay
            }.ProvideValue(serviceProvider);
        }

        /// <summary>
        /// Format multiple keys into a localized string.
        /// Chinese (zh-TW) -> concatenated without space.
        /// Others -> joined with space.
        /// </summary>
        private static string FormatKeys(string[] keys)
        {
            return LanguageService.Instance.CurrentCulture.Name == "zh-TW"
                ? string.Concat(keys.Select(k => LocBindingSource.Get(k)))
                : string.Join(" ", keys.Select(k => LocBindingSource.Get(k)));
        }

        /// <summary>
        /// Provides localized strings and raises updates on language change.
        /// </summary>
        private class LocBindingSource : INotifyPropertyChanged
        {
            private readonly ILanguageService _languageService = LanguageService.Instance;

            public event PropertyChangedEventHandler? PropertyChanged;

            public LocBindingSource()
            {
                // Subscribe to language change events
                _languageService.LanguageChanged += (s, e) => OnLanguageChanged();
            }

            private void OnLanguageChanged()
            {
                // Notify that all bindings should update
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }

            // Indexer for localized string lookup
            public string this[string key] => _languageService.GetUIStrings(key);

            public static string Get(string key) =>
                LanguageService.Instance.GetUIStrings(key);
        }
    }
}
