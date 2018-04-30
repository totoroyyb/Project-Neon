using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Neon.Helper
{
    public static class MediaPlayerHelper
    {
        public static void CleanUpMediaPlayerSource(Windows.Media.Playback.MediaPlayer mp)
        {
            if (mp?.Source != null)
            {
                var source = mp.Source as Windows.Media.Core.MediaSource;
                source?.Dispose();

                var item = mp.Source as Windows.Media.Playback.MediaPlaybackItem;
                item?.Source?.Dispose();

                mp.Source = null;
            }
        }
    }
}
