using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
{
    public abstract class ImageSource : VideoSource
    {
        protected ImageSource(CS_Source source)
           : base(source)
        {

        }

        public void NotifyError(string msg)
        {
            CsCore.NotifySourceError(Handle, msg.AsSpan());
        }

        public override bool Connected
        {
            get => base.Connected; 
            set
            {
                CsCore.SetSourceConnected(Handle, value);
            }
        }

        public override string Description
        {
            get => base.Description;
            set
            {
                CsCore.SetSourceDescription(Handle, value.AsSpan());
            }
        }

        public VideoProperty CreateProperty(string name,
                                      PropertyKind kind,
                                      int minimum,
                                      int maximum,
                                      int step,
                                      int defaultValue,
                                      int value)
        {
            return new VideoProperty(CsCore.CreateSourceProperty(Handle, name.AsSpan(), kind, minimum, maximum, step, defaultValue, value));
        }

        public VideoProperty CreateIntegerProperty(string name,
                              int minimum,
                              int maximum,
                              int step,
                              int defaultValue,
                              int value)
        {
            return new VideoProperty(CsCore.CreateSourceProperty(Handle, name.AsSpan(), PropertyKind.Integer, minimum, maximum, step, defaultValue, value));
        }

        public VideoProperty CreateBooleanProperty(string name, bool defaultValue, bool value)
        {
            return new VideoProperty(CsCore.CreateSourceProperty(Handle, name.AsSpan(), PropertyKind.Boolean, 0, 1, 1, defaultValue ? 1 : 0, value ? 1 : 0));
        }

        public VideoProperty CreateStringProperty(string name, string value)
        {
            VideoProperty prop = new VideoProperty(CsCore.CreateSourceProperty(Handle, name.AsSpan(), PropertyKind.String, 0, 0, 0, 0 ,0));
            prop.SetString(value);
            return prop;
        }

        public void SetEnumPropertyChoices(VideoProperty property, params string[] choices)
        {
            CsCore.SetSourceEnumPropertyChoices(Handle, property.m_handle, choices);
        }

    }
}
