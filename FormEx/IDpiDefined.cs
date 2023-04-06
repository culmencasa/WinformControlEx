using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public interface IDpiDefined
    {
        float DesigntimeScaleFactorX { get; set; }
        float DesigntimeScaleFactorY { get; set; }

        float RuntimeScaleFactorX { get; set; }


        float RuntimeScaleFactorY { get; set; }


        float ScaleFactorRatioX { get;  }
        float ScaleFactorRatioY { get;  }
    }
}
