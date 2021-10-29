using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filestransformer.Settings
{
    public interface ISettingsProvider
    {
        Setting GetSettings();
    }
}
