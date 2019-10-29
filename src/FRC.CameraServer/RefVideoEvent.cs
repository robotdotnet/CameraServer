using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
{
    public delegate void VideoEventDelegate(in RefVideoEvent videoEvent);

    public unsafe readonly ref struct RefVideoEvent
    {
        private readonly CS_Event* evnt;
        public EventKind Kind => evnt->kind;
        public CS_Source Source => evnt->source;
        public CS_Sink Sink => evnt->sink;
        public string Name => UTF8String.ReadUTF8String(evnt->name);
        public VideoMode Mode => evnt->mode;
        public CS_Property Property => evnt->property;
        public PropertyKind PropertyKind => evnt->propertyKind;
        public int Value => evnt->value;
        public string ValueStr => UTF8String.ReadUTF8String(evnt->valueStr);

        public RefVideoEvent(CS_Event* evnt) {
            this.evnt = evnt;
        }
    }
}
