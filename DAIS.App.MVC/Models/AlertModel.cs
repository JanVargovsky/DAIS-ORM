using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace DAIS.App.MVC.Models
{
    public enum AlertType
    {
        Success,
        Info,
        Warning,
        Danger,
    }

    public class AlertModel
    {
        public string Text { get; set; }
        public AlertType AlertType { get; set; }

        public bool ShowClose { get; set; }

        public string AlertTypeClass()
        {
            switch (AlertType)
            {
                case AlertType.Success:
                    return "alert-success";
                case AlertType.Info:
                    return "alert-info";
                case AlertType.Warning:
                    return "alert-warning";
                case AlertType.Danger:
                    return "alert-danger";
                default:
                    throw new ApplicationException($"{AlertType}'s class is missing.");
            }
        }

        public string ShowCloseClass()
        {
            return ShowClose ? "data-dismissible" : string.Empty;
        }
    }

    public class AlertsListModel
    {
        public IList<AlertModel> Alerts { get; set; }

        public AlertsListModel(params AlertModel[] alerts)
        {
            if (alerts == null)
                throw new ArgumentNullException($"{alerts} is null.");

            Alerts = alerts;
        }
    }

    public enum AlertCode
    {
        Created,
        Deleted,
        Edited,
        Error,
        InvalidFormData,
        NotSelectedUser
    }

    public class AlertModelFactory
    {
        private static readonly AlertModelFactory instance = new AlertModelFactory();
        public static AlertModelFactory Instance => instance;

        public AlertModel Create(AlertCode code)
        {
            switch (code)
            {
                case AlertCode.Created:
                    return new AlertModel { Text = "Created.", AlertType = AlertType.Success, ShowClose = true };
                case AlertCode.Deleted:
                    return new AlertModel { Text = "Deleted.", AlertType = AlertType.Success, ShowClose = true };
                case AlertCode.Edited:
                    return new AlertModel { Text = "Edited.", AlertType = AlertType.Success, ShowClose = true };
                case AlertCode.Error:
                    return new AlertModel { Text = "We're sorry, but something went wrong with your request :(", AlertType = AlertType.Danger, ShowClose = true };
                case AlertCode.InvalidFormData:
                    return new AlertModel { Text = "Invalid values.", AlertType = AlertType.Danger, ShowClose = true };
                case AlertCode.NotSelectedUser:
                    return new AlertModel { Text = "Not selected user.", AlertType = AlertType.Danger, ShowClose = true };
                default:
                    throw new NotImplementedException($"{code} is not implemented yet.");
            }
        }
    }
}