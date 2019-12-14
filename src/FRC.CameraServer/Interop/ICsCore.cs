using System;

namespace FRC.CameraServer.Interop
{
    public unsafe delegate void CsListenerEvent(void* data, CS_Event* csEvent);

    public unsafe interface ICsCore
    {
        unsafe PropertyKind CS_GetPropertyKind(CS_Property property, int* status);
        unsafe byte* CS_GetPropertyName(CS_Property property, int* status);
        unsafe int CS_GetProperty(CS_Property property, int* status);
        void CS_SetProperty(CS_Property property, int value, int* status);
        int CS_GetPropertyMin(CS_Property property, int* status);
        int CS_GetPropertyMax(CS_Property property, int* status);
        int CS_GetPropertyStep(CS_Property property, int* status);
        int CS_GetPropertyDefault(CS_Property property, int* status);
        unsafe byte* CS_GetStringProperty(CS_Property property, int* status);
        void CS_SetStringProperty(CS_Property property, byte* value,
                                  int* status);
        unsafe byte** CS_GetEnumPropertyChoices(CS_Property property, int* count,
                                 int* status);


        /**
         * @defgroup cscore_source_create_cfunc Source Creation Functions
         * @{
         */
        CS_Source CS_CreateUsbCameraDev(byte* name, int dev, int* status);
        CS_Source CS_CreateUsbCameraPath(byte* name, byte* path,
                                         int* status);
        CS_Source CS_CreateHttpCamera(byte* name, byte* url,
                                      HttpCameraKind kind, int* status);
        CS_Source CS_CreateHttpCameraMulti(byte* name, byte** urls,
                                   int count, HttpCameraKind kind,
                                   int* status);
        /** @} */

        void CS_AllocateRawFrameData(CS_RawFrame* frame, int requestedSize);
        void CS_FreeRawFrameData(CS_RawFrame* frame);

        ulong CS_GrabRawSinkFrame(CS_Sink sink, CS_RawFrame* rawImage,
                             int* status);
        ulong CS_GrabRawSinkFrameTimeout(CS_Sink sink, CS_RawFrame* rawImage,
                                    double timeout, int* status);

        CS_Sink CS_CreateRawSink(byte* name, int* status);

        CS_Sink CS_CreateRawSinkCallback(byte* name, void* data, IntPtr processFrame,
                                         int* status);

        void CS_PutRawSourceFrame(CS_Source source, CS_RawFrame* image,
                          int* status);

        CS_Source CS_CreateRawSource(byte* name, VideoMode* mode,
                                     int* status);

        /**
         * @defgroup cscore_source_cfunc Source Functions
         * @{
         */
        SourceKind CS_GetSourceKind(CS_Source source, int* status);
        byte* CS_GetSourceName(CS_Source source, int* status);
        byte* CS_GetSourceDescription(CS_Source source, int* status);
        ulong CS_GetSourceLastFrameTime(CS_Source source, int* status);
        void CS_SetSourceConnectionStrategy(CS_Source source,
                                    ConnectionStrategy strategy,
                                    int* status);
        CS_Bool CS_IsSourceConnected(CS_Source source, int* status);
        CS_Bool CS_IsSourceEnabled(CS_Source source, int* status);
        CS_Property CS_GetSourceProperty(CS_Source source, byte* name,
                                         int* status);
        CS_Property* CS_EnumerateSourceProperties(CS_Source source, int* count,
                                                  int* status);
        void CS_GetSourceVideoMode(CS_Source source, VideoMode* mode,
                                   int* status);
        CS_Bool CS_SetSourceVideoMode(CS_Source source, VideoMode* mode,
                                      int* status);
        CS_Bool CS_SetSourceVideoModeDiscrete(CS_Source source,
                                              PixelFormat pixelFormat,
                                              int width, int height, int fps,
                                              int* status);
        CS_Bool CS_SetSourcePixelFormat(CS_Source source,
                                        PixelFormat pixelFormat,
                                        int* status);
        CS_Bool CS_SetSourceResolution(CS_Source source, int width, int height,
                                       int* status);
        CS_Bool CS_SetSourceFPS(CS_Source source, int fps, int* status);
        CS_Bool CS_SetSourceConfigJson(CS_Source source, byte* config,
                                       int* status);
        byte* CS_GetSourceConfigJson(CS_Source source, int* status);
        VideoMode* CS_EnumerateSourceVideoModes(CS_Source source, int* count,
                                                   int* status);
        CS_Sink* CS_EnumerateSourceSinks(CS_Source source, int* count,
                                         int* status);
        CS_Source CS_CopySource(CS_Source source, int* status);
        void CS_ReleaseSource(CS_Source source, int* status);
        /** @} */

        /**
         * @defgroup cscore_source_prop_cfunc Camera Source Common Property Fuctions
         * @{
         */
        void CS_SetCameraBrightness(CS_Source source, int brightness,
                                    int* status);
        int CS_GetCameraBrightness(CS_Source source, int* status);
        void CS_SetCameraWhiteBalanceAuto(CS_Source source, int* status);
        void CS_SetCameraWhiteBalanceHoldCurrent(CS_Source source, int* status);
        void CS_SetCameraWhiteBalanceManual(CS_Source source, int value,
                                            int* status);
        void CS_SetCameraExposureAuto(CS_Source source, int* status);
        void CS_SetCameraExposureHoldCurrent(CS_Source source, int* status);
        void CS_SetCameraExposureManual(CS_Source source, int value, int* status);
        /** @} */

        /**
         * @defgroup cscore_usbcamera_cfunc UsbCamera Source Functions
         * @{
         */
        byte* CS_GetUsbCameraPath(CS_Source source, int* status);
        CS_UsbCameraInfo* CS_GetUsbCameraInfo(CS_Source source, int* status);
        /** @} */

        /**
         * @defgroup cscore_httpcamera_cfunc HttpCamera Source Functions
         * @{
         */
        HttpCameraKind CS_GetHttpCameraKind(CS_Source source,
                                                    int* status);
        void CS_SetHttpCameraUrls(CS_Source source, byte** urls, int count,
                                  int* status);
        byte** CS_GetHttpCameraUrls(CS_Source source, int* count, int* status);
        /** @} */

        /**
         * @defgroup cscore_opencv_source_cfunc OpenCV Source Functions
         * @{
         */
        void CS_NotifySourceError(CS_Source source, byte* msg, int* status);
        void CS_SetSourceConnected(CS_Source source, CS_Bool connected,
                                   int* status);
        void CS_SetSourceDescription(CS_Source source, byte* description,
                                     int* status);
        CS_Property CS_CreateSourceProperty(CS_Source source, byte* name,
                                            PropertyKind kind, int minimum,
                                            int maximum, int step, int defaultValue,
                                            int value, int* status);
        void CS_SetSourceEnumPropertyChoices(CS_Source source, CS_Property property,
                                             byte** choices, int count,
                                             int* status);
        /** @} */

        /**
         * @defgroup cscore_sink_create_cfunc Sink Creation Functions
         * @{
         */
        CS_Sink CS_CreateMjpegServer(byte* name, byte* listenAddress,
                                     int port, int* status);
        CS_Sink CS_CreateCvSink(byte* name, int* status);
        CS_Sink CS_CreateCvSinkCallback(byte* name, void* data, IntPtr processFrame,
                                        int* status);
        /** @} */

        /**
         * @defgroup cscore_sink_cfunc Sink Functions
         * @{
         */
        SinkKind CS_GetSinkKind(CS_Sink sink, int* status);
        byte* CS_GetSinkName(CS_Sink sink, int* status);
        byte* CS_GetSinkDescription(CS_Sink sink, int* status);
        CS_Property CS_GetSinkProperty(CS_Sink sink, byte* name,
                                       int* status);
        CS_Property* CS_EnumerateSinkProperties(CS_Sink sink, int* count,
                                                int* status);
        void CS_SetSinkSource(CS_Sink sink, CS_Source source, int* status);
        CS_Property CS_GetSinkSourceProperty(CS_Sink sink, byte* name,
                                             int* status);
        CS_Bool CS_SetSinkConfigJson(CS_Sink sink, byte* config,
                                     int* status);
        byte* CS_GetSinkConfigJson(CS_Sink sink, int* status);
        CS_Source CS_GetSinkSource(CS_Sink sink, int* status);
        CS_Sink CS_CopySink(CS_Sink sink, int* status);
        void CS_ReleaseSink(CS_Sink sink, int* status);
        /** @} */

        /**
         * @defgroup cscore_mjpegserver_cfunc MjpegServer Sink Functions
         * @{
         */
        byte* CS_GetMjpegServerListenAddress(CS_Sink sink, int* status);
        int CS_GetMjpegServerPort(CS_Sink sink, int* status);
        /** @} */

        /**
         * @defgroup cscore_opencv_sink_cfunc OpenCV Sink Functions
         * @{
         */
        void CS_SetSinkDescription(CS_Sink sink, byte* description,
                                   int* status);
        byte* CS_GetSinkError(CS_Sink sink, int* status);
        void CS_SetSinkEnabled(CS_Sink sink, CS_Bool enabled, int* status);
        /** @} */

        /**
         * @defgroup cscore_listener_cfunc Listener Functions
         * @{
         */
        void CS_SetListenerOnStart(IntPtr onStart, void* data);
        void CS_SetListenerOnExit(IntPtr onEnd, void* data);

        CS_Listener CS_AddListener(
            void* data, IntPtr callback,
    int eventMask, int immediateNotify, int* status);

        void CS_RemoveListener(CS_Listener handle, int* status);
        /** @} */

        int CS_NotifierDestroyed();

        /**
         * @defgroup cscore_telemetry_cfunc Telemetry Functions
         * @{
         */
        void CS_SetTelemetryPeriod(double seconds);
        double CS_GetTelemetryElapsedTime();
        long CS_GetTelemetryValue(CS_Handle handle, TelemetryKind kind,
                             int* status);
        double CS_GetTelemetryAverageValue(CS_Handle handle, TelemetryKind kind,
                                   int* status);
        /** @} */

        /**
         * @defgroup cscore_logging_cfunc Logging Functions
         * @{
         */

        void CS_SetLogger(IntPtr func, uint min_level);
        void CS_SetDefaultLogger(uint min_level);
        /** @} */

        /**
         * @defgroup cscore_shutdown_cfunc Library Shutdown Function
         * @{
         */
        void CS_Shutdown();
        /** @} */

        /**
         * @defgroup cscore_utility_cfunc Utility Functions
         * @{
         */

        CS_UsbCameraInfo* CS_EnumerateUsbCameras(int* count, int* status);
        void CS_FreeEnumeratedUsbCameras(CS_UsbCameraInfo* cameras, int count);

        CS_Source* CS_EnumerateSources(int* count, int* status);
        void CS_ReleaseEnumeratedSources(CS_Source* sources, int count);

        CS_Sink* CS_EnumerateSinks(int* count, int* status);
        void CS_ReleaseEnumeratedSinks(CS_Sink* sinks, int count);

        void CS_FreeString(byte* str);
        void CS_FreeEnumPropertyChoices(byte** choices, int count);
        void CS_FreeUsbCameraInfo(CS_UsbCameraInfo* info);
        void CS_FreeHttpCameraUrls(byte** urls, int count);

        void CS_FreeEnumeratedProperties(CS_Property* properties, int count);
        void CS_FreeEnumeratedVideoModes(VideoMode* modes, int count);

        byte* CS_GetHostname();

        byte** CS_GetNetworkInterfaces(int* count);
        void CS_FreeNetworkInterfaces(byte** interfaces, int count);
    }
}