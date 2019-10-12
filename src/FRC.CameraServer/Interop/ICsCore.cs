using System;

namespace FRC.CameraServer.Interop
{
    public interface ICsCore
    {
        unsafe PropertyKind CS_GetPropertyKind(CsProperty property, CsStatus* status);
        unsafe byte* CS_GetPropertyName(CsProperty property, CsStatus* status);
        unsafe int CS_GetProperty(CsProperty property, CsStatus* status);

        unsafe byte** CS_GetEnumPropertyChoices(CsProperty property, int* count, CsStatus* status);


        unsafe CsSource CS_CreateUsbCameraDev(byte* name, int dev, CsStatus* status);
        unsafe CsSource CS_CreateUsbCameraPath(byte* name, byte* path, CsStatus* status);
        unsafe CsSource CS_CreateHttpCamera(byte* name, byte* url, HttpCameraKind kind, CsStatus* status);
        unsafe CsSource CS_CreateHttpCameraMulti(byte* name, byte** urls, int count, HttpCameraKind kind, CsStatus* status);
        
        unsafe SourceKind CS_GetSourceKind(CsSource source, CsStatus* status);
        unsafe char* CS_GetSourceName(CsSource source, CsStatus* status);




        unsafe void CS_Shutdown();

        unsafe void CS_FreeString(byte* str);
        unsafe void CS_FreeEnumPropertyChoices(byte** choices, int count);
        unsafe void CS_FreeHttpCameraUrls(byte** urls, int count);
        unsafe void CS_FreeEnumeratedProperties(CsProperty* properties, int count);
        unsafe byte* CS_GetHostname();
        unsafe byte** CS_GetNetworkInterfaces(int* count);
        unsafe void CS_FreeNetworkInterfaces(byte** interfaces, int count);
    }
}