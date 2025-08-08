namespace InventoryManager.Domain.Enums
{
    public enum InnerStatusCode
    {
        NotInitStatusCode = 0,
        EntityNotFound = 1,

        EntityCreate = 10,
        EntityUpdate = 20,
        EntityDelete = 30,
        EntityRead = 40,
        EntityExist = 50,
        AccountAuthenticate = 60,

        OK = 200,
        OKNoContent = 204,
        Forbiden = 403,
        Locked = 423,
        Conflict = 409,
        UnsupportedMediaType = 415,
        InternalServerError = 500,
    }
}
