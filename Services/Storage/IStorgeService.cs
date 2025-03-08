namespace ASP_SPR311.Services.Storage
{
    public interface IStorageService
    {
        String SaveFile(IFormFile formFile);
        String GetRealPath(String name);
    }
}
/* Служба збереження файлів.
 * Особливість роботи з файлами у тому, що їх бажано виводити за "простір" сайту.
 * Після оновлення файлів сайту (redeploy) можливі ситуації з втратою 
 * напрацьованих сайтом файлів.
 */
