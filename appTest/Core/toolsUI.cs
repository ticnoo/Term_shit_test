using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appTest.Core.toolsUI
{
    static class toolsUI
    {
        private const int ScrollPixelBorderError = 0;
        public static void MoveScrollToEnd(ScrollViewer scroll, double prevMaxHeight)
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                double current_offset = scroll.Offset.Y;
                //double current_extent = scroll.ScrollBarMaximum.Y - pixelLock;
                //if (current_offset > current_extent)
                //{
                //    scroll?.ScrollToEnd();
                //}
                if (prevMaxHeight <= (current_offset + ScrollPixelBorderError))
                {
                    scroll?.ScrollToEnd();
                }
            });
        }

        public static void MoveScrollToEnd(ScrollViewer scroll)
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                scroll?.ScrollToEnd();
            });
        }
    }
}
