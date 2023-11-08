using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UICore.interfaces
{
    public interface IHighLighter
    {
        Task HighLightElement(Point point);
    }
}
