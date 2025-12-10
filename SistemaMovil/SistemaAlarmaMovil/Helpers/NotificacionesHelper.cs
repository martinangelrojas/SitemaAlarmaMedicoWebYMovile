using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using SistemaAlarmaMovil.Domain;
using SistemaAlarmaMovil.Helpers.Interfaces;

namespace SistemaAlarmaMovil.Helpers
{
    public class NotificacionesHelper : INotificacionesHelper
    {
        private const string CHANNEL_ID = "medicamentos_channel";
        private const string CHANNEL_NAME = "Recordatorios de Medicamentos";
        private const string CHANNEL_DESCRIPTION = "Notificaciones para recordatorios de medicamentos";

        public NotificacionesHelper()
        {
            CrearCanalNotificaciones();
        }

        private void CrearCanalNotificaciones()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    CHANNEL_ID,
                    CHANNEL_NAME,
                    NotificationImportance.High)
                {
                    Description = CHANNEL_DESCRIPTION
                };

                var notificationManager = Platform.CurrentActivity.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager?.CreateNotificationChannel(channel);
            }
        }

        public async Task ProgramarNotificacionPrueba(int segundos)
        {
            try
            {
                if (!PermissionHelper.CheckNotificationPermissions())
                {
                    var granted = await PermissionHelper.RequestNotificationPermissions();
                    if (!granted)
                    {
                        //await Application.Current.MainPage.DisplayAlert("Error", "Se requieren permisos para programar notificaciones", "OK");
                        return;
                    }
                }

                var alarmManager = Platform.CurrentActivity.GetSystemService(Context.AlarmService) as AlarmManager;
                var pendingIntentFlags = PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable;

                // Programamos la notificación para 30 segundos después
                var tiempoNotificacion = DateTime.Now.AddSeconds(segundos);
                var tiempoMillis = new DateTimeOffset(tiempoNotificacion).ToUnixTimeMilliseconds();

                var notificationId = $"test_notification_{DateTime.Now.Ticks}"; // ID único

                var intent = new Intent(Platform.CurrentActivity, typeof(NotificationReceiver));
                intent.PutExtra("nombre", "Medicamento de Prueba");
                intent.PutExtra("cantidad", "1");
                intent.PutExtra("notificacionId", "test_notification");

                var pendingIntent = PendingIntent.GetBroadcast(
                    Platform.CurrentActivity,
                    notificationId.GetHashCode(), intent,
                    pendingIntentFlags);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    alarmManager?.SetExactAndAllowWhileIdle(
                        AlarmType.RtcWakeup,
                        tiempoMillis,
                        pendingIntent);
                }
                else
                {
                    alarmManager?.SetExact(
                        AlarmType.RtcWakeup,
                        tiempoMillis,
                        pendingIntent);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al programar notificación: {ex.Message}");
            }
        }

        public async Task ProgramarNotificacionesLineaOrdenMedica(LineaOrdenMedicaDto lineaOrdenMedica)
        {
            try
            {
                if (!PermissionHelper.CheckNotificationPermissions())
                {
                    var granted = await PermissionHelper.RequestNotificationPermissions();
                    if (!granted)
                    {
                        return;
                    }
                }

                var alarmManager = Platform.CurrentActivity.GetSystemService(Context.AlarmService) as AlarmManager;
                var pendingIntentFlags = PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable;

                for (int i = 0; i < lineaOrdenMedica.Cantidad; i++)
                {
                    var tiempoNotificacion = DateTime.Now.AddSeconds(lineaOrdenMedica.FrecuenciaHoras.Value * (i + 1));
                    var tiempoMillis = new DateTimeOffset(tiempoNotificacion).ToUnixTimeMilliseconds();

                    var intent = new Intent(Platform.CurrentActivity, typeof(NotificationReceiver));
                    intent.PutExtra("ordenMedicaId", lineaOrdenMedica.OrdenMedicaId.ToString());
                    intent.PutExtra("lineaId", lineaOrdenMedica.LineaOrdenMedicaId.ToString());
                    intent.PutExtra("nombre", lineaOrdenMedica.Nombre);
                    intent.PutExtra("cantidad", lineaOrdenMedica.Cantidad.ToString());
                    intent.PutExtra("observacion", lineaOrdenMedica.Observacion);
                    intent.PutExtra("notificacionId", $"{lineaOrdenMedica.OrdenMedicaId}_{lineaOrdenMedica.LineaOrdenMedicaId}_{i}");

                    var pendingIntent = PendingIntent.GetBroadcast(
                        Platform.CurrentActivity,
                        $"{lineaOrdenMedica.OrdenMedicaId}_{lineaOrdenMedica.LineaOrdenMedicaId}_{i}".GetHashCode(),
                        intent,
                        pendingIntentFlags);

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        alarmManager?.SetExactAndAllowWhileIdle(
                            AlarmType.RtcWakeup,
                            tiempoMillis,
                            pendingIntent);
                    }
                    else
                    {
                        alarmManager?.SetExact(
                            AlarmType.RtcWakeup,
                            tiempoMillis,
                            pendingIntent);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al programar notificaciones: {ex.Message}");
            }
        }

        public async Task EliminarNotificacionesOrdenMedica(int ordenMedicaId)
        {
            try
            {
                var alarmManager = Platform.CurrentActivity.GetSystemService(Context.AlarmService) as AlarmManager;
                var pendingIntentFlags = PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable;

                // Obtenemos todas las notificaciones activas
                var intent = new Intent(Platform.CurrentActivity, typeof(NotificationReceiver));
                var pendingIntent = PendingIntent.GetBroadcast(
                    Platform.CurrentActivity,
                    0,
                    intent,
                    pendingIntentFlags);

                alarmManager?.Cancel(pendingIntent);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al eliminar notificaciones: {ex.Message}");
            }
        }
    }

    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class NotificationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                var notificationId = intent.GetStringExtra("notificacionId").GetHashCode();

                var textoObservacion = string.IsNullOrEmpty(intent.GetStringExtra("observacion")) ? "" : intent.GetStringExtra("observacion");

                var builder = new NotificationCompat.Builder(context, "medicamentos_channel")
                    .SetContentTitle("SUPER REMINDER")
                    .SetContentText($"Es hora de tomar {intent.GetStringExtra("nombre")} - Obs.: {textoObservacion}")
                    .SetSmallIcon(Resource.Mipmap.appicon)
                    .SetPriority(NotificationCompat.PriorityHigh)
                    .SetAutoCancel(true);

                notificationManager?.Notify(notificationId, builder.Build());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al mostrar notificación: {ex.Message}");
            }
        }
    }
}
