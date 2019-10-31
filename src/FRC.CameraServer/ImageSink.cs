using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
{
    public abstract class ImageSink : VideoSink
    {
        protected ImageSink(CS_Sink handle)
            : base(handle)
        {

        }

        public override string Description 
        { 
            get => base.Description; 
            set
            {
                CsCore.SetSinkDescription(Handle, value.AsSpan());
            }
        }

        public string GetError()
        {
            return CsCore.GetSinkError(Handle);
        }

        public bool Enabled
        {
            set
            {
                CsCore.SetSinkEnabled(Handle, value);
            }
        }
    }
}
