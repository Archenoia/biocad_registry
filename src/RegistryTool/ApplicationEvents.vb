Imports System.Runtime.CompilerServices
Imports Galaxy.Workbench
Imports Ollama
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports registry_data
Imports RegistryTool.Configs

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    ' **NEW** ApplyApplicationDefaults: Raised when the application queries default values to be set for the application.

    ' Example:
    ' Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
    '
    '   ' Setting the application-wide default Font:
    '   e.Font = New Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
    '
    '   ' Setting the HighDpiMode for the Application:
    '   e.HighDpiMode = HighDpiMode.PerMonitorV2
    '
    '   ' If a splash dialog is used, this sets the minimum display time:
    '   e.MinimumSplashScreenDisplayTime = 4000
    ' End Sub

    Partial Friend Class MyApplication

        Public Shared ReadOnly Property biocad_registry As biocad_registry
        Public Shared ReadOnly Property host As FormMain
            Get
                Return DirectCast(CommonRuntime.AppHost, FormMain)
            End Get
        End Property

        Public Shared ReadOnly Property settings As Configs.Settings

        Public Shared ReadOnly Property ollama As LLMClient
            Get
                With Configs.Settings.Load
                    Return New LLMClient(LLMUrl.Create($"{ .ollama_server}:{ .ollama_service}"), .model)
                End With
            End Get
        End Property

        Public Shared Function Load() As Boolean
            Dim config As Settings = Settings.Load
            Dim mysqli As New ConnectionUri With {
                .Database = config.dbname,
                .IPAddress = config.host,
                .Password = config.password,
                .Port = config.port,
                .User = config.user
            }

            Try
                _biocad_registry = New biocad_registry(mysqli)
                _settings = config
            Catch ex As Exception
                If MessageBox.Show("Invalid Mysql Connection information, please re-config your parameters!" & vbCrLf & vbCrLf & ex.Message,
                                   "Invalid Mysql Connection",
                                   MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Warning) = DialogResult.OK Then

                    Call New FormSettings().ShowDialog()
                    Return Load()
                End If

                Return False
            End Try

            Return biocad_registry.getDriver.Ping >= 0
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function Loading(Of T)(getter As Func(Of Action(Of String), T)) As T
            Return TaskProgress.LoadData(getter, info:="Loading data, this may takes for a long time, please wait for a while...")
        End Function

    End Class
End Namespace
