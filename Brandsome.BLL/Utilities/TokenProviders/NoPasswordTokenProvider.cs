using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities.TokenProviders
{
    public class NoPasswordTokenProvider<TUser> : DataProtectorTokenProvider<Timer> where TUser : IdentityUser
    {
        public NoPasswordTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<Timer>> logger) : base(dataProtectionProvider, options, logger)
        {
        }


    }
}
