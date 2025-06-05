#if TUANJIE_1_5_OR_NEWER
using minihost.editor;
using System;

namespace TapTapMiniGame
{
    [Serializable]
    public class TapProjectConf: TJProjectConf 
    {
        public TapProjectConf()
        {
            bgImageSrc = TapTapUtil.tapTapBgImageSrc;
        }
    }
}

#endif