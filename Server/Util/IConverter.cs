using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_CommerceContinental.Server.Util;

public interface IConverter
{
    public Task<byte[]> ConvertImageToByte(Guid id);
}
