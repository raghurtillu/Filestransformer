namespace Filestransformer.Settings
{
    /// <summary>
    /// Setting provider interface <seealso cref="Setting"/>
    /// </summary>
    public interface ISettingsProvider
    {
        Setting GetSettings();
    }
}
