namespace AerationSterilize.Persistence.Constants;
internal class TableNames
{
    // *********** Plural Nouns ***********
    internal const string Actions = nameof(Actions);
    internal const string Functions = nameof(Functions);
    internal const string ActionInFunctions = nameof(ActionInFunctions);
    internal const string Permissions = nameof(Permissions);

    internal const string AppUsers = nameof(AppUsers);
    internal const string AppRoles = nameof(AppRoles);
    internal const string AppUserRoles = nameof(AppUserRoles);

    internal const string AppUserClaims = nameof(AppUserClaims); // IdentityUserClaim
    internal const string AppRoleClaims = nameof(AppRoleClaims); // IdentityRoleClaim
    internal const string AppUserLogins = nameof(AppUserLogins); // IdentityRoleClaim
    internal const string AppUserTokens = nameof(AppUserTokens); // IdentityUserToken

    internal const string UserSessions = nameof(UserSessions);

    // *********** Singular Nouns ***********
    internal const string Product = nameof(Product);
    internal const string AerationColumn = nameof(AerationColumn);
    internal const string AerationPosition = nameof(AerationPosition);
    internal const string Batch = nameof(Batch);
    internal const string BatchInAerationPosition = nameof(BatchInAerationPosition);
    internal const string BatchItem = nameof(BatchItem);
    internal const string DataAmiQ411 = nameof(DataAmiQ411);
    internal const string DataPlan = nameof(DataPlan);
    internal const string Settings = nameof(Settings);
    internal const string PackingColumn = nameof(PackingColumn);
    internal const string PackingPosition = nameof(PackingPosition);
}
