namespace Opteamix.AuthorizationFramework.Common
{
    public class Constants
    {
        //application level exception message
        public static string RequiredFieldError = "Required fields cannot be blank.";
        public static string NoAccessToRole = "Role does not have access to any component.";
        public static string UndefinedError = "Undefined error occured.";
        public static string InvalidApplicaName = "Aplication does not exist for the given client.";
        public static string DuplicateValueInExcel = "Duplicate values not allowed unique excel column.";
        public static string BlankColumnInExcel = "Column name in excel cannot be blank";
        public static string BlankValueInExcel = "Value cannot be blank in excel";
        public static string ExcelError = "There are some undefined issues with excel";
        public static string WrongColumnInExcel = "Some column names are invalid";
        public static string ExcelExtension = "File not supported, extension must be '.xlsx'";
        public static string ExcelTemplateError = "Template file does not exist for export.";
        public static string IntegerValueError = "Non integer value provided for integer.";

        public static string ResultNotFound = "There are some issues on fetching the data";

        //success messages
        public static string ImportSuccess = "Excel file has been successfully imported";
        public static string ModuleAddSuccess = "Module has been successfully added";
        public static string PrivilegeAddSuccess = "Privilege has been successfully added";
        public static string ModuleRemoveSuccess = "Module has been successfully removed";
        public static string PrivilegeRemoveSuccess = "Privilege has been successfully removed";
        public static string ClientAddSuccess= "Client has been successfully added";
        public static string ClientRemoveSuccess= "Client has been successfully removed";
        public static string RoleAddSuccess = "Role has been successfully added";
        public static string RoleRemoveSuccess = "Role has been successfully removed";

        //database
        public static string EntityRolePrivilegeView = "vw_EntityRolePrivilege";

        //action method
        public const string GetAccessActionName = "GetEntityAccess";
        public const string ImportExcelActionName = "Import";
        public const string ExportExcelActionName = "Export";

        //file
        public static string ExportExcelFile = "ExportExcel.xlsx";
        public static string FilePackage = "File";
        public static string TemplateFile = "Template\\Template.xlsx";

    }
}
