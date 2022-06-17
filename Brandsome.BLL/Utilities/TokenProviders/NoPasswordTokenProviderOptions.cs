using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities.TokenProviders
{
    public class NoPasswordTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public NoPasswordTokenProviderOptions()
        {
            Name = AppSetting.NoPasswordTokenProviderName;
            TokenLifespan = TimeSpan.FromMilliseconds(-1);
        }
    }
}
