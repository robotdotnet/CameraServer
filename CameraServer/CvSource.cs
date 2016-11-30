﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenCvSharp;
using static CameraServer.Native.NativeMethods;

namespace CameraServer
{
    public class CvSource : VideoSource
    {
        public CvSource(string name, VideoMode mode) 
            : base (CreateCvSource(name, mode.PixelFormat, mode.Width, mode.Height, mode.FPS))
        {

        }

        public CvSource(string name, PixelFormat pixelFormat, int width, int height, int fps)
            : base(CreateCvSource(name, pixelFormat, width, height, fps))
        {

        }

        public void PutFrame(Mat image)
        {
            PutSourceFrame(m_handle, image.CvPtr);
        }

        public void NotifyError(string msg)
        {
            NotifySourceError(m_handle, msg);
        }

        public override bool Connected
        {
            set
            {
                SetSourceConnected(m_handle, value);
            }
        }

        public override string Description
        {
            set
            {
                SetSourceDescription(m_handle, value);
            }
        }

        public VideoProperty CreateProperty(string name, PropertyKind kind, int minimum, int maximum, int step, int defaultValue, int value)
        {
            return new VideoProperty(
                CreateSourceProperty(m_handle, name, kind, minimum, maximum, step, defaultValue, value));
        }

        public void SetEnumPropertyChoices(VideoProperty property, IList<string> choices)
        {
            SetSourceEnumPropertyChoices(m_handle, property.m_handle, choices);
        }
    }
}