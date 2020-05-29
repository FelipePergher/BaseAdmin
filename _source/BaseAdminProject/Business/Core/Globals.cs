// <copyright file="Globals.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

namespace BaseAdminProject.Business.Core
{
    public static class Globals
    {
        public const string StatusMessageKey = "StatusMessage";
        public const string StatusMessageTypeKey = "StatusMessageType";

        // Error messages
        public const string RequiredMessage = "O campo {0} é obrigatório";
        public const string EmailRequiredMessage = "Insira um endereço de email válido.";

        // Error Messages Type
        public const string StatusMessageTypeSuccess = "success";
        public const string StatusMessageTypeDanger = "danger";

        // Masks
        public const string MaskCpf = "000.000.000-00";
        public const string MaskPrice = "000.000.000.000.000,00";
        public const string MaskDate = "99/99/9999";
        public const string MaskPhone = "(00) 0 0000-0000";
    }
}
