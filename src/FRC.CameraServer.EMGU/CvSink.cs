﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRC.CameraServer.EMGU
{
    public unsafe class CvSink : ImageSink
    {
        private CS_RawFrame frame = new CS_RawFrame();
        Mat tmpMat;
        byte* dataPtr;
        int width;
        int height;
        int pixelFormat;
        
        private (DepthType type, int channels) GetCvFormat(PixelFormat format)
        {
            DepthType type = DepthType.Cv8U;
            int num = 1;
            switch (format)
            {
                case PixelFormat.BGR:
                    num = 3;
                    break;
                case PixelFormat.YUYV:
                case PixelFormat.RGB565:
                    num = 2;
                    break;
            }
            return (type, num);
        }

        public CvSink(string name)
            :base(CsCore.CreateRawSink(name.AsSpan()))
        {

        }

        public long GrabFrame(Mat image)
        {
            return GrabFrame(image, .225);
        }

        public unsafe long GrabFrame(Mat image, double timeout)
        {
            frame.width = 0;
            frame.height = 0;
            frame.pixelFormat = (int)PixelFormat.BGR;

            ulong rv = CsCore.GrabRawSinkFrameTimeout(Handle, ref frame, timeout);
            if (rv == 0) return 0;

            var (type, channels) = GetCvFormat((PixelFormat)frame.pixelFormat);

            if (dataPtr != frame.data || width != frame.width || height != frame.height || pixelFormat != frame.pixelFormat)
            {
                dataPtr = frame.data;
                width = frame.width;
                height = frame.height;
                pixelFormat = frame.pixelFormat;
                tmpMat = new Mat(frame.height, frame.width, type, channels, (IntPtr)frame.data, 0);
            }

            tmpMat.CopyTo(image);
            return (long)rv;
        }
    }
}
