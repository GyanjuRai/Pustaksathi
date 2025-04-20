using System.ComponentModel;


namespace E2_Dynamics.Model.Shared.Enum
{
    public enum EnumResponse
    {
        [Description("The request is complete!")]
        Success,
        [Description("Please review the exception to continue!")]
        Exception,
        [Description("Invalid credentials!")]
        InvalidCredentials,
        [Description("The request is invalid!")]
        InvalidRequest,
        [Description("The request is unauthorized!")]
        Unauthorized,
        [Description("The request has failed!")]
        Failed,
        [Description("No record found!")]
        NoRecordFound,
        [Description("Something went wrong!")]
        SomethingWentWrong,
    }
}
