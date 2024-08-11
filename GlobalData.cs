using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddSlider {
    public static class GlobalData {
        public static Mode Mode { get; set; } = Mode.音量控制;
    }

    public enum Mode {
        音量控制 = 0,
        屏幕亮度控制 = 1
    }
}
